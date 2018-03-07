using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Models
{
	public class TemplateModel
	{
		public List<string> TemplatePaths { get; set; } = new List<string>();
		public string MainTemplatePath { get; set; } = "";
		public object Model { get; set; } = null;
		public Type MainTemplateModelType { get; set; } = null;
	}
}