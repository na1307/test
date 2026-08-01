#include <windows.h>
#include <BluehillLoader.h>

typedef void (*FUN_004a5bb0_t)();

FUN_004a5bb0_t org4a5bb0 = nullptr;

void FUN_004a5bb0() {
    org4a5bb0();

    unsigned short order = 136;
    const auto unknown_type_instance = reinterpret_cast<void*>(0x5f9210);
    const auto FUN_004a7120 = reinterpret_cast<unsigned short*(__thiscall*)(void *, wchar_t *)>(0x4a7120);

    for (auto character :
         L"가간감갖개게경계공과금기끔나너녕노높뉴느는니님다더도동됩뒤드든디딩때래랭레려력로록료를리릭림마막만많머멀메며면명모문뮤받방버벨보북브쁘사새서설세셔셨소속수순쉽스습시십아악안않야어에예오와완요용운원위으은을음이익인임입있자작잘잠재저전점접정제좋주죽중즘지직차체총취커켬코큐크클키킹타터텐통트튼티포표프플하할합해했향화환회효희히") {
        auto pCharacter = &character;
        auto pOrder = FUN_004a7120(unknown_type_instance, pCharacter);
        *pOrder = ++order;
    }
}

void RegisterHooks(RegisterEdgeHook_t out) {
    out(reinterpret_cast<void*>(0x4a5bb0), reinterpret_cast<void*>(FUN_004a5bb0), reinterpret_cast<void**>(&org4a5bb0));
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID) {
    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hModule);
    }

    return TRUE;
}
