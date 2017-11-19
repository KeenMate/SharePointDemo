using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class DevExpressReportModel
	{
		public string FullName { get; set; }
		public DateTime On { get; set; } = DateTime.Now;
		public int TotalStockItems { get; set; }
		public long TotalCost { get; set; }
		public List<StockRequestItem> Items { get; set; } = new List<StockRequestItem>();
	}
}