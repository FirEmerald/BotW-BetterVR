namespace RulesGenerator.Options
{
    internal class BoolOption(string name, bool defaultValue, string comment = "") : Option<bool>(name, defaultValue, comment)
    {
        public override string GetTypeString()
        {
            return ":int";
        }

        public override string ToString(bool value)
        {
            return value ? "1" : "0";
        }
    }
}
