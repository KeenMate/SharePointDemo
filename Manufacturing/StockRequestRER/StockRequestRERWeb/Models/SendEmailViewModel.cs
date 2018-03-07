using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestRERWeb.Models
{
	public class SendEmailViewModel
	{
		public string To { get; set; }
		public string Model { get; set; } = MailTemplatesKeys.ApprovalRequestForManager;
		public List<string> AvaiableTemplates = new List<string>() { MailTemplatesKeys.ApprovalRequestForManager, MailTemplatesKeys.ConfirmationRequestForUser };
	}
}