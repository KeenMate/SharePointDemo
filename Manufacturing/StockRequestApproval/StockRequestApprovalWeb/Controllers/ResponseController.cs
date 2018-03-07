using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using StockRequestApprovalWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockRequestApprovalWeb.Models;
using StockRequestApprovalWeb.Services;
using System.IO;
using StockRequestApprovalWeb.Managers;
using NLog;

namespace StockRequestApprovalWeb.Controllers
{
	public class ResponseController : Controller
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		// GET: Response
		public ActionResult Index()
		{
			//var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			return RedirectToAction("About", "Home"/*, new { SPHostUrl = spContext.SPHostUrl.ToString() }*/);
		}

		public ActionResult Approve()
		{
			logger.Trace("Approve called.");
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					return ProcessRequest("Approve");
				}
				else
				{
					logger.Info("FinalAccessToken is null. Redirecting to authentication");
					Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Index", "Authenticate");
				}
			}
			else
			{
				logger.Warn("Query string guid is null.");
			}
			return View("Close");
		}
		[HttpPost]
		public object Approve(string guid)
		{
			logger.Trace("Approve called by POST method.");
			if (guid != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					try
					{
						return ProcessRequest("Approve", guid);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Authentication required")
						{
							logger.Warn("Authentication required. Access token is probably not valid.");
							return "Authentication required";
						}
						else
						{
							logger.Error(ex, "Exception occured.");
							return ex.Message;
						}
					}
				}
				else
				{
					logger.Info("FinalAccessToken is null. returning \"Authentication required\".");
					return "Authentication required";
				}
			}
			else
			{
				logger.Warn("Guid is null.");
			}
			return View("Close");
		}

		public ActionResult Reject()
		{
			logger.Trace("Reject called.");
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					return ProcessRequest("Reject");
				}
				else
				{
					logger.Info("FinalAccessToken is null. Redirecting to authentication");
					Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Authenticate", "Authenticate");
				}
			}
			else
			{
				logger.Warn("Query string guid is null.");
			}
			return View("Close");
		}
		[HttpPost]
		public object Reject(string guid)
		{
			logger.Trace("Reject called by POST method.");
			if (guid != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					try
					{
						return ProcessRequest("Reject", guid);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Authentication required")
						{
							logger.Warn("Authentication required. Access token is probably not valid.");
							return "Authentication required";
						}
						else
						{
							logger.Error(ex, "Exception occured.");
							return ex.Message;
						}
					}
				}
				else
				{
					logger.Info("FinalAccessToken is null. returning \"Authentication required\".");
					return "Authentication required";
				}
			}
			else
			{
				logger.Warn("Guid is null.");
			}
			return View("Close");
		}

		private ActionResult ProcessRequest(string action, string guid = null)
		{
			logger.Trace($"ProcessRequest called with parameters action: {action}, guid: {guid}");
			logger.Debug("Getting clientContext.");
			using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value))
			{
				if (clientContext != null)
				{
					logger.Debug("Loading current user.");
					clientContext.Load(clientContext.Web);
					clientContext.Load(clientContext.Web.CurrentUser);
					StockRequestApproveData data;
					try
					{
						logger.Debug("Parsing data from SP");
						if (guid == null)
							data = SharepointListHelper.ParseStockRequestList(clientContext, Request.QueryString["guid"]);
						else
							data = SharepointListHelper.ParseStockRequestList(clientContext, guid);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Not Authenticated")
						{
							logger.Warn("Not authenticated. Redirecting to authentication.");
							if (guid != null) throw new Exception("Authentication required");
							if (action == "Reject")
								Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
							else
								Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
							return RedirectToAction("Index", "Authenticate");
						}
						else
						{
							logger.Error(ex, "Error occured when trying to parse data.");
							throw;
						}
					}

					if (action == "Reject")
					{
						if (data.Status == Status.Rejected)
							throw new Exception("This item was already rejected.");
						if (data.Status == Status.Approved)
							throw new Exception("This item was approved. You can not reject it anymore.");
						if (!data.AllowedApprovers.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
							throw new Exception("You are not allowed to reject this request.");
						logger.Trace("Rejecting order.");
						data.UpdateItem(ConfigurationManager.AppSettings["StatusFieldName"], "Rejected");
						clientContext.ExecuteQuery();
						logger.Info("Order rejected. Generating PDF report.");

						data = SharepointListHelper.ParseStockRequestList(clientContext, data.RequestID.ToString());

						ReportService rep = new ReportService();
						DevExpressConfirmationModel m = new DevExpressHelper().ParseModel(data);
						List<DevExpressConfirmationModel> devModel = new List<DevExpressConfirmationModel>();
						devModel.Add(m);
						string path = rep.GenerateConfirmationReport(devModel, Server.MapPath("~/Temp"));
						logger.Debug("Report generated. Uploading to SP.");

						SharepointListHelper.UploadFile(clientContext, path);
						logger.Debug("File uploaded. Sending email.");

						NotificationManager nm = new NotificationManager(ConfigurationHelper.GetApiKey(ApiKeys.SendGridApiKey), ConfigurationManager.AppSettings["templateDefsPath.json"]);
						FieldUserValue val = (FieldUserValue)data.OriginalItem.FieldValues["Author"];
						ConfirmationEmailModel emailModel = new ConfirmationEmailModel();
						emailModel.Created = m.Created;
						emailModel.CreatedBy = m.CreatedBy;
						emailModel.Items = m.Items;
						emailModel.Rejector = m.ModifiedBy;
						emailModel.RejectorEmail = ((FieldUserValue)data.OriginalItem.FieldValues["Editor"]).Email;
						emailModel.ResponseID = m.ResponseID;
						emailModel.Status = m.Status;
						nm.SendApprovalConfirmation(emailModel, val.Email, path);

					}
					else
					{
						processApprove(clientContext, data);
					}
					clientContext.ExecuteQuery();
				}
				else
				{
					logger.Warn("ClientContext is null");
				}
			}
			return View("Close");
		}

		private void processApprove(ClientContext clientContext, StockRequestApproveData data)
		{
			logger.Trace("ProcessApprove called.");
			if (data.Status == Status.Rejected)
				throw new Exception("This item was rejected. You can not approve it anymore.");
			if (data.Status == Status.Approved)
				throw new Exception("This item was already approved. You can not approve it anymore.");
			if (data.DeliveredOn < DateTime.Now)
				throw new Exception($"Too late to approve this request. The request should be delivered on {data.DeliveredOn.ToString()}");

			logger.Debug("Adding current user to approved by. Current user is " + clientContext.Web.CurrentUser.Title);
			FieldUserValue me = new FieldUserValue();
			me.LookupId = clientContext.Web.CurrentUser.Id;

			if (!data.AllowedApprovers.Any(x => x.LookupId == me.LookupId))
				throw new Exception("You are not allowed to approve this request.");

			if (data.ApprovedBy.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
				throw new Exception("You already approved this item.");

			data.ApprovedBy.Add(me);

			logger.Debug("Mapping stock request model.");
			StockRequestModel model = StockRequestMapper.MapStockRequestModel(data.OriginalItem);


			if (!data.ApprovedBy.Select(x => x.LookupId).Except(data.AllowedApprovers.Select(x => x.LookupId)).Any())
			{
				if (data.ApprovedBy.Count == data.AllowedApprovers.Count)
				{
					logger.Debug("Getting Stock Items list.");
					ListItemCollection stockItemList = SharepointListHelper.GetListItems(clientContext, ConfigurationManager.AppSettings["StockItemsListName"]);
					clientContext.ExecuteQuery();
					logger.Debug("Getting Stock list.");
					ListItemCollection stockList = SharepointListHelper.GetRequiredItems(clientContext, model.Items, stockItemList);

					logger.Debug("Checking if items are avaiable to withdraw.");
					SharepointListHelper.AreItemsAvaiable(clientContext, model.Items, stockItemList, stockList);

					logger.Debug("Withdrawing items.");
					SharepointListHelper.WithdrawItems(clientContext, model.Items, stockList);
					data.UpdateItem(ConfigurationManager.AppSettings["StatusFieldName"], Status.Approved.ToUserFriendlyString());
				}
			}
			else
			{
				throw new Exception("An unautorized person approved this request. Please contact your administrator.");
			}

			logger.Debug("Updating approved by.");
			data.UpdateItem(ConfigurationManager.AppSettings["ApprovedByFieldName"], data.ApprovedBy.ToArray());
			clientContext.ExecuteQuery();
			logger.Info("Order approved.");

			data = SharepointListHelper.ParseStockRequestList(clientContext, data.RequestID.ToString());

			logger.Debug("Generating PDF.");
			ReportService rep = new ReportService();
			DevExpressConfirmationModel devmodel = new DevExpressHelper().ParseModel(data);
			List<DevExpressConfirmationModel> devModel = new List<DevExpressConfirmationModel>();
			devModel.Add(devmodel);
			string path = rep.GenerateConfirmationReport(devModel, Server.MapPath("~/Temp"));
			logger.Info("PDF generated. Uploading to SP");

			SharepointListHelper.UploadFile(clientContext, path);
			logger.Debug("File uploaded. Sending email.");

			NotificationManager nm = new NotificationManager(ConfigurationHelper.GetApiKey(ApiKeys.SendGridApiKey), ConfigurationManager.AppSettings["templateDefsPath.json"]);
			FieldUserValue val = (FieldUserValue)data.OriginalItem.FieldValues["Author"];
			ConfirmationEmailModel emailModel = new ConfirmationEmailModel();
			emailModel.Created = devmodel.Created;
			emailModel.CreatedBy = devmodel.CreatedBy;
			emailModel.Items = devmodel.Items;
			emailModel.Rejector = devmodel.ModifiedBy;
			emailModel.RejectorEmail = ((FieldUserValue)data.OriginalItem.FieldValues["Editor"]).Email;
			emailModel.ResponseID = devmodel.ResponseID;
			emailModel.Status = devmodel.Status;
			nm.SendApprovalConfirmation(emailModel, val.Email, path);

		}

		protected override void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;
			logger.Error(filterContext.Exception, "Exception occured");
			var model = new HandleErrorInfo(filterContext.Exception, "Response", "Reject/Approve");

			filterContext.Result = new ViewResult()
			{
				ViewName = "Error",
				ViewData = new ViewDataDictionary(model)
			};
		}
	}
}