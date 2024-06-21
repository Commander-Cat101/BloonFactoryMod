using BloonFactoryMod.API.Bloons;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.BloonMenu;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonFactoryMod.API.Patches
{
    [HarmonyPatch(typeof(BloonMenu), nameof(BloonMenu.CreateBloonButtons))]
    public static class SandboxPatch
    {
        [HarmonyPrefix]
        public static void Prefix(BloonMenu __instance, Il2CppSystem.Collections.Generic.List<BloonModel> sortedBloons)
        {
            foreach (var bloon in CustomBloon.ActiveBloons)
            {
                if (!sortedBloons.Any(a => a.id == bloon.Value.Name))
                {
                    sortedBloons.Add(Game.instance.model.GetBloon(bloon.Value.Name));
                }
            }
        }
    }
}
