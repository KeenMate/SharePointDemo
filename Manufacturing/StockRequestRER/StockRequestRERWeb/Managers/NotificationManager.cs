using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockRequestRERWeb.Models;
using StockRequestRERWeb.Services;
using System.Configuration;
using NLog;

namespace StockRequestRERWeb.Managers
{
	public class NotificationManager
	{
		private TemplateService ts;
		private MailerService ms;
		private string apiKey;
		private Dictionary<string, TemplateDefinitionModel> templates = new Dictionary<string, TemplateDefinitionModel>();

		private static Logger logger = LogManager.GetCurrentClassLogger();

		public NotificationManager(string apiKey, string jsonPath)
		{
			this.apiKey = apiKey;
			JsonLoader loader = new JsonLoader();
			templates = loader.LoadJsonDef(jsonPath);
		}

		public void SendMail(object model, TemplateDefinitionModel tModel)
		{
			tModel.TemplateModel.Model = model;
			tModel.TemplateModel.MainTemplateModelType = model.GetType();

			ts = new TemplateService();
			logger.Info("Creating html message.");
			tModel.Message.HtmlMessage = ts.CreateMessage(tModel.TemplateModel);

			ms = new MailerService(apiKey);
			ms.SendMail(tModel.Message);
		}

		public void SendApprovalRequest(ApprovalRequestModel model, string to)
		{			
			var templModel = templates[MailTemplatesKeys.ApprovalRequestForManager];
			model.Url = ConfigurationManager.AppSettings["approvalAppUrl"];
			templModel.Message.To = to;
			logger.Info("Sending email to " + to);
			SendMail(model, templModel);
		}

		public void SendRequestApproved(StockRequestModel model, string to)
		{
			var templModel = templates[MailTemplatesKeys.ConfirmationRequestForUser];
			templModel.Message.To = to;
			SendMail(model, templModel);
		}
	}
}