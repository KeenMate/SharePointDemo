using NLog;
using StockRequestApprovalWeb.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Services
{
	public class ReportService
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public string GenerateConfirmationReport(object dataSource, string dirPath)
		{
			ConfirmationReport rep = new ConfirmationReport();
			rep.DataSource = dataSource;
			string path = Path.Combine(dirPath, $"ConfirmationReport-{DateTime.Now.ToString("yyyyMMdd-hhmmss")}.pdf");
			logger.Debug("Exporting PDF to " + path);
			rep.ExportToPdf(path);

			return path;
		}

		public string GenerateStockReport(object dataSource, string dirPath)
		{
			StockReport rep = new StockReport();
			rep.DataSource = dataSource;
			string path = Path.Combine(dirPath, $"StockReport-{DateTime.Now.ToString("yyyyMMdd-hhmmss")}.pdf");
			logger.Debug("Exporting PDF to " + path);
			rep.ExportToPdf(path);

			return path;
		}
	}
}