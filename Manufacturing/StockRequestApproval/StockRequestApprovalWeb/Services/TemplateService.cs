using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using StockRequestApprovalWeb.Models;

namespace StockRequestApprovalWeb.Services
{
	public class TemplateService
	{
		public string CreateMessage(TemplateModel template)
		{
			var service = Engine.Razor;
			service.AddTemplate(Path.GetFileNameWithoutExtension(template.MainTemplatePath), File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template.MainTemplatePath)));

			foreach (string s in template.TemplatePaths)
			{
				service.AddTemplate(Path.GetFileNameWithoutExtension(s), File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s)));
			}
			if (!service.IsTemplateCached(Path.GetFileNameWithoutExtension(template.MainTemplatePath), template.MainTemplateModelType))
				service.Compile(Path.GetFileNameWithoutExtension(template.MainTemplatePath), template.MainTemplateModelType);

			foreach (string s in template.TemplatePaths)
			{
				if (!service.IsTemplateCached(Path.GetFileNameWithoutExtension(s), null))
					service.Compile(Path.GetFileNameWithoutExtension(s));
			}
			var htmlContent = service.Run(Path.GetFileNameWithoutExtension(template.MainTemplatePath), template.MainTemplateModelType, template.Model);
			return htmlContent;
		}
	}
}