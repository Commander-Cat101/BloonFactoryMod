using BloonFactoryMod.API.Bloons;
using Il2Cpp;
using Il2CppSystem;
using JetBrains.Annotations;
using MelonLoader;
using System.Text.Json.Serialization;
using UnityEngine;

namespace BloonFactoryMod.API.Serializables
{
    public class CustomBloonRound
    {
        [JsonInclude]
        public bool IsMultiRound = false;

        [JsonInclude]
        public int EndRound = 10;

        [JsonInclude]
        public int StartRound = 10;

        [JsonInclude]
        public float Spacing = 0.25f;

        [JsonInclude]
        public int Amount = 10;
    }
}
