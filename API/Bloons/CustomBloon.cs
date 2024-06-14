using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Bloons;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Towers.Upgrades;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppInterop.Runtime;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Data.Boss;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Data.Bloons;

namespace BloonFactoryMod.API.Bloons
{
    public class CustomBloon : ModBloon
    {
        public static Dictionary<string, CustomBloon> ActiveBloons = new Dictionary<string, CustomBloon>();
        public override bool UseIconAsDisplay => false;
        public override string Name => $"BloonFactoryBloon:{BloonSave.GUID}";
        public override string BaseBloon => "Red";

        public CustomBloonSave BloonSave;
        public override SpriteReference IconReference => GetSpriteReference("BaseBloon");

        public CustomBloon()
        {

        }
        public CustomBloon(CustomBloonSave bloonsave)
        {
            BloonSave = bloonsave;
        }
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.id = Name;

            var display = new CustomBloonDisplay(BloonSave);
            display.Apply(bloonModel);

            bloonModel.disallowCosmetics = true;
            bloonModel.maxHealth = BloonSave.Health;
            bloonModel.leakDamage = BloonSave.Damage;
            bloonModel.speed = BloonSave.Speed;

            if (!bloonModel.HasBehavior<CreateSoundOnDamageBloonModel>())
            {
                var createsound = Game.instance.model.bloons.First(bl => bl.id == "Bfb").GetBehavior<CreateSoundOnDamageBloonModel>().Duplicate();
                bloonModel.AddBehavior(createsound);
            }
            bloonModel.bloonProperties = BloonSave.BloonProperties;

            bloonModel.RemoveAllChildren();

            foreach (var bloonchild in BloonSave.BloonChildren)
            {
                bloonModel.AddToChildren(bloonchild.BloonName, bloonchild.Amount);
            }

            if (!bloonModel.HasBehavior<DistributeCashModel>())
            {
                bloonModel.AddBehavior(new DistributeCashModel("GiveCash", BloonSave.CashDropped));
            }
            else
            {
                bloonModel.GetBehavior<DistributeCashModel>().cash = BloonSave.CashDropped;
            }
        }
        public override void Register()
        {
            base.Register();
            ActiveBloons.Add(BloonSave.GUID, this);
        }
        public override IEnumerable<ModContent> Load()
        {
            SaveHandler.LoadBloonsFromFile();
            foreach (var BloonSave in SaveHandler.LoadedBloons)
            {
                MelonLogger.Msg($"Loading {BloonSave.Name} bloon...");
                yield return new CustomBloon(BloonSave);
            }
        }

        public void ModifyLoadedBloon(BloonModel bloon)
        {
            ModifyBaseBloonModel(bloon);
        }
    }
}
