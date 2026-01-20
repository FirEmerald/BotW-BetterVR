using RulesGenerator;

Rules rules = new("Better VR", "\"The Legend of Zelda: Breath of the Wild/Better VR\"", "Used to enable and change VR settings for BetterVR.||Made by Crementif.", true);
rules.AddTitle(0x00050000101C9300UL);
rules.AddTitle(0x00050000101C9400UL);
rules.AddTitle(0x00050000101C9500UL);

Option<int>.Integer cameraMode = new("cameraMode", 1);
Option<double>.Double modelOffsetSmoothingFactor = new("modelOffsetSmoothingFactor", 0.1);
Option<int>.Integer hideModelHead = new("hideModelHead", 1);
Option<int>.Integer allowGamepadInput = new("allowGamepadInput", 1);
Option<int>.Integer enable2DView = new("enable2DView", 1);
Option<int>.Integer cropFlatTo16_9 = new("cropFlatTo16_9", 1);
Option<int>.Integer guiFollowMode = new("guiFollowMode", 1);

Option<double>.Double cameraDistance = new("cameraDistance", 0.5);
Option<double>.Double cameraHeight = new("cameraHeight", 1.6);
Option<double>.Double worldScale = new("worldScale", 0.0);
Option<int>.Integer enableDebugOverlay = new("enableDebugOverlay", 0);
Option<int>.Integer leftHanded = new("leftHanded", 0);
Option<int>.Integer buggyAngularVelocity = new("buggyAngularVelocity", 0, comment: "Whether the angular velocity is wrongly provided in local space. Possible Values: 0 is auto; 1 is forced on; 2 is forced off");

Option<int>.Integer cutsceneCameraMode = new("cutsceneCameraMode", 1);
Option<int>.Integer cutsceneBlackBars = new("cutsceneBlackBars", 1);

Option<int>.Integer accessibilityUIColor = new("accessibilityUIColor", 0);
Option<double>.Double accessibilityUIColor_x = new("accessibilityUIColor_x", 0.0);
Option<double>.Double accessibilityUIColor_y = new("accessibilityUIColor_y", 0.804);
Option<double>.Double accessibilityUIColor_z = new("accessibilityUIColor_z", 1.0);
Option<double>.Double accessibilityUIColor_w = new("accessibilityUIColor_w", 1.0);

string isFirstPersonCondition = cameraMode.GetVariableString() + " != 0";
string isThirdPersonCondition = cameraMode.GetVariableString() + " == 0";
string isDynamicOffsetCondition = cameraMode.GetVariableString() + " == 2";
string is2DViewCondition = enable2DView.GetVariableString() + " == 1";

rules
    .AddOption<int>(cameraMode)
    .AddOption<double>(modelOffsetSmoothingFactor)
    .AddOption<int>(hideModelHead)
    .AddOption<int>(allowGamepadInput)
    .AddOption<int>(enable2DView)
    .AddOption<int>(cropFlatTo16_9)
    .AddOption<int>(guiFollowMode)

    .AddOption<double>(cameraDistance)
    .AddOption<double>(cameraHeight)
    .AddOption<double>(worldScale)
    .AddOption<int>(enableDebugOverlay)
    .AddOption<int>(leftHanded)
    .AddOption<int>(buggyAngularVelocity)

    .AddOption<int>(cutsceneCameraMode)
    .AddOption<int>(cutsceneBlackBars)

    .AddOption<int>(accessibilityUIColor)
    .AddOption<double>(accessibilityUIColor_x)
    .AddOption<double>(accessibilityUIColor_y)
    .AddOption<double>(accessibilityUIColor_z)
    .AddOption<double>(accessibilityUIColor_w)

    .AddPresetCategory(
        new PresetCategory("Camera Mode", "Camera Mode")
            .AddPreset(
                new Preset("Third-Person Camera (Most Stable & Casual, Seated Recommended)")
                    .AddValue<int>(cameraMode, 0)
                    .AddValue<double>(cameraHeight, 1.6)
                    .AddValue<int>(hideModelHead, 0)
            )
            .AddPreset(
                new Preset("First-Person Camera (Most Immersive for Roomscale, Standing Recommended)", isDefault: true, comment: "TODO: use Player->mode_slippingOrTreeClimbing to maybe zoom out when a wall is climbed")
                    .AddValue<int>(cameraMode, 1)
                    .AddValue<int>(hideModelHead, 1)
            )
            .AddPreset(
                new Preset("First-Person Camera - Dynamic Offset (Most Immersive for Seated, Suitalble for Any Play Style)")
                    .AddValue<int>(cameraMode, 2)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Model Head", "Hide Model Head")
            .AddPreset(
                new Preset("Hide Model Head", condition: isDynamicOffsetCondition, isDefault: true)
                    .AddValue<int>(hideModelHead, 1)
            )
            .AddPreset(
                new Preset("Don't Hide Model Head", condition: isDynamicOffsetCondition)
                    .AddValue<int>(hideModelHead, 0)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Model Offset Smoothing", "Dynamic Offset Smoothing")
            .AddPreset(
                new Preset("No Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue<double>(modelOffsetSmoothingFactor, 1)
            )
            .AddPreset(
                new Preset("Low Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue<double>(modelOffsetSmoothingFactor, 0.8)
            )
            .AddPreset(
                new Preset("Medium Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue<double>(modelOffsetSmoothingFactor, 0.5)
            )
            .AddPreset(
                new Preset("High Smoothing (recommended)", condition: isDynamicOffsetCondition, isDefault: true)
                    .AddValue<double>(modelOffsetSmoothingFactor, 0.1)
            )
            .AddPreset(
                new Preset("Very High Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue<double>(modelOffsetSmoothingFactor, 0.01)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Motion & Gamepad Controls", "Motion & Gamepad Controls")
            .AddPreset(
                new Preset("Use VR Motion Controls & Wii U Gamepad Input (Recommended)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue<int>(allowGamepadInput, 1)
            )
            .AddPreset(
                new Preset("Only Use VR Controllers (some inputs might require the gamepad)", condition: isFirstPersonCondition)
                    .AddValue<int>(allowGamepadInput, 0)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Left Or Right Handed (Not Working Yet)", "Left Or Right Handed")
            .AddPreset(
                new Preset("Left-Handed Weapon (Shield On Right Hand)", condition: isFirstPersonCondition)
                    .AddValue<int>(leftHanded, 1)
            )
            .AddPreset(
                new Preset("Right-Handed Weapon (Default)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue<int>(leftHanded, 0)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Cutscene Camera Mode", "Cutscene Settings")
            .AddPreset(
                new Preset("Watch All Cutscenes From Link's Perspective (For Testing, Not Recommended)", condition: isFirstPersonCondition)
                    .AddValue<int>(cutsceneCameraMode, 1)
            )
            .AddPreset(
                new Preset("Use Optimal Cutscene Settings, Mix Of Both (Recommended)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue<int>(cutsceneCameraMode, 2)
            )
            .AddPreset(
                new Preset("Watch All Cutscenes In Third Person Like Original Game (Less Immersive, Most Stable)", condition: isFirstPersonCondition)
                    .AddValue<int>(cutsceneCameraMode, 3)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Cutscene Black Bars", "Cutscene Black Bars\nThis is a user-configurable setting to add black boxes around a third-person cutscene, since those can be disorientating with floating, animated and non-FPS perspectives.")
            .AddPreset(
                new Preset("Use Black Bars During Third-Person Cutscenes (Recommended For Comfort)")
                    .AddValue<int>(cutsceneBlackBars, 1)
            )
            .AddPreset(
                new Preset("Use Fullscreen During Third-Person Cutscenes (Recommended For Brave People, Lots Of Movement)", isDefault: true)
                    .AddValue<int>(cutsceneBlackBars, 0)
            )
        )
    .AddPresetCategory(
        new PresetCategory("GUI Follow Mode", "GUI Follow Mode")
            .AddPreset(
                new Preset("HUD & Menus Will Follow Your Head (Default)", isDefault: true)
                    .AddValue<int>(guiFollowMode, 1)
            )
            .AddPreset(
                new Preset("HUD & Menus Stays In Front")
                    .AddValue<int>(guiFollowMode, 0)
            )
        )
    ;

PresetCategory eyeHeightCategory = new PresetCategory("Player Eye Height", "First Person - Eye Height")
    .AddPreset(
        new Preset("Automatic (calibrated when loading a save for the first time or recentering)", isDefault: true, condition: isFirstPersonCondition)
            .AddValue<double>(cameraHeight, 0)
    );
for (double eyeHeight = 1; eyeHeight < 1.59; eyeHeight += 0.05)
{
    eyeHeightCategory.AddPreset(
        new Preset(eyeHeight.ToString("0.###") + "m/" + Utils.ToFeetAndInches(eyeHeight), condition: isFirstPersonCondition)
            .AddValue<double>(cameraHeight, eyeHeight)
    );
}
eyeHeightCategory.AddPreset(
    new Preset(1.6.ToString("0.###") + "m/" + Utils.ToFeetAndInches(1.6) + " (roughly link's default eye height)", condition: isFirstPersonCondition)
        .AddValue<double>(cameraHeight, 1.6)
);
for (double eyeHeight = 1.65; eyeHeight <= 2.01; eyeHeight += 0.05)
{
    eyeHeightCategory.AddPreset(
        new Preset(eyeHeight.ToString("0.###") + "m/" + Utils.ToFeetAndInches(eyeHeight), condition: isFirstPersonCondition)
            .AddValue<double>(cameraHeight, eyeHeight)
    );
}
rules
    .AddPresetCategory(eyeHeightCategory);

rules
    .AddPresetCategory(
        new PresetCategory("Camera Distance", "Third Person - Camera Distance")
            .AddPreset(
                new Preset("Zero", condition: isThirdPersonCondition)
                    .AddValue<double>(cameraDistance, 0)
            )
            .AddPreset(
                new Preset("Low", condition: isThirdPersonCondition)
                    .AddValue<double>(cameraDistance, 0.4)
            )
            .AddPreset(
                new Preset("Normal (Default)", condition: isThirdPersonCondition, isDefault: true)
                    .AddValue<double>(cameraDistance, 0.5)
            )
            .AddPreset(
                new Preset("High", condition: isThirdPersonCondition)
                    .AddValue<double>(cameraDistance, 0.675)
            )
            .AddPreset(
                new Preset("Very High", condition: isThirdPersonCondition)
                    .AddValue<double>(cameraDistance, 0.8)
            )
            .AddPreset(
                new Preset("Extreme", condition: isThirdPersonCondition)
                    .AddValue<double>(cameraDistance, 0.9)
            )
        )
    ;

PresetCategory worldScaleCategory = new PresetCategory("World Scale", "World Scale")
    .AddPreset(
        new Preset("Automatic (scales to player height, not suitable for seated play)", isDefault: true, condition: isFirstPersonCondition)
            .AddValue<double>(worldScale, 0)
    );
for (double worldScaleVal = 0.5; worldScaleVal < 0.99; worldScaleVal += 0.1)
{
    worldScaleCategory.AddPreset(
        new Preset(worldScaleVal.ToString("0.###"))
            .AddValue<double>(worldScale, worldScaleVal)
    );
}
worldScaleCategory
    .AddPreset(
        new Preset("1", condition: isFirstPersonCondition, comment: "Not default for first person")
            .AddValue<double>(worldScale, 1)
    )
    .AddPreset(
        new Preset("1", condition: isThirdPersonCondition, isDefault: true, comment: "Default for third person")
            .AddValue<double>(worldScale, 1)
    );
for (double worldScaleVal = 1.1; worldScaleVal <= 2.01; worldScaleVal += 0.1)
{
    worldScaleCategory.AddPreset(
        new Preset(worldScaleVal.ToString("0.###"))
            .AddValue<double>(worldScale, worldScaleVal)
    );
}
rules
    .AddPresetCategory(worldScaleCategory);

rules
    .AddPresetCategory(
        new PresetCategory("More Contrasty UI For Crosshair, Buttons etc.", "Accessibility UI Color")
            .AddPreset(
                new Preset("Use Default Colors (Default, White)", isDefault: true)
                    .AddValue<int>(accessibilityUIColor, 0)
            )
            .AddPreset(
                new Preset("Thematic Cyan (Best Looking Preset)")
                    .AddValue<int>(accessibilityUIColor, 1)
                    .AddValue<double>(accessibilityUIColor_x, 0)
                    .AddValue<double>(accessibilityUIColor_y, 0.804)
                    .AddValue<double>(accessibilityUIColor_z, 1)
                    .AddValue<double>(accessibilityUIColor_w, 1)
            )
            .AddPreset(
                new Preset("Bright Purple (Most Visible Preset)")
                    .AddValue<int>(accessibilityUIColor, 1)
                    .AddValue<double>(accessibilityUIColor_x, 0.6728)
                    .AddValue<double>(accessibilityUIColor_y, 0)
                    .AddValue<double>(accessibilityUIColor_z, 0.3272)
                    .AddValue<double>(accessibilityUIColor_w, 1)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Crop VR Image", "2D Viewer - Crop VR Image To 16:9")
            .AddPreset(
                new Preset("Crop 3D Game World To 16:9 (Recommended)", condition: is2DViewCondition, isDefault: true)
                    .AddValue<int>(cropFlatTo16_9, 1)
            )
            .AddPreset(
                new Preset("Crop 3D Game World To 4:3", condition: is2DViewCondition)
                    .AddValue<int>(cropFlatTo16_9, 0)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Enable VR Debugging Overlay (Experimental)", "Debug Overlay")
            .AddPreset(
                new Preset("Enabled (Might Cause Performance & Crashing Issues)", condition: is2DViewCondition)
                    .AddValue<int>(enableDebugOverlay, 1)
            )
            .AddPreset(
                new Preset("Disabled (Default)", condition: is2DViewCondition, isDefault: true)
                    .AddValue<int>(enableDebugOverlay, 0)
            )
        )
    ;

string output = rules.Compile();
Console.WriteLine(output);

string outputPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "resources", "BreathOfTheWild_BetterVR", "rules.txt");
Console.WriteLine("Writing to " + outputPath);

using StreamWriter outputFile = new(outputPath);
outputFile.Write(output);
