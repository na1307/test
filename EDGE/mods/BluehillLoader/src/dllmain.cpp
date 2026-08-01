// ReSharper disable CppParameterMayBeConst
#include <windows.h>

import fakexinput;
import edgeminhook;
import dllloader;
import global;
import functions;
import bonus;
import level;
import music;
import fontbin;
import textloc;

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
    case DLL_PROCESS_ATTACH:
        {
            DisableThreadLibraryCalls(hModule);
            InitializeXInput();
            InitializeEdgeHook();
            load_dlls();
            RegisterEdgeHook(reinterpret_cast<void*>(0x474160), reinterpret_cast<void*>(sub_474160), reinterpret_cast<void**>(&org474160));
            RegisterEdgeHook(reinterpret_cast<void*>(0x474fa0), reinterpret_cast<void*>(sub_474FA0), reinterpret_cast<void**>(&org474FA0));
            RegisterEdgeHook(reinterpret_cast<void*>(0x4947e0), reinterpret_cast<void*>(sub_4947E0), reinterpret_cast<void**>(&org4947E0));
            RegisterEdgeHook(reinterpret_cast<void*>(0x4a63f0), reinterpret_cast<void*>(sub_4A63F0), reinterpret_cast<void**>(&org4A63F0));
            RegisterEdgeHook(reinterpret_cast<void*>(0x4a75f0), reinterpret_cast<void*>(sub_4A75F0), reinterpret_cast<void**>(&org4A75F0));

            break;
        }

    case DLL_PROCESS_DETACH:
        {
            unload_dlls();
            UninitializeEdgeHook(lpReserved);
            FreeXInput();

            break;
        }

    default:
        break; // Explicit break
    }

    return TRUE;
}
