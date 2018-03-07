using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class StockRequestApproveData
	{
		public List<StockRequestItem> Items { get; set; }
		public Status Status { get; set; }
		public List<FieldUserValue> ApprovedBy { get; set; }
		public List<FieldUserValue> AllowedApprovers { get; set; }
		public Guid RequestID { get; set; }
		public DateTime DeliveredOn { get; set; }
		public ListItem OriginalItem { get; private set; }

		public StockRequestApproveData(ListItem listItem)
		{
			OriginalItem = listItem;
		}
		public void UpdateItem(string index, object value)
		{
			OriginalItem[index] = value;
			OriginalItem.Update();
		}
	}


	public enum Status
	{
		WaitingForApproval,
		Approved,
		Rejected,
		UnableToParse
	}
}