#pragma once

#ifndef JAVAMUSIC_MIDIPLAYER_H
#define JAVAMUSIC_MIDIPLAYER_H
#include <string>
#include <thread>

class MidiPlayer final {
    std::unique_ptr<std::jthread> midiThread = nullptr;

    static const wchar_t* getMusicName(unsigned int musicJava);

    static void checkAndRepeatMidi(std::stop_token tk, const std::wstring &midiName);

public:
    inline static std::unique_ptr<MidiPlayer> currentPlayer;
    unsigned int musicIndex;

    explicit MidiPlayer(unsigned int musicJava);

    ~MidiPlayer();
};

#endif //JAVAMUSIC_MIDIPLAYER_H
