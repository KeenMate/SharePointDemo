using FluentlySharepoint;
using FluentlySharepoint.Extensions;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using NLog;
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
		private static Logger logger = LogManager.GetCurrentClassLogger();
		// GET: RequestsOverview
		public ActionResult Index()
		{
			return View();
		}

		string currentUser = "";

		[HttpPost]
		public ActionResult IsAuthenticated()
		{
			logger.Trace("IsAuthenticated called.");
			if (Request.Cookies["FinalAccessToken"] != null)
			{
				logger.Trace("Getting clientContext with access token.");
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
						logger.Info("SP returned 401. Setting cookie to redirect to RequestsOverview and returning status code Unauthorized");
						Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else
					{
						logger.Error(ex, "Unexpected exception.");
						throw;
					}
				}
				currentUser = clientContext.Web.CurrentUser.Title;
				logger.Info("Current user is " + currentUser + ". Returning status code Accepted.");
				return new HttpStatusCodeResult(HttpStatusCode.Accepted);
			}
			else
			{
				logger.Info("FinalAccessToken is null. Returning status code Unauthorized.");
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
		}

		[HttpPost]
		public ActionResult GetData(int count = 15, string position = "", bool onlyUnresolved = false)
		{
			logger.Trace($"GetData called with parameters count: {count}, position: {position}, onlyUnresolved: {onlyUnresolved}");
			List<StockRequestApproveData> model = new List<StockRequestApproveData>();
			List<StockRequestApproveDataJSON> jlist = new List<StockRequestApproveDataJSON>();

			if (Request.Cookies["FinalAccessToken"] != null)
			{
				logger.Trace("Getting clientContext with access token");
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
						logger.Info("SP returned 401, setting cookie to redirect to RequestsOverview and returning status code Unauthorized");
						Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else
					{
						logger.Error(ex, "Unexpected exception occured.");
						throw;
					}
				}
				logger.Debug("Loading stock request list.");
				CSOMOperation op = new CSOMOperation(clientContext);
				op.LoadList("Stock Request");

				CamlQuery query = new CamlQuery();

				query.ViewXml =
					onlyUnresolved ?
					$"<View><Query><Where><Eq><FieldRef Name='" + ConfigurationManager.AppSettings["StatusFieldName"] + "'/><Value Type='Text'>Waiting for approval</Value></Eq></Where><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy></Query><RowLimit>{count}</RowLimit></View>" :
					$"<View><Query><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy></Query><RowLimit>{count}</RowLimit></View>";
				logger.Debug("Caml query set to: " + query.ViewXml);
				if (position != "")
				{
					ListItemCollectionPosition pos = new ListItemCollectionPosition();
					pos.PagingInfo = position.Replace("_AMP_", "&").Replace(" ", "%20").Replace(":", "%3a");
					query.ListItemCollectionPosition = pos;
					logger.Trace("Setting query position to " + pos.PagingInfo);
				}


				ListItemCollection col = op.GetItems(query);
				position = col.ListItemCollectionPosition?.PagingInfo;
				logger.Debug("Data fetched. Converting to JSON.");
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
				logger.Info("FinalAccessToken is null, setting cookie to redirect to RequestsOverview and returning status code Unauthorized");
				Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
			logger.Trace("Returning JSON.");
			return new JsonResult() { Data = new { Data = jlist, Pos = position } };
		}

		[HttpPost]
		public ActionResult GetItemCount(bool onlyUnresolved = false)
		{
			logger.Trace($"GetItemCount called with parameter onlyUnresolved: {onlyUnresolved}");
			int i;
			if (Request.Cookies["FinalAccessToken"] != null)
			{
				logger.Trace("Getting clientContext with access token");
				ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value);
				try
				{
					logger.Debug("Loading stock request list.");
					CSOMOperation op = new CSOMOperation(clientContext);
					op.LoadList("Stock Request");

					CamlQuery query = new CamlQuery();
					if (onlyUnresolved) query.ViewXml = "<View><Query><Where><Eq><FieldRef Name='" + ConfigurationManager.AppSettings["StatusFieldName"] + "'/><Value Type='Text'>Waiting for approval</Value></Eq></Where></Query></View>";

					ListItemCollection col = op.GetItems(query);
					i = col.Count;
					logger.Debug("Counted items: " + i);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("401"))
					{
						logger.Info("SP returned 401, setting cookie to redirect to RequestsOverview and returning status code Unauthorized");
						Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else
					{
						logger.Error(ex, "Unexpected exception occured.");
						throw;
					}
				}
				return new JsonResult() { Data = i };
			}
			else
			{
				logger.Warn("FinalAccessToken is null");
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
		}
		[HttpPost]
		public ActionResult GetCurrentUser()
		{
			logger.Trace("GetCurrentUser called.");
			if (currentUser != "")
				return new ContentResult() { Content = currentUser };

			if (Request.Cookies["FinalAccessToken"] != null)
			{
				logger.Trace("Getting clientContext with access token");
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
						logger.Info("SP returned 401, setting cookie to redirect to RequestsOverview and returning status code Unauthorized");
						Response.SetCookie(new HttpCookie("redirect", "RequestsOverview"));
						return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
					}
					else
					{
						logger.Error(ex, "Unexpected exception occured.");
						throw;
					}
				}
				return new ContentResult() { Content = clientContext.Web.CurrentUser.Title };
			}
			else
			{
				logger.Warn("FinalAccessToken is null");
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
		}
	}
}