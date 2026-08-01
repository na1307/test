export module functions;

import std;

export inline bool file_exists(const std::string &path) {
    return std::filesystem::exists(path) && std::filesystem::is_regular_file(path);
}
