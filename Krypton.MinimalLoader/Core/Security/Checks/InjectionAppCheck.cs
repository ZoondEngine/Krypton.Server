using System.Diagnostics;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class InjectionAppCheck : ISecurityCheck
	{
		public bool Check(out string message)
		{
			var processes = Process.GetProcessesByName("notepad");
			if (processes.Length > 0)
			{
				foreach(var process in processes)
				{
					process.Kill();
					process.Dispose();
				}
			}

			message = "";
			return true;
		}
	}
}
