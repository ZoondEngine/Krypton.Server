#pragma once

#include "../../support/singleton/singleton.hpp"
using namespace krypton::support;

namespace krypton::core::hardware
{

class HardwareController : public singleton::Singleton< HardwareController >
{
public:
	bool Create();
	void Destroy();

public:
	const std::string GetHardwareIdentifier() const;

protected:
	SYSTEM_INFO m_system_information = { };
	HW_PROFILE_INFO m_hw_profile = { };
};

}