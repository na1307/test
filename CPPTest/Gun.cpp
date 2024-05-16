#include "Gun.h"
using namespace std;

Gun::Gun() : Weapon("총") {}

void Gun::Use() {
    cout << "총을 쏨" << endl;
}