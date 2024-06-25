using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using static Il2CppSystem.Linq.Expressions.Interpreter.CastInstruction.CastInstructionNoT;
using static UnityEngine.UIElements.UIR.GradientSettingsAtlas;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;

namespace BloonFactoryMod.API.Bloons
{
    internal class CustomBloonDisplay : ModDisplay2D
    {
        public const int TextureWidth = 128;

        public const int TextureHeight = 128;

        internal static readonly Dictionary<string, Texture2D> Cache = new();
        public override string Name => bloonSave.Name;
        public override DisplayCategory DisplayCategory => DisplayCategory.Bloon;
        protected override string TextureName => "BaseBloonInGame";
        public override string BaseDisplay => "9d3c0064c3ace7448bf8fefa4a97a70f";

        public static Sprite sprite = GetSprite<BloonFactoryMod>("BaseBloonInGame");
        public CustomBloonSave bloonSave { get; set; }
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

                var sprite = Sprite.Create(value, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f), 10f);
                node.GetRenderer<SpriteRenderer>().sprite = sprite;
                node.IsSprite = true;
            }
            else
            {
                Texture2D texture = GenerateTexture();

                var sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f), 10f);
                node.GetRenderer<SpriteRenderer>().sprite = sprite;
                node.IsSprite = true;
            }
        }
        public Texture2D GenerateTexture()
        {
            MelonLogger.Msg($"Generating Texture for {bloonSave.Name}...");
            var timer = Stopwatch.StartNew();
            timer.Start();

            Il2CppStructArray<Color> decal1TextureColors = null;
            if (bloonSave.Decal1.Type != CustomBloonDecal.DecalType.None)
            {
                var decal1Texture = GetSprite<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal1.Type).Item2);
                decal1TextureColors = decal1Texture.texture.GetPixels(0, 0, TextureWidth, TextureHeight);
            }

            Il2CppStructArray<Color> decal2TextureColors = null;
            if (bloonSave.Decal2.Type != CustomBloonDecal.DecalType.None)
            {
                var decal2Texture = GetSprite<BloonFactoryMod>(CustomBloonDecal.GetSpriteNames(bloonSave.Decal2.Type).Item2);
                decal2TextureColors = decal2Texture.texture.GetPixels(0, 0, TextureWidth, TextureHeight);
            }
            Il2CppStructArray<Color> baseSpriteColors = CustomBloonDisplay.sprite.texture.GetPixels(0, 0, TextureWidth, TextureHeight);

            Il2CppStructArray<Color> outputColors = new Il2CppStructArray<Color>(new Color[256 * 256]);

            for (int i = 0; i < baseSpriteColors.Length; i++)
            {
                int y = Math.DivRem(i, 128, out int x);
                outputColors[(x + 64) + (y * 256) + (64 * 256)] = BlendColors(baseSpriteColors[i], bloonSave.Color);
            }
            if (decal1TextureColors != null)
            {
                for (int i = 0; i < decal1TextureColors.Length; i++)
                {
                    int y = Math.DivRem(i, 128, out int x);
                    if (decal1TextureColors[i].a > 0.2)
                    {
                        outputColors[(bloonSave.Decal1.GetOffsetX() + x) + ((y * 256) + (bloonSave.Decal1.GetOffsetY() * 256))] = BlendColors(decal1TextureColors[i], bloonSave.Decal1.Color);
                    }
                }
            }
            if (decal2TextureColors != null)
            {
                for (int i = 0; i < decal2TextureColors.Length; i++)
                {
                    int y = Math.DivRem(i, 128, out int x);
                    if (decal2TextureColors[i].a > 0.2)
                    {
                        outputColors[(bloonSave.Decal2.GetOffsetX() + x) + ((y * 256) + (bloonSave.Decal2.GetOffsetY() * 256))] = BlendColors(decal2TextureColors[i], bloonSave.Decal2.Color);
                    }
                }
            }

            var texture = new Texture2D(256, 256) { filterMode = FilterMode.Bilinear, mipMapBias = -0.5f };
            texture.SetPixels(0, 0, 256, 256, outputColors);
            texture.Apply(true, false);

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

