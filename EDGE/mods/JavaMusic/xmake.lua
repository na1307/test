includes("../include.lua")
add_rules("mode.debug", "mode.release")
set_defaultmode("debug")
set_plat("windows")
set_arch("x86")
set_toolchains("clang-cl")
set_languages("c17", "cxx20")

if is_mode("debug") then
    set_runtimes("MTd")
elseif is_mode("release") then
    set_runtimes("MT")
end

set_policy("build.optimization.lto", is_mode("release"))
add_repositories("EdgeMinHook ../EdgeMinHook/build")
add_requires("edgeminhook", {plat = "windows", arch = "x86"})

target("JavaMusic")
    add_rules("asi")
    add_files("src/dllmain.cpp", "src/MidiPlayer.cpp")
    add_includedirs("src")
    add_shflags("winmm.lib", {force = true})
    add_packages("edgeminhook")
