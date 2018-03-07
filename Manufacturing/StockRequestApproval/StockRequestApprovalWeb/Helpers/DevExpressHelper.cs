using Microsoft.SharePoint.Client;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Helpers
{
	public class DevExpressHelper
	{
		public DevExpressConfirmationModel ParseModel(StockRequestApproveData data)
		{
			return new DevExpressConfirmationModel()
			{
				ApprovedBy = data.ApprovedBy,
				Created = data.OriginalItem["Created"].ToString(),
				CreatedBy = ((FieldUserValue)data.OriginalItem["Author"]).LookupValue,
				DeliveredOn = data.DeliveredOn,
				Items = data.Items,
				ModifiedBy = ((FieldUserValue)data.OriginalItem["Editor"]).LookupValue,
				ResponseID = data.RequestID,
				Status = data.Status.ToUserFriendlyString()
			};
		}
	}
}