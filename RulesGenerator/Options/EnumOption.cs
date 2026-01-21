namespace RulesGenerator.Options
{
    internal class EnumOption<T>(string name, T defaultValue, string comment = "") : Option<T>(name, defaultValue, comment) where T : Enum
    {
        public override string GetTypeString()
        {
            return ":int";
        }

        public override string ToString(T value)
        {
            return Convert.ToInt32(value).ToString();
        }
    }
}
