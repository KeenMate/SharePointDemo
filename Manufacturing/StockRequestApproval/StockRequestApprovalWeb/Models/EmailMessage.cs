using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class EmailMessage
	{
		public string To { get; set; }
		public List<string> Cc { get; private set; }
		public List<string> Bcc { get; private set; }
		public string From { get; set; }
		public string DisplayName { get; set; }
		public string Subject { get; set; }
		public bool SendHtmlOnly { get; set; } = false;
		public bool SendPlainOnly { get; set; } = false;
		public string PlainMessage { get; set; } = "";
		public string HtmlMessage { get; set; } = "";
		public List<string> Attachments { get; private set; }

		public EmailMessage()
		{
			Cc = new List<string>();
			Bcc = new List<string>();
			Attachments = new List<string>();
		}
	}
}