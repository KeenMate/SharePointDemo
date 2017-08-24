using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using StockRequestApprovalWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockRequestApprovalWeb.Models;

namespace StockRequestApprovalWeb.Controllers
{
	public class ResponseController : Controller
	{
		// GET: Response
		public ActionResult Index()
		{
			//var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
			return RedirectToAction("About", "Home"/*, new { SPHostUrl = spContext.SPHostUrl.ToString() }*/);
		}

		public ActionResult Approve()
		{
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					return ProcessRequest("Approve");
				}
				else
				{
					Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Authenticate", "Authenticate");
				}
			}
			return View("Close");
		}

		public ActionResult Reject()
		{
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					return ProcessRequest("Reject");
				}
				else
				{
					Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Authenticate", "Authenticate");
				}
			}
			return View("Close");
		}

		private ActionResult ProcessRequest(string action)
		{
			using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value))
			{
				if (clientContext != null)
				{
					clientContext.Load(clientContext.Web);
					clientContext.Load(clientContext.Web.CurrentUser);
					StockRequestApproveData data;
					try
					{
						data = SharepointListHelper.ParseStockRequestList(clientContext, Request.QueryString["guid"]);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Not Authenticated")
						{
							if (action == "Reject")
								Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
							else
								Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
							return RedirectToAction("Authenticate", "Authenticate");
						}
						else throw;
					}

					if (action == "Reject")
					{
						if (data.Status == Status.Rejected)
							throw new Exception("This item was already rejected.");
						if (data.Status == Status.Approved)
							throw new Exception("This item was approved. You can not reject it anymore.");
						if (!data.AllowedApprovers.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
							throw new Exception("You are not allowed to reject this request.");
						data.UpdateItem("Approved", "Rejected");
					}
					else
					{
						processApprove(clientContext, data);
					}
					clientContext.ExecuteQuery();
				}
			}
			return View("Close");
		}

		private void processApprove(ClientContext clientContext, StockRequestApproveData data)
		{
			if (data.Status == Status.Rejected)
				throw new Exception("This item was rejected. You can not approve it anymore.");
			if (data.Status == Status.Approved)
				throw new Exception("This item was already approved. You can not approve it anymore.");
			if (data.DeliveredOn < DateTime.Now)
				throw new Exception($"Too late to approve this request. The request should be delivered on {data.DeliveredOn.ToString()}");

			FieldUserValue me = new FieldUserValue();
			me.LookupId = clientContext.Web.CurrentUser.Id;

			if (!data.AllowedApprovers.Any(x => x.LookupId == me.LookupId))
				throw new Exception("You are not allowed to approve this request.");

			if (data.ApprovedBy.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
				throw new Exception("You already approved this item.");

			data.ApprovedBy.Add(me);

			StockRequestModel model = StockRequestMapper.MapStockRequestModel(data.OriginalItem);

			if (data.ApprovedBy.Count == data.AllowedApprovers.Count)
			{
				if (!data.ApprovedBy.Select(x => x.LookupId).Except(data.AllowedApprovers.Select(x => x.LookupId)).Any())
				{
					ListItemCollection coll = SharepointListHelper.GetRequiredItems(clientContext, model.Items);
					clientContext.ExecuteQuery();

					SharepointListHelper.AreItemsAvaiable(clientContext, model.Items, coll);

					SharepointListHelper.WithdrawItems(clientContext, model.Items, coll);
					data.UpdateItem("Approved", "Approved");
				}
				else
				{
					throw new Exception("An unautorized person approved this request. Please contact your administrator.");
				}
			}

			data.UpdateItem("ApprovedBy", data.ApprovedBy.ToArray());
			clientContext.ExecuteQuery();
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			Exception ex = filterContext.Exception;
			filterContext.ExceptionHandled = true;

			var model = new HandleErrorInfo(filterContext.Exception, "Response", "Reject/Approve");

			filterContext.Result = new ViewResult()
			{
				ViewName = "Error",
				ViewData = new ViewDataDictionary(model)
			};

		}
	}
}/*if (item["Approved"].ToString() == "Rejected")
							throw new Exception("This item was rejected. You can not approve it anymore.");
						FieldUserValue[] users = item["ApprovedBy"] as FieldUserValue[];
						FieldUserValue me = new FieldUserValue();
						me.LookupId = clientContext.Web.CurrentUser.Id;
						List<FieldUserValue> lusers = new List<FieldUserValue>();
						if (users != null)
							lusers = users.ToList();
						if (lusers.Any(x => x.LookupId == clientContext.Web.CurrentUser.Id))
							throw new Exception("You already approved this item.");
						lusers.Add(me);
						
						List list = clientContext.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["ConfigurationListName"]);
						ListItemCollection confListItem = list.GetItems(new CamlQuery());
						clientContext.Load(confListItem);
						clientContext.ExecuteQuery();

						StockRequestModel model = StockRequestMapper.MapStockRequestModel(item);
						List<string> neededApproves = new List<string>();

						foreach (StockRequestItem sitem in model.Items)
						{
							if (!neededApproves.Contains(sitem.MaterialType))
								neededApproves.Add(sitem.MaterialType);
						}
						int cnt = 0;
						foreach (string s in neededApproves)
						{
							foreach (ListItem citem in confListItem)
							{
								if (citem["Title"].ToString() == s)
								{
									if (lusers.Any(x => x.LookupId == ((FieldUserValue)citem["Value"]).LookupId))
									{
										cnt++;
										break;
									}
								}
							}
						}
						List stockList = clientContext.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["StockListName"]);
						CamlQuery query = new CamlQuery();
						query.ViewXml = $@"<View><Query><Where><Or>";
						foreach (StockRequestItem stockitem in model.Items)
						{
							query.ViewXml += $"<Eq><FieldRef Name=\"Item\" LookupId=\"TRUE\"/><Value Type=\"Lookup\">{stockitem.EditedID}</Value></Eq>";
						}
						query.ViewXml += "</Or></Where></Query></View>";
						if (model.Items.Count == 1) query.ViewXml = query.ViewXml.Replace("<Or>", "").Replace("</Or>", "");
						ListItemCollection stockListItemCollection = stockList.GetItems(query);
						clientContext.Load(stockListItemCollection);
						clientContext.ExecuteQuery();
						if (stockListItemCollection.Count != model.Items.Count)
						{
							throw new Exception("Some items no longer exist.");
						}
						foreach (ListItem stockListItem in stockListItemCollection)
						{
							stockListItem["Amount"] = int.Parse(stockListItem["Amount"].ToString()) - model.Items.Where(x => x.Title == stockListItem["Item"].ToString()).First().Amount;
						}

						SharepointListHelper.UpdateItem(ref item, "ApprovedBy", lusers.ToArray());
						if (neededApproves.Count == cnt)
							SharepointListHelper.UpdateItem(ref item, "Approved", "Approved");
							*/
