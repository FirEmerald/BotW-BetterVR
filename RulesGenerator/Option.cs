namespace RulesGenerator
{
    internal interface IOption
    {
        public string GetCommentString();

        public string GetVariableString();

        public string GetDefaultString();
    }

    internal abstract class Option<T>(string name, T defaultValue, string comment = "") : IOption
    {
        public readonly string Name = name;
        public readonly T DefaultValue = defaultValue;
        public readonly string Comment = comment;

        public abstract string GetTypeString();

        public abstract string ToString(T value);

        public string GetCommentString()
        {
            return Utils.ToCommentLine(Comment);
        }

        public string GetVariableString()
        {
            return "$" + Name;
        }

        public string GetDefaultString()
        {
            return Utils.ToCommentLine(Comment) + GetValueString(DefaultValue);
        }

        public string GetValueString(T value)
        {
            return GetVariableString() + GetTypeString() + " = " + ToString(value);
        }

        public class Integer(string name, int defaultValue, string formatting = "", string comment = "") : Option<int>(name, defaultValue, comment)
        {
            public readonly string Formatting = formatting;

            public override string GetTypeString()
            {
                return ":int";
            }

            public override string ToString(int value)
            {
                return value.ToString(Formatting);
            }
        }

        public class Double(string name, double defaultValue, string formatting = "0.0###", string comment = "") : Option<double>(name, defaultValue, comment)
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
}
