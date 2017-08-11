using SendGrid;
using SendGrid.Helpers.Mail;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Services
{
	public class MailerService
	{

		private string apiKey;

		public MailerService(string apiKey)
		{
			this.apiKey = apiKey;
		}
		async public System.Threading.Tasks.Task SendMail(EmailMessage message)
		{
			var client = new SendGridClient(apiKey);
			{
				EmailAddress to = new EmailAddress(message.To);

				if (message.SendHtmlOnly && message.SendPlainOnly)
					throw new InvalidOperationException("Invalid configuration.");
				if (message.SendPlainOnly)
					message.HtmlMessage = "";
				if (message.SendHtmlOnly)
					message.PlainMessage = "";

				var from = new EmailAddress(message.From, message.DisplayName);
				var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, message.PlainMessage, message.HtmlMessage);

				foreach (string s in message.Cc)
				{
					msg.Personalizations.First().Ccs.Add(new EmailAddress(s));
				}
				foreach (string s in message.Bcc)
				{
					msg.Personalizations.First().Bccs.Add(new EmailAddress(s));
				}

				var response = await client.SendEmailAsync(msg);
				var c = response.StatusCode;
			}
		}

		public void SendMail(List<EmailMessage> messages)
		{
			foreach (EmailMessage msg in messages)
			{
				SendMail(msg);
			}
		}
	}
}