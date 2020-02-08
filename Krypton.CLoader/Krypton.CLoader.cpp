#include "support/singleton/singleton.hpp"

#include "core/bootstrap.hpp"

using namespace krypton::support;
using namespace krypton::core;

int main()
{
	auto& bootstrap = Bootstrap::Instance();
	if( bootstrap.BuildCore() )
	{
		const auto& hardware = bootstrap.GetHardware();
		const auto identifier = hardware->GetHardwareIdentifier();
		std::cout << identifier << std::endl;

		std::cout << "Creating a process ..." << std::endl;
		auto injection = bootstrap.GetInjection();
		if( injection.RunProcess( "C:\\Windows\\System32\\notepad.exe" ) )
		{
			system( "pause" );
			std::cout << "Process created! Injecting ..." << std::endl;
			injection.SetupW( "notepad.exe", L"C:\\Users\\Shabora\\AppData\\Local\\temporary\\shell_host.dll" );
			if( injection.Inject() )
			{
				std::cout << "Injected! All fine" << std::endl;
			}
			else
			{
				std::cerr << "Unable to inject" << std::endl;
			}
		}
		else
		{
			std::cerr << "Process unable to creating" << std::endl;
		}
	}
	else
	{
		std::cout << "Core not builded!" << std::endl;
		std::cout << "Last Error: 0x" << std::hex << GetLastError() << std::endl;
	}

	system( "pause" );
}