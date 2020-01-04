using System.ServiceProcess;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class EasyAntiCheatCheck : ISecurityCheck
	{
		public bool Check(out string message)
		{
			var services = ServiceController.GetServices();
			for (int i = 0; i < services.Length; i++)
			{
				if (services[i].ServiceName.ToLower().Contains("easyanticheat"))
				{
					if (services[i].Status == ServiceControllerStatus.Running)
					{
						message = "Easy Anti Cheat Service Detected";
						return false;
					}
				}
			}

			message = "";
			return true;
		}
	}
}
