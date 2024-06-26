using MelonLoader;
using BTD_Mod_Helper;
using BloonFactoryMod;
using BloonFactoryMod.API.Serializables;
using UnityEngine;
using Il2CppAssets.Scripts.Models;
using BloonFactoryMod.API.Bloons;
using System.Linq;
using System;
using BTD_Mod_Helper.Extensions;
using System.Reflection;
using BloonFactoryMod.API;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors.Triggers;

[assembly: MelonInfo(typeof(BloonFactoryMod.BloonFactoryMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonFactoryMod;

public class BloonFactoryMod : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ResourceHandler.LoadBundle();
        ModHelper.Msg<BloonFactoryMod>("BloonFactoryMod loaded!");
    }
    public override void OnApplicationQuit()
    {
        SaveHandler.SaveAllBloons();
    }
    public override void OnNewGameModel(GameModel result)
    {
        base.OnNewGameModel(result);

        CustomBloonDisplay.Cache.Clear();

        foreach (var bloonkvp in CustomBloon.ActiveBloons)
        {
            try
            {
                var bloonModel = result.bloons.First(bl => bl.id == bloonkvp.Value.Name);

                if (bloonModel == null)
                    continue;

                MelonLogger.Msg($"Updating {bloonkvp.Value.Name}");

                bloonkvp.Value.ModifyLoadedBloon(result.bloons.First(bl => bl.id == bloonkvp.Value.Name));


                result.bloonsByName[bloonModel.name] = bloonModel;

                var roundSet = result.roundSet;

                foreach (var round in bloonkvp.Value.BloonSave.BloonRounds)
                {
                    if (!round.IsMultiRound)
                    {
                        roundSet.rounds[round.StartRound - 1].AddBloonGroup(bloonModel.id, round.Amount, 0, round.Spacing * round.Amount * 60);
                    }
                    else
                    {
                        for (int i = round.StartRound; i < round.EndRound + 1; i++)
                        {
                            if (i - 1 < 0 || roundSet.rounds.Length <= i - 1)
                            {
                                continue;
                            }
                            roundSet.rounds[i - 1].AddBloonGroup(bloonModel.id, round.Amount, 0, round.Spacing * round.Amount * 60);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Failed to load bloon: {e}");
            }
        }
    }
}