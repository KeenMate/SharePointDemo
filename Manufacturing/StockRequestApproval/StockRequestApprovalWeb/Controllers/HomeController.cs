using Microsoft.SharePoint.Client;
using RERHostDemoWeb;
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
			}

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult TestMail()
		{
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
	}
}
