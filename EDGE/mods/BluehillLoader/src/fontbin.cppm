module;

#include <windows.h>

export module fontbin;

import functions;

typedef void (*sub_4A63F0_t)();
constexpr char mods_font_bin[] = "mods/font.bin";
export sub_4A63F0_t org4A63F0 = nullptr;

// Replace font.bin to mods
export void sub_4A63F0() {
    static bool is_patched = false;

    if (!is_patched && file_exists(mods_font_bin)) {
        const auto target_start = reinterpret_cast<LPVOID>(0x4a6402);
        DWORD old_protect;

        if (VirtualProtect(target_start, 10, PAGE_EXECUTE_READWRITE, &old_protect) != 0) {
            constexpr uintptr_t size_addr = 0x4a6403;
            constexpr uintptr_t string_addr = 0x4a6407;
            *reinterpret_cast<unsigned char*>(size_addr) = static_cast<unsigned char>(sizeof(mods_font_bin) - 1);
            *reinterpret_cast<const char**>(string_addr) = mods_font_bin;

            VirtualProtect(target_start, 10, old_protect, &old_protect);

            is_patched = true;
        }
    }

    org4A63F0();
}
