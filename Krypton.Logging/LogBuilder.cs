using Krypton.Logging.Contracts;
using Krypton.Logging.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Logging
{
	public static class LogBuilder
	{
		public static IKryptonLogger CreateTxt()
			=> new TxtLogger();
	}
}
