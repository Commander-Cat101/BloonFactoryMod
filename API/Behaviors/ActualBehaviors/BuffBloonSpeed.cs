using BloonFactoryMod.API.Serializables;
using BloonFactoryMod.API.Serializables.Behaviors;
using BloonFactoryMod.API.Serializables.Behaviors.Actions;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Behaviors.ActualBehaviors
{
    internal class BuffBloonSpeed : CustomBloonBehavior<BuffBloonSpeedSerializable>
    {
        public override BehaviorType Type => BehaviorType.Behavior;

        public override string Name => "Buff Bloon Speed";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var speed = (BuffBloonSpeedSerializable)serializable;
            bloon.AddBehavior(new BuffBloonSpeedModel($"BuffBloonSpeed:{serializable.GUID}", speed.Multiplier, speed.DebuffRadius, "VortexBloonSpeedBuff"));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var spawn = (BuffBloonSpeedSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 500), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 150, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);


            panel.AddText(new Info("Speed", -237.5f, 0, 400, 100), "Speed Multiplier:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var timeInput = panel.AddInputField(new Info("SpeedInput", 100, 0, 200, 100), $"{spawn.Multiplier}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (float.TryParse(value, out float result))
                {
                    spawn.Multiplier = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Decimal);
            timeInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            timeInput.InputField.characterLimit = 4;

            panel.AddText(new Info("DistanceBehind", -237.5f, -150, 400, 100), "Distance behind:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var distanceInput = panel.AddInputField(new Info("DistanceBehindInput", 100, -150, 200, 100), $"{spawn.DebuffRadius}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (int.TryParse(value, out int result))
                {
                    spawn.DebuffRadius = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Integer);
            distanceInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            distanceInput.InputField.characterLimit = 4;

            timeInput.SetActive(false);
            timeInput.SetActive(true);
            distanceInput.SetActive(false);
            distanceInput.SetActive(true);

            return panel;
        }
    }
}
