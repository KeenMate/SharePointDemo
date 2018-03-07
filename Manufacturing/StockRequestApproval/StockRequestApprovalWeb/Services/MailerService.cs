using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockRequestApprovalWeb.Models;
using NLog;

namespace StockRequestApprovalWeb.Services
{
	public class MailerService
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
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
				foreach (string s in message.Attachments)
				{
					var bytes = System.IO.File.ReadAllBytes(s);
					msg.AddAttachment(System.IO.Path.GetFileName(s), Convert.ToBase64String(bytes));
				}
				logger.Info("Sending final email message to " + to);
				var response = await client.SendEmailAsync(msg);
				var c = response.StatusCode;
				logger.Debug("Email sent with status code: " + c);
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