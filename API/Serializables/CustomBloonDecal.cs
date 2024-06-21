﻿using Newtonsoft.Json;
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
        public enum DecalType
        {
            None,
            HalfHorizontalBloon,
            HalfVerticalBloon,
            Bowtie,
            Face1,
            Sword
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
                _ => throw new System.NotImplementedException()
            };
        }
        /// <summary>
        /// Returns the visualizer texture and the ingame texture
        /// </summary>
        /// <param name="decal"></param>
        /// <returns></returns>
        public static (string, string) GetSpriteNames(DecalType decal)
        {
            return decal switch
            {
                DecalType.None => (string.Empty, string.Empty),
                DecalType.HalfHorizontalBloon => ("HalfHorizontalBloonDecal", "HalfHorizontalBloonDecalInGame"),
                DecalType.HalfVerticalBloon => ("HalfVerticalBloonDecal", "HalfVerticalBloonDecalInGame"),
                DecalType.Bowtie => ("BowtieDecal", "BowtieDecalInGame"),
                DecalType.Face1 => ("Face1Decal", "Face1DecalInGame"),
                DecalType.Sword => ("IronSwordDecal", "IronSwordDecalInGame")
            };
        }
    }
}