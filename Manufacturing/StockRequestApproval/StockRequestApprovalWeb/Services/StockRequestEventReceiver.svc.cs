using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using StockRequestApprovalWeb.Models;
using RazorEngine.Templating;
using RazorEngine;
using Newtonsoft.Json;
using System.Web;

namespace StockRequestApprovalWeb.Services
{
	public class StockRequestEventReceiver : IRemoteEventService
	{
		/// <summary>
		/// Handles events that occur before an action occurs, such as when a user adds or deletes a list item.
		/// </summary>
		/// <param name="properties">Holds information about the remote event.</param>
		/// <returns>Holds information returned from the remote event.</returns>
		public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
		{
			SPRemoteEventResult result = new SPRemoteEventResult();
			result.ChangedItemProperties.Add("RequestID", Guid.NewGuid().ToString());
			return result;
		}

		/// <summary>
		/// Handles events that occur after an action occurs, such as after a user adds an item to a list or deletes an item from a list.
		/// </summary>
		/// <param name="properties">Holds information about the remote event.</param>
		public void ProcessOneWayEvent(SPRemoteEventProperties properties)
		{
			using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
			{
				if (clientContext != null)
				{
					clientContext.Load(clientContext.Web.CurrentUser);
					clientContext.Load(clientContext.Web);

					List oList = clientContext.Web.Lists.GetByTitle("Configuration");

					CamlQuery camlQuery = new CamlQuery();
					ListItemCollection collListItem = oList.GetItems(camlQuery);

					clientContext.Load(collListItem);
					clientContext.ExecuteQuery();					

					StockRequestModel model = StockRequestMapper.MapStockRequestModel(properties, clientContext.Web.CurrentUser.Title);

					List<string> neededMaterials = new List<string>();

					foreach (StockRequestItem item in model.Items)
					{
						if (!neededMaterials.Contains(item.MaterialType))
							neededMaterials.Add(item.MaterialType);
					}

					foreach (string s in neededMaterials)
					{
						foreach (ListItem item in collListItem)
						{
							if (item.FieldValues["Title"].ToString() == s)
							{
								ConfirmationRequestModel cModel = new ConfirmationRequestModel();
								cModel.Name = ((FieldUserValue)item.FieldValues["Value"]).LookupValue;
								cModel.Items = model.Items;
								cModel.RequesterName = clientContext.Web.CurrentUser.Title;
								cModel.RequesterEmail = clientContext.Web.CurrentUser.Email;
								MailerService.SendMail("ConfirmationRequest", ((FieldUserValue)item.FieldValues["Value"]).Email, cModel);
								break;
							}
						}
					}

					MailerService.SendMail("StockRequest", clientContext.Web.CurrentUser.Email, model);
				}
			}
		}
	}
}
