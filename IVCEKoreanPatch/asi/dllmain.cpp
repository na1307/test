#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "Utils/MemoryMgr.h"
#include "Utils/Patterns.h"
#include "Utils/ScopedUnprotect.hpp"

using namespace hook;
using namespace Memory;

int hkGetLanguageCode() {
    return 7;
}

const char *__fastcall hkGetGxtName(void *pThis, void *, char) {
    *reinterpret_cast<unsigned short *>(static_cast<char *>(pThis) + 0x260) = 0x6b;

    return "KOREAN.GXT";
}

DWORD WINAPI Init(LPVOID) {
    std::unique_ptr<ScopedUnprotect::Unprotect> Protect = ScopedUnprotect::UnprotectSectionOrFullModule(GetModuleHandleW(nullptr), ".text");

    auto getLanguageCodeAddr = get_pattern("83 EC 78 A1 ? ? ? ? 33 C4 89 44 24 74 A1 ? ? ? ? 8B 0D ? ? ? ? 53");

    InjectHook(getLanguageCodeAddr, &hkGetLanguageCode, HookType::Jump);

    auto getGxtNameAddr = get_pattern("80 7C 24 04 00 56 8B F1 74 07");

    InjectHook(getGxtNameAddr, &hkGetGxtName, HookType::Jump);

    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD reason, LPVOID lpReserved) {
    if (reason == DLL_PROCESS_ATTACH) {
        Sleep(5000);
        auto ct = CreateThread(nullptr, 0, Init, nullptr, 0, nullptr);
        CloseHandle(ct);
    }

    return TRUE;
}
