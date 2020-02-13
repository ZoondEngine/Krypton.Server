using System;
using System.Diagnostics;
using System.IO;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class InjectionAppCheck : ISecurityCheck
	{
		public bool Check(out string message)
		{
			var processes = Process.GetProcessesByName("notepad");
			if (processes.Length > 0)
			{
				foreach (var process in processes)
				{
					process.Kill();
					process.Dispose();
				}
			}

			try
			{
				var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\temporary";
				if (Directory.Exists(directory))
				{
					Directory.Delete(directory, true);
				}
			}
			catch
			{
				message = "Abnormal prepared. Please restart loader";
				return false;
			}

			message = "";
			return true;
		}
	}
}
