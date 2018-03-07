using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class ConfirmationEmailModel
	{
		public List<StockRequestItem> Items { get; set; }
		public Guid ResponseID { get; set; }
		public string CreatedBy { get; set; }
		public string Status { get; set; }
		public DateTime DeliveredOn { get; set; }
		public string Created { get; set; }
		public string Rejector { get; set; }
		public string RejectorEmail { get; set; }
	}
}