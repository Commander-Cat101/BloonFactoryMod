using BloonFactoryMod.API.Serializables;
using BloonFactoryMod.API.Serializables.Behaviors;
using BloonFactoryMod.API.Serializables.Behaviors.Actions;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Behaviors.Actions
{
    internal class StunTowersAction : CustomBloonBehavior<StunTowersActionSerializable>
    {
        public override BehaviorType Type => BehaviorType.Action;
        public override string Name => "Stun Towers Action";
        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var stun = (StunTowersActionSerializable)serializable;
            bloon.AddBehavior(new StunTowersInRadiusActionModel($"StunTowersAction:{serializable.GUID}", serializable.GUID, stun.Radius, stun.StunDuration, 1, new PrefabReference { guidRef = "289f511b736a06a4c993b9e0e73d2b8a" }, false));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var spawn = (StunTowersActionSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 500), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 150, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);


            panel.AddText(new Info("Radius", -237.5f, 0, 400, 100), "Radius:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var timeInput = panel.AddInputField(new Info("RadiusInput", 100, 0, 200, 100), $"{spawn.Radius}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (float.TryParse(value, out float result))
                {
                    spawn.Radius = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Decimal);
            timeInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            timeInput.InputField.characterLimit = 4;

            panel.AddText(new Info("Duration", -237.5f, -150, 400, 100), "Stun Duration:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var distanceInput = panel.AddInputField(new Info("DurationInput", 100, -150, 200, 100), $"{spawn.StunDuration}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (float.TryParse(value, out float result))
                {
                    spawn.StunDuration = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Digit);
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
