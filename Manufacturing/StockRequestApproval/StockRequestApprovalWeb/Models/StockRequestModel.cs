using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class StockRequestModel
	{
		public List<StockRequestItem> Items { get; set; }
		public DateTime DeliveredOn { get; set; }
		public string UserName { get; set; }
		public Guid ItemGuid { get; set; }
	}
}