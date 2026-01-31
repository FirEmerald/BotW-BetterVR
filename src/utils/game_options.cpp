#include "game_options.h"
#include <limits>
#include "imgui.h"

const char* BooleanOption::serializeValue() {
    return value ? "true" : "false";
}

void BooleanOption::deserializeValue(const char* valueString) {
    if (valueString == "true")
        value = true;
    else if (valueString == "false")
        value = false;
    else {
        Log::print<ERROR>("{} had invalid value {}, only \"true\" or \"false\" are supported. Correcting to default {}", name, valueString, defaultValue);
    }
}

void BooleanOption::addToMenu(bool& changed) {
    if (ImGui::Checkbox("Left Handed Mode", &value)) {
        changed = true;
    }
}

template <typename T>
const char* NumberOption<T>::serializeValue() {
    return std::to_string(this->value).c_str();
}

template <typename T>
void NumberOption<T>::deserializeValue(const char* valueString) {
    try {
        this->value = std::stof(valueString);
        if (this->value < minValue) {
            this->value = minValue;
            Log::print<ERROR>("{} was too low - must be at least {}. Setting to minimum value {}", this->name, minValue, minValue);
        }
        else if (this->value > maxValue) {
            this->value = maxValue;
            Log::print<ERROR>("{} was too high - must be at most {}. Setting to maximum value {}", this->name, maxValue, maxValue);
        }
    }
    catch (const std::invalid_argument& invalidArg) {
        this->value = this->defaultValue;
        Log::print<ERROR>("{} had invalid value {}, only numbers are supported. Correcting to default {}", this->name, valueString, this->defaultValue);
    }
    catch (const std::out_of_range& outOfRange) {
        this->value = this->defaultValue;
        Log::print<ERROR>("{} was outside the range of it's values. Correcting to default {}", this->name, valueString, this->defaultValue);
    }
}

template <typename T>
void NumberOption<T>::addSetToMenu(bool& changed, const char* label, T setTo) {
    if (ImGui::Button(label)) {
        this->value = setTo;
        changed = true;
    }
}

void FloatOption::addToMenu(bool& changed, float minValue, float maxValue, std::function<const char*(float)> getValueString) {
    if (ImGui::SliderFloat(displayName, &value, minValue, maxValue, getValueString(value))) {
        changed = true;
    }
    addSetToMenu(changed, std::string("Reset {}", displayName).c_str(), defaultValue);
}

void IntOption::addToMenu(bool& changed, int32_t minValue, int32_t maxValue, std::function<const char*(int32_t)> getValueString) {
    if (ImGui::SliderInt(displayName, &value, minValue, maxValue, getValueString(value))) {
        changed = true;
    }
    addSetToMenu(changed, std::string("Reset {}", displayName).c_str(), defaultValue);
}

template <typename T>
int32_t EnumOption<T>::getIndex(T value) {
    for (int i = 0; i < numValues; ++i) {
        Value val = values[i];
        if (val.value == value) return i;
    }
    throw std::invalid_argument("Value not found");
}

template <typename T>
const char* EnumOption<T>::getName(T value) {
    for (int i = 0; i < numValues; ++i) {
        Value val = values[i];
        if (val.value == value) return val.name;
    }
    throw std::invalid_argument("Value not found");
}

template <typename T>
const T EnumOption<T>::getValue(const char* name) {
    for (int i = 0; i < numValues; ++i) {
        Value val = values[i];
        if (val.name == name) return val.value;
    }
    throw std::invalid_argument("Name not found: {}", name);
}

template <typename T>
const char* EnumOption<T>::serializeValue() {
    return EnumOption<T>::getName(this->value);
}

template <typename T>
void EnumOption<T>::deserializeValue(const char* valueString) {
    try {
        this->value = getValue(valueString);
    }
    catch (const std::invalid_argument& invalidArg) {
        this->value = this->defaultValue;
        std::string possibleValues = values[0].second;
        for (int i = 1; i < numValues; ++i) possibleValues += ", " + values[i].second;
        Log::print<ERROR>("{} had invalid value {}, only {} are supported. Correcting to default {}", this->name, valueString, possibleValues, defaultValueName);
    }
}

template <typename T>
void EnumOption<T>::addToMenu(bool& changed) {
    ImGui::Text(this->displayName);
    int index = getIndex(this->value);
    for (int i = 0; i < numValues; ++i) {
        T val = values[i].value;
        if (ImGui::RadioButton(values[i].displayName, &index, i)) {
            this->value = val;
            changed = true;
        }
        ImGui::SameLine();
    }
    ImGui::NewLine();
}
