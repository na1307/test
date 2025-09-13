// dllmain.cpp : DLL 애플리케이션의 진입점을 정의합니다.
#include "pch.h"

HMODULE thisModule = nullptr;

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    UNREFERENCED_PARAMETER(ul_reason_for_call);
    UNREFERENCED_PARAMETER(lpReserved);

    thisModule = hModule;

    return TRUE;
}

extern "C" {
    HMODULE __declspec(dllexport) __stdcall GetBridgeInstance() {
        return thisModule;
    }

    LPWSTR __declspec(dllexport) __stdcall GetResource() {
        return MAKEINTRESOURCE(IDD_PROXY_HOST);
    }
}
