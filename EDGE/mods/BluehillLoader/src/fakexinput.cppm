// ReSharper disable CppParameterMayBeConst
module;

#include <windows.h>

export module fakexinput;

import std;

typedef DWORD (WINAPI *xinput_func_t)(DWORD, void *);

HMODULE real_xinput = nullptr;
xinput_func_t real_XInputGetState = nullptr;
xinput_func_t real_XInputSetState = nullptr;

DWORD WINAPI XInputGetState(DWORD dwUserIndex, void *pState) {
    return real_XInputGetState(dwUserIndex, pState);
}

DWORD WINAPI XInputSetState(DWORD dwUserIndex, void *pVibration) {
    return real_XInputSetState(dwUserIndex, pVibration);
}

export void InitializeXInput() {
    if (real_xinput != nullptr) {
        return;
    }

    wchar_t buffer[MAX_PATH];

    if (GetSystemDirectoryW(buffer, MAX_PATH) == 0) {
        return;
    }

    std::wstring real_dll_path(buffer);

    real_dll_path.append(L"\\xinput1_3.dll");

    const auto ptr = LoadLibraryW(real_dll_path.c_str());

    if (ptr == nullptr) {
        return;
    }

    real_xinput = ptr;
    const auto xigs = GetProcAddress(ptr, "XInputGetState");
    const auto xiss = GetProcAddress(ptr, "XInputSetState");

    if (xigs == nullptr || xiss == nullptr) {
        return;
    }

    real_XInputGetState = reinterpret_cast<xinput_func_t>(xigs);
    real_XInputSetState = reinterpret_cast<xinput_func_t>(xiss);
}

export void FreeXInput() {
    FreeLibrary(real_xinput);
}
