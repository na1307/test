#pragma once

#ifndef BLUEHILLLOADER_EXPORTS
#define BLUEHILLLOADER_API __declspec(dllexport)
#else // BLUEHILLLOADER_EXPORTS
#define BLUEHILLLOADER_API
#endif // BLUEHILLLOADER_EXPORTS

#ifdef __cplusplus
extern "C" {
#endif //__cplusplus

typedef void (__cdecl *RegisterEdgeHook_t)(void *target, void *detour, void **original);
typedef void (__cdecl *RegisterHooks_t)(RegisterEdgeHook_t out);

BLUEHILLLOADER_API void __cdecl RegisterHooks(RegisterEdgeHook_t out);

#ifdef __cplusplus
}
#endif //__cplusplus
