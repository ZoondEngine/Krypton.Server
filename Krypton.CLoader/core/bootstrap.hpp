#pragma once

#include "hardware/hardware_controller.hpp"
#include "injection/injection_controller.hpp"

namespace krypton::core
{

class Bootstrap : public support::singleton::Singleton< Bootstrap >
{
public:
	bool BuildCore();
	void Destroy();

public:
	hardware::HardwareController* GetHardware();
	injection::InjectionController GetInjection();

private:
	hardware::HardwareController* m_hardware_controller;
};

}