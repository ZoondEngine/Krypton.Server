#pragma once

#if defined( _M_IX86 )
#define KRYPTON_X86
#elif defined( _M_X64 )
#define KRYPTON_X64
#else
#error "Not supported architecture!"
#endif // _M_IX86

#if defined( _DEBUG )
#define KRYPTON_DEBUG
#elif defined( NDEBUG )
#define KRYPTON_RELEASE
#else
#error "Not supported configuration!"
#endif // _DEBUG

#define API_CDECL										__cdecl
#define API_STDCALL									__stdcall
#define API_THISCALL								__thiscall
#define API_FASTCALL								__fastcall
#define API_VECTORCALL							__vectorcall

#define API_NT											API_STDCALL
#define API_WIN32										API_STDCALL
#define API_D3D											API_STDCALL

#define API_FORCEINLINE							__forceinline

#define JOIN_IMPL( A, B )						A ## B
#define JOIN( A, B )								JOIN_IMPL( A, B )

#define FIELD_PAD( Size )						std::uint8_t JOIN( __pad, __COUNTER__ )[ Size ] = { }

#define KRYPTON_THREAD							__declspec( thread )
#define KRYPTON_ALLOCATE( Section )	__declspec( allocate( Section ) )

#define KRYPTON_PACK_BEGIN( Size )	__pragma( pack( push, Size ) )
#define KRYPTON_PACK_END()					__pragma( pack( pop ) )

#define FUNCTION										XOR( __FUNCTION__ )
#define FN													XOR( __FUNCTION__ )

#define USE_IMAGE_MAP_DATA					false