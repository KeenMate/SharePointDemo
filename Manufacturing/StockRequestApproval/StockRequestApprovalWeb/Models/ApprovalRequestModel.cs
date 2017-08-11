using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class ApprovalRequestModel
	{
		public string ApproverName { get; set; }
		public List<StockRequestItem> Items { get; set; } = new List<StockRequestItem>();
		public string RequesterName { get; set; }
		public string RequesterEmail { get; set; }
		public Guid ItemGuid { get; set; }
	}
}