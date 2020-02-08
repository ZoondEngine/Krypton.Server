#pragma once
#include "../../includes/auto.hpp"
#include "../../includes/base.hpp"
#include "../../includes/win32.hpp"

namespace krypton::core::injection
{

class InjectionController
{
public:
	void SetupW( const std::string& process_name, const std::wstring& dll_path );
	bool Inject();

public:
	bool RunProcess( const std::string& binary_name );

private:
	const int GetProcessId() const;
	const std::string& GetProcessName() const;
	const std::wstring& GetDllPath() const;

private:
	std::string m_process_name = { };
	std::wstring m_dll_path = { };
};

}