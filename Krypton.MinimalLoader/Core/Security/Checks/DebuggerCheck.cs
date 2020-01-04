using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader.Core.Security.Checks
{
	public class DebuggerCheck : ISecurityCheck
	{
		[DllImport("kernel32.dll")]
		static extern bool IsDebuggerPresent();

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

		public bool Check(out string message)
		{
			if(Debugger.IsAttached || IsDebuggerPresent())
			{
				message = "Debugger detected. Your PC information has been sended on server for analyze";
				return false;
			}
			else
			{
				bool attached = false;
				CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref attached);

				if(attached)
				{
					message = "Debugger detected. Your PC information has been sended on server for analyze";
					return false;
				}
			}

			message = "";
			return true;
		}
	}
}
