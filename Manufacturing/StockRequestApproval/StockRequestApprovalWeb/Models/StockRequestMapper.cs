﻿using Microsoft.SharePoint.Client.EventReceivers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
				DeliveredOn = DateTime.Parse(properties.ItemEventProperties.AfterProperties["DeliveredOn"].ToString()),
				UserName = userName,
				Items = JsonConvert.DeserializeObject<List<StockRequestItem>>(properties.ItemEventProperties.AfterProperties["Data"].ToString())
			};
		}
	}
}