using Krypton.Support;
using System.Diagnostics;

namespace Krypton.MinimalLoader.Core.Native
{
	public class ProcessStarterComponent
	{
		private Process m_process;
		private bool m_is_hidden;
		private string m_process_name;
		private bool m_is_runned;

		public void Run(string process_name, bool hidden_mode = true)
		{
			m_is_hidden = hidden_mode;
			m_process_name = process_name;

			ProcessStartInfo psi = new ProcessStartInfo();
			Prepare(ref psi);

			m_is_runned = true;
			m_process = Process.Start(psi);

			m_process.WaitForExit(5000);
			m_is_runned = !m_process.HasExited;
		}

		public void Stop()
		{
			if(GetProcess() != null)
			{
				GetProcess().Kill();
				GetProcess().Close();
				GetProcess().Dispose();
			}

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
