using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using RazorEngine.Templating;
using SendGrid;
using SendGrid.Helpers.Mail;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockRequestApprovalWeb.Controllers
{
	public class MailController : Controller
	{
		// GET: Mail
		public ActionResult Index()
		{
			return View();
		}
	}
}