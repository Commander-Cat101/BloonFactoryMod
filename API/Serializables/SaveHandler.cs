using MelonLoader.Utils;
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
            var content = JsonSerializer.Serialize(bloon, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(Path.Combine(path == "" ? FolderDirectory : path, $"{bloon.Name}.bln"), content);
        }
        public static void SaveAllBloons()
        {
            foreach (var bloon in LoadedBloons)
            {
                SaveBloon(bloon);
            }
        }
        public static CustomBloonSave LoadBloonFromFile(string fileLocation)
        {
            var bloon = JsonSerializer.Deserialize<CustomBloonSave>(File.ReadAllText(fileLocation));

            if (LoadedBloons.Any(a => a.GUID == bloon.GUID))
                return null;

            LoadedBloons.Add(bloon);
            return bloon;
        }
    }
}
