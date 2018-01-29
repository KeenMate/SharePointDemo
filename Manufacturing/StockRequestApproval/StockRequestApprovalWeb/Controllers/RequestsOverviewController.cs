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
using System.Net;
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
			return View();
		}

		string currentUser = "";

		[HttpPost]
		public ActionResult IsAuthenticated()
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
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else throw;
				}
				currentUser = clientContext.Web.CurrentUser.Title;
				return new HttpStatusCodeResult(HttpStatusCode.Accepted);
			}
			return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
		}

		[HttpPost]
		public ActionResult GetData(int count = 15, string position = "", bool prev = false)
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

						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else throw;
				}
				CSOMOperation op = new CSOMOperation(clientContext);
				op.LoadList("Stock Request");

				CamlQuery query = new CamlQuery();
				query.ViewXml = $"<View><Query><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy></Query><RowLimit>{count}</RowLimit></View>";
				if (position != "")
				{
					ListItemCollectionPosition pos = new ListItemCollectionPosition();
					pos.PagingInfo = (prev ? "PagePrev=True&" : "") + position.Replace("_AMP_", "&").Replace(" ", "%20").Replace(":", "%3a");
					query.ListItemCollectionPosition = pos;
				}


				ListItemCollection col = op.GetItems(query);
				position = col.ListItemCollectionPosition?.PagingInfo;
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
					jmodel.ModifiedBy = ((FieldUserValue)item["Editor"]).LookupValue;
					if (d.RequestID == Guid.Empty) jmodel.Status = "Invalid GUID";
					else jmodel.Status = d.Status.ToUserFriendlyString();
					jlist.Add(jmodel);
				}
			}
			else
			{
				Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}

			return new JsonResult() { Data = new { Data = jlist, Pos = position } };
		}

		[HttpPost]
		public ActionResult GetItemCount()
		{
			int i;
			if (Request.Cookies["FinalAccessToken"] != null)
			{
				ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value);
				try
				{
					CSOMOperation op = new CSOMOperation(clientContext);
					op.LoadList("Stock Request");

					ListItemCollection col = op.GetItems();
					i = col.Count;
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("401"))
					{
						Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else throw;
				}
				return new JsonResult() { Data = i };
			}
			return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
		}
		[HttpPost]
		public ActionResult GetCurrentUser()
		{
			if (currentUser != "")
				return new ContentResult() { Content = currentUser };

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
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else throw;
				}
				return new ContentResult() { Content = clientContext.Web.CurrentUser.Title };
			}
			return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
		}
	}
}