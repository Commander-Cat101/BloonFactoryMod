using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main;

namespace BloonFactoryMod.UI.Patches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Open))]
    internal static class MainMenu_Open
    {
        [HarmonyPostfix]
        private static void Postfix(MainMenu __instance)
        {
            BloonEditorButton.Create(__instance);
        }
    }
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.ReOpen))]
    internal static class MainMenu_ReOpen
    {
        [HarmonyPostfix]
        private static void Postfix(MainMenu __instance)
        {
            BloonEditorButton.Create(__instance);
        }
    }
}
