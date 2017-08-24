using Microsoft.SharePoint.Client;
using StockRequestApprovalWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StockRequestApprovalWeb.Controllers
{
	public class AuthenticateController : Controller
	{
		// GET: Authenticate
		public ActionResult Index()
		{
			return View();
		}

		public void Authenticate()
		{
			string url = TokenHelper.GetAuthorizationUrl(ConfigurationManager.AppSettings["SharepointUrl"], "Web.Manage", "https://localhost:44322/RedirectAccept");
			Response.Redirect(url);
		}

		public void TestClientContext()
		{

			/*
			var cContext = TokenHelper.GetClientContextWithAccessToken(Request.QueryString["SPHostUrl"], "");
			cContext.Load(cContext.Web.CurrentUser);
			cContext.ExecuteQuery();
			var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			using (var clientContext = spContext.CreateUserClientContextForSPHost())
			{
				if (clientContext != null)
				{
					clientContext.Load(clientContext.Web.CurrentUser);
					clientContext.Load(clientContext.Web);
					clientContext.ExecuteQuery();
				}
			}
			*/
			//return RedirectToAction("Index", "Authenticate", new { SPHostUrl = Request.QueryString["SPHostUrl"] });
		}
	}
}