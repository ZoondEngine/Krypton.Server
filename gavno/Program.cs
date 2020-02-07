using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gavno
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
			Console.ReadKey();
		}
	}
}
