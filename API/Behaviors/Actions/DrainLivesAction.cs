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
    internal class DrainLivesAction : CustomBloonBehavior<DrainLivesActionSerializable>
    {
        public override BehaviorType Type => BehaviorType.Action;
        public override string Name => "Drain Lives Action";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var lives = (DrainLivesActionSerializable)serializable;
            bloon.AddBehavior(new DrainLivesActionModel($"DrainLivesAction:{lives.GUID}", serializable.GUID, lives.Lives, new PrefabReference { guidRef = "16977201d6852c348a8f90c77293f0d4" }, 2));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var lives = (DrainLivesActionSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 400), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 100, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);

            panel.AddText(new Info("Lives", -237.5f, -100, 400, 100), "Lives:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var livesInput = panel.AddInputField(new Info("LivesInput", 100, -100, 200, 100), $"{lives.Lives}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (int.TryParse(value, out int result))
                {
                    lives.Lives = result;
                }
            }), 50, Il2CppTMPro.TMP_InputField.CharacterValidation.Integer);
            livesInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            livesInput.InputField.characterLimit = 4;

            livesInput.SetActive(false);
            livesInput.SetActive(true);

            return panel;
        }
    }
}
