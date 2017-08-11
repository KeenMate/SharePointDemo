using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class SendEmailViewModel
	{
		public string To { get; set; }
		public string Model { get; set; } = MailTemplatesKeys.ApprovalRequestForManager;
		public List<string> AvaiableTemplates = new List<string>() { MailTemplatesKeys.ApprovalRequestForManager, MailTemplatesKeys.ConfirmationRequestForUser };
	}
}