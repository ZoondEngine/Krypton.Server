#pragma once

#include "../../includes/auto.hpp"
#include "../../includes/base.hpp"
#include "../../includes/win32.hpp"

namespace krypton::support::singleton
{

class NoCopy
{
protected:
	NoCopy() = default;
	NoCopy( const NoCopy& ) = delete;

protected:
	NoCopy& operator=( const NoCopy& ) = delete;
};

}