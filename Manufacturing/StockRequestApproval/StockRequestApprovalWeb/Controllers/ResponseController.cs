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

namespace StockRequestApprovalWeb.Controllers
{
	public class ResponseController : Controller
	{
		// GET: Response
		public ActionResult Index()
		{
			//var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			return RedirectToAction("About", "Home"/*, new { SPHostUrl = spContext.SPHostUrl.ToString() }*/);
		}

		public ActionResult Approve()
		{
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					return ProcessRequest("Approve");
				}
				else
				{
					Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Authenticate", "Authenticate");
				}
			}
			return View("Close");
		}

		public ActionResult Reject()
		{
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					return ProcessRequest("Reject");
				}
				else
				{
					Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Authenticate", "Authenticate");
				}
			}
			return View("Close");
		}

		private ActionResult ProcessRequest(string action)
		{
			using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value))
			{
				if (clientContext != null)
				{
					clientContext.Load(clientContext.Web);
					clientContext.Load(clientContext.Web.CurrentUser);
					StockRequestApproveData data;
					try
					{
						data = SharepointListHelper.ParseStockRequestList(clientContext, Request.QueryString["guid"]);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Not Authenticated")
						{
							if (action == "Reject")
								Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
							else
								Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
							return RedirectToAction("Authenticate", "Authenticate");
						}
						else throw;
					}

					if (action == "Reject")
					{
						if (data.Status == Status.Rejected)
							throw new Exception("This item was already rejected.");
						if (data.Status == Status.Approved)
							throw new Exception("This item was approved. You can not reject it anymore.");
						if (!data.AllowedApprovers.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
							throw new Exception("You are not allowed to reject this request.");
						data.UpdateItem("Approved", "Rejected");
					}
					else
					{
						processApprove(clientContext, data);
					}
					clientContext.ExecuteQuery();
				}
			}
			return View("Close");
		}

		private void processApprove(ClientContext clientContext, StockRequestApproveData data)
		{
			if (data.Status == Status.Rejected)
				throw new Exception("This item was rejected. You can not approve it anymore.");
			if (data.Status == Status.Approved)
				throw new Exception("This item was already approved. You can not approve it anymore.");
			if (data.DeliveredOn < DateTime.Now)
				throw new Exception($"Too late to approve this request. The request should be delivered on {data.DeliveredOn.ToString()}");

			FieldUserValue me = new FieldUserValue();
			me.LookupId = clientContext.Web.CurrentUser.Id;

			if (!data.AllowedApprovers.Any(x => x.LookupId == me.LookupId))
				throw new Exception("You are not allowed to approve this request.");

			if (data.ApprovedBy.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
				throw new Exception("You already approved this item.");

			data.ApprovedBy.Add(me);

			StockRequestModel model = StockRequestMapper.MapStockRequestModel(data.OriginalItem);

			if (data.ApprovedBy.Count == data.AllowedApprovers.Count)
			{
				if (!data.ApprovedBy.Select(x => x.LookupId).Except(data.AllowedApprovers.Select(x => x.LookupId)).Any())
				{
					ListItemCollection stockItemList = SharepointListHelper.GetListItems(clientContext, ConfigurationManager.AppSettings["StockItemsListName"]);
					clientContext.ExecuteQuery();
					ListItemCollection stockList = SharepointListHelper.GetRequiredItems(clientContext, model.Items, stockItemList);

					SharepointListHelper.AreItemsAvaiable(clientContext, model.Items, stockItemList, stockList);

					SharepointListHelper.WithdrawItems(clientContext, model.Items, stockList);
					data.UpdateItem("Approved", "Approved");
				}
				else
				{
					throw new Exception("An unautorized person approved this request. Please contact your administrator.");
				}
			}

			data.UpdateItem("ApprovedBy", data.ApprovedBy.ToArray());
			clientContext.ExecuteQuery();

			TemplateModel m = new TemplateModel();
			ApprovalRequestModel am = new ApprovalRequestModel();
			FieldUserValue val = (FieldUserValue)data.OriginalItem.FieldValues["Author"];
			am.ApproverName = clientContext.Web.CurrentUser.Title;
			am.ItemGuid = data.RequestID;
			am.Items = data.Items;
			am.RequesterEmail = val.Email;
			am.RequesterName = val.LookupValue;
			m.Model = am;
			m.MainTemplatePath = "Templates\\ApprovalRequest.cshtml";
			m.TemplatePaths = new List<string>();
			m.TemplatePaths.AddRange(new string[] { "Templates\\Head.cshtml", "Templates\\Header.cshtml", "Templates\\Footer.cshtml" });
			m.MainTemplateModelType = am.GetType();
			string s = new TemplateService().CreateMessage(m);

			
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;

			var model = new HandleErrorInfo(filterContext.Exception, "Response", "Reject/Approve");

			filterContext.Result = new ViewResult()
			{
				ViewName = "Error",
				ViewData = new ViewDataDictionary(model)
			};
		}
	}
}