#pragma once
#include <iostream>
#include "Weapon.h"

class Sword : public Weapon {
public:
    Sword();
    void Use() override;
};