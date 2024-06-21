using Newtonsoft.Json;
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

        public int GetOffsetX()
        {
            return GetSpriteOffset(Type).x + 64;
        }
        public int GetOffsetY()
        {
            return GetSpriteOffset(Type).y + 64;
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
        public static (string, string) GetSpriteNames(DecalType decal)
        {
            return decal switch
            {
                DecalType.None => (string.Empty, string.Empty),
                DecalType.HalfHorizontalBloon => ("HalfHorizontalBloonDecal", "HalfHorizontalBloonDecalInGame"),
                DecalType.HalfVerticalBloon => ("HalfVerticalBloonDecal", "HalfVerticalBloonDecalInGame"),
                DecalType.Bowtie => ("BowtieDecal", "BowtieDecalInGame"),
                DecalType.Face1 => ("Face1Decal", "Face1DecalInGame"),
                DecalType.Sword => ("IronSwordDecal", "IronSwordDecalInGame"),
                DecalType.Fedora => ("FedoraDecal", "FedoraDecalInGame"),
                DecalType.Disguise => ("DisguiseDecal", "DisguiseDecalInGame"),
                DecalType.Camo => ("CamoDecal", "CamoDecalInGame")
            };
        }

        public static (int x, int y) GetSpriteOffset(DecalType decal)
        {
            return decal switch
            {
                DecalType.Fedora => new (0, 36),
                _ => new (0, 0)
            };
        }
    }
}
