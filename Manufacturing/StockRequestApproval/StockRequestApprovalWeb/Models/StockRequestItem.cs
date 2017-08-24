using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class StockRequestItem
	{
		public string Title { get; set; }
		public int Amount { get; set; }
		public int TotalPrice { get; set; }
		public string MaterialType { get; set; }
		public int EditedID { get; set; }
	}
}