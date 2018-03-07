using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentlySharepoint;
using FluentlySharepoint.Extensions;
using System.Configuration;
using Microsoft.SharePoint.Client;
using StockRequestApprovalWeb.Models;
using StockRequestApprovalWeb.Services;
using System.Net;
using NLog;

namespace StockRequestApprovalWeb.Controllers
{
	public class ReportController : Controller
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		// GET: Report
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult GenerateReport()
		{
			logger.Trace("GenerateReport called.");
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
						logger.Warn("SP returned 401. Setting cookie to Report/GenerateReport and redirecting to authenticate");
						Response.SetCookie(new HttpCookie("redirect", "Report/GenerateReport"));
						return RedirectToAction("Index", "Authenticate");
					}
					else
					{
						logger.Error(ex, "Unexpected Exception.");
						throw;
					}
				}
				logger.Trace("Loading Stock items and stock list");
				CSOMOperation op = new CSOMOperation(clientContext);
				ListItemCollection stock = op.LoadList(ConfigurationManager.AppSettings["StockItemsListName"]).LoadList(ConfigurationManager.AppSettings["StockListName"]).GetItems();
				ListItemCollection stockItems = op.SelectList(ConfigurationManager.AppSettings["StockItemsListName"]).GetItems();
				DevExpressReportModel model = new DevExpressReportModel();
				List<DevExpressReportModel> lst = new List<DevExpressReportModel>();
				List<StockRequestItem> items = new List<StockRequestItem>();
				model.FullName = op.Context.Web.CurrentUser.Title;
				logger.Debug("Fetching data");
				foreach (ListItem item in stock)
				{
					StockRequestItem i = new StockRequestItem();
					i.Amount = int.Parse(item[ConfigurationManager.AppSettings["AmountFieldName"]].ToString());
					i.MaterialType = stockItems.FirstOrDefault(x => x[ConfigurationManager.AppSettings["TitleFieldName"]].ToString() == ((FieldLookupValue)item[ConfigurationManager.AppSettings["ItemFieldName"]]).LookupValue)[ConfigurationManager.AppSettings["MaterialTypeFieldName"]].ToString();
					i.Title = ((FieldLookupValue)item[ConfigurationManager.AppSettings["TitleFieldName"]]).LookupValue;
					i.TotalPrice = int.Parse(item[ConfigurationManager.AppSettings["PriceFieldName"]].ToString());
					items.Add(i);
				}
				model.Items = items;
				model.TotalStockItems = model.Items.Count;
				model.TotalCost = items.Sum(x => x.TotalPrice);
				lst.Add(model);
				logger.Debug("Generating PDF");
				ReportService rep = new ReportService();
				string path = rep.GenerateStockReport(lst, Server.MapPath("~/Temp"));

				return File(path, "application/pdf", Server.UrlEncode(System.IO.Path.GetFileName(path)));
			}
			else
			{
				logger.Warn("FinalAccessToken is null, setting cookie to Report/GenerateReport and redirecting to authenticate");
				Response.SetCookie(new HttpCookie("redirect", "Report/GenerateReport"));
				return RedirectToAction("Index", "Authenticate");
			}
		}
	}
}