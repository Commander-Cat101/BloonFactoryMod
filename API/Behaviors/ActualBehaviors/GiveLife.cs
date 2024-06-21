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
    internal class GiveLife : CustomBloonBehavior<GiveLifeSerializable>
    {
        public override BehaviorType Type => BehaviorType.Behavior;

        public override string Name => "Give Lives On Death";

        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            var life = (GiveLifeSerializable)serializable;
            bloon.AddBehavior(new GiveLifeModel($"GiveLife:{serializable.GUID}", life.Life));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var lives = (GiveLifeSerializable)serializable;

            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 400), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 50, 425, 200), Name, 60, Il2CppTMPro.TextAlignmentOptions.Center);


            panel.AddText(new Info("Give Lives", -237.5f, -100, 400, 100), "Lives to give:").Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var livesInput = panel.AddInputField(new Info("GiveLivesInput", 100, -100, 200, 100), $"{lives.Life}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
            {
                if (int.TryParse(value, out int result))
                {
                    lives.Life = result;
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
