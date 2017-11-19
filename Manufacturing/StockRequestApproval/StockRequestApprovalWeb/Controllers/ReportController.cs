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

namespace StockRequestApprovalWeb.Controllers
{
	public class ReportController : Controller
	{
		// GET: Report
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult GenerateReport()
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
						Response.SetCookie(new HttpCookie("redirect", "Report/GenerateReport"));
						return RedirectToAction("Authenticate", "Authenticate");
					}
					else throw;
				}
				CSOMOperation op = new CSOMOperation(clientContext);
				ListItemCollection stock = op.LoadList("Stock Items").LoadList("Stock").GetItems();
				ListItemCollection stockItems = op.SelectList("Stock Items").GetItems();
				DevExpressReportModel model = new DevExpressReportModel();
				List<DevExpressReportModel> lst = new List<DevExpressReportModel>();
				List<StockRequestItem> items = new List<StockRequestItem>();
				model.FullName = op.Context.Web.CurrentUser.Title;				
				foreach (ListItem item in stock)
				{
					StockRequestItem i = new StockRequestItem();
					i.Amount = int.Parse(item["Amount"].ToString());
					i.MaterialType = stockItems.FirstOrDefault(x => x["Title"].ToString() == ((FieldLookupValue)item["Item"]).LookupValue)["MaterialType"].ToString();
					i.Title = ((FieldLookupValue)item["Item"]).LookupValue;
					i.TotalPrice = int.Parse(item["Price"].ToString());
					items.Add(i);
				}
				model.Items = items;
				model.TotalStockItems = model.Items.Count;
				model.TotalCost = items.Sum(x => x.TotalPrice);
				lst.Add(model);
				XtraReport1 rep = new XtraReport1();
				rep.DataSource = lst;
				rep.ExportToPdf(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\test.pdf");

				return View();
			}
			else
			{
				Response.SetCookie(new HttpCookie("redirect", "Report/GenerateReport"));
				return RedirectToAction("Authenticate", "Authenticate");
			}
		}
	}
}