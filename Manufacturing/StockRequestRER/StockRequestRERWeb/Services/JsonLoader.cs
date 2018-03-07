using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using StockRequestRERWeb.Models;

namespace StockRequestRERWeb.Services
{
	public class JsonLoader
	{
		public TemplateDefinitionModel LoadJsonDef(string defName, string jsonPath)
		{
			string json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonPath));
			JArray arr = JArray.Parse(json);
			foreach (JObject obj in arr)
			{
				if ((string)obj["Name"] == defName)
				{
					string j = obj.ToString();
					return JsonConvert.DeserializeObject<TemplateDefinitionModel>(j);
				}
			}
			return null;
		}
		public Dictionary<string, TemplateDefinitionModel> LoadJsonDef(string jsonPath)
		{
			Dictionary<string, TemplateDefinitionModel> toReturn = new Dictionary<string, TemplateDefinitionModel>();
			string json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonPath));
			JArray arr = JArray.Parse(json);
			foreach (JObject obj in arr)
			{
				string j = obj.ToString();
				TemplateDefinitionModel m = JsonConvert.DeserializeObject<TemplateDefinitionModel>(j);
				toReturn.Add(m.Name, m);
			}
			return toReturn;
		}
	}
}