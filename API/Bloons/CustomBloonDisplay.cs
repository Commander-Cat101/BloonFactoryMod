using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Unity.Display;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BloonFactoryMod.API.Bloons
{
    internal class CustomBloonDisplay : ModDisplay2D
    {
        public override string Name => bloonSave.Name;
        public override string BaseDisplay => Bloon2dDisplay;
        public override DisplayCategory DisplayCategory => DisplayCategory.Bloon;
        protected override string TextureName => "BaseBloonInGame";
        public override float Scale { get; } = 10;
        public CustomBloonSave bloonSave { get; set; }
        public CustomBloonDisplay(CustomBloonSave save)
        {
            mod = ModHelper.GetMod<BloonFactoryMod>();
            bloonSave = save;

            Register();
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            base.ModifyDisplayNode(node);
            node.gameObject.GetComponent<SpriteRenderer>().color = bloonSave.Color;
        }
    }
}
