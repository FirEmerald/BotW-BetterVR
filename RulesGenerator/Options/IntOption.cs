namespace RulesGenerator.Options
{
    internal class IntOption(string name, int defaultValue, string comment = "") : Option<int>(name, defaultValue, comment)
    {
        public override string GetTypeString()
        {
            return ":int";
        }

        public override string ToString(int value)
        {
            return value.ToString();
        }
    }
}
