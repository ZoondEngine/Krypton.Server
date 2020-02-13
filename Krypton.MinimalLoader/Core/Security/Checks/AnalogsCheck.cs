using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class AnalogsCheck : ISecurityCheck
	{
		public bool Check(out string message)
		{
			var current_file_name = Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last();

			try
			{
				var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.exe");
				foreach (var file in files)
				{
					if (file.ToLower().Contains("kryptonware"))
					{
						if (file.ToLower() != current_file_name.ToLower())
							File.Delete(file);
					}
				}
			}
			catch
			{

			}

			try
			{
				var processes = Process.GetProcessesByName("KryptonWare");
				foreach(var process in processes)
				{
					if (process == Process.GetCurrentProcess())
						continue;

					process.Kill();
					process.Close();
				}
			}
			catch
			{

			}

			message = "";
			return true;
		}
	}
}
