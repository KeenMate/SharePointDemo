using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockRequestApprovalWeb.Helpers;
using NLog;

namespace StockRequestApprovalWeb.Controllers
{
	public class RedirectAcceptController : Controller
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		// GET: RedirectAccept
		public ActionResult Index()
		{
			logger.Info("Redirected back from SP.");
			if (Request.QueryString["code"] != null)
			{
				logger.Trace("Trying to get access token with code.");
				string accessToken = TokenHelper.GetAccessToken(Request.QueryString["code"], "00000003-0000-0ff1-ce00-000000000000", ConfigurationManager.AppSettings["endpoint"], TokenHelper.GetRealmFromTargetUrl(new Uri(ConfigurationManager.AppSettings["SharepointUrl"])), new Uri(Request.Url.GetLeftPart(UriPartial.Path))).AccessToken;
				Response.SetCookie(new HttpCookie("FinalAccessToken", accessToken));
				logger.Info("Access token obtained.");
				if (Request.Cookies["redirect"] != null)
				{
					string redirect = Request.Cookies["redirect"].Value;
					logger.Debug("Redirecting back to " + redirect);
					Request.DeleteCookie("redirect", Response);
					return Redirect(redirect);
				}
			}
			else
			{
				logger.Error("Code for access token was not included in query string");
				string error = "";
				foreach (string s in Request.QueryString.AllKeys)
				{
					
					error += s + ": " + Request.QueryString[s] + "<br>";
				}
				error = error.TrimEnd("<br>".ToCharArray());
				return View(model: error);
			}

			return View();
		}
	}
}