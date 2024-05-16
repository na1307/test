#include "Sword.h"
using namespace std;

Sword::Sword() : Weapon("검") {}

void Sword::Use() {
    cout << "검을 휘두름" << endl;
}