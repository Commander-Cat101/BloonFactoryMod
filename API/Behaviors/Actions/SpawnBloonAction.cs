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
    internal class SpawnBloonAction : CustomBloonBehavior<SpawnBloonActionSerializable>
    {
        public override BehaviorType Type => BehaviorType.Action;
        public override string Name => "Spawn Bloon Action";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var spawn = (SpawnBloonActionSerializable)serializable;
            bloon.AddBehavior(new SpawnBloonsActionModel($"SpawnBloonAction:{serializable.GUID}", serializable.GUID, spawn.BloonType, spawn.Amount, spawn.Time, spawn.DistanceAhead, 0, 0, new Il2CppStringArray(new string[] { "BloonariusAttackSpew" }), new Il2CppStringArray(new string[] { "BloonariusAttackSpewMoab" }), 1f, true, "Bloonarius"));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var spawn = (SpawnBloonActionSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 700), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 250, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);

            Il2CppSystem.Collections.Generic.List<string> Bloons = Game.instance.model.bloons.Select(a => a.id).ToIl2CppList();
            panel.AddDropdown(new Info("Bloon", 237.5f, 250, 425, 150), Bloons, 500, new Action<int>(value =>
            {
                spawn.BloonType = Bloons[value];
            }), VanillaSprites.BlueInsertPanelRound).Dropdown.SetValue(Bloons.IndexOf(spawn.BloonType));

            panel.AddText(new Info("Amount", -237.5f, 100, 400, 100), "Amount:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var amountInput = panel.AddInputField(new Info("AmountInput", 100, 100, 200, 100), $"{spawn.Amount}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (int.TryParse(value, out int result))
                {
                    spawn.Amount = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Digit);
            amountInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            amountInput.InputField.characterLimit = 4;

            panel.AddText(new Info("DistanceAhead", -237.5f, -50, 400, 100), "Distance Ahead:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var distanceInput = panel.AddInputField(new Info("DistanceAheadInput", 100, -50, 200, 100), $"{spawn.DistanceAhead}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (int.TryParse(value, out int result))
                {
                    spawn.DistanceAhead = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Integer);
            distanceInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            distanceInput.InputField.characterLimit = 4;

            panel.AddText(new Info("TimeBetweenBloons", -237.5f, -200, 400, 100), "Time Between Bloons:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var timeInput = panel.AddInputField(new Info("TimeBetweenBloonsInput", 100, -200, 200, 100), $"{spawn.Time}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (float.TryParse(value, out float result))
                {
                    spawn.Time = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Decimal);
            timeInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            timeInput.InputField.characterLimit = 4;

            amountInput.SetActive(false);
            amountInput.SetActive(true);
            distanceInput.SetActive(false);
            distanceInput.SetActive(true);
            timeInput.SetActive(false);
            timeInput.SetActive(true);

            return panel;
        }
    }
}
