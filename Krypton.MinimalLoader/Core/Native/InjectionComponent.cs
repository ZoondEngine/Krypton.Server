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

		public InjectionComponent()
		{
			Native = new NativeProcedures();
		}

		public InjectionComponent SetupInjection(string dll_path, string process_name)
		{
			DllPath = dll_path;
			ProcessName = process_name;

			return this;
		}

		public bool Inject()
		{
			if(string.IsNullOrEmpty(DllPath) & string.IsNullOrEmpty(ProcessName))
			{
				return false;
			}

			var handle = GetHandle();
			if(handle == IntPtr.Zero)
			{
				return false;
			}

			var load_library = GetLoadLibaryAddress();
			if(load_library == IntPtr.Zero)
			{
				return false;
			}

			var allocated = Native.VirtualAllocEx(handle, IntPtr.Zero, CalculateDllSize(), NativeProcedures.AllocationType.MemoryCommit | NativeProcedures.AllocationType.MemoryReserve, NativeProcedures.AllocationProtect.PageReadWrite);
			if(allocated == IntPtr.Zero)
			{
				return false;
			}

			var result = Native.WriteProcessMemory(handle, allocated, Encoding.Default.GetBytes(DllPath), CalculateDllSize(), out var written);

			if(result)
			{
				if (written != UIntPtr.Zero)
				{
					Native.CreateRemoteThread(handle, IntPtr.Zero, 0, load_library, allocated, 0, IntPtr.Zero);
				}
			}

			return result;
		}

		private IntPtr GetHandle()
		{
			var process = Process.GetProcessesByName(ProcessName)[0];
			if (process == null)
			{
				return IntPtr.Zero;
			}

			return process.Handle;
		}
		private IntPtr GetLoadLibaryAddress()
		{
			var module = Native.GetModuleHandle("kernel32.dll");
			if(module == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return Native.GetProcAddress(module, "LoadLibraryA");
		}
		private uint CalculateDllSize()
			=> (uint)((DllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
	}
}
