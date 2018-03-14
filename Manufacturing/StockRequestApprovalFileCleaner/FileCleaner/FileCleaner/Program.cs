using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCleaner
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 4)
			{
				Console.WriteLine("Usage: -f \"path\" -o x will delete all files in path older then x hours.");
				Console.WriteLine("Optional parameter [-u] specifies time unit: d - day, h - hour, m - minute, s - second. Default is hour.");
				Console.ReadKey();
				return;
			}
			try
			{
				string path = "";
				int older = int.MinValue;
				string unit = "h";
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] == "-f" || args[i] == "/f")
					{
						if (args.Length >= i)
						{
							path = args[i + 1];
							continue;
						}
					}
					if (args[i] == "-o" || args[i] == "/o")
					{
						if (args.Length >= i)
						{
							older = int.Parse(args[i + 1]);
							continue;
						}
					}
					if (args[i] == "-u" || args[i] == "/u")
					{
						if (args.Length >= i)
						{
							unit = args[i + 1];
							continue;
						}
					}
				}

				if (path == "")
				{
					Console.WriteLine("Folder path is not specified.");
					Console.ReadKey();
					return;
				}
				if (!Directory.Exists(path))
				{
					Console.WriteLine("Folder does not exists.");
					Console.ReadKey();
					return;
				}
				if (older <= 1)
				{
					Console.WriteLine("Age was not specified or is not valid.");
					Console.ReadKey();
					return;
				}
				bool ok = false;
				switch (unit)
				{
					case "d":
					case "h":
					case "m":
					case "s":
						ok = true;
						break;
					default:
						break;
				}
				if (!ok)
				{
					Console.WriteLine("Invalid time unit.");
					Console.ReadKey();
					return;
				}

				string[] files = Directory.GetFiles(path);
				foreach (string s in files)
				{
					DeleteFile(s, older, unit);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadKey();
			}
		}

		private static void DeleteFile(string path, int older, string unit)
		{
			FileInfo info = new FileInfo(path);
			switch (unit)
			{
				case "d":
					if ((DateTime.Now - info.CreationTime).TotalDays > older)
					{
						File.Delete(path);
					}
					break;
				case "h":
					if ((DateTime.Now - info.CreationTime).TotalHours > older)
					{
						File.Delete(path);
					}
					break;
				case "m":
					if ((DateTime.Now - info.CreationTime).TotalMinutes > older)
					{
						File.Delete(path);
					}
					break;
				case "s":
					if ((DateTime.Now - info.CreationTime).TotalSeconds > older)
					{
						File.Delete(path);
					}
					break;
				default:
					break;
			}
		}
	}
}
