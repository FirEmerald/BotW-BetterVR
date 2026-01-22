using RulesGenerator;
using RulesGenerator.Options;

Rules rules = new Rules("Better VR", "\"The Legend of Zelda: Breath of the Wild/Better VR\"", "Used to enable and change VR settings for BetterVR.||Made by Crementif.", true)
    .AddTitle(0x00050000101C9300UL)
    .AddTitle(0x00050000101C9400UL)
    .AddTitle(0x00050000101C9500UL);

EnumOption<CameraMode> cameraMode = new("cameraMode", CameraMode.FIRST_PERSON);
EnumOption<PlayStyle> playStyle = new("playStyle", PlayStyle.STANDING);
EnumOption<CameraAnchor> cameraAnchor = new("cameraAnchor", CameraAnchor.GROUND);

BoolOption dynamicCameraOffset = new("dynamicEyeOffset", false);
DoubleOption cameraOffsetSmoothingFactor = new("cameraOffsetSmoothingFactor", 0.1);
BoolOption hideModelHead = new("hideModelHead", true);
BoolOption allowGamepadInput = new("allowGamepadInput", true);
BoolOption enable2DView = new("enable2DView", true);
BoolOption cropFlatTo16_9 = new("cropFlatTo16_9", true);
EnumOption<GuiMode> guiFollowMode = new("guiFollowMode", GuiMode.FOLLOW_HEAD);

DoubleOption cameraDistance = new("cameraDistance", 0.5);
DoubleOption cameraHeight = new("cameraHeight", 0.0);
DoubleOption eyeHeightOption = new("eyeHeight", 1.6);
DoubleOption worldScale = new("worldScale", 0.0);
BoolOption enableDebugOverlay = new("enableDebugOverlay", false);
BoolOption leftHanded = new("leftHanded", false);
EnumOption<AngularVelocityMode> buggyAngularVelocity = new("buggyAngularVelocity", AngularVelocityMode.AUTO, comment: "Whether the angular velocity is wrongly provided in local space. Possible Values: 0 is auto; 1 is forced on; 2 is forced off");

EnumOption<CutsceneCameraMode> cutsceneCameraMode = new("cutsceneCameraMode", CutsceneCameraMode.MIXED);
BoolOption cutsceneBlackBars = new("cutsceneBlackBars", true);

BoolOption accessibilityUIColor = new("accessibilityUIColor", false);
DoubleOption accessibilityUIColor_x = new("accessibilityUIColor_x", 0.0);
DoubleOption accessibilityUIColor_y = new("accessibilityUIColor_y", 0.804);
DoubleOption accessibilityUIColor_z = new("accessibilityUIColor_z", 1.0);
DoubleOption accessibilityUIColor_w = new("accessibilityUIColor_w", 1.0);

string isFirstPersonCondition = cameraMode.GetVariableString() + " == " + (int)CameraMode.FIRST_PERSON;
string isThirdPersonCondition = cameraMode.GetVariableString() + " == " + (int)CameraMode.THIRD_PERSON;

string isSeatedCondition = playStyle.GetVariableString() + " == " + (int)PlayStyle.SEATED;
string isStandingCondition = playStyle.GetVariableString() + " == " + (int)PlayStyle.STANDING;

string isGroundAnchorCondition = cameraAnchor.GetVariableString() + " == " + (int)CameraAnchor.GROUND;
string isEyeAnchorCondition = cameraAnchor.GetVariableString() + " == " + (int)CameraAnchor.EYES;

string isDynamicOffsetCondition = dynamicCameraOffset.GetVariableString() + " == 1";

string is2DViewCondition = enable2DView.GetVariableString() + " == 1";

rules
    .AddOption(cameraMode)
    .AddOption(playStyle)
    .AddOption(cameraAnchor)

    .AddOption(dynamicCameraOffset)
    .AddOption(cameraOffsetSmoothingFactor)
    .AddOption(hideModelHead)
    .AddOption(allowGamepadInput)
    .AddOption(enable2DView)
    .AddOption(cropFlatTo16_9)
    .AddOption(guiFollowMode)

    .AddOption(cameraDistance)
    .AddOption(cameraHeight)
    .AddOption(eyeHeightOption)
    .AddOption(worldScale)
    .AddOption(enableDebugOverlay)
    .AddOption(leftHanded)
    .AddOption(buggyAngularVelocity)

    .AddOption(cutsceneCameraMode)
    .AddOption(cutsceneBlackBars)

    .AddOption(accessibilityUIColor)
    .AddOption(accessibilityUIColor_x)
    .AddOption(accessibilityUIColor_y)
    .AddOption(accessibilityUIColor_z)
    .AddOption(accessibilityUIColor_w)

    .AddPresetCategory(
        new PresetCategory("Camera Mode", "Camera Mode")
            .AddPreset(
                new Preset("Third-Person Camera (Most Stable & Casual, Seated Recommended)")
                    .AddValue(cameraMode, CameraMode.THIRD_PERSON)
                    .AddValue(cameraAnchor, CameraAnchor.GROUND)
                    .AddValue(cameraHeight, 1.6)
                    .AddValue(hideModelHead, false)
                    .AddValue(dynamicCameraOffset, false)
            )
            .AddPreset(
                new Preset("First-Person Camera (Most Immersive, Standing Recommended)", isDefault: true, comment: "TODO: use Player->mode_slippingOrTreeClimbing to maybe zoom out when a wall is climbed")
                    .AddValue(cameraMode, CameraMode.FIRST_PERSON)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Play Style", "Play Style")
            .AddPreset(
                new Preset("Standing", isDefault: true)
                    .AddValue(playStyle, PlayStyle.STANDING)
            )
            .AddPreset(
                new Preset("Seated")
                    .AddValue(playStyle, PlayStyle.SEATED)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Camera Height Anchor", "Camera Height Anchor")
            .AddPreset(
                new Preset("Ground (Calibrates ground at playspace origin)", condition: isFirstPersonCondition)
                    .AddValue(cameraAnchor, CameraAnchor.GROUND)
                    .AddValue(dynamicCameraOffset, false)
                    .AddValue(hideModelHead, true)
            )
            .AddPreset(
                new Preset("Eyes (Calibrates Link's eyes at player eye height)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue(cameraAnchor, CameraAnchor.EYES)
            )
        )

    .AddPresetCategory(
        new PresetCategory("Dynamic Camera Offset", "Dynamic Camera Offset")
            .AddPreset(
                new Preset("Use Static Camera Offsets (Recommended)", condition: isEyeAnchorCondition, isDefault: true)
                    .AddValue(dynamicCameraOffset, false)
            )
            .AddPreset(
                new Preset("Use Dynamic Camera Offsets (Not Recommended, Motion Sickness Warning! Moves view up and down to match Link's eyes)", condition: isEyeAnchorCondition)
                    .AddValue(dynamicCameraOffset, true)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Model Head", "Hide Model Head")
            .AddPreset(
                new Preset("Hide Model Head (prevents clipping)", condition: isDynamicOffsetCondition)
                    .AddValue(hideModelHead, true)
            )
            .AddPreset(
                new Preset("Don't Hide Model Head", condition: isDynamicOffsetCondition, isDefault: true)
                    .AddValue(hideModelHead, false)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Dynamic Camera Offset Smoothing", "Dynamic Offset Smoothing")
            .AddPreset(
                new Preset("No Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue(cameraOffsetSmoothingFactor, 1)
            )
            .AddPreset(
                new Preset("Low Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue(cameraOffsetSmoothingFactor, 0.8)
            )
            .AddPreset(
                new Preset("Medium Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue(cameraOffsetSmoothingFactor, 0.5)
            )
            .AddPreset(
                new Preset("High Smoothing (recommended)", condition: isDynamicOffsetCondition, isDefault: true)
                    .AddValue(cameraOffsetSmoothingFactor, 0.1)
            )
            .AddPreset(
                new Preset("Very High Smoothing", condition: isDynamicOffsetCondition)
                    .AddValue(cameraOffsetSmoothingFactor, 0.01)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Motion & Gamepad Controls", "Motion & Gamepad Controls")
            .AddPreset(
                new Preset("Use VR Motion Controls & Wii U Gamepad Input (Recommended)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue(allowGamepadInput, true)
            )
            .AddPreset(
                new Preset("Only Use VR Controllers (some inputs might require the gamepad)", condition: isFirstPersonCondition)
                    .AddValue(allowGamepadInput, false)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Left Or Right Handed (Not Working Yet)", "Left Or Right Handed")
            .AddPreset(
                new Preset("Left-Handed Weapon (Shield On Right Hand)", condition: isFirstPersonCondition)
                    .AddValue(leftHanded, true)
            )
            .AddPreset(
                new Preset("Right-Handed Weapon (Default)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue(leftHanded, false)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Cutscene Camera Mode", "Cutscene Settings")
            .AddPreset(
                new Preset("Watch All Cutscenes From Link's Perspective (For Testing, Not Recommended)", condition: isFirstPersonCondition)
                    .AddValue(cutsceneCameraMode, CutsceneCameraMode.FIRST_PERSON_ONLY)
            )
            .AddPreset(
                new Preset("Use Optimal Cutscene Settings, Mix Of Both (Recommended)", condition: isFirstPersonCondition, isDefault: true)
                    .AddValue(cutsceneCameraMode, CutsceneCameraMode.MIXED)
            )
            .AddPreset(
                new Preset("Watch All Cutscenes In Third Person Like Original Game (Less Immersive, Most Stable)", condition: isFirstPersonCondition)
                    .AddValue(cutsceneCameraMode, CutsceneCameraMode.THIRD_PERSON_ONLY)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Cutscene Black Bars", "Cutscene Black Bars\nThis is a user-configurable setting to add black boxes around a third-person cutscene, since those can be disorientating with floating, animated and non-FPS perspectives.")
            .AddPreset(
                new Preset("Use Black Bars During Third-Person Cutscenes (Recommended For Comfort)")
                    .AddValue(cutsceneBlackBars, true)
            )
            .AddPreset(
                new Preset("Use Fullscreen During Third-Person Cutscenes (Recommended For Brave People, Lots Of Movement)", isDefault: true)
                    .AddValue(cutsceneBlackBars, false)
            )
        )
    .AddPresetCategory(
        new PresetCategory("GUI Follow Mode", "GUI Follow Mode")
            .AddPreset(
                new Preset("HUD & Menus Will Follow Your Head (Default)", isDefault: true)
                    .AddValue(guiFollowMode, GuiMode.FOLLOW_HEAD)
            )
            .AddPreset(
                new Preset("HUD & Menus Stays In Front")
                    .AddValue(guiFollowMode, GuiMode.IN_FRONT)
            )
        )
    ;

PresetCategory cameraOffsetCategory = new PresetCategory("Player Height Offset (for seated play)", "First Person - Camera Offset");

for (double cameraOffsetValue = -.2; cameraOffsetValue <= 0.76; cameraOffsetValue += 0.05)
{
    string name;
    switch (Math.Round(cameraOffsetValue * 100))
    {
        case -20:
            name = "Short (-0.20m)";
            break;
        case -10:
            name = "Slightly Short (-0.10m)";
            break;
        case 00:
            name = "Default (0.00m)";
            break;
        case 10:
            name = "Slightly Taller (+0.10m)";
            break;
        case 20:
            name = "Tall (+0.20m)";
            break;
        case 35:
            name = "Very Tall (+0.35m)";
            break;
        case 50:
            name = "Super Tall (+0.5m)";
            break;
        case 75:
            name = "Extremely Tall (+0.75m)";
            break;
        default:
            name = cameraOffsetValue.ToString("F2") + "m";
            if (cameraOffsetValue > 0)
            {
                name = "+" + name;
            }
            break;
    }
    cameraOffsetCategory.AddPreset(
        new Preset(name, condition: isGroundAnchorCondition, isDefault: Math.Abs(cameraOffsetValue) < 0.01)
            .AddValue(cameraHeight, cameraOffsetValue)
    );
}
rules
    .AddPresetCategory(cameraOffsetCategory);

PresetCategory eyeHeightCategory = new PresetCategory("User's Eye Height", "First Person - Eye Height")
    .AddPreset(
        new Preset("Automatic (calibrated when loading a save for the first time or recentering)", isDefault: true, condition: isEyeAnchorCondition)
            .AddValue(eyeHeightOption, 0)
    );
for (double eyeHeight = 1; eyeHeight <= 2.01; eyeHeight += 0.05)
{
    string name = eyeHeight.ToString("0.###") + "m/" + Utils.ToFeetAndInches(eyeHeight);
    if (eyeHeight >= 1.59 && eyeHeight <= 1.61) name += " (roughly link's default eye height)";
    eyeHeightCategory.AddPreset(
        new Preset(name, condition: isEyeAnchorCondition)
            .AddValue(eyeHeightOption, eyeHeight)
    );
}
rules
    .AddPresetCategory(eyeHeightCategory);

rules
    .AddPresetCategory(
        new PresetCategory("Camera Distance", "Third Person - Camera Distance")
            .AddPreset(
                new Preset("Zero", condition: isThirdPersonCondition)
                    .AddValue(cameraDistance, 0)
            )
            .AddPreset(
                new Preset("Low", condition: isThirdPersonCondition)
                    .AddValue(cameraDistance, 0.4)
            )
            .AddPreset(
                new Preset("Normal (Default)", condition: isThirdPersonCondition, isDefault: true)
                    .AddValue(cameraDistance, 0.5)
            )
            .AddPreset(
                new Preset("High", condition: isThirdPersonCondition)
                    .AddValue(cameraDistance, 0.675)
            )
            .AddPreset(
                new Preset("Very High", condition: isThirdPersonCondition)
                    .AddValue(cameraDistance, 0.8)
            )
            .AddPreset(
                new Preset("Extreme", condition: isThirdPersonCondition)
                    .AddValue(cameraDistance, 0.9)
            )
        )
    ;

string isStandingEyes = "((" + isStandingCondition + ") + (" + isEyeAnchorCondition + "))";
PresetCategory worldScaleCategory = new PresetCategory("World Scale", "World Scale")
    .AddPreset(
        new Preset("Automatic (scales to player eye height)", isDefault: true, condition: isStandingEyes + " == 2")
            .AddValue(worldScale, 0)
    );
for (double worldScaleVal = 0.5; worldScaleVal <= 2.01; worldScaleVal += 0.05)
{
    double height = worldScaleVal * 1.73;
    string name = worldScaleVal.ToString("0.###") + " (suitable for people " + height.ToString("0.##") + "m/" + Utils.ToFeetAndInches(height) + " tall with vanilla link scale)";
    if (worldScaleVal >= 0.99 && worldScaleVal <= 1.01)
    {
        worldScaleCategory
            .AddPreset(
                new Preset(name, condition: isStandingEyes + " == 2", comment: "Not default for standing")
                    .AddValue(worldScale, 1)
            )
            .AddPreset(
                new Preset(name, condition: isStandingEyes + " != 2", isDefault: true, comment: "Default for seated")
                    .AddValue(worldScale, 1)
            );
    }
    else
    {
        worldScaleCategory.AddPreset(
            new Preset(name)
                .AddValue(worldScale, worldScaleVal)
        );
    }
}
rules
    .AddPresetCategory(worldScaleCategory);

rules
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
    .AddPresetCategory(
        new PresetCategory("Crop VR Image", "2D Viewer - Crop VR Image To 16:9")
            .AddPreset(
                new Preset("Crop 3D Game World To 16:9 (Recommended)", condition: is2DViewCondition, isDefault: true)
                    .AddValue(cropFlatTo16_9, true)
            )
            .AddPreset(
                new Preset("Crop 3D Game World To 4:3", condition: is2DViewCondition)
                    .AddValue(cropFlatTo16_9, false)
            )
        )
    .AddPresetCategory(
        new PresetCategory("Enable VR Debugging Overlay (Experimental)", "Debug Overlay")
            .AddPreset(
                new Preset("Enabled (Might Cause Performance & Crashing Issues)", condition: is2DViewCondition)
                    .AddValue(enableDebugOverlay, true)
            )
            .AddPreset(
                new Preset("Disabled (Default)", condition: is2DViewCondition, isDefault: true)
                    .AddValue(enableDebugOverlay, false)
            )
        )
    ;

string output = rules.Compile();
Console.WriteLine(output);

string outputPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "resources", "BreathOfTheWild_BetterVR", "rules.txt");
Console.WriteLine("Writing to " + outputPath);

using StreamWriter outputFile = new(outputPath);
outputFile.Write(output);




internal enum CameraMode
{
    THIRD_PERSON = 0,
    FIRST_PERSON = 1
}

internal enum PlayStyle
{
    SEATED = 0,
    STANDING = 1
}

internal enum CameraAnchor
{
    GROUND = 0,
    EYES = 1
}

internal enum GuiMode
{
    IN_FRONT = 0,
    FOLLOW_HEAD = 1
}

internal enum AngularVelocityMode
{
    AUTO = 0,
    FORCED_ON = 1,
    FORCED_OFF = 2
}

internal enum CutsceneCameraMode
{
    FIRST_PERSON_ONLY = 1,
    MIXED = 2,
    THIRD_PERSON_ONLY = 3
}