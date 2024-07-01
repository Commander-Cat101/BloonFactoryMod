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
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloonFactoryMod.API.Behaviors.Actions
{
    /*internal class PlaySoundAction : CustomBloonBehavior<PlaySoundActionSerializable>
    {
        public override BehaviorType Type => BehaviorType.Action;
        public override string Name => "Play Sound Action";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var sound = (PlaySoundActionSerializable)serializable;
            bloon.AddBehavior(new CreateSoundOnActionModel($"PlaySoundAction:{serializable.GUID}", serializable.GUID, new Il2CppReferenceArray<AudioSourceReference>([new AudioSourceReference() { guidRef = "b81ef0c6119090b4887095ecedee9704" }]), sound.Delay));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var spawn = (PlaySoundActionSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 500), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 150, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);


            panel.AddText(new Info("Time", -237.5f, 0, 400, 100), "Delay:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var timeInput = panel.AddInputField(new Info("DelayInput", 100, 0, 200, 100), $"{spawn.Delay}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (float.TryParse(value, out float result))
                {
                    spawn.Delay = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Decimal);
            timeInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            timeInput.InputField.characterLimit = 4;

            timeInput.SetActive(false);
            timeInput.SetActive(true);

            return panel;
        }
    }*/
}
