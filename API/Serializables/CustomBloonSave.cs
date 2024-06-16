using BloonFactoryMod.API.Bloons;
using BloonFactoryMod.API.Serializables.Behaviors;
using Il2Cpp;
using Il2CppSystem;
using MelonLoader;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

namespace BloonFactoryMod.API.Serializables
{
    public class CustomBloonSave
    {
        [JsonInclude]
        public string Name = "";

        [JsonInclude]
        public float Speed = 5;

        [JsonInclude]
        public int Health = 1;

        [JsonInclude]
        public int Damage = 1;

        [JsonIgnore]
        public Color Color { get { return new Color((float)R / 255, (float)G / 255, (float)B / 255); } set { R = (int)value.r * 255; G = (int)value.g * 255; B = (int)value.b * 255; } }

        [JsonInclude]
        public int R = 0;

        [JsonInclude]
        public int G = 255;

        [JsonInclude]
        public int B = 255;

        [JsonInclude]
        public int CashDropped = 1;

        [JsonInclude]
        public string GUID;

        [JsonInclude]
        public bool IsCamo = false;

        [JsonInclude]
        public bool IsLead = false;

        [JsonInclude]
        public bool IsPurple = false;

        [JsonInclude]
        public bool IsBlack = false;

        [JsonInclude]
        public bool IsWhite = false;

        [JsonInclude]
        public bool IsFrozen = false;

        [JsonIgnore]
        public BloonProperties BloonProperties => GetBloonProperties();

        [JsonInclude]
        public bool IsRegrow = false;

        [JsonInclude]
        public float RegrowRate = 3;

        [JsonInclude]
        public List<CustomBloonChild> BloonChildren = new List<CustomBloonChild>();

        [JsonInclude]
        public List<CustomBloonRound> BloonRounds = new List<CustomBloonRound>();

        [JsonInclude]
        public List<CustomBloonBehaviorSerializable> BloonBehaviors = new List<CustomBloonBehaviorSerializable>();

        [JsonInclude]
        public CustomBloonDecal Decal1 = new CustomBloonDecal();

        [JsonInclude]
        public CustomBloonDecal Decal2 = new CustomBloonDecal();

        public static CustomBloonSave CreateBloonSave(string name)
        {
            
            var guid = Guid.NewGuid();
            var bloon = new CustomBloonSave()
            {
                GUID = guid.ToString(),
                Name = name
            };
            MelonLogger.Msg($"Created new BloonsSave with the GUID: {bloon.GUID}");
            return bloon;
        }
        public static CustomBloonSave CreateBloonSave()
        {
            return CreateBloonSave("Default Bloon");
        }

        public bool IsActive()
        {
            return CustomBloon.ActiveBloons.ContainsKey(GUID);
        }

        private BloonProperties GetBloonProperties()
        {
            int property = 0;

            property += IsLead ? 1 : 0;
            property += IsBlack ? 2 : 0;
            property += IsWhite ? 4 : 0;
            property += IsPurple ? 8 : 0;
            property += IsFrozen ? 16 : 0;

            return (BloonProperties)property;
        }
    }
}
