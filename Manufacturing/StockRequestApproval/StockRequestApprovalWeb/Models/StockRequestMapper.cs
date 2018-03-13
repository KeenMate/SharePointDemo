using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public static class StockRequestMapper
	{
		public static StockRequestModel MapStockRequestModel(SPRemoteEventProperties properties, string userName)
		{
			return new StockRequestModel()
			{
				DeliveredOn = DateTime.Parse(properties.ItemEventProperties.AfterProperties[ConfigurationManager.AppSettings["FieldName:DeliveredOn"]].ToString()),
				UserName = userName,
				Items = JsonConvert.DeserializeObject<List<StockRequestItem>>(properties.ItemEventProperties.AfterProperties[ConfigurationManager.AppSettings["FieldName:Data"]].ToString())
			};
		}
		public static StockRequestModel MapStockRequestModel(ListItem item, string userName = null)
		{
			return new StockRequestModel()
			{
				DeliveredOn = DateTime.Parse(item[ConfigurationManager.AppSettings["FieldName:DeliveredOn"]].ToString()),
				UserName = userName,
				Items = JsonConvert.DeserializeObject<List<StockRequestItem>>(item[ConfigurationManager.AppSettings["FieldName:Data"]].ToString())
			};
		}
	}
}