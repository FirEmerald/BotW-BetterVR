using RulesGenerator.Options;

namespace RulesGenerator
{
    internal readonly struct Preset(string name, string condition = "", bool isDefault = false, string comment = "")
    {
        public readonly string Name = name;
        public readonly string Condition = condition;
        public readonly bool IsDefault = isDefault;
        public readonly List<IOptionEntry> Values = [];
        public readonly string Comment = comment;

        public readonly Preset AddValue<T>(Option<T> option, T value)
        {
            Values.Add(new OptionEntry<T>(option, value));
            return this;
        }

        public readonly string Compile(string category)
        {
            string compiled =
                "[Preset]\n" + 
                Utils.ToCommentLine(Comment) +
                "name = " + Name + "\n" +
                "category = " + category + "\n" +
                (Condition != "" ? ("condition = " + Condition + "\n") : "") +
                (IsDefault ? "default = 1\n" : "");
            foreach (var optionEntry in Values)
            {
                compiled += optionEntry.Compile();
            }
            return compiled + "\n";
        }
    }
}
