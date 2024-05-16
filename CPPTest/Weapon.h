#pragma once
#include <iostream>

class Weapon {
private:
    const std::string name;

public:
    explicit Weapon(std::string const &name);
    std::string GetName() const;
    virtual void Use() = 0;
    virtual ~Weapon() = default;
};