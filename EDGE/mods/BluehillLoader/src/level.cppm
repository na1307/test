export module level;

import std;
import global;
import functions;

typedef std::string* (__cdecl *sub_474FA0_t)(std::string &, std::string &);
export sub_474FA0_t org474FA0 = nullptr;

// Replace levels with mods
export std::string* __cdecl sub_474FA0(std::string &ptr, std::string &level_file_name) {
    const auto value = org474FA0(ptr, level_file_name);
    auto mod_level_bin = mods_root;

    mod_level_bin.append("levels/");
    mod_level_bin.append(level_file_name);
    mod_level_bin.append(".bin");

    if (file_exists(mod_level_bin)) {
        value->assign(mod_level_bin);
    }

    return value;
}
