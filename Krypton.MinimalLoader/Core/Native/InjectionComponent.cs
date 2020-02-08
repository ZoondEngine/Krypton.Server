using Krypton.Support;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Lunar;

namespace Krypton.MinimalLoader.Core.Native
{
	public class InjectionComponent : KryptonComponent<InjectionComponent>
	{
		private NativeProcedures Native;
		private string DllPath;
		private string ProcessName;
		private Process NotepadProcess;
		private IntPtr NotePadManualHandle = IntPtr.Zero;

		public InjectionComponent()
		{
			Native = new NativeProcedures();
		}

		public InjectionComponent SetupInjection(string dll_path, string process_name, Process proc = null)
		{
			DllPath = dll_path;
			ProcessName = process_name;

			if (proc != null)
			{
				NotepadProcess = proc;
			}

			return this;
		}

		public bool Inject()
		{
			if (string.IsNullOrEmpty(DllPath) & string.IsNullOrEmpty(ProcessName))
			{
				return false;
			}

			Console.WriteLine("Injection dllpath: " + DllPath);

			IntPtr handle;
			if (NotepadProcess != null)
			{
				handle = NotepadProcess.Handle;

				Console.WriteLine("Process Handle: 0x" + handle.ToString("X16"));
			}
			else
			{
				handle = GetHandle();
			}

			//if (NotePadManualHandle != IntPtr.Zero)
			//{
			//	handle = NotePadManualHandle;
			//}

			if (handle == IntPtr.Zero)
			{
				return false;
			}

			var load_library = GetLoadLibaryAddress();
			if (load_library == IntPtr.Zero)
			{
				return false;
			}

			Console.WriteLine("LoadLibraryA handle: 0x" + load_library.ToString("X16"));

			var allocated = Native.VirtualAllocEx(handle, IntPtr.Zero, CalculateDllSize(), NativeProcedures.AllocationType.MemoryCommit, NativeProcedures.AllocationProtect.PageReadWrite);
			if (allocated == IntPtr.Zero)
			{
				return false;
			}

			Console.WriteLine("Allocated: 0x" + allocated.ToString("X16"));
			Console.WriteLine("DllSize: 0x" + CalculateDllSize().ToString("X16"));

			var result = Native.WriteProcessMemoryIn(handle, allocated, Encoding.Default.GetBytes(DllPath), CalculateDllSize(), out var written);
			Console.WriteLine("written: 0x" + written.ToString());

			if (result)
			{
				var thread = Native.CreateRemoteThreadIn(handle, IntPtr.Zero, 0, load_library, allocated, 0, IntPtr.Zero);
				var wait_result = Native.WaitForSingleObjectIn(thread, 10 * 60 * 1000);
				Console.WriteLine("thread: 0x" + thread.ToString("X16"));

				//var wait_result = Native.WaitForSingleObjectIn(thread, 10 * 60 * 1000);
				if (wait_result == 0x00000080L || wait_result == 0x00000102L || wait_result == 0xFFFFFFFF)
				{
					if (thread != null)
					{
						Native.CloseHandleIn(thread);
					}

					result = false;

					throw new System.ComponentModel.Win32Exception("Error: 0x" + Native.GetLastErrorIn().ToString("X8"));
				}

				if (thread != null)
				{
					Native.CloseHandleIn(thread);
				}
			}

			return result;
		}

		public NativeProcedures GetNative()
			=> Native;

		private IntPtr GetHandle()
		{
			var processes = Process.GetProcessesByName(ProcessName);
			if (processes.Length <= 0)
			{
				return IntPtr.Zero;
			}

			var process = processes[0];
			if (process == null)
			{
				return IntPtr.Zero;
			}
			else
			{
				var handle = Native.OpenProcess(NativeProcedures.ProcessPrivileges.AllAccess, process.Id);
				return handle;
			}
		}
		private IntPtr GetLoadLibaryAddress()
		{
			var module = Native.GetModuleHandleIn("kernel32.dll");
			if (module == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return Native.GetProcAddressIn(module, "LoadLibraryA");
		}
		private uint CalculateDllSize()
			=> (uint)((DllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
	}
}
