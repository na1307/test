#include "Weapon.h"
using namespace std;

Weapon::Weapon(string const &name) : name(name) {}

string Weapon::GetName() const {
    return name;
}