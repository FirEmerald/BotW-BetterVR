namespace RulesGenerator.Options
{
    internal class DoubleOption(string name, double defaultValue, string formatting = "0.0###", string comment = "") : Option<double>(name, defaultValue, comment)
    {
        public readonly string Formatting = formatting;

        public override string GetTypeString()
        {
            return "";
        }

        public override string ToString(double value)
        {
            return value.ToString(Formatting);
        }
    }
}
