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
		public ActionResult Subscribe(string pass, string url)
		{
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

			if (pass != ConfigurationHelper.GetPassword("RERPassword"))
				return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString(), msg = "Wrong password" });

			string listTitle = ConfigurationManager.AppSettings["StockRequestListName"];
			if (string.IsNullOrEmpty(url))
				url = "https://stockrequest.servicebus.windows.net/936123709/1934699884/obj/b2f02062-1030-4c24-9a8f-e3adfd502bf7/Services/StockRequestRER.svc";
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
						url,
								10);
					RERUtility.AddListItemRemoteEventReceiver(
						clientContext,
						listTitle,
						EventReceiverType.ItemAdding,
						EventReceiverSynchronization.Synchronous,
						"RERHostReceiver",
						url,
								10);
				}
			}
			return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString(), msg = "Something happened... now pray for it to work." });
		}
	}
}
