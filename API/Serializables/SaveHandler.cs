using BloonFactoryMod.API.Bloons;
using MelonLoader.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BloonFactoryMod.API.Serializables
{
    internal static class SaveHandler
    {
        public static string FolderDirectory = Path.Combine(MelonEnvironment.ModsDirectory, "BloonFactoryBloons");

        public static List<CustomBloonSave> LoadedBloons = new List<CustomBloonSave>();

        public static void LoadBloonsFromFile()
        {
            if (!Directory.Exists(FolderDirectory))
            {
                Directory.CreateDirectory(FolderDirectory);
                return;
            }

            foreach (var file in Directory.GetFiles(FolderDirectory).Where(f => f.EndsWith(".bln")))
            {
                LoadBloonFromFile(file);
            }
        }
        public static void SaveBloon(CustomBloonSave bloon, string path = "")
        {
            var content = JsonConvert.SerializeObject(bloon, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented });
            File.WriteAllText(Path.Combine(path == "" ? FolderDirectory : path, $"{bloon.GUID}.bln"), content);
        }
        public static void SaveAllBloons()
        {
            foreach (var bloon in LoadedBloons)
            {
                SaveBloon(bloon);
            }
        }

        public static void DeleteBloon(CustomBloonSave save)
        {
            LoadedBloons.Remove(save);
            if (CustomBloon.ActiveBloons.TryGetValue(save.GUID, out var value))
            {
                value.ShouldLoad = false;
            }
            File.Delete(Path.Combine(FolderDirectory, $"{save.GUID}.bln"));
        }
        public static CustomBloonSave LoadBloonFromFile(string fileLocation)
        {
            try
            {
                var bloon = JsonConvert.DeserializeObject<CustomBloonSave>(File.ReadAllText(fileLocation), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented });

                if (LoadedBloons.Any(a => a.GUID == bloon.GUID))
                    return null;

                LoadedBloons.Add(bloon);
                return bloon;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
