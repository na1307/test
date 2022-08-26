from random import randrange as ranr

ONLY1TO3 = '\n1부터 3까지의 숫자를 입력하세요\n'

class NotInRange(Exception):
    def __init__(self): super().__init__(ONLY1TO3)

def toGBB(inputnum: int):
    if inputnum not in range(1, 4): raise NotInRange

    return { 1: "가위", 2: "바위", 3: "보" }.get(inputnum)

def main():
    while True:
        print("가위 바위 보\n")

        print("1. 가위")
        print("2. 바위")
        print("3. 보")

        print()
        print("0. 끝내기")

        try:
            yourChoice = int(input("\n당신의 선택은? : "))
            if yourChoice == 0: break
            computersChoice = ranr(1,4)

            print("\n당신 " + toGBB(yourChoice) + " / " + toGBB(computersChoice) + " 컴퓨터\n")

            if yourChoice == computersChoice:
                print("비겼습니다!")
            elif yourChoice == 1 and computersChoice == 3:
                print("당신이 이겼습니다!")
            elif yourChoice == 3 and computersChoice == 1:
                print("컴퓨터가 이겼습니다!")
            elif yourChoice > computersChoice:
                print("당신이 이겼습니다!")
            else:
                print("컴퓨터가 이겼습니다!")

            print()
        except NotInRange as e:
            print(e)
            continue
        except ValueError:
            print(ONLY1TO3)
            continue
        except Exception as e2:
            raise e2

if __name__ == "__main__":
    main()
else:
    raise Exception("This can only be run directly, not imported.")
