using BloonFactoryMod.API.Bloons;
using Il2CppSystem.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace BloonFactoryMod.API.Serializables
{
    public class CustomBloonDecal
    {
        [JsonIgnore]
        public Color Color { get { return new Color((float)R / 255, (float)G / 255, (float)B / 255); } set { R = (int)value.r * 255; G = (int)value.g * 255; B = (int)value.b * 255; } }

        [JsonInclude]
        public int R = 255;

        [JsonInclude]
        public int G = 255;

        [JsonInclude]
        public int B = 255;

        [JsonInclude]
        public DecalType Type = DecalType.None;

        public int OffsetX = 0;

        public int OffsetY = 0;

        public int GetOffsetX()
        {
            return Math.Clamp(GetSpriteOffset(Type).x + CustomBloonDisplay.TextureWidth / 2 + OffsetX, 0, CustomBloonDisplay.TextureWidth);
        }
        public int GetOffsetY()
        {
            return Math.Clamp(GetSpriteOffset(Type).y + CustomBloonDisplay.TextureHeight / 2 + OffsetY, 0, CustomBloonDisplay.TextureHeight); 
        }  
        public enum DecalType
        {
            None,
            HalfHorizontalBloon,
            HalfVerticalBloon,
            Bowtie,
            Face1,
            Sword,
            Fedora,
            Disguise,
            Camo
        }
        public static string GetDecalName(DecalType decal)
        {
            return decal switch
            {
                DecalType.None => "None",
                DecalType.HalfHorizontalBloon => "Half Horizontal Bloon",
                DecalType.HalfVerticalBloon => "Half Vertical Bloon",
                DecalType.Bowtie => "Bowtie",
                DecalType.Face1 => "Face 1",
                DecalType.Sword => "Sword",
                DecalType.Fedora => "Fedora",
                DecalType.Disguise => "Disguise",
                DecalType.Camo => "Camo",
                _ => "Uh..."
            };
        }
        /// <summary>
        /// Returns the visualizer texture and the ingame texture
        /// </summary>
        /// <param name="decal"></param>
        /// <returns></returns>
        public static string GetSpriteNames(DecalType decal)
        {
            return decal switch
            {
                DecalType.None => string.Empty,
                DecalType.HalfHorizontalBloon => "HalfHorizontalBloonDecal",
                DecalType.HalfVerticalBloon => "HalfVerticalBloonDecal",
                DecalType.Bowtie => "BowtieDecal",
                DecalType.Face1 => "Face1Decal",
                DecalType.Sword => "IronSwordDecal",
                DecalType.Fedora => "FedoraDecal",
                DecalType.Disguise => "DisguiseDecal",
                DecalType.Camo => "CamoDecal"
            };
        }
            
        public static (int x, int y) GetSpriteOffset(DecalType decal)
        {
            return decal switch
            {
                DecalType.Fedora => new (0, (int)(CustomBloonDisplay.TextureHeight / 4f)),
                _ => new (0, 0)
            };
        }

        public int GetIndex(int x, int y)
        {
            return GetOffsetX() + x + (y * CustomBloonDisplay.TextureHeight * 2) + GetOffsetY() * CustomBloonDisplay.TextureHeight * 2;
        }
    }
}
