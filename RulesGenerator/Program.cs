using RulesGenerator;
using RulesGenerator.Options;

Rules rules = new Rules("Better VR", "\"The Legend of Zelda: Breath of the Wild/Better VR\"", "Used to enable BetterVR.||||Most settings will be found inside the in-game menu by long pressing the mod menu button (X on Touch/Right A on Index).||Made by Crementif.", true)
    .AddTitle(0x00050000101C9300UL)
    .AddTitle(0x00050000101C9400UL)
    .AddTitle(0x00050000101C9500UL);

BoolOption accessibilityUIColor = new("accessibilityUIColor", false);
DoubleOption accessibilityUIColor_x = new("accessibilityUIColor_x", 0.0);
DoubleOption accessibilityUIColor_y = new("accessibilityUIColor_y", 0.804);
DoubleOption accessibilityUIColor_z = new("accessibilityUIColor_z", 1.0);
DoubleOption accessibilityUIColor_w = new("accessibilityUIColor_w", 1.0);

rules
    .AddOption(accessibilityUIColor)
    .AddOption(accessibilityUIColor_x)
    .AddOption(accessibilityUIColor_y)
    .AddOption(accessibilityUIColor_z)
    .AddOption(accessibilityUIColor_w)

    .AddPresetCategory(
        new PresetCategory("More Contrasty UI For Crosshair, Buttons etc.", "Accessibility UI Color")
            .AddPreset(
                new Preset("Use Default Colors (Default, White)", isDefault: true)
                    .AddValue(accessibilityUIColor, false)
            )
            .AddPreset(
                new Preset("Thematic Cyan (Best Looking Preset)")
                    .AddValue(accessibilityUIColor, true)
                    .AddValue(accessibilityUIColor_x, 0)
                    .AddValue(accessibilityUIColor_y, 0.804)
                    .AddValue(accessibilityUIColor_z, 1)
                    .AddValue(accessibilityUIColor_w, 1)
            )
            .AddPreset(
                new Preset("Bright Purple (Most Visible Preset)")
                    .AddValue(accessibilityUIColor, true)
                    .AddValue(accessibilityUIColor_x, 0.6728)
                    .AddValue(accessibilityUIColor_y, 0)
                    .AddValue(accessibilityUIColor_z, 0.3272)
                    .AddValue(accessibilityUIColor_w, 1)
            )
        )
    ;

string output = rules.Compile();
Console.WriteLine(output);

string outputPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "resources", "BreathOfTheWild_BetterVR", "rules.txt");
Console.WriteLine("Writing to " + outputPath);

using StreamWriter outputFile = new(outputPath);
outputFile.Write(output);
