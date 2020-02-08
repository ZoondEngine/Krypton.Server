#include "hardware_controller.hpp"

bool krypton::core::hardware::HardwareController::Create()
{
	GetSystemInfo( &m_system_information );
	return GetCurrentHwProfileA( &m_hw_profile );
}

void krypton::core::hardware::HardwareController::Destroy()
{ 
	m_system_information = { };
}

const std::string krypton::core::hardware::HardwareController::GetHardwareIdentifier() const
{
	std::string id = { };
	id.append( m_hw_profile.szHwProfileGuid );

	return id;
}
