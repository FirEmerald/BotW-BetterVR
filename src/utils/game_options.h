#pragma once
#include <limits>
#include <cassert>

template <typename T>
class GameOption {
private:
    std::atomic<T> value;

public:
    const char* name;
    const char* displayName;
    const T defaultValue;

    GameOption(const char* name, const char* displayName, T defaultValue): name(name), displayName(displayName), defaultValue(defaultValue) {
        this->value.store(defaultValue);
    }

    T getValue();

    void setValueSilently(T value);

    void setValue(T value);

    virtual const char* serializeValue();

    virtual void deserializeValue(const char* string);

    operator T() {
        return this->getValue();
    }

    T operator=(const T value) {
        this->setValue(value);
        return value;
    }
};

class BoolOption : public GameOption<bool> {
public:
    BoolOption(const char* name, const char* displayName, bool defaultValue): GameOption<bool>(name, displayName, defaultValue) {}

    const char* serializeValue() override;

    void deserializeValue(const char* string) override;

    void addToMenu(boolean& changed);

    operator bool() {
        return this->getValue();
    }

    bool operator=(const bool value) {
        this->setValue(value);
        return value;
    }

    bool operator!() {
        return !this->getValue();
    }
};

template <typename T>
class NumberOption : public GameOption<T> {
private:
    void deserializeValue(const char* string, std::function<T(const char*)> parse);

public:
    const T minValue;
    const T maxValue;

    NumberOption(const char* name, const char* displayName, T defaultValue, T minValue = std::numeric_limits<T>::min(), T maxValue = std::numeric_limits<T>::max()): GameOption<T>(name, displayName, defaultValue), minValue(minValue), maxValue(maxValue) {
        assert(minValue < maxValue);
        assert(defaultValue >= minValue && defaultValue <= maxValue);
    }

    const char* serializeValue() override;

    void deserializeValue(const char* string) override;

    void addToMenu(boolean& changed, T minValue = this->minValue, T maxValue = this->maxValue, std::function<const char*(T)> getDisplayString = [](T value) const char* -> { return std::to_string(value); });

    void addToMenu(boolean& changed, const char* label, T setTo = this->defaultValue);

    operator T() {
        return this->getValue();
    }

    T operator=(const T value) {
        this->setValue(value);
        return value;
    }
};

template <typename T>
class EnumOption : public GameOption<T> {
private:
    int getIndex(T value);

    const char* getName(T value);

    T getValue(const char* name);

public:
    struct Value {
        const char* name;
        const char* displayName;
        const T value;
    };

    const Value* values;
    const int32_t numValues;
    EnumOption(const char* name, const char* displayName, T defaultValue, const Value* values, const int32_t numValues): GameOption<T>(name, displayName, defaultValue), values(values), numValues(numValues) {}

    const char* serializeValue() override;

    void deserializeValue(const char* string) override;

    operator T() {
        return this->GameOption<T>::getValue();
    }

    T operator=(const T value) {
        this->setValue(value);
        return value;
    }
};

enum class Enum : int32_t {
    A = 1,
    B = 2,
    C = 3
};

class Tests {
public:
    void test() {
        BoolOption testBool("testBool", "Boolean test", false);
        testBool = true;
        bool aaa = testBool;
        NumberOption<float> testFloat("testFloat", "Float test", 0, -1.0f, 1.0f);
        testFloat = 0.5f;
        float bbb = testFloat;
        NumberOption<int32_t> testInt("testInt", "Int test", 0, -10, 10);
        testInt = 1;
        int32_t ccc = testInt;
        EnumOption<Enum> testEnum("testEnum", "Enum test", Enum::A, new EnumOption<Enum>::Value[3] { { "a", "A", Enum::A }, { "b", "B", Enum::B }, { "c", "C", Enum::C } }, 3);
        testEnum = Enum::C;
        Enum ddd = testEnum;
    }
};
