using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
			if (Request.Cookies["FinalAccessToken"] != null)
			{
				ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value);
				if (clientContext != null)
				{
					clientContext.Load(clientContext.Web);
					clientContext.Load(clientContext.Web.CurrentUser);
					clientContext.ExecuteQuery();
				}
			}
			else
			{
				Response.SetCookie(new HttpCookie("redirect", "Response/Approve?guid=" + Request.QueryString["guid"]));
				return RedirectToAction("Authenticate", "Authenticate");
			}
			return RedirectToAction("Index");
		}

		public ActionResult Reject()
		{
			if (Request.QueryString["guid"] != null)
			{
				if (Request.Cookies["FinalAccessToken"] != null)
				{
					ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(ConfigurationManager.AppSettings["SharepointUrl"], Request.Cookies["FinalAccessToken"].Value);
					if (clientContext != null)
					{
						clientContext.Load(clientContext.Web);
						clientContext.Load(clientContext.Web.CurrentUser);
						List oList = clientContext.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["StockRequestListName"]);

						CamlQuery camlQuery = new CamlQuery();
						camlQuery.ViewXml = $@"<View><Query><Where>               
                                    <Eq>                   
                                        <FieldRef Name='{ConfigurationManager.AppSettings["RequestIDFieldName"]}' />                   
                                        <Value Type='Text'>{Request.QueryString["guid"]}</Value>              
                                    </Eq>            
                                  </Where></View></Query>"; ;
						ListItemCollection collListItem = oList.GetItems(camlQuery);
						clientContext.Load(collListItem);
						clientContext.ExecuteQuery();
						if (collListItem.Count == 0)
							throw new Exception("There is no item with response id " + Request.QueryString["guid"]);
						if (collListItem.Count > 1)
							throw new Exception("There were found more items! Response id must be unique.");
						ListItem item = collListItem.First();
						clientContext.Load(item);
						clientContext.ExecuteQuery();
						item["Approved"] = "Rejected";
						item.Update();
						clientContext.ExecuteQuery();
					}
				}
				else
				{
					Response.SetCookie(new HttpCookie("redirect", "Response/Reject?guid=" + Request.QueryString["guid"]));
					return RedirectToAction("Authenticate", "Authenticate");
				}
			}
			return RedirectToAction("Index");
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
}