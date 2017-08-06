using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestApprovalWeb.Controllers
{
	public class ResponseController : Controller
	{
		// GET: Response
		public ActionResult Index()
		{
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			return RedirectToAction("Index", "Home", new { SPHostUrl = spContext.SPHostUrl.ToString() });
		}

		public ActionResult Approve()
		{
			string query = Request.QueryString["response"];
			string url = Request.Url.OriginalString;
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
		}

		public ActionResult Reject()
		{
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			return RedirectToAction("Index", new { SPHostUrl = spContext.SPHostUrl.ToString() });
		}
	}
}