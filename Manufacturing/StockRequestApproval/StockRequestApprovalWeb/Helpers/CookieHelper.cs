using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Helpers
{
	public static class CookieHelper
	{
		public static void DeleteCookie(this HttpRequestBase b, string cookieName, HttpResponseBase Response)
		{
			HttpCookie c = b.Cookies[cookieName];
			c.Expires = DateTime.Now.AddHours(-1);
			Response.SetCookie(c);
		}
	}
}