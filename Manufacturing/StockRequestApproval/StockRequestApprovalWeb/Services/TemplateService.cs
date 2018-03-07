using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using StockRequestApprovalWeb.Models;
using NLog;

namespace StockRequestApprovalWeb.Services
{
	public class TemplateService
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public string CreateMessage(TemplateModel template)
		{
			try
			{
				logger.Debug("Adding template.");
				Engine.Razor.AddTemplate(Path.GetFileNameWithoutExtension(template.MainTemplatePath), File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template.MainTemplatePath)));
			}
			catch { logger.Warn("Can not add template, template probably added."); }

			logger.Debug("Adding subtemplates.");
			foreach (string s in template.TemplatePaths)
			{
				try
				{
					Engine.Razor.AddTemplate(Path.GetFileNameWithoutExtension(s), File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s)));
				}
				catch
				{
					logger.Warn("Can not add subtemplate " + Path.GetFileNameWithoutExtension(s) + ". Subtemplate probably added.");
				}
			}
			if (!Engine.Razor.IsTemplateCached(Path.GetFileNameWithoutExtension(template.MainTemplatePath), template.MainTemplateModelType))
			{
				logger.Debug("Compiling template " + Path.GetFileNameWithoutExtension(template.MainTemplatePath));
				Engine.Razor.Compile(Path.GetFileNameWithoutExtension(template.MainTemplatePath), template.MainTemplateModelType);
			}

			foreach (string s in template.TemplatePaths)
			{
				if (!Engine.Razor.IsTemplateCached(Path.GetFileNameWithoutExtension(s), null))
				{
					logger.Debug("Compiling subtemplate " + Path.GetFileNameWithoutExtension(s));
					Engine.Razor.Compile(Path.GetFileNameWithoutExtension(s));
				}
			}
			logger.Debug("Running final proccess.");
			var htmlContent = Engine.Razor.Run(Path.GetFileNameWithoutExtension(template.MainTemplatePath), template.MainTemplateModelType, template.Model);
			logger.Info("Message created.");
			return htmlContent;
		}
	}
}