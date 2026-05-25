includes("../include.lua")
add_rules("mode.debug", "mode.release")
set_defaultmode("debug")
set_plat("windows")
set_arch("x86")
set_toolchains("clang-cl[llvm]")
set_languages("c17", "cxx23")

if is_mode("debug") then
    set_runtimes("MTd")
elseif is_mode("release") then
    set_runtimes("MT")
end

set_policy("build.optimization.lto", is_mode("release"))
add_repositories("BluehillLoader ../BluehillLoader/build")
add_requires("xinput1_3", {plat = "windows", arch = "x86"})

target("EdgeKoreanPatch")
    add_rules("edgemod")
    add_files("src/dllmain.cpp")
    add_packages("xinput1_3")
