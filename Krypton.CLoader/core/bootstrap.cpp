#include "bootstrap.hpp"

namespace krypton::core
{

bool Bootstrap::BuildCore()
{
	m_hardware_controller = &hardware::HardwareController::Instance();
	if( !this->GetHardware()->Create() )
	{
		return false;
	}

	return true;
}

void Bootstrap::Destroy()
{
	this->GetHardware()->Destroy();
	delete m_hardware_controller;
}

hardware::HardwareController* Bootstrap::GetHardware()
{
	return m_hardware_controller;
}

injection::InjectionController Bootstrap::GetInjection()
{
	return injection::InjectionController();
}

}
