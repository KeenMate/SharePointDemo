using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace StockRequestApprovalWeb.Helpers
{
	public class AuthenticationHelper
	{
		public string GetSAMLToken(string xmlToPost, string url = "https://login.microsoftonline.com/extSTS.srf")
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			byte[] bytes;
			bytes = System.Text.Encoding.ASCII.GetBytes(xmlToPost);
			request.ContentType = "text/xml; encoding='utf-8'";
			request.ContentLength = bytes.Length;
			request.Method = "POST";
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse response;
			response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				Stream responseStream = response.GetResponseStream();
				string responseStr = new StreamReader(responseStream).ReadToEnd();
				return responseStr;
			}
			return null;
		}

		public string ExtractSAMLToken(string responseXML, string tagName = "wsse:BinarySecurityToken")
		{
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.LoadXml(responseXML);
			XmlNodeList nodeList = xmldoc.GetElementsByTagName(tagName);
			string token = string.Empty;
			foreach (XmlNode node in nodeList)
			{
				token = node.InnerText;
			}
			return token;
		}

		public Dictionary<string, string> GetSPOCookies(string token, string userAgent, string url = "https://keenmate.sharepoint.com/_forms/default.aspx?wa=wsignin1.0")
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			byte[] bytes;
			bytes = System.Text.Encoding.ASCII.GetBytes(token);
			request.Host = "keenmate.sharepoint.com";
			request.CookieContainer = new CookieContainer();
			request.UserAgent = userAgent;
			request.ContentLength = bytes.Length;
			request.Method = "POST";
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse response;
			response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				var cookies = response.Cookies;
				Dictionary<string, string> toReturn = new Dictionary<string, string>();
				foreach (Cookie cook in response.Cookies)
				{
					toReturn.Add(cook.Name, cook.Value);
				}
				return toReturn;
			}
			return null;
		}
	}
}