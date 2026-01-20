namespace RulesGenerator
{
    internal readonly struct Rules(string name, string path, string description, bool enabledByDefault)
    {
        public readonly string Name = name;
        public readonly string Path = path;
        public readonly string Description = description;
        public readonly int Version = 7;
        public readonly bool EnabledByDefault = enabledByDefault;

        public readonly List<ulong> Titles = [];
        public readonly List<IOption> Options = [];
        public readonly List<PresetCategory> PresetCategories = [];

        public readonly Rules AddTitle(ulong title)
        {
            Titles.Add(title);
            return this;
        }

        public readonly Rules AddOption<T>(Option<T> option)
        {
            Options.Add(option);
            return this;
        }

        public readonly Rules AddPresetCategory(PresetCategory presetCategory)
        {
            PresetCategories.Add(presetCategory);
            return this;
        }

        public readonly string Compile()
        {
            String compiled = "[Definition]\n" +
                "titleIds = ";
            bool isFirst = true;
            foreach (var title in Titles)
            {
                if (isFirst) isFirst = false;
                else compiled += ",";
                compiled += title.ToString("X16");
            }
            compiled += "\n" +
                "name = " + Name + "\n" +
                "path = " + Path + "\n" +
                "description = " + Description + "\n" +
                "version = " + Version.ToString() + "\n";
            if (EnabledByDefault) compiled += "default = 1\n";
            compiled += "\n" +
                "[Default]\n";
            foreach (var option in Options)
            {
                compiled += option.GetDefaultString() + "\n";
            }
            foreach (var presetCategory in PresetCategories)
            {
                compiled += presetCategory.Compile();
            }
            return compiled;
        }
    }
}
