using System;
using System.Diagnostics;
using Lunar;

namespace InjectionTestCore.cs
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Process prc = new Process();
			prc.StartInfo.Verb = "runas";
			prc.StartInfo.FileName = "notepad.exe";
			prc.Start();

			Console.WriteLine("Runned");
			Console.ReadKey();

			var mapper = new LibraryMapper(prc, "C:\\Users\\Валера\\AppData\\Local\\temporary\\shell_host.dll");
			Console.WriteLine("Prepared");
			Console.ReadKey();

			mapper.MapLibrary();
			Console.WriteLine("Injected");
			prc.WaitForExit(6000);

			var result = prc.HasExited;
			if(result)
			{
				Console.WriteLine("Process closed. Code: " + prc.ExitCode);
			}
			else
			{
				Console.WriteLine("Process runned. All fine");
			}

			Console.ReadKey();
		}
	}
}
