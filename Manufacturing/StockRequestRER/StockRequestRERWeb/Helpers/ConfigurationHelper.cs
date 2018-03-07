using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StockRequestRERWeb.Helpers
{
	public static class ConfigurationHelper
	{
		static Dictionary<string, string> apiKeys;
		static Dictionary<string, string> camlQuery;
		static Dictionary<string, string> passwords;

		static ConfigurationHelper()
		{
			string json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api.keys.json"));
			JObject obj = JObject.Parse(json);
			apiKeys = obj.ToObject<Dictionary<string, string>>();
			json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CamlQueryTemplates.json"));
			obj = JObject.Parse(json);
			camlQuery = obj.ToObject<Dictionary<string, string>>();
			json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "passwords.json"));
			obj = JObject.Parse(json);
			passwords = obj.ToObject<Dictionary<string, string>>();

		}
		public static string GetApiKey(string keyName)
		{
			return apiKeys[keyName];
		}
		public static string GetCamlQuery(string keyName)
		{
			return camlQuery[keyName];
		}
		public static string GetPassword(string keyName)
		{
			return passwords[keyName];
		}
	}
}