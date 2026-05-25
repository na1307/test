rule("edgemod")
    on_load(function (target)
        target:set("kind", "shared")
        if is_mode("debug") and not has_config("nomtd") then
            target:set("runtimes", "MTd")
        else
            target:set("runtimes", "MT")
        end
        target:set("policy", "build.c++.modules", true)
        target:add("cxflags", "-m32", "-msse3", "--target=i686-pc-windows-msvc")
    end)

    after_load(function (target)
        if not target:values("windows.subsystem") then
            target:values_set("windows.subsystem", "windows")
        end

        target:add("defines", "WIN32", "_WINDOWS", "_USRDLL", "_WINDLL", "_UNICODE", "UNICODE")
        target:add("cxflags", "-Gd")
        if is_mode("debug") then
            if not has_config("nomtd") then
                target:add("defines", "DEBUG", "_DEBUG")
            else
                target:add("defines", "NDEBUG")
            end
        elseif is_mode("release") then
            target:add("cxflags", "-Gw")
        end

        target:add("syslinks", "kernel32", "user32", "gdi32", "winspool", "comdlg32", "advapi32")
        target:add("syslinks", "shell32", "ole32", "oleaut32", "uuid", "odbc32", "odbccp32", "comctl32")
        target:add("syslinks", "comdlg32", "setupapi", "shlwapi")
        if not target:is_plat("mingw") then
            target:add("syslinks", "strsafe")
        end
    end)
