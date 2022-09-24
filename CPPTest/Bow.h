#pragma once
#include <iostream>
#include "Weapon.h"

class Bow : public Weapon {
public:
    Bow();
    void Use() override;
};