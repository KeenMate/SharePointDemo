using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StockRequestApprovalWeb.Services
{
	public interface ITokenRepository
	{
		Uri GetHostUrl();
		bool IsConnectedToO365 { get; }

		string GetSiteTitle();

		void CreateList(string title);
	}
}