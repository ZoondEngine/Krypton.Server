#include "injection_controller.hpp"

#include <Windows.h>
#include <tlhelp32.h>
#include <Shlwapi.h>

#include <filesystem>

#define INVALID_PROCESS_ID -1

void krypton::core::injection::InjectionController::SetupW( const std::string& process_name, const std::wstring& dll_path )
{ 
	m_process_name = process_name;
	m_dll_path = dll_path;
}

bool krypton::core::injection::InjectionController::Inject()
{
	const auto dll_size = m_dll_path.length() + 1;
	const auto process_id = this->GetProcessId();
	if( process_id == INVALID_PROCESS_ID )
		return false;

	auto process_handle = OpenProcess( PROCESS_ALL_ACCESS, true, this->GetProcessId() );
	if( process_handle == 0 )
		return false;

	auto allocated = VirtualAllocEx( process_handle, 0, dll_size, MEM_COMMIT, PAGE_EXECUTE_READWRITE );
	if( allocated == 0 )
		return false;

	auto written_bytes = WriteProcessMemory( process_handle, allocated, m_dll_path.c_str(), dll_size, 0 );
	if( written_bytes <= 0 )
		return false;

	auto kernel32 = LoadLibrary( "kernel32" );
	if( kernel32 == 0 )
		return false;

	auto load_library_address = GetProcAddress( kernel32, "LoadLibraryW" );
	if( load_library_address == 0 )
		return false;

	DWORD thread_id;
	auto thread_routine = ( LPTHREAD_START_ROUTINE )load_library_address;
	auto thread = CreateRemoteThread( process_handle, 0, 0, thread_routine, allocated, 0, &thread_id );
	if( thread == 0 )
	{
		return false;
	}

	if( ( process_handle != 0 ) && ( allocated != 0 ) && ( written_bytes != ERROR_INVALID_HANDLE ) && ( thread != 0 ) )
	{
		return true;
	}

	return false;
}

bool krypton::core::injection::InjectionController::RunProcess( const std::string& binary_name )
{
	if( !std::filesystem::exists( binary_name ) )
	{
		//TRACE( "\"%s\" doesn't exist!", binary_name.c_str() );
		return false;
	}

	// const auto command = ( L"\"" + binary + L"\" -steam -novid" );
	const auto command = ( "\"" + binary_name + "\"" );

	STARTUPINFOA startup_info =
	{
		sizeof( startup_info ),
	};

	startup_info.dwFlags = STARTF_USESHOWWINDOW;
	startup_info.wShowWindow = SW_HIDE;

	PROCESS_INFORMATION process_information = { };

	if( CreateProcessA( nullptr, const_cast< LPSTR >( command.c_str() ), nullptr, nullptr, FALSE, CREATE_SUSPENDED, nullptr, nullptr, &startup_info, &process_information ) == FALSE )
	{
		//TRACE( "CreateProcessW() error! (0x%08X)", GetLastError() );
		return false;
	}
	/*if( !std::filesystem::exists( image_binary ) )
	{
		TerminateProcess( process_information.hProcess, EXIT_SUCCESS );

		ResumeThread( process_information.hThread );

		CloseHandle( process_information.hThread );
		CloseHandle( process_information.hProcess );

		return false;
	}

	if( !LoadLibraryProcess( process_information.hProcess, image_binary ) )
	{
		TerminateProcess( process_information.hProcess, EXIT_SUCCESS );

		ResumeThread( process_information.hThread );

		CloseHandle( process_information.hThread );
		CloseHandle( process_information.hProcess );

		win32::Warning( L"Can't load library!" );

		return false;
	}*/

	ResumeThread( process_information.hThread );

	CloseHandle( process_information.hThread );
	CloseHandle( process_information.hProcess );

	return true;
}

const int krypton::core::injection::InjectionController::GetProcessId() const
{
	if( !m_process_name.empty() )
	{
		HANDLE snapshot = CreateToolhelp32Snapshot( TH32CS_SNAPPROCESS, 0 );
		PROCESSENTRY32 snapshot_entry = { };
		snapshot_entry.dwSize = sizeof( PROCESSENTRY32 );

		if( snapshot == INVALID_HANDLE_VALUE )
			return INVALID_PROCESS_ID;

		if( Process32First( snapshot, &snapshot_entry ) == FALSE )
			return INVALID_PROCESS_ID;

		while( Process32Next( snapshot, &snapshot_entry ) )
		{
			if( !strcmp( snapshot_entry.szExeFile, m_process_name.c_str() ) )
			{
				CloseHandle( snapshot );
				return snapshot_entry.th32ProcessID;
			}
		}

		CloseHandle( snapshot );
		return INVALID_PROCESS_ID;
	}

	return INVALID_PROCESS_ID;
}

const std::string& krypton::core::injection::InjectionController::GetProcessName() const
{
	return m_process_name;
}

const std::wstring& krypton::core::injection::InjectionController::GetDllPath() const
{
	return m_dll_path;
}
