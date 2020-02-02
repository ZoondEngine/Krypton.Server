using Krypton.MinimalLoader.Core.Native;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class RedistributableCheck : ISecurityCheck
	{
		public bool Check(out string message)
		{
			if(!RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2015x64))
			{
				if (RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2015to2019x64))
				{
					message = "";
					return true;
				}
				else
				{
					message = "VC++ 2015 x64 not founded on your PC. Please install VC++ 2015x64 redistributable";
					return false;
				}
			}
			else
			{
				message = "";
				return true;
			}
		}
	}
}
