#include "game_options.h"
#include <cassert>
#include <cstdlib>


template <typename T>
T GameOption<T>::getValue() {
    return this->value.load();
}

template <typename T>
void GameOption<T>::setValueSilently(T value) {
    this->value.store(value);
}

template <typename T>
void GameOption<T>::setValue(T value) {
    this->setValueSilently(value);
    //TODO mark config dirty
}


const char* BoolOption::serializeValue() {
    return this->getValue() ? "true" : "false";
}

void BoolOption::deserializeValue(const char* string) {
    if (string == "true") {
        this->setValueSilently(true);
    }
    else if (string == "false") {
        this->setValueSilently(false);
    }
    else {
        try {
            this->setValue(std::atoi(string)); //old format
            return;
        }
        catch (const std::out_of_range& e) {}
        catch (const std::invalid_argument& e) {}
        Log::print<ERROR>("{} had invalid value {}, valid values are \"true\" or \"false\", setting to default value {}.", this->name, string, this->defaultValue);
        this->setValueSilently(this->defaultValue);
    }
}

void BoolOption::addToMenu(boolean& changed) {
    bool value = this->getValue();
    if (ImGui::Checkbox(this->displayName, &value)) {
        this->setValueSilently(value);
        changed = true;
    }
}


template <typename T>
const char* NumberOption<T>::serializeValue() {
    std::string string = std::to_string(this->getValue());
    const char* str = string.c_str();
    return str;
}

template <typename T>
void NumberOption<T>::deserializeValue(const char* string, std::function<T(const char*)> parse) {
    try {
        T deserialized = parse(string);
        if (deserialized < this->minValue) {
            Log::print<ERROR>("{} had too low value {}, minimum is {}, setting to minimum.", this->name, deserialized, this->minValue);
            this->setValueSilently(this->minValue);
        }
        else if (deserialized > this->maxValue) {
            Log::print<ERROR>("{} had too high value {}, maximum is {}, setting to maximum.", this->name, deserialized, this->maxValue);
            this->setValueSilently(this->maxValue);
        }
        else {
            this->setValueSilently(deserialized);
        }
    }
    catch (const std::out_of_range& e) {
        Log::print<ERROR>("{} had out of range value {}, setting to default value {}.", this->name, string, this->defaultValue);
        this->setValueSilently(this->defaultValue);
    }
    catch (const std::invalid_argument& e) {
        Log::print<ERROR>("{} had invalid value {}, setting to default value {}.", this->name, string, this->defaultValue);
        this->setValueSilently(this->defaultValue);
    }
}

template<>
void NumberOption<float>::deserializeValue(const char* string) {
    return this->deserializeValue(string, std::atof);
}

template <>
void NumberOption<float>::addToMenu(boolean& changed, float minValue, float maxValue, std::function<const char*(float)> getDisplayString) {
    assert(minValue < maxValue);
    assert(minValue >= this->minValue && maxValue <= this->maxValue);
    float value = this->getValue();
    if (ImGui::SliderFloat(this->displayName, &value, minValue, maxValue, getDisplayString(value))) {
        this->setValueSilently(value);
        changed = true;
    }
}

template <>
void NumberOption<int32_t>::deserializeValue(const char* string) {
    return this->deserializeValue(string, std::atoi);
}

template <>
void NumberOption<int32_t>::addToMenu(boolean& changed, int32_t minValue, int32_t maxValue, std::function<const char*(int32_t)> getDisplayString) {
    assert(minValue < maxValue);
    assert(minValue >= this->minValue && maxValue <= this->maxValue);
    int32_t value = this->getValue();
    if (ImGui::SliderInt(this->displayName, &value, minValue, maxValue, getDisplayString(value))) {
        this->setValueSilently(value);
        changed = true;
    }
}

template <typename T>
void NumberOption<T>::addToMenu(boolean& changed, const char* label, T setTo) {
    assert(setTo >= this->minValue && setTo <= this->maxValue);
    if (ImGui::Button(label)) {
        this->setValueSilently(setTo);
        changed = true;
    }
}


template <typename T>
int EnumOption<T>::getIndex(T value) {
    for (int i = 0; i < this->numValues; ++i) {
        if (this->values[i].value == value) return i;
    }
    throw std::invalid_argument("Could not get index");
}

template <typename T>
const char* EnumOption<T>::getName(T value) {
    for (int i = 0; i < this->numValues; ++i) {
        if (this->values[i].value == value) return this->values[i].name;
    }
    throw std::invalid_argument("Could not get index");
}

template <typename T>
T EnumOption<T>::getValue(const char* name) {
    for (int i = 0; i < this->numValues; ++i) {
        if (this->values[i].name == name) return this->values[i].value;
    }
    throw std::invalid_argument("Could not get value for " + std::string(name));
}

template <typename T>
const char* EnumOption<T>::serializeValue() {
    return this->getName(this->GameOption<T>::getValue());
}

template <typename T>
void EnumOption<T>::deserializeValue(const char* string) {
    try {
        this->setValueSilently(this->getValue(string));
    }
    catch (const std::invalid_argument& e) {
        try {
            int deserializedIndex = std::atoi(string); //old format
            if (deserializedIndex >= 0 && deserializedIndex < this->numValues) {
                this->setValueSilently(this->values[deserializedIndex].value);
            }
        }
        catch (const std::out_of_range& e) {}
        catch (const std::invalid_argument& e) {}
        std::string possibleValues = this->values[0].name;
        for (int i = 1; i < this->numValues; ++i) {
            possibleValues += "\", \"";
            possibleValues += this->values[i].name;
        }
        Log::print<ERROR>("{} had invalid value {}, valid values are {\"{}\"}, setting to default value {}.", this->name, string, this->getName(this->defaultValue));
        this->setValueSilently(this->defaultValue);
    }
}
