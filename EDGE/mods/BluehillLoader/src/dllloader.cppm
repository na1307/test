module;

#include <windows.h>
#include "BluehillLoader.h"

export module dllloader;

import std;
import edgeminhook;

bool is_loaded = false;
std::vector<HMODULE> loaded_dlls;

export void load_dlls() {
    if (is_loaded) {
        return;
    }

    wchar_t buffer[MAX_PATH];

    if (GetModuleFileNameW(nullptr, buffer, MAX_PATH) == 0) {
        return;
    }

    const auto mods_folder = std::filesystem::path(buffer).replace_filename("mods");

    if (!std::filesystem::is_directory(mods_folder)) {
        return;
    }

    for (auto entry : std::filesystem::directory_iterator(mods_folder)) {
        if (!entry.exists()) {
            continue;
        }

        if (!entry.is_regular_file()) {
            continue;
        }

        if (entry.path().extension() != ".dll") {
            continue;
        }

        const auto dll = LoadLibraryW(entry.path().c_str());

        if (dll == nullptr) {
            continue;
        }

        const auto func = reinterpret_cast<RegisterHooks_t>(GetProcAddress(dll, "RegisterHooks"));

        if (func == nullptr) {
            continue;
        }

        func(reinterpret_cast<RegisterEdgeHook_t>(RegisterEdgeHook));

        loaded_dlls.push_back(dll);
    }

    is_loaded = true;
}

export void unload_dlls() {
    for (const auto hm : loaded_dlls) {
        FreeLibrary(hm);
    }
}
