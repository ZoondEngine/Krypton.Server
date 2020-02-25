using System;

namespace Krypton.MinimalLoader
{
	class Program
	{
		static void ExecuteEntry()
		{
			var executor = new SystemExecutor();

			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				executor.GetConsole().Exception((Exception)e.ExceptionObject);
			};

			executor.GetConsole().SetTitle(executor.GetSecurity().GetRandomWindowName(58));
			if(executor.GetStageManager().Check(out string message))
			{
				executor.GetConsole().Write(ConsoleColor.Green, $"Done. Please close loader and start the game");
			}
			else
			{
				executor.GetConsole().Write(ConsoleColor.Red, $"Error -- {message}", true);
				executor.GetConsole().Write("Press any key to continue", true);
			}

			Console.ReadKey();
		}

		static void Main(string[] args)
			=> ExecuteEntry();
	}
}
