#pragma once
#include <limits>

template <typename T>
class GameOption {
public:
    const char* name;
    const char* displayName;
    const T defaultValue;
    T value;

    GameOption(const char* name, const char* displayName, const T defaultValue): name(name), displayName(displayName), defaultValue(defaultValue) {
        this->value = defaultValue;
    }

    virtual const char* serializeValue();

    virtual void deserializeValue(const char* valueString);

    virtual void addToMenu(bool& changed);

    virtual ~GameOption() = default;
};

class BooleanOption : public GameOption<bool> {
public:
    BooleanOption(const char* name, const char* displayName, const bool defaultValue): GameOption(name, displayName, defaultValue) {}

    const char* serializeValue() override;

    void deserializeValue(const char* valueString) override;

    void addToMenu(bool& changed) override;
};

template <typename T>
class NumberOption : public GameOption<T> {
public:
    const T minValue;
    const T maxValue;
    NumberOption(const char* name, const char* displayName, const T defaultValue, const T minValue = -std::numeric_limits<T>::min(), const T maxValue = std::numeric_limits<T>::max()): GameOption<T>(name, displayName, defaultValue), minValue(minValue), maxValue(maxValue) {}

    const char* serializeValue() override;

    void deserializeValue(const char* valueString) override;

    void addToMenu(bool& changed) override {
        addToMenu(changed, [](T value)->const char* { return std::to_string(value).c_str(); });
    }

    void addToMenu(bool& changed, std::function<const char*(T)> getValueString) {
        addToMenu(changed, minValue, maxValue, getValueString);
    }

    virtual void addToMenu(bool& changed, T minValue, T maxValue, std::function<const char*(T)> getValueString = [](T value) -> const char* { return std::to_string(value).c_str(); });

    void addSetToMenu(bool& changed, const char* label, T setTo);

    virtual ~NumberOption() = default;
};

class FloatOption : public NumberOption<float> {
public:
    FloatOption(const char* name, const char* displayName, const float defaultValue, const float minValue = -std::numeric_limits<float>::min(), const float maxValue = std::numeric_limits<float>::max()): NumberOption<float>(name, displayName, defaultValue, minValue, maxValue) {}

    void addToMenu(bool& changed, float minValue, float maxValue, std::function<const char*(float)> getValueString = [](float value) -> const char* { return std::to_string(value).c_str(); }) override;
};

class IntOption : public NumberOption<int32_t> {
public:
    IntOption(const char* name, const char* displayName, const int32_t defaultValue, const int32_t minValue = -std::numeric_limits<int32_t>::min(), const int32_t maxValue = std::numeric_limits<int32_t>::max()): NumberOption<int32_t>(name, displayName, defaultValue, minValue, maxValue) {}

    void addToMenu(bool& changed, int32_t minValue, int32_t maxValue, std::function<const char*(int32_t)> getValueString = [](int32_t value) -> const char* { return std::to_string(value).c_str(); }) override;
};

template <typename T>
class EnumOption : public GameOption<T> {
private:
    int32_t getIndex(T value);

    const char* getName(T value);

    const T getValue(const char* name);

public:
    struct Value {
        const char* name;
        const char* displayName;
        const T value;
    };

    const char* defaultValueName;
    const Value values[];
    const int32_t numValues;
    EnumOption(const char* name, const char* displayName, const T defaultValue, const Value values[], const int32_t numValues): GameOption<T>(name, displayName, defaultValue), values(values), numValues(numValues), defaultValueName(getName(defaultValue)) {}

    const char* serializeValue() override;

    void deserializeValue(const char* valueString) override;

    void addToMenu(bool& changed) override;
};
