using BloonFactoryMod.API.Bloons;
using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppNinjaKiwi.Common;
using Il2CppTMPro;
using MelonLoader;
using MelonLoader.Utils;
using NfdSharp;
using Semver;
using System;
using System.IO;
using UnityEngine;

namespace BloonFactoryMod.UI.Editor
{
    internal class CustomBloonSelector : ModGameMenu<ExtraSettingsScreen>
    {
        public static ModHelperScrollPanel scrollPanel;

        public override void OnMenuClosed()
        {

        }
        public override bool OnMenuOpened(Il2CppSystem.Object data)
        {
            if (BloonEditor.SelectedBloon != null)
            {
                if (BloonEditor.SelectedBloon.IsActive())
                {
                    CustomBloon.ActiveBloons[BloonEditor.SelectedBloon.GUID].BloonSave = BloonEditor.SelectedBloon;
                }
            }
            BloonEditor.SelectedBloon = null;

            CommonForegroundHeader.SetText("Bloon Editor");

            var panelTransform = GameMenu.gameObject.GetComponentInChildrenByName<RectTransform>("Panel");
            var panel = panelTransform.gameObject;
            panel.DestroyAllChildren();

            var BloonMenu = panel.AddModHelperPanel(new Info("BloonMenu", 3600, 1900));

            CreateExtraButtons(BloonMenu);
            CreateMainPanel(BloonMenu);

            return false;
        }
        public void CreateMainPanel(ModHelperPanel menu)
        {
            var scrollpaneloutline = menu.AddPanel(new Info("ScrollPanelOutline", 0, 200, 3200, 1800), VanillaSprites.MainBGPanelBlue);
            scrollPanel = scrollpaneloutline.AddScrollPanel(new Info("ScrollPanel", 0, 0, 3100, 1700), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, -150, -200);
            LoadBloons();
        }
        public void LoadBloons()
        { 
            scrollPanel.ScrollContent.transform.DestroyAllChildren();
            foreach (var bloon in SaveHandler.LoadedBloons)
            {
                scrollPanel.AddScrollContent(GenerateScrollContent(bloon));
            }
        }
        public ModHelperPanel GenerateScrollContent(CustomBloonSave bloon)
        {
            var mainpanel = ModHelperPanel.Create(new Info("MainPanel", 0, 0, 3200, 450));

            var panel = mainpanel.AddPanel(new Info(bloon.Name, 0, 0, 3000, 250), VanillaSprites.MainBGPanelBlue);
            var text = panel.AddText(new Info("NameText", -1000, 0, 900, 200), bloon.Name);
            NK_TextMeshProUGUI textComp = text.transform.GetComponent<NK_TextMeshProUGUI>();
            textComp.enableAutoSizing = true;
            textComp.alignment = TextAlignmentOptions.MidlineLeft;

            if (!bloon.IsActive())
            {
                mainpanel.AddButton(new Info("RestartRequired", 1500, 125, 100, 100), VanillaSprites.NoticeBtn, new System.Action(() =>
                {
                    MenuManager.instance.buttonClickSound.Play("ClickSounds");

                    PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.menuCenter,
                    "Restart Required",
                    "A restart is required to activate this bloon. " +
                    "Would you like to do that now?", new Action(ProcessHelper.RestartGame),
                    "Yes", null, "No", Popup.TransitionAnim.Scale));
                }));
            }
            else if (SemVersion.Compare(SemVersion.Parse(bloon.ModVersion), SemVersion.Parse(ModHelperData.Version)) < 0)
            {
                
                mainpanel.AddButton(new Info("VersionWarning", 1500, 125, 100, 100), VanillaSprites.NoticeBtn, new System.Action(() =>
                {
                    MenuManager.instance.buttonClickSound.Play("ClickSounds");

                    PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.menuCenter,
                    "Outdated",
                    "This bloon was made in an older version and may not work as expected.\nThis popup will not show up again.", new Action(() =>
                    {
                        bloon.ModVersion = ModHelperData.Version;
                        LoadBloons();
                    }),
                    "Ok", null, null, Popup.TransitionAnim.Scale));
                }));
            }

            panel.AddButton(new Info("EditButton", 1350, 0, 200, 200), VanillaSprites.EditBtn, new System.Action(() =>
            {
                BloonEditor.SelectedBloon = bloon;
                Open<BloonEditor>();
                MenuManager.instance.buttonClickSound.Play("ClickSounds");
            }));
            var export = panel.AddButton(new Info("ExportButton", 1100, 0, 200, 200), VanillaSprites.BlueBtn, new System.Action(() =>
            {
                MenuManager.instance.buttonClickSound.Play("ClickSounds");

                PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.menuCenter,
                    "Export Bloon",
                    "Would you like export your bloon?", new Action(() => { ExportBloon(bloon); }),
                    "Yes", null, "No", Popup.TransitionAnim.Scale));
            }));
            var exit = export.AddImage(new Info("Exit", 130), VanillaSprites.ExitIcon);
            exit.RectTransform.Rotate(0, 0, -90);

            //panel.AddButton(new Info("Duplicate", 850, 0, 200, 200), VanillaSprites.CopyClipboardBtn, new Action(() => { DuplicateBloon(bloon); }));

            panel.AddButton(new Info("Delete", 850, 0, 200, 200), VanillaSprites.CloseBtn, new Action(() => { DeleteBloon(bloon); }));

            return mainpanel;
        }
        public void CreateExtraButtons(ModHelperPanel menu)
        {
            menu.AddButton(new Info("CreateNewBloon", 500, -900, 800, 300), VanillaSprites.GreenBtnLong, new System.Action(() =>
            {
                MenuManager.instance.buttonClickSound.Play("ClickSounds");
                PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup("Create Bloon", "Name of bloon to create.\n", new Action<string>(name =>
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        CreateBloon(name);
                    }
                }), null));
                PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                {
                    tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                    tmpInputField.characterLimit = 20;
                }));
            }))
            .AddText(new Info("Text", 0, 0, 700, 250), "Create", 100);

            menu.AddButton(new Info("ImportNewBloon", -500, -900, 800, 300), VanillaSprites.GreenBtnLong, new System.Action(() =>
            {
                MenuManager.instance.buttonClickSound.Play("ClickSounds");
                ImportBloon();
            }))
            .AddText(new Info("Text", 0, 0, 700, 250), "Import", 100);
        }

        private void CreateBloon(string name)
        {
            SaveHandler.LoadedBloons.Add(CustomBloonSave.CreateBloonSave(name));
            LoadBloons();
        }
        public void DeleteBloon(CustomBloonSave save)
        {
            PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.inGameCenter, "Delete Bloon.", "Would you like to delete this bloon?\nThis can not be undone!", new Action(() => { SaveHandler.DeleteBloon(save); LoadBloons(); }), "Continue", null, "Cancel", Popup.TransitionAnim.Scale));
        }
        public void DuplicateBloon(CustomBloonSave save)
        {
            PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup("Duplicate Bloon", "Name of bloon duplicate.\n", new Action<string>(name =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    SaveHandler.LoadedBloons.Add(CustomBloonSave.DuplicateBloonSave(name, save));
                    LoadBloons();
                }
            }), null));
            PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
            {
                tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                tmpInputField.characterLimit = 20;
                tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
            }));
        }
        public void ImportBloon()
        {
            FileDialogHelper.PrepareNativeDlls();
            if (Nfd.OpenDialog("bln", "", out string path) == Nfd.NfdResult.NFD_OKAY)
            {
                var bloon = SaveHandler.LoadBloonFromFile(path);

                if (bloon == null)
                {
                    PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.inGameCenter, "Import Failed!", "Import Failed for what ever reason.", null, "Continue", null, null, Popup.TransitionAnim.Scale));
                    return;
                }

                LoadBloons();
                PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.inGameCenter, "Import Success!", $"Successfully Imported {bloon.Name}!", null, "Continue", null, null, Popup.TransitionAnim.Scale));
            }
        }
        public void ExportBloon(CustomBloonSave save)
        {
            FileDialogHelper.PrepareNativeDlls();
            if (Nfd.PickFolder("", out string path) == Nfd.NfdResult.NFD_OKAY)
            {
                SaveHandler.SaveBloon(save, path);
                PopupScreen.instance.SafelyQueue(screen => screen.ShowPopup(PopupScreen.Placement.inGameCenter, "Export Success!", $"Successfully Exported {save.Name}!", null, "Continue", null, null, Popup.TransitionAnim.Scale));
            }
        }
    }
}
