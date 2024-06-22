using BloonFactoryMod.API.Serializables;
using BloonFactoryMod.API.Serializables.Behaviors;
using BloonFactoryMod.API.Serializables.Behaviors.Actions;
using BloonFactoryMod.API.Serializables.Behaviors.Triggers;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloonFactoryMod.API.Behaviors.Triggers
{
    internal class TowerSoldTrigger : CustomBloonBehavior<TowerSoldTriggerSerializable>
    {
        public override BehaviorType Type => BehaviorType.Trigger;
        public override string Name => "Tower Sold Trigger";
        public override void AddToBloon(BloonModel bloon, CustomBloonBehaviorSerializable serializable)
        {
            bloon.AddBehavior<SellTriggerModel>(new SellTriggerModel("SellTrigger:" + serializable.GUID, new Il2CppStringArray(((CustomBloonBehaviorTriggerSerializable)serializable).ActionIDs.ToArray())));
        }

        public override ModHelperPanel CreatePanel(CustomBloonBehaviorSerializable serializable, CustomBloonSave save)
        {
            var panel = ModHelperPanel.Create(new Info(Name, 0, 0, 950, 700), VanillaSprites.MainBGPanelBlue);
            panel.AddText(new Info("Text", -237.5f, 250, 450, 200), Name, 70, Il2CppTMPro.TextAlignmentOptions.Center);
            var scrollpanel = panel.AddScrollPanel(new Info("Actions", 225, 100, 400, 400), UnityEngine.RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound);
            panel.AddButton(new Info("Button", 225f, -250, 400, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
            {
                BehaviorHelper.ShowAddActionPopup(save, (CustomBloonBehaviorTriggerSerializable)serializable, new Action<CustomBloonBehaviorSerializable>(behavior =>
                {
                    ((CustomBloonBehaviorTriggerSerializable)serializable).ActionIDs.Add(behavior.GUID);
                    UpdateScrollPanel(scrollpanel, (CustomBloonBehaviorTriggerSerializable)serializable, save);
                }));
            })).AddText(new Info("Text", 0, 0, 400, 150), "Link Action", 60);

            UpdateScrollPanel(scrollpanel, (CustomBloonBehaviorTriggerSerializable)serializable, save);
            return panel;
        }

        public void UpdateScrollPanel(ModHelperScrollPanel panel, CustomBloonBehaviorTriggerSerializable trigger, CustomBloonSave save)
        {
            panel.ScrollContent.transform.DestroyAllChildren();
            List<string> IDsToRemove = new List<string>();
            foreach (var action in trigger.ActionIDs)
            {
                if (!save.BloonBehaviors.Any(bln => bln.GUID == action))
                {
                    IDsToRemove.Add(action);
                    continue;
                }

                panel.AddScrollContent(CreateActionPanel(save.BloonBehaviors.First(bln => bln.GUID == action).Name, new Action(() =>
                {
                    trigger.ActionIDs.Remove(action);
                    UpdateScrollPanel(panel, trigger, save);
                })));
            }
            trigger.ActionIDs.RemoveAll(id => IDsToRemove.Contains(id));
        }
        public ModHelperPanel CreateActionPanel(string name, Action onRemove)
        {
            var panel = ModHelperPanel.Create(new Info("ActionText", 0, 0, 350, 100));
            panel.AddText(new Info("Text", -50, 0, 250, 80), name).Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

            panel.AddButton(new Info("Remove", 150, 0, 80, 80), VanillaSprites.AddRemoveBtn, onRemove);

            return panel;
        }
    }
}
