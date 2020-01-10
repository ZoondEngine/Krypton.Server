using Krypton.Support;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Krypton.MinimalLoader.Core.Native
{
	public class InjectionComponent : KryptonComponent<InjectionComponent>
	{
		private NativeProcedures Native;
		private string DllPath;
		private string ProcessName;
		private Process NotepadProcess;

		public InjectionComponent()
		{
			Native = new NativeProcedures();
		}

		public InjectionComponent SetupInjection(string dll_path, string process_name, Process proc = null)
		{
			DllPath = dll_path;
			ProcessName = process_name;

			if(proc != null)
			{
				NotepadProcess = proc;
			}

			return this;
		}

		public bool Inject()
		{
			if(string.IsNullOrEmpty(DllPath) & string.IsNullOrEmpty(ProcessName))
			{
				return false;
			}

			//Console.WriteLine("DllPath &  ProcessName Valid");
			//Console.WriteLine($"DllPath: {DllPath}");
			//Console.WriteLine($"Process: {ProcessName}");

			IntPtr handle;
			if(NotepadProcess != null)
			{
				handle = NotepadProcess.Handle;
			}
			else
			{
				handle = GetHandle();
			}

			if(handle == IntPtr.Zero)
			{
				return false;
			}

			//Console.WriteLine($"{ProcessName}: Handle: [0x{handle.ToString("X16")}]");

			var load_library = GetLoadLibaryAddress();
			if(load_library == IntPtr.Zero)
			{
				return false;
			}

			//Console.WriteLine($"{ProcessName}: LoadLibraryA: [0x{load_library.ToString("X16")}]");

			var allocated = Native.VirtualAllocEx(handle, IntPtr.Zero, CalculateDllSize(), NativeProcedures.AllocationType.MemoryCommit | NativeProcedures.AllocationType.MemoryReserve, NativeProcedures.AllocationProtect.PageReadWrite);
			if(allocated == IntPtr.Zero)
			{
				return false;
			}

			//Console.WriteLine($"{ProcessName}: Allocated: [0x{allocated.ToString("X16")}]");

			var result = Native.WriteProcessMemoryIn(handle, allocated, Encoding.Default.GetBytes(DllPath), CalculateDllSize(), out var written);

			if(result)
			{
				//Console.WriteLine($"{ProcessName}: Written bytes: {written}");

				Native.CreateRemoteThreadIn(handle, IntPtr.Zero, 0, load_library, allocated, 0, IntPtr.Zero);
			}

			return result;
		}

		private IntPtr GetHandle()
		{
			var processes = Process.GetProcessesByName(ProcessName);
			if(processes.Length <= 0)
			{
				return IntPtr.Zero;
			}

			var process = processes[0];
			if (process == null)
			{
				return IntPtr.Zero;
			}

			return process.Handle;
		}
		private IntPtr GetLoadLibaryAddress()
		{
			var module = Native.GetModuleHandleIn("kernel32.dll");
			if(module == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return Native.GetProcAddressIn(module, "LoadLibraryA");
		}
		private uint CalculateDllSize()
			=> (uint)((DllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
	}
}
