using Microsoft.SharePoint.Client;
using NLog;
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
		private static Logger logger = LogManager.GetCurrentClassLogger();
		// GET: Authenticate
		public void Index()
		{
			logger.Info("Authenticating user");
			string url = TokenHelper.GetAuthorizationUrl(ConfigurationManager.AppSettings["SharepointUrl"], "Web.Manage", ConfigurationManager.AppSettings["RedirectUrl"]);
			logger.Info("Redirecting to " + url);
			Response.Redirect(url);
		}
	}
}