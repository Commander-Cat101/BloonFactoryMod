using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using UnityEngine;
using UnityEngine.UI;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.Menu;
using BloonFactoryMod.UI.Editor;

namespace BloonFactoryMod.UI
{
    internal static class BloonEditorButton
    {
        public static void Create(MainMenu menu)
        {
            var mainMenuTransform = menu.transform.Cast<RectTransform>();
            var trophyStore = mainMenuTransform.FindChild("TrophyStore");
            var modsButton = trophyStore.Duplicate(mainMenuTransform);

            modsButton.name = "BloonEditor";
            modsButton.transform.FindChild("LimitedTime").gameObject.SetActive(false);
            modsButton.GetComponentInChildren<Image>().SetSprite(VanillaSprites.AlternateBloonsBtn);
            modsButton.GetComponentInChildren<Button>().SetOnClick(() => { ModGameMenu.Open<CustomBloonSelector>(); MenuManager.instance.buttonClickSound.Play("ClickSounds"); });

            var matchLocalPosition = modsButton.transform.gameObject.AddComponent<MatchLocalPosition>();
            matchLocalPosition.transformToCopy = trophyStore.transform;
            matchLocalPosition.offset = new Vector3(0 ,-325);

            var text = ModHelperText.Create(new Info("BloonEditor", 100, 150), "Bloon\nEditor");
            text.transform.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            text.transform.SetParent(modsButton.GetComponentInChildren<Button>().transform);
            
            text.transform.localPosition = new Vector3(150, -275, 0);
            text.transform.SetSiblingIndex(0);
        }
    }
}
