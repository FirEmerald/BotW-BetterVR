namespace RulesGenerator
{
    internal interface IOptionEntry
    {
        public string Compile();
    }

    internal unsafe class OptionEntry<T>(Option<T> option, T value) : IOptionEntry
    {
        public readonly Option<T> Option = option;
        public readonly T Value = value;

        public string Compile()
        {
            return Option.GetValueString(Value) + "\n";
        }
    }
}
