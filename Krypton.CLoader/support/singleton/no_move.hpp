#pragma once

#include "../../includes/auto.hpp"
#include "../../includes/base.hpp"
#include "../../includes/win32.hpp"

namespace krypton::support::singleton
{

class NoMove
{
protected:
	NoMove() = default;
	NoMove( NoMove&& ) = delete;

protected:
	NoMove& operator=( NoMove&& ) = delete;
};

}