using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Krypton.MinimalLoader.Core.Native
{
	public class NativeProcedures
	{
		#region Import Enums
		public enum ProcessPrivileges : int
		{
			CreateThread = 0x0002,
			QueryInformation = 0x0400,
			MemoryOperate = 0x0008,
			MemoryWrite = 0x0020,
			MemoryRead = 0x0010,
			AllAccess = CreateThread | QueryInformation | MemoryOperate | MemoryWrite | MemoryRead,
		}

		public enum AllocationType : uint
		{
			MemoryCommit = 0x00001000,
			MemoryReserve = 0x00002000,
		}

		public enum AllocationProtect : uint
		{
			PageReadWrite = 0x00000004
		}
		#endregion

		#region Imports
		[DllImport("kernel32.dll")]
		private static extern IntPtr OpenProcess(int desired_access, bool inherited_handle, int process_id);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetModuleHandle(string module_name);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr module, string procedure_name);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		private static extern IntPtr VirtualAllocEx(IntPtr process_handle, IntPtr address, uint size, uint allocation_type, uint protect);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool WriteProcessMemory(IntPtr process_handle, IntPtr destination, byte[] buffer, uint size, out UIntPtr written_bytes);

		[DllImport("kernel32.dll")]
		private static extern IntPtr CreateRemoteThread(IntPtr process_handle, IntPtr thread_attributes, uint stack_size, IntPtr start_address, IntPtr parameters, uint creation_flags, IntPtr thread_id);
		#endregion

		#region Methods
		public IntPtr OpenProcess(ProcessPrivileges privileges, int process_id, bool inherited_handle = false)
		{
			return OpenProcess((int)privileges, inherited_handle, process_id);
		}

		public IntPtr GetModuleHandleIn(string module)
		{
			return GetModuleHandle(module);
		}

		public IntPtr GetProcAddressIn(IntPtr module, string procedure_name)
		{
			return GetProcAddress(module, procedure_name);
		}
		public IntPtr GetProcAddressIn(string module, string procedure_name)
		{
			return GetProcAddress(GetModuleHandle(module), procedure_name);
		}

		public IntPtr VirtualAllocEx(IntPtr process_handle, IntPtr address, uint size, AllocationType type, AllocationProtect protect)
		{
			return VirtualAllocEx(process_handle, address, size, (uint)type, (uint)protect);
		}

		public bool WriteProcessMemoryIn(IntPtr process_handle, IntPtr destination, byte[] buffer, uint size, out UIntPtr written_bytes)
		{
			return WriteProcessMemory(process_handle, destination, buffer, size, out written_bytes);
		}

		public IntPtr CreateRemoteThreadIn(IntPtr process_handle, IntPtr attributes, uint stack_size, IntPtr start_address, IntPtr parameters, uint creation_flags, IntPtr thread_id)
		{
			return CreateRemoteThread(process_handle, attributes, stack_size, start_address, parameters, creation_flags, thread_id);
		}
		#endregion
	}
}
