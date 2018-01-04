using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class StockRequestApproveDataJSON
	{
		public int ID { get; set; }
		public string Created { get; set; }
		public string CreatedBy { get; set; }
		public List<StockRequestItem> Items { get; set; }
		public string Status { get; set; }
		public List<string> ApprovedBy { get; set; }
		public List<string> AllowedApprovers { get; set; }
		public Guid RequestID { get; set; }
		public string DeliveredOn { get; set; }
		public object VueData { get; set; } = new object();
	}
}