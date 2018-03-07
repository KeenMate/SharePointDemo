using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class DevExpressConfirmationModel
	{
		public List<StockRequestItem> Items { get; set; }
		public Guid ResponseID { get; set; }
		public string CreatedBy { get; set; }
		public string Status { get; set; }
		public List<FieldUserValue> ApprovedBy { get; set; }
		public DateTime DeliveredOn { get; set; }
		public string Created { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime Generated { get; } = DateTime.Now;
	}
}