#include "MidiPlayer.h"

#include <windows.h>
#include <sstream>
#include "global.h"

const wchar_t* MidiPlayer::getMusicName(const unsigned int musicJava) {
    switch (musicJava) {
    case 0:
        return L"00_menus";

    case 1:
        return L"01_braintonik";

    case 2:
        return L"02_cube_dance";

    case 3:
        return L"03_essai_2";

    case 4:
        return L"04_essai_01";

    case 5:
        return L"05_test";

    case 6:
        return L"06_mysterycube";

    case 7:
        return L"07_03_EDGE";

    case 8:
        return L"08_jungle";

    case 9:
        return L"09_RetardTonic";

    case 10:
        return L"10_oldschool_simon";

    case 11:
        return L"11_planant";

    default:
        return L"00_menus";
    }
}

// ReSharper disable once CppPassValueParameterByConstReference
void MidiPlayer::checkAndRepeatMidi(std::stop_token tk, const std::wstring &midiName) { // NOLINT(*-unnecessary-value-param)
    std::wstringstream wss;

    wss << L"open \"" << baseDir << L"midi\\" << midiName << L".mid\" type sequencer alias " << midiName;

    mciSendStringW(wss.str().c_str(), nullptr, 0, nullptr);
    mciSendStringW((L"play " + midiName).c_str(), nullptr, 0, nullptr);

    while (!tk.stop_requested()) {
        wchar_t status[128];

        mciSendStringW((L"status " + midiName + L" mode").c_str(), status, sizeof
            (status), nullptr);

        if (wcscmp(status, L"stopped") == 0) {
            mciSendStringW((L"seek " + midiName + L" to start").c_str(), nullptr, 0, nullptr);
            mciSendStringW((L"play " + midiName).c_str(), nullptr, 0, nullptr);
        }

        std::this_thread::sleep_for(std::chrono::seconds(1));
    }

    mciSendStringW((L"stop " + midiName).c_str(), nullptr, 0, nullptr);
    mciSendStringW((L"close " + midiName).c_str(), nullptr, 0, nullptr);
}

MidiPlayer::MidiPlayer(const unsigned int musicJava) : midiThread(std::make_unique<std::jthread>(checkAndRepeatMidi, getMusicName(musicJava))),
                                                       musicIndex(musicJava) {}

MidiPlayer::~MidiPlayer() {
    midiThread->request_stop();
}
