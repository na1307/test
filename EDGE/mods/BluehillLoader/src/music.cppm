// ReSharper disable CppParameterMayBeConst
export module music;

import std;
import global;
import functions;

typedef void (__thiscall *sub_474160_t)(void *, int);
auto sub_473E60 = reinterpret_cast<const char* (__cdecl *)(int)>(0x473e60);
export sub_474160_t org474160 = nullptr;

// Replace a music to mods
export void __thiscall sub_474160(void *thisptr, int music) {
    const auto music_name = sub_473E60(music);
    auto music_path = mods_root;

    music_path.append("music/");
    music_path.append(music_name);
    music_path.append(".ogg");

    if (file_exists(music_path)) {
        const auto ptr = *(static_cast<void**>(thisptr) + 2);
        const auto music_function = reinterpret_cast<bool (__thiscall *)(void *, std::string *, bool)>(*(*static_cast<int**>(ptr) + 1));

        music_function(ptr, &music_path, true);
    } else {
        org474160(thisptr, music);
    }
}
