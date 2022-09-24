#include <iostream>
#include "Sword.h"
#include "Bow.h"
using namespace std;

int main(void) {
    Weapon *weapon = new Sword;
    Bow bow;

    cout << weapon->GetName() << endl;
    weapon->Use();

    cout << bow.GetName() << endl;
    bow.Use();

    delete weapon;
}