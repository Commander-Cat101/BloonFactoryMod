using BloonFactoryMod.API.Serializables;
using BloonFactoryMod.API.Serializables.Behaviors;
using BloonFactoryMod.API.Serializables.Behaviors.Actions;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloonFactoryMod.API.Behaviors.Actions
{
    internal class MoveAction : CustomBloonBehavior<MoveActionSerializable>
    {
        public override BehaviorType Type => BehaviorType.Action;
        public override string Name => "Move Action";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var spawn = (MoveActionSerializable)serializable;
            bloon.AddBehavior(new SetSpeedPercentActionModel($"MoveAction:{serializable.GUID}", serializable.GUID, 0, false, spawn.Time, spawn.Distance));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var spawn = (MoveActionSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 500), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 150, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);


            panel.AddText(new Info("Time", -237.5f, 0, 400, 100), "Time:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var timeInput = panel.AddInputField(new Info("TimeInput", 100, 0, 200, 100), $"{spawn.Time}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (float.TryParse(value, out float result))
                {
                    spawn.Time = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Decimal);
            timeInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            timeInput.InputField.characterLimit = 4;

            panel.AddText(new Info("Distance", -237.5f, -150, 400, 100), "Distance:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var distanceInput = panel.AddInputField(new Info("DistanceInput", 100, -150, 200, 100), $"{spawn.Distance}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (int.TryParse(value, out int result))
                {
                    spawn.Distance = result;
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
