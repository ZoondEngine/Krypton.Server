#pragma once

#include "../../includes/auto.hpp"
#include "../../includes/base.hpp"
#include "../../includes/win32.hpp"

#include "no_copy.hpp"
#include "no_move.hpp"

namespace krypton::support::singleton
{

template< class Type >
class Singleton : public NoCopy, public NoMove
{
public:
	static Type& Instance();
};

template< class Type >
Type& Singleton< Type >::Instance()
{
	static Type instance = { };
	return instance;
}

}