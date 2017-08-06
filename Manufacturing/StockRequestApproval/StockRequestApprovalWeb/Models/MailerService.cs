using RazorEngine;
using RazorEngine.Templating;
using SendGrid;
using SendGrid.Helpers.Mail;
using StockRequestApprovalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Services
{
	public static class MailerService
	{
		public static void SendMail(string template, string toAdress, object modelO = null)
		{
			switch (template)
			{
				case "StockRequest":
					{
						if (modelO == null)
							throw new ArgumentNullException("model", "Model can not be null for this template");
						if (modelO.GetType() != typeof(StockRequestModel))
						{
							throw new ArrayTypeMismatchException("Model must be type StockRequestModel");
						}

						StockRequestModel model = (StockRequestModel)modelO;

						var to = new EmailAddress(toAdress, model.UserName);

						var apiKey = "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";
						var client = new SendGridClient(apiKey);
						var from = new EmailAddress("confirmation@keenmate.com", "Stock Request");
						var subject = "An Item was sended to confirmation";
						var plainTextContent = $"item request";

						var service = Engine.Razor;
						service.AddTemplate("Head", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Head.cshtml")));
						service.AddTemplate("Header", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Header.cshtml")));
						service.AddTemplate("Footer", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Footer.cshtml")));
						service.AddTemplate("StockRequestTempl", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\StockRequest.cshtml")));
						service.Compile("StockRequestTempl", typeof(StockRequestModel));
						service.Compile("Head");
						service.Compile("Header");
						service.Compile("Footer");
						var htmlContent = service.Run("StockRequestTempl", typeof(StockRequestModel), model);

						//var htmlContent = $"You just requested {properties.ItemEventProperties.AfterProperties["Amount"]} of {itemName} for {properties.ItemEventProperties.AfterProperties["TotalPrice"]} €. This will be delivered on {properties.ItemEventProperties.AfterProperties["DeliveredOn"]}";
						var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
						var response = client.SendEmailAsync(msg);
						break;
					}
				case "ConfirmationRequest":
					{
						if (modelO == null)
							throw new ArgumentNullException("model", "Model can not be null for this template");
						if (modelO.GetType() != typeof(ConfirmationRequestModel))
						{
							throw new ArrayTypeMismatchException("Model must be type ConfirmationRequestModel");
						}

						ConfirmationRequestModel model = (ConfirmationRequestModel)modelO;

						var to = new EmailAddress(toAdress, model.Name);

						var apiKey = "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";
						var client = new SendGridClient(apiKey);
						var from = new EmailAddress("confirmation@keenmate.com", "Stock Request");
						var subject = "New request is waiting for confirmtion";
						var plainTextContent = $"item request";

						var service = Engine.Razor;
						service.AddTemplate("Head", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Head.cshtml")));
						service.AddTemplate("Header", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Header.cshtml")));
						service.AddTemplate("Footer", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Footer.cshtml")));
						service.AddTemplate("ConfirmRequestTempl", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\ConfirmationRequest.cshtml")));
						service.Compile("ConfirmRequestTempl", typeof(ConfirmationRequestModel));
						service.Compile("Head");
						service.Compile("Header");
						service.Compile("Footer");
						var htmlContent = service.Run("ConfirmRequestTempl", typeof(ConfirmationRequestModel), model);

						//var htmlContent = $"You just requested {properties.ItemEventProperties.AfterProperties["Amount"]} of {itemName} for {properties.ItemEventProperties.AfterProperties["TotalPrice"]} €. This will be delivered on {properties.ItemEventProperties.AfterProperties["DeliveredOn"]}";
						var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
						var response = client.SendEmailAsync(msg);
						break;
					}
				default:
					break;
			}
		}
	}
}