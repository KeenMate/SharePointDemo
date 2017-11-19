using FluentlySharepoint;
using FluentlySharepoint.Extensions;
using Microsoft.SharePoint.Client;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestApprovalWeb.Controllers
{
	public class RequestsOverviewController : Controller
	{
		// GET: RequestsOverview
		public ActionResult Index()
		{
			List<StockRequestApproveData> model = new List<StockRequestApproveData>();

			if (Request.Cookies["FinalAccessToken"] != null)
			{
				ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value);
				try
				{
					clientContext.Load(clientContext.Web.CurrentUser);
					clientContext.ExecuteQuery();
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("401"))
					{
						Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
						return RedirectToAction("Authenticate", "Authenticate");
					}
					else throw;
				}
				CSOMOperation op = new CSOMOperation(clientContext);
				op.LoadList("Stock Request");
				
				ListItemCollection col = op.GetItems("<View><Query><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy></Query><RowLimit>15</RowLimit></View>");
				foreach (var item in col)
				{
					model.Add(StockRequestApproveDataMapper.MapStockRequestModel(clientContext, item));
				}
			}
			else
			{
				Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
				return RedirectToAction("Authenticate", "Authenticate");
			}
			return View(model);
		}
	}
}