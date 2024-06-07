using BloonFactoryMod.API.Bloons;
using Il2Cpp;
using Il2CppSystem;
using MelonLoader;
using System.Text.Json.Serialization;
using UnityEngine;

namespace BloonFactoryMod.API.Serializables
{
    public class CustomBloonChild
    {
        [JsonInclude]
        public string BloonName = "Red";

        [JsonInclude]
        public int Amount = 1;
    }
}
