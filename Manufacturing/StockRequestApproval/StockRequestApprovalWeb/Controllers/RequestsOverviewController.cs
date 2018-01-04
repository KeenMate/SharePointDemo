using FluentlySharepoint;
using FluentlySharepoint.Extensions;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StockRequestApprovalWeb.Controllers
{
	public class RequestsOverviewController : Controller
	{
		// GET: RequestsOverview
		public ActionResult Index()
		{
			/*
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
			return View(model);*/
			return View();
		}
		[HttpPost]
		public string GetData()
		{
			List<StockRequestApproveData> model = new List<StockRequestApproveData>();
			List<StockRequestApproveDataJSON> jlist = new List<StockRequestApproveDataJSON>();

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
						return "Authentication required";
					}
					else throw;
				}
				CSOMOperation op = new CSOMOperation(clientContext);
				op.LoadList("Stock Request");

				ListItemCollection col = op.GetItems("<View><Query><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy></Query><RowLimit>15</RowLimit></View>");
				foreach (var item in col)
				{
					StockRequestApproveData d = StockRequestApproveDataMapper.MapStockRequestModel(clientContext, item);
					StockRequestApproveDataJSON jmodel = new StockRequestApproveDataJSON();
					jmodel.ID = int.Parse(item["ID"].ToString());
					jmodel.Created = item["Created"].ToString();
					jmodel.CreatedBy = ((FieldUserValue)item["Author"]).LookupValue;
					jmodel.AllowedApprovers = d.AllowedApprovers.ConvertAll(x => x.LookupValue);
					jmodel.ApprovedBy = d.ApprovedBy.ConvertAll(x => x.LookupValue);
					jmodel.DeliveredOn = d.DeliveredOn.ToString();
					jmodel.Items = d.Items;
					jmodel.RequestID = d.RequestID;
					if (d.RequestID == Guid.Empty) jmodel.Status = "Invalid GUID";
					else jmodel.Status = d.Status.ToUserFriendlyString();
					jlist.Add(jmodel);
				}
			}
			else
			{
				Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
				return "Authentication required";
			}

			string wtf = JsonConvert.SerializeObject(jlist);
			return wtf;
		}
		[HttpPost]
		public string GetCurrentUser()
		{
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
						return "Authentication required";
					}
					else throw;
				}
				System.Threading.Thread.Sleep(1000);
				return clientContext.Web.CurrentUser.Title;
			}
			return "Authentication required";
		}
	}
}