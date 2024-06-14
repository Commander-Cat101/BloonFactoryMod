using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Il2CppSystem.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>;

namespace BloonFactoryMod.API.Bloons
{
    internal class CustomBloonDisplay : ModDisplay2D
    {
        public static Dictionary<string, Texture2D> Cache = new Dictionary<string, Texture2D>();
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
        public override void Apply(BloonModel bloonModel)
        {
            base.Apply(bloonModel);
            var sprite = GetSprite<BloonFactoryMod>("BaseBloonInGame").Duplicate();

            
            if (!Cache.ContainsKey(bloonSave.GUID))
            {
                MelonLogger.Msg($"Generating Texture...");

                Il2CppStructArray<Color> decal1TextureColors = null;
                if (bloonSave.Decal1.Type != CustomBloonDecal.DecalType.None)
                {
                    var decal1Texture = ModContent.GetSprite<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal1.Type).Item2);
                    decal1TextureColors = decal1Texture.texture.GetPixels(0, 0, 128, 128);
                }

                Il2CppStructArray<Color> decal2TextureColors = null;
                if (bloonSave.Decal2.Type != CustomBloonDecal.DecalType.None)
                {
                    var decal2Texture = ModContent.GetSprite<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal2.Type).Item2);
                    decal2TextureColors = decal2Texture.texture.GetPixels(0, 0, 128, 128);
                }

                Il2CppStructArray<Color> baseSpriteColors = sprite.texture.GetPixels(0, 0, 128, 128);


                for (int i = 0; i < baseSpriteColors.Length; i++)
                {
                    if (decal2TextureColors != null && decal2TextureColors[i].a > 0.2)
                    {
                        baseSpriteColors[i] = BlendColors(decal2TextureColors[i], bloonSave.Decal2.Color);
                    }
                    else if (decal1TextureColors != null && decal1TextureColors[i].a > 0.2)
                    {
                        baseSpriteColors[i] = BlendColors(decal1TextureColors[i], bloonSave.Decal1.Color);
                    }
                    else if (baseSpriteColors[i].a > 0.2)
                    {
                        baseSpriteColors[i] = BlendColors(baseSpriteColors[i], bloonSave.Color);
                    }
                }
                sprite.texture.SetPixels(baseSpriteColors);
                sprite.texture.Apply();

                Cache[bloonSave.GUID] = sprite.texture;
            }
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            var sprite = GetSprite<BloonFactoryMod>("BaseBloonInGame").Duplicate();

            if (Cache.TryGetValue(bloonSave.GUID, out Texture2D value))
            {
                MelonLogger.Msg("Loading from Cache...");
                sprite.SetTexture(value);
                node.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                MelonLogger.Msg($"Generating Texture...");

                Il2CppStructArray<Color> decal1TextureColors = null;
                if (bloonSave.Decal1.Type != CustomBloonDecal.DecalType.None)
                {
                    var decal1Texture = ModContent.GetSprite<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal1.Type).Item2);
                    decal1TextureColors = decal1Texture.texture.GetPixels(0, 0, 128, 128);
                }

                Il2CppStructArray<Color> decal2TextureColors = null;
                if (bloonSave.Decal2.Type != CustomBloonDecal.DecalType.None)
                {
                    var decal2Texture = ModContent.GetSprite<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal2.Type).Item2);
                    decal2TextureColors = decal2Texture.texture.GetPixels(0, 0, 128, 128);
                }

                Il2CppStructArray<Color> baseSpriteColors = sprite.texture.GetPixels(0, 0, 128, 128);


                for (int i = 0; i < baseSpriteColors.Length; i++)
                {
                    if (decal2TextureColors != null && decal2TextureColors[i].a > 0.2)
                    {
                        baseSpriteColors[i] = BlendColors(decal2TextureColors[i], bloonSave.Decal2.Color);
                    }
                    else if (decal1TextureColors != null && decal1TextureColors[i].a > 0.2)
                    {
                        baseSpriteColors[i] = BlendColors(decal1TextureColors[i], bloonSave.Decal1.Color);
                    }
                    else if (baseSpriteColors[i].a > 0.2)
                    {
                        baseSpriteColors[i] = BlendColors(baseSpriteColors[i], bloonSave.Color);
                    }
                }
                sprite.texture.SetPixels(baseSpriteColors);
                sprite.texture.Apply();

                node.gameObject.GetComponent<SpriteRenderer>().sprite = sprite.Duplicate();

                Cache[bloonSave.GUID] = sprite.texture.Duplicate();
            }

            node.isSprite = true;
        }

        public static Color BlendColors(Color color1, Color color2)
        {
            //return new Color((color1.r + color2.r) / 2, (color1.g + color2.g) / 2, (color1.b + color2.b) / 2, (color1.a + color2.a) / 2);
            return color1 * color2;
        }
    }
}

