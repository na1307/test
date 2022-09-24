#include "Bow.h"
using namespace std;

Bow::Bow() : Weapon("활") {}

void Bow::Use() {
    cout << "화살을 쏨" << endl;
};