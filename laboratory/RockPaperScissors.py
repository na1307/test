from random import randrange as ranr

class NotInRange(Exception):
    def __init__(self): super().__init__('\n1부터 3까지의 숫자를 입력하세요\n')

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
            yourChoiceNum = int(input("\n당신의 선택은? : "))
            if yourChoiceNum == 0: break
            yourChoice = toGBB(yourChoiceNum)
            computersChoiceNum = ranr(1,4)
            computersChoice = toGBB(computersChoiceNum)
        except NotInRange as e:
            print(e)
            continue
        except ValueError:
            print('\n1부터 3까지의 숫자를 입력하세요\n')
            continue
        except Exception as e2:
            raise e2

        print()
        print("당신의 선택은 " + yourChoice + " 입니다.")
        print("컴퓨터의 선택은 " + computersChoice + " 입니다.")
        print()

        if yourChoiceNum == computersChoiceNum:
            print("비겼습니다!")
        elif yourChoiceNum == 3 and computersChoiceNum == 1:
            print("컴퓨터가 이겼습니다!")
        elif yourChoiceNum == 1 and computersChoiceNum == 3:
            print("당신이 이겼습니다!")
        elif yourChoiceNum < computersChoiceNum:
            print("컴퓨터가 이겼습니다!")
        else:
            print("당신이 이겼습니다!")

        print()

if __name__ == "__main__":
    main()
else:
    raise Exception("This can only be run directly, not imported.")
