using BloonFactoryMod.API.Serializables;
using BloonFactoryMod.API.Serializables.Behaviors;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppTMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BloonFactoryMod.API.Behaviors.CustomBloonBehavior;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;

namespace BloonFactoryMod.API.Behaviors
{
    public static class BehaviorHelper
    {
        public static T CreateBehavior<T>()
            where T : CustomBloonBehaviorSerializable
        {
            T behavior = (T)Activator.CreateInstance(typeof(T));
            string guid = Guid.NewGuid().ToString();
            behavior.GUID = guid;
            return behavior;
        }
        public static CustomBloonBehaviorSerializable CreateBehavior(Type type)
        {
            CustomBloonBehaviorSerializable behavior = (CustomBloonBehaviorSerializable)Activator.CreateInstance(type);
            string guid = Guid.NewGuid().ToString();
            behavior.GUID = guid;
            return behavior;
        }

        public static void ShowAddActionPopup(CustomBloonSave save, CustomBloonBehaviorTriggerSerializable trigger, Action<CustomBloonBehaviorSerializable> onComplete)
        {
            ModHelperDropdown dropdown = null!;

            List<CustomBloonBehaviorSerializable> actions = save.BloonBehaviors.Where(a => BehaviorByType[a.GetType()].Type == BehaviorType.Action || BehaviorByType[a.GetType()].Name == "Wait for Seconds Trigger").ToList();

            PopupScreen.instance.SafelyQueue(p =>
            {
                p.ShowPopup(PopupScreen.Placement.menuCenter,
                    "Add Action", "Choose the action to add.",
                    new Action(() => { onComplete.Invoke(actions[dropdown.Dropdown.value]); }), "Confirm", null, "Cancel", Popup.TransitionAnim.Scale, instantClose: true);

                TaskScheduler.ScheduleTask(() =>
                {
                    dropdown = p.GetFirstActivePopup().bodyObj.AddModHelperPanel(new Info("BloonsPanel", 400, 700))
                        .AddDropdown(new Info("Filter",
                                421.5F * 1.5f, 150F * 1.5f, new Vector2(.5f, 0.2f)), actions.Select(a => a.Name).ToIl2CppList(), 400, null, VanillaSprites.BlueInsertPanelRound, 70
                        );

                    TaskScheduler.ScheduleTask(() =>
                    {
                        p.GetFirstActivePopup().bodyObj.transform.localPosition = new Vector3(0, 50, 0);
                    });
                }, () => p.GetFirstActivePopup()?.bodyObj is not null);
            });
        }
        public static void ShowAddBehaviorPopup(CustomBloonSave save, BehaviorType type, Action onAdded)
        {
            ModHelperDropdown dropdown = null!;

            PopupScreen.instance.SafelyQueue(p =>
            {
                List<Type> behaviors = BehaviorByType.Select(a => a.Value).Where(b => b.Type == type).Select(b => b.SerializableType).ToList();

                p.ShowPopup(PopupScreen.Placement.menuCenter,
                    $"Add {type}", $"Choose the {type} to add.",
                    new Action(() =>
                    {
                        Type type = behaviors[dropdown.Dropdown.value];
                        save.BloonBehaviors.Add(CreateBehavior(type));
                        onAdded.Invoke();
                    }), "Confirm", null, "Cancel", Popup.TransitionAnim.Scale, instantClose: true);

                TaskScheduler.ScheduleTask(() =>
                {
                    dropdown = p.GetFirstActivePopup().bodyObj.AddModHelperPanel(new Info("BloonsPanel", 400, 700))
                        .AddDropdown(new Info("Filter",
                                421.5F * 1.5f, 150F * 1.5f, new Vector2(.5f, 0.2f)), behaviors.Select(a => BehaviorByType[a].Name).ToIl2CppList(), 400, null, VanillaSprites.BlueInsertPanelRound, 70
                        );

                    TaskScheduler.ScheduleTask(() =>
                    {
                        p.GetFirstActivePopup().bodyObj.transform.localPosition = new Vector3(0, 50, 0);
                    });
                }, () => p.GetFirstActivePopup()?.bodyObj is not null);
            });
        }
    }
}
