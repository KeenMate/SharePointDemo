using Microsoft.SharePoint.Client;
using RERHostDemoWeb;
using StockRequestApprovalWeb.Models;
using StockRequestApprovalWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestApprovalWeb.Controllers
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
			}/*
			StockRequestModel model = new StockRequestModel()
			{
				DeliveredOn = new DateTime(),
				Items = new List<StockRequestItem>(),
				UserName = "Párek v rohlíku"
			};
			model.Items.Add(new StockRequestItem() { Amount = 5, Title = "Metal Drill", TotalPrice = 250 });
			model.Items.Add(new StockRequestItem() { Amount = 10, Title = "Ear Muffs", TotalPrice = 100 });
			MailerService.SendMail("StockRequest", "jakub.senk@keenmate.com", model);*/
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

		public ActionResult ConnectTo()
		{
			ViewBag.Message = "";

			return View();
		}

		public ActionResult Connect(string hostUrl)
		{
			TokenRepository repository = new TokenRepository(Request, Response);
			repository.Connect(hostUrl);
			return View();
		}
		public ActionResult Callback(string code)
		{
			TokenRepository repository = new TokenRepository(Request, Response);
			repository.Callback(code);
			return RedirectToAction("Index");
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
						EventReceiverType.ItemAdding,
						EventReceiverSynchronization.Synchronous,
						"StockRequestEventReceiver",
						"https://demostockrequest.servicebus.windows.net/2117014651/1934699884/obj/9e92b40e-512e-4313-836e-c9e8fefc1dc2/Services/StockRequestEventReceiver.svc",
								10);
				}
			}
			return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
		}
	}
}
