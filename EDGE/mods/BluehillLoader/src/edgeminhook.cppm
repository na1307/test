module;

#include <windows.h>
#include <MinHook.h>

export module edgeminhook;

HANDLE g_hHookEvent = nullptr;

DWORD WINAPI EnableHooksThread(LPVOID) {
    // ReSharper disable once CppDFAEndlessLoop
    while (TRUE) {
        if (WaitForSingleObject(g_hHookEvent, INFINITE) != WAIT_OBJECT_0) {
            ExitThread(1);
        }

        Sleep(100);

        // ReSharper disable once CppZeroConstantCanBeReplacedWithNullptr
        MH_EnableHook(MH_ALL_HOOKS);
    }
}

export void InitializeEdgeHook() {
    if (MH_Initialize() != MH_OK) {
        return;
    }

    g_hHookEvent = CreateEventW(nullptr, FALSE, FALSE, nullptr);

    if (g_hHookEvent == nullptr) {
        return;
    }

    const auto g_hThread = CreateThread(nullptr, 0, EnableHooksThread, nullptr, 0, nullptr);

    if (g_hThread == nullptr) {
        return;
    }

    CloseHandle(g_hThread);
}

export void UninitializeEdgeHook(const LPVOID lpReserved) {
    if (lpReserved != nullptr) {
        if (MH_Uninitialize() != MH_OK) {
            return;
        }

        if (g_hHookEvent != nullptr) {
            CloseHandle(g_hHookEvent);
        }
    }
}

export void RegisterEdgeHook(void *target, void *detour, void **original) {
    if (MH_CreateHook(target, detour, original) == MH_OK) {
        if (g_hHookEvent != nullptr) {
            SetEvent(g_hHookEvent);
        }
    }
}
