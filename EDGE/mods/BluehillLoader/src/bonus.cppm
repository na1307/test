export module bonus;

typedef int (*sub_4947E0_t)();
export sub_4947E0_t org4947E0 = nullptr;

// Bypass Bonus levels
export int sub_4947E0() {
    const auto value = org4947E0();
    *(*reinterpret_cast<bool**>(0x5f9080) + 724) = true;

    return value;
}
