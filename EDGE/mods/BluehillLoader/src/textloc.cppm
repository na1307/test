// ReSharper disable CppParameterMayBeConst
module;

#include <windows.h>

export module textloc;

import functions;

typedef void (__cdecl *sub_4A75F0_t)(int);
constexpr char mods_localization_text_loc[] = "mods/localization/text.loc";
export sub_4A75F0_t org4A75F0 = nullptr;

// Replace text.loc to mods
export void __cdecl sub_4A75F0(int lang_code) {
    static bool is_patched = false;

    if (!is_patched && file_exists(mods_localization_text_loc)) {
        const auto target_start = reinterpret_cast<LPVOID>(0x4a7606);
        DWORD old_protect;

        if (VirtualProtect(target_start, 10, PAGE_EXECUTE_READWRITE, &old_protect) != 0) {
            constexpr uintptr_t size_addr = 0x4a7608;
            constexpr uintptr_t string_addr = 0x4a760a;
            *reinterpret_cast<unsigned char*>(size_addr) = static_cast<unsigned char>(sizeof(mods_localization_text_loc) - 1);
            *reinterpret_cast<const char**>(string_addr) = mods_localization_text_loc;

            VirtualProtect(target_start, 10, old_protect, &old_protect);

            is_patched = true;
        }
    }

    org4A75F0(lang_code);
}
