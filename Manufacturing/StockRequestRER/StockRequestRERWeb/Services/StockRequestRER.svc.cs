using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using System.Configuration;
using StockRequestRERWeb.Models;
using StockRequestRERWeb.Managers;
using StockRequestRERWeb.Helpers;
using NLog;

namespace StockRequestRERWeb.Services
{
	public class StockRequestRER : IRemoteEventService
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		/// <summary>
		/// Handles events that occur before an action occurs, such as when a user adds or deletes a list item.
		/// </summary>
		/// <param name="properties">Holds information about the remote event.</param>
		/// <returns>Holds information returned from the remote event.</returns>
		public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
		{
			logger.Debug("Received signal from SP item adding.");
			SPRemoteEventResult result = new SPRemoteEventResult();
			string guid = Guid.NewGuid().ToString();
			result.ChangedItemProperties.Add("RequestID", guid);
			logger.Info("Created requestID: " + guid);
			return result;
		}

		/// <summary>
		/// Handles events that occur after an action occurs, such as after a user adds an item to a list or deletes an item from a list.
		/// </summary>
		/// <param name="properties">Holds information about the remote event.</param>
		public void ProcessOneWayEvent(SPRemoteEventProperties properties)
		{
			logger.Debug("Received signal from SP item added.");
			using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
			{
				if (clientContext != null)
				{
					logger.Debug("Created clientContext.");
					clientContext.Load(clientContext.Web.CurrentUser);
					clientContext.Load(clientContext.Web);
					logger.Debug("Loaded currentUser.");

					List oList = clientContext.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["ConfigurationListName"]);

					CamlQuery camlQuery = new CamlQuery();
					ListItemCollection collListItem = oList.GetItems(camlQuery);

					clientContext.Load(collListItem);
					clientContext.ExecuteQuery();
					logger.Debug("Loaded Configuration list.");

					StockRequestModel model = StockRequestMapper.MapStockRequestModel(properties, clientContext.Web.CurrentUser.Title);

					List<string> neededMaterials = new List<string>();

					foreach (StockRequestItem item in model.Items)
					{
						if (!neededMaterials.Contains(item.MaterialType))
							neededMaterials.Add(item.MaterialType);
					}

					JsonLoader loader = new JsonLoader();
					NotificationManager m = new NotificationManager(ConfigurationHelper.GetApiKey(ApiKeys.SendGridApiKey), ConfigurationManager.AppSettings["templateDefsPath.json"]);
					List<string> alreadySend = new List<string>();
					foreach (string s in neededMaterials)
					{
						foreach (ListItem item in collListItem)
						{
							if (item.FieldValues["Title"].ToString() == s && !alreadySend.Contains(((FieldUserValue)item.FieldValues["Value"]).Email))
							{
								ApprovalRequestModel aModel = new ApprovalRequestModel();
								aModel.ApproverName = ((FieldUserValue)item.FieldValues["Value"]).LookupValue;
								aModel.Items = model.Items;
								aModel.ItemGuid = model.ItemGuid;
								aModel.RequesterName = clientContext.Web.CurrentUser.Title;
								aModel.RequesterEmail = clientContext.Web.CurrentUser.Email;
								m.SendApprovalRequest(aModel, ((FieldUserValue)item.FieldValues["Value"]).Email);
								alreadySend.Add(((FieldUserValue)item.FieldValues["Value"]).Email);
								break;
							}
						}
					}
					m.SendRequestApproved(model, clientContext.Web.CurrentUser.Email);
				}
				else
				{
					logger.Warn("ClientContext is null");
				}
			}
		}
	}
}
