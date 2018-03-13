using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using StockRequestApprovalWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
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
				DeliveredOn = DateTime.Parse(item[ConfigurationManager.AppSettings["FieldName:DeliveredOn"]].ToString()),
				Items = JsonConvert.DeserializeObject<List<StockRequestItem>>(item[ConfigurationManager.AppSettings["FieldName:Data"]].ToString()),
				ApprovedBy = (item[ConfigurationManager.AppSettings["FieldName:ApprovedBy"]] as FieldUserValue[])?.ToList() ?? new List<FieldUserValue>(),
				Status = MapStatus(item[ConfigurationManager.AppSettings["FieldName:Status"]].ToString()),
			};
			if (item[ConfigurationManager.AppSettings["FieldName:RequestID"]] != null)
			{
				try
				{
					toReturn.RequestID = Guid.Parse(item[ConfigurationManager.AppSettings["FieldName:RequestID"]].ToString());
				}
				catch
				{
					toReturn.RequestID = Guid.Empty;
				}
			}
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

		public static string ToUserFriendlyString(this Status s)
		{
			switch (s)
			{
				case Status.WaitingForApproval: return "Waiting for approval";
				case Status.Approved: return "Approved";
				case Status.Rejected: return "Rejected";
				default: return "Unable to parse";
			}
		}
	}
}