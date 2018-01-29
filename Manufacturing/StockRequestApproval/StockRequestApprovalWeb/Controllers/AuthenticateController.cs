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
		public void Index()
		{
			string url = TokenHelper.GetAuthorizationUrl(ConfigurationManager.AppSettings["SharepointUrl"], "Web.Manage", ConfigurationManager.AppSettings["RedirectUrl"]);
			Response.Redirect(url);
		}
	}
}