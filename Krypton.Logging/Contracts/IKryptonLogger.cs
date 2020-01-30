using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Logging.Contracts
{
	public interface IKryptonLogger
	{
		void Build(string folder);

		void Warning(string text, string filename = "warnings.txt");
		void Error(string text, string filename = "errors.txt");
		void Debug(string text, string filename = "debug.txt");
		void Success(string text, string filename = "success.txt");
	}
}
