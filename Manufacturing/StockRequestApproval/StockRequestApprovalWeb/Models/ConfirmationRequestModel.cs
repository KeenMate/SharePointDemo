using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class ConfirmationRequestModel
	{
		public string Name { get; set; }
		public List<StockRequestItem> Items { get; set; }
		public string RequesterName { get; set; }
		public string RequesterEmail { get; set; }
	}
}