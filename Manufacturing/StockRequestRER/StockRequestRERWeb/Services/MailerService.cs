using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockRequestRERWeb.Models;
using NLog;

namespace StockRequestRERWeb.Services
{
	public class MailerService
	{

		private string apiKey;
		private static Logger logger = LogManager.GetCurrentClassLogger();

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

				logger.Info("Sending final email message to " + to);
				var response = await client.SendEmailAsync(msg);
				var c = response.StatusCode;
				logger.Debug("Email response: " + c);
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