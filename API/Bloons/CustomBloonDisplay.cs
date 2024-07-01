using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Unity.Display;
using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace BloonFactoryMod.API.Bloons
{
    internal class CustomBloonDisplay : ModDisplay2D
    {
        public const int TextureWidth = 500;//128;
        public const int TextureHeight = 500;//128;
        public const float PixelPerUnit = 39.0625f;//10f;

        internal static readonly Il2CppSystem.Collections.Generic.Dictionary<string, Texture2D> Cache = new();
        internal Color[] ColorCache = new Color[TextureWidth * TextureHeight];

        public override string Name => bloonSave.Name;
        public override DisplayCategory DisplayCategory => DisplayCategory.Bloon;
        protected override string TextureName => "BaseBloon";
        public override string BaseDisplay => "9d3c0064c3ace7448bf8fefa4a97a70f";

        public static Texture2D sprite = GetTexture<BloonFactoryMod>("BaseBloon");
        public CustomBloonSave bloonSave { get; set; }

        public override void Register()
        {
            base.Register();
        }
        public CustomBloonDisplay(CustomBloonSave save)
        {
            mod = ModHelper.GetMod<BloonFactoryMod>();
            bloonSave = save;
            
            Register();
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            if (Cache.TryGetValue(bloonSave.GUID, out Texture2D value))
            {
                if (value == null)
                {
                    value = GenerateTexture();
                }

                var sprite = Sprite.Create(value, new Rect(0, 0, TextureWidth * 2, TextureHeight * 2), new Vector2(0.5f, 0.5f), PixelPerUnit / bloonSave.Size);
                node.GetRenderer<SpriteRenderer>().sprite = sprite;
                node.IsSprite = true;
            }
            else
            {
                Texture2D texture = GenerateTexture();

                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), PixelPerUnit / bloonSave.Size);
                node.GetRenderer<SpriteRenderer>().sprite = sprite;
                node.IsSprite = true;
            }
        }
        public Texture2D GenerateTexture()
        {
            MelonLogger.Msg($"Generating Texture for {bloonSave.Name}...");
            var timer = Stopwatch.StartNew();
            timer.Start();

            if (bloonSave.IsCustomDisplay && bloonSave.CustomDisplay != null)
            {
                var customDisplay = new Texture2D(2, 2) { filterMode = FilterMode.Bilinear, mipMapBias = -0.5f };
                ImageConversion.LoadImage(customDisplay, bloonSave.CustomDisplay);
                Cache[bloonSave.GUID] = customDisplay;
                return customDisplay;
            }

            var baseSpriteColors = sprite.GetPixels();

            Color[] decal1TextureColors = null;
            if (bloonSave.Decal1.Type != CustomBloonDecal.DecalType.None)
            {
                var decal1Texture = GetTexture<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal1.Type));
                decal1TextureColors = decal1Texture.GetPixels(0, 0, TextureWidth, TextureHeight);
            }

            Color[] decal2TextureColors = null;
            if (bloonSave.Decal2.Type != CustomBloonDecal.DecalType.None)
            {
                var decal2Texture = GetTexture<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal2.Type));
                decal2TextureColors = decal2Texture.GetPixels(0, 0, TextureWidth, TextureHeight);
            }

            Color[] outputColors = new Color[(TextureHeight * 2) * (TextureWidth * 2)];

            for (int i = 0; i < baseSpriteColors.Length; i++)
            {
                int y = Math.DivRem(i, TextureWidth, out int x);
                outputColors[(x + TextureWidth / 2) + (y * TextureHeight * 2) + (TextureHeight * TextureHeight)] = BlendColors(baseSpriteColors[i], bloonSave.Color);
            }
            if (decal1TextureColors != null)
            {
                for (int i = 0; i < decal1TextureColors.Length; i++)
                {
                    int y = Math.DivRem(i, TextureWidth, out int x);
                    if (decal1TextureColors[i].a > 0.2)
                    {
                        outputColors[bloonSave.Decal1.GetIndex(x, y)] = BlendColors(decal1TextureColors[i], bloonSave.Decal1.Color);
                    }
                }
            }
            if (decal2TextureColors != null)
            {
                for (int i = 0; i < decal2TextureColors.Length; i++)
                {
                    int y = Math.DivRem(i, TextureWidth, out int x);
                    if (decal2TextureColors[i].a > 0.2)
                    {
                        outputColors[bloonSave.Decal2.GetIndex(x, y)] = BlendColors(decal2TextureColors[i], bloonSave.Decal2.Color);
                    }
                }
            }

            var texture = new Texture2D(TextureWidth * 2, TextureHeight * 2) { filterMode = FilterMode.Bilinear, mipMapBias = -0.5f };
            texture.SetPixels(0, 0, TextureWidth * 2, TextureHeight * 2, outputColors);
            texture.Apply();

            Cache[bloonSave.GUID] = texture;

            timer.Stop();
            MelonLogger.Msg($"Generated Texture in {timer.ElapsedMilliseconds}ms");

            return texture;
        }

        public static Color BlendColors(Color color1, Color color2)
        {
            return color1 * color2;
        }
        public static void SaveImage(Texture2D texture, string GUID)
        {
            texture.SaveToPNG(Path.Combine(MelonEnvironment.ModsDirectory, "BloonFactoryBloons", $"{GUID}.png"));
        }

    }
}

