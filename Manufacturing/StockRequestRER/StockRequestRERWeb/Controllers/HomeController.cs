using Microsoft.SharePoint.Client;
using StockRequestRERWeb.Helpers;
using StockRequestRERWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestRERWeb.Controllers
{
	public class HomeController : Controller
	{
		[SharePointContextFilter]
		public ActionResult Index()
		{
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			using (var clientContext = spContext.CreateUserClientContextForSPHost())
			{
				if (clientContext != null)
				{
					var spWeb = clientContext.Web;
					var hostListColl = spWeb.Lists;
					clientContext.Load(spWeb, w => w.Id);
					clientContext.Load(hostListColl);
					clientContext.ExecuteQuery();
					ViewBag.HostLists = hostListColl.Select(l => new SelectListItem() { Text = l.Title, Value = l.Title });
				}
			}
			Managers.NotificationManager m = new Managers.NotificationManager(ConfigurationHelper.GetApiKey(ApiKeys.SendGridApiKey), ConfigurationManager.AppSettings["templateDefsPath.json"]);
			ApprovalRequestModel model = new ApprovalRequestModel()
			{
				ApproverName = "Someone",
				Items = new List<StockRequestItem>(),
				RequesterEmail = "foo@foo.foo",
				RequesterName = "foo",
				Url = "https://localhost"
			};
			model.Items.Add(new StockRequestItem() { Amount = 10, Title = "Some item", TotalPrice = 999 });
			model.Items.Add(new StockRequestItem() { Amount = 10, Title = "Some other item", TotalPrice = 499 });
			model.Items.Add(new StockRequestItem() { Amount = 10, Title = "Some great item", TotalPrice = 4999 });
			m.SendApprovalRequest(model, "foo@foo.foo");
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[SharePointContextFilter]
		[HttpPost]
		public ActionResult Subscribe(string listTitle)
		{
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			using (var clientContext = spContext.CreateUserClientContextForSPHost())
			{
				if (!string.IsNullOrEmpty(listTitle))
				{
					RERUtility.AddListItemRemoteEventReceiver(
						clientContext,
						listTitle,
						EventReceiverType.ItemAdded,
						EventReceiverSynchronization.Asynchronous,
						"RERHostReceiver",
						"https://stockrequest.servicebus.windows.net/2117014651/1934699884/obj/b2f02062-1030-4c24-9a8f-e3adfd502bf7/Services/StockRequestRER.svc",
								10);
					RERUtility.AddListItemRemoteEventReceiver(
						clientContext,
						listTitle,
						EventReceiverType.ItemAdding,
						EventReceiverSynchronization.Synchronous,
						"RERHostReceiver",
						"https://stockrequest.servicebus.windows.net/2117014651/1934699884/obj/b2f02062-1030-4c24-9a8f-e3adfd502bf7/Services/StockRequestRER.svc",
								10);
				}
			}
			return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
		}
	}
}
