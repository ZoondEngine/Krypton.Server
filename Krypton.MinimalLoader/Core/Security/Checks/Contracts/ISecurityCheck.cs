using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public interface ISecurityCheck
	{
		bool Check(out string message);
	}
}
