using MelonLoader;
using BTD_Mod_Helper;
using BloonFactoryMod;
using BloonFactoryMod.API.Serializables;
using UnityEngine;
using Il2CppAssets.Scripts.Models;
using BloonFactoryMod.API.Bloons;
using System.Linq;
using System;

[assembly: MelonInfo(typeof(BloonFactoryMod.BloonFactoryMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonFactoryMod;

public class BloonFactoryMod : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<BloonFactoryMod>("BloonFactoryMod loaded!");
    }
    public override void OnApplicationQuit()
    {
        SaveHandler.SaveAllBloons();
    }
    public override void OnNewGameModel(GameModel result)
    {
        base.OnNewGameModel(result);
        foreach (var bloonkvp in CustomBloon.ActiveBloons)
        {
            var bloonModel = result.bloons.First(bl => bl.id == bloonkvp.Value.Name);

            if (bloonModel == null)
                continue;

            MelonLogger.Msg("Modifying custom bloon");
            try
            {
                bloonkvp.Value.ModifyLoadedBloon(result.bloons.First(bl => bl.id == bloonkvp.Value.Name));
            }
            catch(Exception e)
            {
                MelonLogger.Error(e);
            }

            result.bloonsByName[bloonModel.name] = bloonModel;
        }
    }
}