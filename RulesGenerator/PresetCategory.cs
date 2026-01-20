namespace RulesGenerator
{
    internal readonly struct PresetCategory(string name, string comment = "")
    {
        public readonly string Name = name;
        public readonly string Comment = comment;
        public readonly List<Preset> Presets = [];

        public readonly PresetCategory AddPreset(Preset preset)
        {
            Presets.Add(preset);
            return this;
        }

        public readonly string Compile()
        {
            string compiled = Comment != "" ? (Utils.ToCommentLine(Comment) + "\n") : "";
            foreach (var preset in Presets)
            {
                compiled += preset.Compile(Name);
            }
            return compiled + "\n";
        }
    }
}
