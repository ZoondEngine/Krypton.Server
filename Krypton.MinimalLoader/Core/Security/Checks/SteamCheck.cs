using System.Diagnostics;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class SteamCheck : ISecurityCheck
	{
		public bool Check(out string message)
		{
			var processes = Process.GetProcesses();
			for(int i = 0; i > processes.Length; i++)
			{
				if(processes[i].ProcessName.ToLower().Contains("steam"))
				{
					message = "Steam Process Detected";
					return false;
				}
			}

			message = "";
			return true;
		}
	}
}
