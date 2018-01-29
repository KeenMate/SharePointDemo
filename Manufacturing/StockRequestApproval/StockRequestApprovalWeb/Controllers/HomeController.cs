using Microsoft.SharePoint.Client;
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
			return RedirectToAction("Index", "RequestsOverview");
		}
	}
}
