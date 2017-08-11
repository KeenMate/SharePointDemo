using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using StockRequestApprovalWeb.Helpers;
using StockRequestApprovalWeb.Managers;
using StockRequestApprovalWeb.Models;
using StockRequestApprovalWeb.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestApprovalWeb.Controllers
{
	public class MailController : Controller
	{
		// GET: Mail
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SendMail(SendEmailViewModel email)
		{
			JsonLoader j = new JsonLoader();
			NotificationManager m = new NotificationManager(ConfigurationHelper.GetApiKey(ApiKeys.SendGridApiKey), ConfigurationManager.AppSettings["templateDefsPath.json"]);
			switch (email.Model)
			{
				case MailTemplatesKeys.ApprovalRequestForManager:
					{
						ApprovalRequestModel model = new ApprovalRequestModel();
						model.ApproverName = "Nekdo";
						model.ItemGuid = new Guid();
						model.Items.Add(new StockRequestItem() { Amount = 1, MaterialType = "Gas", Title = "Acetylene", TotalPrice = 800 });
						model.RequesterEmail = "someone@somewhere";
						model.RequesterName = "Zas někdo";
						m.SendApprovalRequest(model, email.To);
						break;
					}
				case MailTemplatesKeys.ConfirmationRequestForUser:
					{
						break;
					}
				default:
					break;
			}


			return RedirectToAction("Index");
		}
	}
}