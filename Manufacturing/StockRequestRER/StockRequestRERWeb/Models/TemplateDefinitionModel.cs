using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestRERWeb.Models
{
	[Serializable]
	public class TemplateDefinitionModel
	{
		public string Name { get; set; }
		public EmailMessage Message { get; set; }
		public TemplateModel TemplateModel { get; set; }
	}
}