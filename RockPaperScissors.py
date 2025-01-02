"""
이 코드는 가위바위보 게임을 구현한 코드입니다.

이 코드는 Windows 콘솔 창 호스트(conhost.exe)가 아닌, Windows Terminal, WSL, Linux, macOS, 그리고 다른 터미널 환경에서 실행해야 색상이 제대로 출력됩니다.
"""
from enum import IntEnum, auto
from random import randint

VALUE_MESSAGE = "1, 2, 3 또는 가위, 바위, 보 중 하나를 입력해주세요."

class GBB(IntEnum):
    가위 = auto()
    바위 = auto()
    보 = auto()


WIN_RULES = {
    GBB.가위: GBB.보,
    GBB.바위: GBB.가위,
    GBB.보: GBB.바위
}


def toColored(value: GBB) -> str:
    if value == GBB.가위:
        return "\033[91m가위\033[0m"
    elif value == GBB.바위:
        return "\033[92m바위\033[0m"
    elif value == GBB.보:
        return "\033[94m보\033[0m"
    else:
        raise ValueError("Invalid value.")

def main() -> None:
    wins, losses, draws = 0, 0, 0

    while True:
        totalGames = wins + losses + draws

        print("가위 바위 보\n")
        print(f"현재 {wins}승 {losses}패 {draws}무 (총 {totalGames}판{f", 승률 {round((wins / totalGames) * 100)}%" if totalGames > 0 else ""})\n")
        print("\033[91m1. 가위\n\033[92m2. 바위\n\033[94m3. 보\n\n\033[0m0. 끝내기\n")
        print(VALUE_MESSAGE)

        yourChoiceStr = input("\n당신의 선택은? : ")

        if yourChoiceStr == "0":
            break
        elif yourChoiceStr in ["1", "2", "3"]:
            yourChoice = GBB(int(yourChoiceStr))
        elif yourChoiceStr in ["가위", "바위", "보"]:
            yourChoice = GBB[yourChoiceStr]
        else:
            print(f"\n{VALUE_MESSAGE}\n")
            continue

        computersChoice = GBB(randint(1, 3))

        print(f"\n당신 {toColored(yourChoice)} / {toColored(computersChoice)} 컴퓨터\n")

        if yourChoice == computersChoice:
            print("비겼습니다!\n")
            draws += 1
        elif WIN_RULES[yourChoice] == computersChoice:
            print("당신이 이겼습니다!\n")
            wins += 1
        else:
            print("컴퓨터가 이겼습니다!\n")
            losses += 1

if __name__ == "__main__":
    main()
else:
    raise Exception("This can only be run directly, not imported.")
