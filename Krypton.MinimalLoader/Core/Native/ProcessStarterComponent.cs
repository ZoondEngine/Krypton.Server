using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Krypton.MinimalLoader.Core.Native
{
	public class ProcessStarterComponent
	{
		private Process m_process;
		private bool m_is_hidden;
		private string m_process_name;
		private bool m_is_runned;
		private Thread m_waiting_thread;

		public void Run(string process_name, bool hidden_mode = true)
		{
			m_is_hidden = hidden_mode;
			m_process_name = process_name;

			ProcessStartInfo psi = new ProcessStartInfo();
			Prepare(ref psi);

			m_is_runned = true;
			m_process = Process.Start(psi);

			m_process.WaitForExit(500);
			m_is_runned = !m_process.HasExited;

			if(IsRunned())
			{
				m_waiting_thread = new Thread(() =>
				{
					Console.WriteLine("Please wait ...");

					Stopwatch watcher = new Stopwatch();
					watcher.Start();

					var code = 0;
					m_process.WaitForExit(5000);
					try
					{
						code = m_process.ExitCode;
					}
					catch
					{
						code = 0;
					}

					watcher.Stop();
					if(watcher.Elapsed.Seconds < 3)
					{
						code = -1;
					}

					if(code == 0)
					{
						Console.WriteLine("Done. Close loader and start the game");
						Console.ReadKey();

						//TODO: normal exit log
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("ERROR");
						Console.WriteLine("Error message: Check your antiviruses and try again.");
						Console.ResetColor();

						Console.WriteLine("Press any key to continue");
						Console.ReadKey();
					}

					Environment.Exit(0);
				});

				m_waiting_thread.Start();
			}
		}

		public void Stop()
		{
			if(GetProcess() != null)
			{
				GetProcess().Kill();
				GetProcess().Close();
				GetProcess().Dispose();
			}

			try
			{
				m_waiting_thread.Abort();
			}
			catch 
			{ }

			m_waiting_thread = null;
			m_process = null;
			m_is_hidden = true;
			m_is_runned = false;
			m_process_name = "";
		}

		public Process GetProcess()
			=> m_process;

		public bool IsHiddenMode()
			=> m_is_hidden;

		public bool IsRunned()
			=> m_is_runned;

		public string GetProcessName()
			=> m_process_name;

		public Thread GetThread()
			=> m_waiting_thread;

		private void Prepare(ref ProcessStartInfo info)
		{
			if(IsHiddenMode())
			{
				info.WindowStyle = ProcessWindowStyle.Hidden;
				info.ErrorDialog = false;
				info.CreateNoWindow = false;
			}

			info.FileName = GetProcessName();
		}
	}
}
