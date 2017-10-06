using Newtonsoft.Json;
using StockRequestRERWeb.Models;
using StockRequestRERWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestRERWeb.Controllers
{
	public class TemplateController : Controller
	{
		// GET: Template
		public ActionResult Index()
		{
			return RedirectToAction("Index", "Home");
		}

		public string Template(string modelJSON)
		{
			try
			{
				return new TemplateService()
					.CreateMessage(JsonConvert.DeserializeObject<TemplateModel>(modelJSON));
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}