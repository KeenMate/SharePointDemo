﻿using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using StockRequestApprovalWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public static class StockRequestApproveDataMapper
	{
		public static StockRequestApproveData MapStockRequestModel(ClientContext clientContext, ListItem item)
		{
			StockRequestApproveData toReturn = new StockRequestApproveData(item)
			{
				DeliveredOn = DateTime.Parse(item["DeliveredOn"].ToString()),
				Items = JsonConvert.DeserializeObject<List<StockRequestItem>>(item["Data"].ToString()),
				ApprovedBy = (item["ApprovedBy"] as FieldUserValue[])?.ToList() ?? new List<FieldUserValue>(),
				RequestID = Guid.Parse(item["RequestID"].ToString()),
				Status = MapStatus(item["Approved"].ToString()),
			};
			toReturn.AllowedApprovers = SharepointListHelper.GetNeededApproves(clientContext, toReturn.Items);
			return toReturn;
		}

		public static Status MapStatus(string en)
		{
			switch (en.ToLower())
			{
				case "waiting for approval": return Status.WaitingForApproval;
				case "approved": return Status.Approved;
				case "rejected": return Status.Rejected;
				default: return Status.UnableToParse;
			}
		}
	}
}