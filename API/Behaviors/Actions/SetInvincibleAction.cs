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
using Il2CppNinjaKiwi.LiNK.AuthenticationProviders;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloonFactoryMod.API.Behaviors.Actions
{
    internal class SetInvincibleAction : CustomBloonBehavior<SetInvincibleActionSerializable>
    {
        public override BehaviorType Type => BehaviorType.Action;
        public override string Name => "Set Invincible Action";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var invincible = (SetInvincibleActionSerializable)serializable;
            bloon.AddBehavior(new SetInvulnerableActionModel($"SetInvulnerable:{serializable.GUID}", serializable.GUID, invincible.IsInvincible, new AudioSourceReference { guidRef =  ""}));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var invincible = (SetInvincibleActionSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 400), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 100, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);

            panel.AddText(new Info("Invincible", -237.5f, -100, 400, 100), "Is Invincible:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

            var button = panel.AddCheckbox(new Info("Checkbox", 100, -100, 150, 150), invincible.IsInvincible, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value =>
            {
                invincible.IsInvincible = value;
            }));

            return panel;
        }
    }
}
