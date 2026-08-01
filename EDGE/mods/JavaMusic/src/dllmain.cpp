// ReSharper disable CppZeroConstantCanBeReplacedWithNullptr
#include <fstream>
#include <sstream>
#include <string>
#include <EdgeMinHook.h>
#include <windows.h>
#include "global.h"
#include "MidiPlayer.h"

typedef void (__thiscall*FUN_00474160_t)(void *, unsigned int);
typedef void (*FUN_00474fa0_t)(void *, char *);
typedef void (__thiscall*FUN_00481590_t)(void *, unsigned int, bool);
typedef void (__fastcall*FUN_004b6a60_t)(void *);
typedef void (__fastcall*FUN_004ba240_t)(void *);

FUN_00474fa0_t org474fa0 = nullptr;
FUN_00481590_t org481590 = nullptr;
FUN_004b6a60_t org4b6a60 = nullptr;
FUN_004ba240_t org4ba240 = nullptr;
std::wstring levelPath;
bool isMenu = false;

void __thiscall FUN_00474160(void *, unsigned int musicJava) {
    if (MidiPlayer::currentPlayer == nullptr || MidiPlayer::currentPlayer != nullptr && MidiPlayer::currentPlayer->musicIndex != musicJava) {
        if (MidiPlayer::currentPlayer != nullptr) {
            MidiPlayer::currentPlayer.reset();
        }

        MidiPlayer::currentPlayer = std::make_unique<MidiPlayer>(musicJava);
    }
}

void FUN_00474fa0(void *unknown, char *value) {
    std::wstringstream wss;

    wss << L"levels/" << value << ".bin";

    levelPath = wss.str();
    isMenu = false;

    org474fa0(unknown, value);
}

void __thiscall FUN_00481590(void *me, unsigned int, bool isDebrief) {
    std::ifstream levelFile(levelPath, std::ios::binary);
    char musicJava;

    levelFile.seekg(-2, std::ios::end);
    levelFile.read(&musicJava, sizeof(musicJava));
    levelFile.close();

    if (!isDebrief) {
        org481590(me, isMenu ? 0 : musicJava, isDebrief);
    }
}

void __fastcall FUN_004b6a60(void *unknown) {
    isMenu = true;

    org4b6a60(unknown);
}

void __fastcall FUN_004ba240(void *unknown) {
    isMenu = true;

    org4ba240(unknown);
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID) {
    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hModule);

        wchar_t buffer[256];

        if (GetModuleFileNameW(nullptr, buffer, sizeof(buffer)) == 0) {
            return FALSE;
        }

        std::wstring ws(buffer);
        const auto index = ws.find_last_of('\\');

        if (index != std::wstring::npos) {
            ws.erase(index + 1);
        }

        baseDir = ws;

        RegisterEdgeHook(reinterpret_cast<LPVOID>(0x474160), reinterpret_cast<LPVOID>(FUN_00474160), nullptr);
        RegisterEdgeHook(reinterpret_cast<LPVOID>(0x474fa0), reinterpret_cast<LPVOID>(FUN_00474fa0), reinterpret_cast<LPVOID*>(&org474fa0));
        RegisterEdgeHook(reinterpret_cast<LPVOID>(0x481590), reinterpret_cast<LPVOID>(FUN_00481590), reinterpret_cast<LPVOID*>(&org481590));
        RegisterEdgeHook(reinterpret_cast<LPVOID>(0x4b6a60), reinterpret_cast<LPVOID>(FUN_004b6a60), reinterpret_cast<LPVOID*>(&org4b6a60));
        RegisterEdgeHook(reinterpret_cast<LPVOID>(0x4ba240), reinterpret_cast<LPVOID>(FUN_004ba240), reinterpret_cast<LPVOID*>(&org4ba240));
    }

    return TRUE;
}
