using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StockRequestApprovalWeb.Helpers
{
	public static class ConfigurationHelper
	{
		static Dictionary<string, string> apiKeys;

		static ConfigurationHelper()
		{
			string json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api.keys.json"));
			JObject obj = JObject.Parse(json);
			apiKeys = obj.ToObject<Dictionary<string, string>>();

		}
		public static string GetApiKey(string keyName)
		{
			return apiKeys[keyName];
		}
	}
}