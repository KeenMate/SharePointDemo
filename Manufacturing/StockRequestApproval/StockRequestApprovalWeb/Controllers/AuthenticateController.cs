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

		public ActionResult Authenticate(string usr, string pass, string sp)
		{
			var post = string.Format(Helpers.Assets.AuthenticationTemplate.SAMLTemplate, usr, pass, ConfigurationManager.AppSettings["endpoint"]);
			AuthenticationHelper a = new AuthenticationHelper();
			string responseXML = a.GetSAMLToken(post);
			string token = a.ExtractSAMLToken(responseXML);
			Response.SetCookie(new HttpCookie("SPTokenForCC", token));
			if (token != string.Empty)
			{
				Dictionary<string, string> d = a.GetSPOCookies(token, Request.UserAgent);
				foreach (var item in d)
				{
					Response.SetCookie(new HttpCookie(item.Key, item.Value));
				}
			}
			return RedirectToAction("Index", "Authenticate", new { SPHostUrl = sp });
		}

		public ActionResult TestClientContext()
		{
			var cContext = TokenHelper.GetClientContextWithAccessToken(Request.QueryString["SPHostUrl"], Request.Cookies["SPTokenForCC"].Value);
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

			return RedirectToAction("Index", "Authenticate", new { SPHostUrl = Request.QueryString["SPHostUrl"] });
		}
	}
}