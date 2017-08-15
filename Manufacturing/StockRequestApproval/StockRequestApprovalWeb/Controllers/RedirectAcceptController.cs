using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockRequestApprovalWeb.Helpers;

namespace StockRequestApprovalWeb.Controllers
{
	public class RedirectAcceptController : Controller
	{
		// GET: RedirectAccept
		public ActionResult Index()
		{
			if (Request.QueryString["code"] != null)
			{
				string accessToken = TokenHelper.GetAccessToken(Request.QueryString["code"], "00000003-0000-0ff1-ce00-000000000000", ConfigurationManager.AppSettings["endpoint"], TokenHelper.GetRealmFromTargetUrl(new Uri(ConfigurationManager.AppSettings["SharepointUrl"])), new Uri(Request.Url.GetLeftPart(UriPartial.Path))).AccessToken;
				Response.SetCookie(new HttpCookie("FinalAccessToken", accessToken));
				if (Request.Cookies["redirect"] != null)
				{
					string redirect = Request.Cookies["redirect"].Value;
					Request.DeleteCookie("redirect", Response);
					return Redirect(redirect);
				}
			}

			return View();
		}
	}
}