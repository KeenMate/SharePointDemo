using Microsoft.SharePoint.Client;
using NLog;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Helpers
{
	public static class SharepointListHelper
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public static ListItemCollection GetItems(List list, string xmlQuery)
		{
			logger.Trace("GetItems called");
			CamlQuery camlQuery = new CamlQuery();
			camlQuery.ViewXml = xmlQuery;
			ListItemCollection collListItem = list.GetItems(camlQuery);
			return collListItem;
		}

		public static StockRequestApproveData ParseStockRequestList(ClientContext clientContext, string guid)
		{
			logger.Trace("ParseStockRequestList called.");
			List oList = clientContext.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["StockRequestListName"]);
			ListItemCollection collListItem = GetItems(oList, string.Format(ConfigurationHelper.GetCamlQuery("CompareText"), ConfigurationManager.AppSettings["RequestIDFieldName"], guid));
			clientContext.Load(collListItem);
			if (!TestConnection(clientContext)) throw new Exception("Not Authenticated");
			if (collListItem.Count == 0)
				throw new Exception("There is no item with response id " + guid);
			if (collListItem.Count > 1)
				throw new Exception("There were found more items! Response id must be unique.");

			return StockRequestApproveDataMapper.MapStockRequestModel(clientContext, collListItem.First());
		}

		public static void UpdateItem(ListItem item, string index, object value)
		{
			logger.Trace("UpdateItem called.");
			item[index] = value;
			item.Update();
		}

		public static void AreItemsAvaiable(ClientContext clientContext, List<StockRequestItem> list, ListItemCollection stockItemsList, ListItemCollection stockList)
		{
			logger.Trace("AreItemsAvaiable called.");
			foreach (var item in list)
			{
				var citem = stockList.Where(x => (int)x["ID"] == item.StockID).FirstOrDefault();
				if (citem == null)
					throw new Exception($"You can not approve this request. {item.Title} is not in stock.");
				if (int.Parse(citem[ConfigurationManager.AppSettings["AmountFieldName"]].ToString()) - item.Amount < 0)
					throw new Exception($"You can not approve this request. {item.Amount - int.Parse(citem[ConfigurationManager.AppSettings["AmountFieldName"]].ToString())} of {item.Title} is missing.");
			}
		}
		public static void WithdrawItems(ClientContext clientContext, List<StockRequestItem> list, ListItemCollection coll)
		{
			logger.Trace("WithdrawItems called.");
			foreach (var item in coll)
			{
				var citem = list.Where(x => x.StockID == (int)item["ID"]).First();
				double pricePerUnit = citem.TotalPrice / citem.Amount;
				if (int.Parse(item[ConfigurationManager.AppSettings["AmountFieldName"]].ToString()) - citem.Amount == 0)
				{
					item.DeleteObject();
				}
				else
				{
					UpdateItem(item, ConfigurationManager.AppSettings["AmountFieldName"], (int.Parse(item[ConfigurationManager.AppSettings["AmountFieldName"]].ToString()) - citem.Amount).ToString());
					UpdateItem(item, ConfigurationManager.AppSettings["PriceFieldName"], (int.Parse(item[ConfigurationManager.AppSettings["PriceFieldName"]].ToString()) - (citem.Amount * pricePerUnit)).ToString());
				}
			}
			clientContext.ExecuteQuery();
		}

		public static ListItemCollection GetListItems(ClientContext clientContext, string title, CamlQuery query = null)
		{
			logger.Trace("GetListItems called.");
			List list = clientContext.Web.Lists.GetByTitle(title);
			ListItemCollection coll = list.GetItems(query ?? new CamlQuery());
			clientContext.Load(coll);
			return coll;
		}

		public static ListItemCollection GetRequiredItems(ClientContext clientContext, List<StockRequestItem> list, ListItemCollection stockItemsList)
		{
			logger.Trace("GetRequiredItems called.");
			CamlQuery query = new CamlQuery();
			query.ViewXml = "<View><Query><Where><In><FieldRef Name=\"Item\" LookupId=\"TRUE\"/><Values>";
			foreach (var item in list)
			{
				var citem = stockItemsList.Where(x => (int)x["ID"] == item.StockItemID).FirstOrDefault();
				if (citem == null)
					throw new Exception($"Item {item.Title} was not found in list of avaiable items.");
				query.ViewXml += $"<Value Type=\"Lookup\">{item.StockItemID}</Value>";
			}
			query.ViewXml += "</Values></In></Where></Query></View>";
			if (list.Count == 1) query.ViewXml = query.ViewXml.TrimQuery();

			ListItemCollection stockList = GetListItems(clientContext, ConfigurationManager.AppSettings["StockListName"], query);
			clientContext.Load(stockList);
			clientContext.ExecuteQuery();

			foreach (var item in stockList)
			{
				list.First(x => x.StockItemID == ((FieldLookupValue)item[ConfigurationManager.AppSettings["ItemFieldName"]]).LookupId).StockID = int.Parse(item["ID"].ToString());
			}


			/*
			query = new CamlQuery();

			query.ViewXml = $"<View><Query><Where><In><FieldRef Name=\"Item\"/><Values>";
			foreach (var item in list)
			{
				query.ViewXml += $"<Value Type=\"Lookup\">{item.Title}</Value>";
			}
			query.ViewXml += "</Values></In></Where></Query></View>";
			if (list.Count == 1) query.ViewXml = query.ViewXml.Replace("<In>", "<Eq>").Replace("</In>", "</Eq>").Replace("<Values>", "").Replace("</Values>", "");
			return GetListItems(clientContext, ConfigurationManager.AppSettings["StockListName"], query);
			*/
			return stockList;
		}

		public static string TrimQuery(this string str)
		{
			return str.Replace("<In>", "<Eq>")
				.Replace("</In>", "</Eq>")
				.Replace("<Values>", "")
				.Replace("</Values>", "");
		}
		public static List<FieldUserValue> GetNeededApproves(ClientContext clientContext, List<StockRequestItem> list)
		{
			logger.Trace("GetNeededApprovers called.");
			List<FieldUserValue> neededApproves = new List<FieldUserValue>();
			List<string> usedMaterials = new List<string>();
			CamlQuery query = new CamlQuery();

			query.ViewXml = $"<View><Query><Where><In><FieldRef Name=\"Title\"/><Values>";
			foreach (StockRequestItem item in list)
			{
				if (!usedMaterials.Contains(item.MaterialType))
				{
					usedMaterials.Add(item.MaterialType);
					query.ViewXml += $"<Value Type=\"Text\">{item.MaterialType}</Value>";
				}
			}
			query.ViewXml += "</Values></In></Where></Query></View>";

			if (list.Count == 1) query.ViewXml = query.ViewXml.TrimQuery();
			ListItemCollection coll = GetListItems(clientContext, ConfigurationManager.AppSettings["ConfigurationListName"], query);
			clientContext.ExecuteQuery();

			foreach (ListItem item in coll)
			{
				if (!neededApproves.Any(x => x.LookupId == ((FieldUserValue)item[ConfigurationManager.AppSettings["ValueFieldName"]]).LookupId))
					neededApproves.Add((FieldUserValue)item[ConfigurationManager.AppSettings["ValueFieldName"]]);
			}

			return neededApproves;
		}

		public static bool TestConnection(ClientContext clientContext)
		{
			try
			{
				clientContext.ExecuteQuery();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("401"))
				{
					return false;
				}
				else
				{
					logger.Error(ex, "Unexpected exception in TestConnection.");
					throw;
				}
			}
			return true;
		}

		public static void UploadFile(ClientContext clientContext, string path, string fileNameOnServer = null)
		{
			Folder f = clientContext.Web.Lists.GetByTitle("Stock Request - Archive").RootFolder;
			clientContext.Load(f);
			clientContext.ExecuteQuery();
			using (FileStream fs = new FileStream(path, FileMode.Open))
			{
				FileCreationInformation creationInformation = new FileCreationInformation();
				creationInformation.ContentStream = fs;
				creationInformation.Url = Path.Combine(f.ServerRelativeUrl, Path.GetFileName(fileNameOnServer ?? path));
				f.Files.Add(creationInformation);
				f.Update();
				clientContext.ExecuteQuery();
			}
		}

	}
}