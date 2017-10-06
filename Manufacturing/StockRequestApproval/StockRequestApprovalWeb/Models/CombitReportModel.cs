using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class CombitReportModel
	{
		public string FullName { get; set; }
		public DateTime On { get; set; } = DateTime.Now;
		public int TotalStockItems { get; set; }
		public long TotalCost { get; set; }
		public Dictionary<string, List<StockRequestItem>> Items { get; set; } = new Dictionary<string, List<StockRequestItem>>();
	}
}