using BloonFactoryMod.API.Behaviors;
using BloonFactoryMod.API.Serializables;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppAssets.Scripts.Unity.UI_New.Settings;
using Il2CppNinjaKiwi.Common;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppSystem.Collections.Generic;
using Il2CppTMPro;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BloonFactoryMod.API.Behaviors.CustomBloonBehavior;
using static BloonFactoryMod.API.Serializables.CustomBloonDecal;

namespace BloonFactoryMod.UI.Editor
{
    internal class BloonEditor : ModGameMenu<SettingsScreen>
    {
        internal SpriteReference Bloon = ModContent.GetSpriteReference<BloonFactoryMod>("BaseBloon");

        public const float OffsetPerPixel = 8f;

        ModHelperButton Visuals;
        ModHelperButton Stats;
        ModHelperButton Behaviors;
        ModHelperButton Spawning;

        public ModHelperPanel Visualizer;
        public ModHelperPanel Settings;
        public ModHelperScrollPanel ChildrenPanel;
        public ModHelperScrollPanel RoundSpawnPanel;
        public ModHelperScrollPanel BehaviorsPanel;

        public ModHelperInputField healthInput;
        public ModHelperInputField speedInput;
        public ModHelperInputField cashdropInput;
        public ModHelperInputField damageInput;

        public static CustomBloonSave SelectedBloon;

        public ModHelperImage Bloonimage;
        public ModHelperImage Decal1;
        public ModHelperImage Decal2;

        public override bool OnMenuOpened(Il2CppSystem.Object data)
        {
            CommonForegroundHeader.SetText("Bloon Editor");
            GameMenu.transform.DestroyAllChildren();

            var BloonMenu = GameMenu.gameObject.AddModHelperPanel(new Info("BloonMenu", 3600, 1900));

            CreateLeftPanel(BloonMenu);
            CreateRightPanel(BloonMenu);

            UpdateVisuals();

            return false;
        }
        public void CreateRightPanel(ModHelperPanel panel)
        {
            CreateRightPanelButtons(panel);
            var outline = panel.AddPanel(new Info("EditorPanel", 600, -100, 2000, 1600), VanillaSprites.MainBGPanelBlue);
            Settings = outline.AddPanel(new Info("Settings", 0, 0, 1900, 1500), VanillaSprites.BlueInsertPanelRound);

            SelectEditorPanel(EditorPanel.Visuals);
        }
        public void CreateRightPanelButtons(ModHelperPanel panel)
        {
            Visuals = panel.AddButton(new Info("VisualsButton", -175, 800, 450, 175), VanillaSprites.BlueBtnLong, new Action(() => { SelectEditorPanel(EditorPanel.Visuals); }));
            Visuals.AddText(new Info("Text", 0, 0, 450, 175), "Visuals", 75);

            Stats = panel.AddButton(new Info("StatsButton", 325, 800, 450, 175), VanillaSprites.BlueBtnLong, new Action(() => { SelectEditorPanel(EditorPanel.Stats); }));
            Stats.AddText(new Info("Text", 0, 0, 450, 175), "Stats", 75);

            Behaviors = panel.AddButton(new Info("BehaviorsButton", 825, 800, 450, 175), VanillaSprites.BlueBtnLong, new Action(() => { SelectEditorPanel(EditorPanel.Behaviors); }));
            Behaviors.AddText(new Info("Text", 0, 0, 450, 175), "Behaviors", 75);

            Spawning = panel.AddButton(new Info("SpawningButton", 1325, 800, 450, 175), VanillaSprites.BlueBtnLong, new Action(() => { SelectEditorPanel(EditorPanel.Spawning); }));
            Spawning.AddText(new Info("Text", 0, 0, 450, 175), "Spawning", 75);
        }
        public void CreateLeftPanel(ModHelperPanel panel)
        {
            var outline = panel.AddPanel(new Info("VisualizerOutline", -1100, 0, 1200, 1800), VanillaSprites.MainBGPanelBlue);
            Visualizer = outline.AddPanel(new Info("Visualizer", 0, 0, 1100, 1700), VanillaSprites.BlueInsertPanel);
            Bloonimage = Visualizer.AddImage(new Info("BloonImage", 0, 0, 1000, 1000), VanillaSprites.LeadFortified);
            Bloonimage.Image.SetSprite(Bloon);
            Bloonimage.Image.color = SelectedBloon.Color;

            Decal1 = Visualizer.AddImage(new Info("BloonImage", 0, 0, 1000, 1000), GetSprite<BloonFactoryMod>("HalfBloonDecal"));
            Decal2 = Visualizer.AddImage(new Info("BloonImage", 0, 0, 1000, 1000), GetSprite<BloonFactoryMod>("HalfBloonDecal"));
        }

        public void SelectEditorPanel(EditorPanel panel)
        {
            Settings.transform.DestroyAllChildren();

            Visuals.Button.interactable = true;
            Stats.Button.interactable = true;
            Behaviors.Button.interactable = true;
            Spawning.Button.interactable = true;

            MenuManager.instance.buttonClickSound.Play("ClickSounds");

            switch (panel)
            {
                case EditorPanel.Visuals:

                    Visuals.Button.interactable = false;

                    var BaseColorPanel = Settings.AddPanel(new Info("Base", -617, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseColorPanel.AddText(new Info("Text", 0, 600, 550, 200), "Base").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BaseColorPanel.AddButton(new Info("UseCustomButton", 0, 0, 0, 0), VanillaSprites.GreenBtnLong, new Action(() =>
                    {

                    }));

                    BaseColorPanel.AddText(new Info("SizeText", 0, 250, 400, 100), "Size", 90);

                    BaseColorPanel.AddSlider(new Info("SizeSlider", 0, 75, 400, 50), 1, 0.1f, 2, 0.1f, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Size = value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Size);

                    BaseColorPanel.AddText(new Info("ColorText", 0, -75, 400, 100), "Color", 90);

                    BaseColorPanel.AddSlider(new Info("RSlider", 0, -250, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.R = (byte)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.R);
                    BaseColorPanel.AddSlider(new Info("GSlider", 0, -400, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.G = (byte)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.G);
                    BaseColorPanel.AddSlider(new Info("BSlider", 0, -550, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.B = (byte)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.B);

                    var BaseDecal1Panel = Settings.AddPanel(new Info("Decal1Panel", 0, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseDecal1Panel.AddText(new Info("Text", 0, 600, 550, 200), "Decal 1").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    List<string> decalOptions = Enum.GetNames<DecalType>().ToIl2CppList();

                    BaseDecal1Panel.AddDropdown(new Info("DecalDropdown", 0, 450, 400, 100), decalOptions, 400, new Action<int>(choice =>
                    {
                        SelectedBloon.Decal1.Type = Enum.GetValues<DecalType>()[choice];
                        UpdateVisuals();
                    }), VanillaSprites.BlueInsertPanelRound).Dropdown.SetValue(Enum.GetValues<DecalType>().ToList().IndexOf(SelectedBloon.Decal1.Type));

                    BaseDecal1Panel.AddText(new Info("ColorText", 0, -75, 400, 100), "Color", 90);

                    BaseDecal1Panel.AddSlider(new Info("XSlider", 0, 250, 400, 50), 0, -64, 64, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal1.OffsetX = (int)value;
                        UpdateVisuals();
                    })).Slider.value = SelectedBloon.Decal1.OffsetX; 
                    BaseDecal1Panel.AddSlider(new Info("YSlider", 0, 100, 400, 50), 0, -64, 64, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal1.OffsetY = (int)value;
                        UpdateVisuals();
                    })).Slider.value = SelectedBloon.Decal1.OffsetY;

                    BaseDecal1Panel.AddSlider(new Info("RSlider", 0, -250, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal1.R = (int)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Decal1.R);
                    BaseDecal1Panel.AddSlider(new Info("GSlider", 0, -400, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal1.G = (int)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Decal1.G);
                    BaseDecal1Panel.AddSlider(new Info("BSlider", 0, -550, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal1.B = (int)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Decal1.B);

                    var BaseDecal2Panel = Settings.AddPanel(new Info("Decal2Panel", 617, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseDecal2Panel.AddText(new Info("Text", 0, 600, 550, 200), "Decal 2").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BaseDecal2Panel.AddDropdown(new Info("DecalDropdown", 0, 450, 400, 100), decalOptions, 400, new Action<int>(choice =>
                    {
                        SelectedBloon.Decal2.Type = Enum.GetValues<DecalType>()[choice];
                        UpdateVisuals();
                    }), VanillaSprites.BlueInsertPanelRound).Dropdown.SetValue(Enum.GetValues<DecalType>().ToList().IndexOf(SelectedBloon.Decal2.Type));

                    BaseDecal2Panel.AddText(new Info("ColorText", 0, -75, 400, 100), "Color",90);

                    BaseDecal2Panel.AddSlider(new Info("XSlider", 0, 250, 400, 50), 0, -64, 64, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal2.OffsetX = (int)value;
                        UpdateVisuals();
                    })).Slider.value = SelectedBloon.Decal2.OffsetX;
                    BaseDecal2Panel.AddSlider(new Info("YSlider", 0, 100, 400, 50), 0, -64, 64, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal2.OffsetY = (int)value;
                        UpdateVisuals();
                    })).Slider.value = SelectedBloon.Decal2.OffsetY;

                    BaseDecal2Panel.AddSlider(new Info("RSlider", 0, -250, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal2.R = (int)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Decal2.R);
                    BaseDecal2Panel.AddSlider(new Info("GSlider", 0, -400, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal2.G = (int)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Decal2.G);
                    BaseDecal2Panel.AddSlider(new Info("BSlider", 0, -550, 400, 50), 0, 0, 255, 1, new Vector2(100, 100), new Action<float>(value =>
                    {
                        SelectedBloon.Decal2.B = (int)value;
                        UpdateVisuals();
                    })).SetCurrentValue(SelectedBloon.Decal2.B);
                    break;
                case EditorPanel.Stats:
                    Stats.Button.interactable = false;

                    #region BaseStats
                    var BaseStatsPanel = Settings.AddPanel(new Info("BaseStats", -617, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseStatsPanel.AddText(new Info("Text", 0, 600, 550, 200), "Base Stats").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BaseStatsPanel.AddText(new Info("Name", 0, 450, 400, 100), "Name", 75, TextAlignmentOptions.Center);

                    var NameInput = BaseStatsPanel.AddInputField(new Info("NameInput", 0, 325, 400, 100), SelectedBloon.Name, VanillaSprites.BlueInsertPanelRound, new Action<string>(value =>
                    {
                        SelectedBloon.Name = value;
                    }), 60);
                    NameInput.InputField.characterLimit = 20;
                    NameInput.Text.Text.enableAutoSizing = true;

                    BaseStatsPanel.AddText(new Info("HealthStatText", -75, 100, 350, 200), "Health:", 65, TextAlignmentOptions.MidlineLeft);
                    healthInput = BaseStatsPanel.AddInputField(new Info("SetHealth", 150, 100, 200, 100), $"{SelectedBloon.Health}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value => { }), 75, TMP_InputField.CharacterValidation.Integer);
                    healthInput.InputField.onValueChanged.AddListener(new Action<string>(value =>
                    {
                        int.TryParse(value, out var intvalue);
                        SelectedBloon.Health = intvalue;
                    }));
                    healthInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                    healthInput.InputField.characterLimit = 9;

                    BaseStatsPanel.AddText(new Info("SpeedStatText", -75, -50, 350, 200), "Speed:", 65, TextAlignmentOptions.MidlineLeft);
                    speedInput = BaseStatsPanel.AddInputField(new Info("SetSpeed", 150, -50, 200, 100), $"{SelectedBloon.Speed}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value => { }), 75, TMP_InputField.CharacterValidation.Integer);
                    speedInput.InputField.onValueChanged.AddListener(new Action<string>(value =>
                    {
                        int.TryParse(value, out var intvalue);
                        SelectedBloon.Speed = intvalue;
                    }));
                    speedInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                    speedInput.InputField.characterLimit = 9;
                    speedInput.InputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;

                    BaseStatsPanel.AddText(new Info("CashDropStatText", -75, -200, 350, 200), "Cash dropped:", 55, TextAlignmentOptions.MidlineLeft);
                    cashdropInput = BaseStatsPanel.AddInputField(new Info("SetCashdrop", 150, -200, 200, 100), $"{SelectedBloon.CashDropped}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value => { }), 75, TMP_InputField.CharacterValidation.Integer);
                    cashdropInput.InputField.onValueChanged.AddListener(new Action<string>(value =>
                    {
                        int.TryParse(value, out var intvalue);
                        SelectedBloon.CashDropped = intvalue;
                    }));
                    cashdropInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                    cashdropInput.InputField.characterLimit = 9;
                    cashdropInput.InputField.characterValidation = TMP_InputField.CharacterValidation.Digit;

                    BaseStatsPanel.AddText(new Info("DamageStatText", -75, -350, 350, 200), "Damage:", 55, TextAlignmentOptions.MidlineLeft);
                    damageInput = BaseStatsPanel.AddInputField(new Info("SetDamage", 150, -350, 200, 100), $"{SelectedBloon.Damage}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value => { }), 75, TMP_InputField.CharacterValidation.Integer);
                    damageInput.InputField.onValueChanged.AddListener(new Action<string>(value =>
                    {
                        int.TryParse(value, out var intvalue);
                        SelectedBloon.Damage = intvalue;
                    }));
                    damageInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                    damageInput.InputField.characterLimit = 9;
                    damageInput.InputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
                    #endregion

                    var BasePropertiesPanel = Settings.AddPanel(new Info("Properties", 0, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BasePropertiesPanel.AddText(new Info("Text", 0, 600, 550, 200), "Properties").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BasePropertiesPanel.AddText(new Info("CamoStatText", -50, 250, 400, 200), "Camo", 100, TextAlignmentOptions.MidlineLeft);
                    BasePropertiesPanel.AddCheckbox(new Info("SetCamo", 175, 250, 90, 90), SelectedBloon.IsCamo, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsCamo = value; }));

                    BasePropertiesPanel.AddText(new Info("LeadStatText", -50, 150, 400, 200), "Lead", 100, TextAlignmentOptions.MidlineLeft);
                    BasePropertiesPanel.AddCheckbox(new Info("SetLead", 175, 150, 90, 90), SelectedBloon.IsLead, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsLead = value; }));

                    BasePropertiesPanel.AddText(new Info("PurpleStatText", -50, 50, 400, 200), "Purple", 80, TextAlignmentOptions.MidlineLeft);
                    BasePropertiesPanel.AddCheckbox(new Info("SetPurple", 175, 50, 90, 90), SelectedBloon.IsPurple, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsPurple = value; }));

                    BasePropertiesPanel.AddText(new Info("WhiteStatText", -50, -50, 400, 200), "White", 100, TextAlignmentOptions.MidlineLeft);
                    BasePropertiesPanel.AddCheckbox(new Info("SetWhite", 175, -50, 90, 90), SelectedBloon.IsWhite, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsWhite = value; }));

                    BasePropertiesPanel.AddText(new Info("BlackStatText", -50, -150, 400, 200), "Black", 100, TextAlignmentOptions.MidlineLeft);
                    BasePropertiesPanel.AddCheckbox(new Info("SetBlack", 175, -150, 90, 90), SelectedBloon.IsBlack, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsBlack = value; }));

                    BasePropertiesPanel.AddText(new Info("FrozenStatText", -50, -250, 400, 200), "Frozen", 100, TextAlignmentOptions.MidlineLeft);
                    BasePropertiesPanel.AddCheckbox(new Info("SetFrozen", 175, -250, 90, 90), SelectedBloon.IsFrozen, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsFrozen = value; }));

                    //BasePropertiesPanel.AddText(new Info("RegrowStatText", -50, -250, 400, 200), "Regrow", 100, TextAlignmentOptions.MidlineLeft);
                    //BasePropertiesPanel.AddCheckbox(new Info("SetRegrow", 175, -250, 90, 90), SelectedBloon.IsRegrow, VanillaSprites.BlueInsertPanelRound, new Action<bool>(value => { SelectedBloon.IsRegrow = value; }));

                    /*BasePropertiesPanel.AddText(new Info("RegrowRateStatText", -50, -350, 400, 200), "Regrow Rate:", 70, TextAlignmentOptions.MidlineLeft);

                    var regrowInput = BasePropertiesPanel.AddInputField(new Info("SetRegrow", 175, -350, 150, 100), $"{SelectedBloon.RegrowRate}", VanillaSprites.BlueInsertPanelRound, new Action<string>(value => { }), 75, TMP_InputField.CharacterValidation.Integer);
                    regrowInput.InputField.onValueChanged.AddListener(new Action<string>(value =>
                    {
                        float.TryParse(value, out var floatvalue);
                        SelectedBloon.RegrowRate = floatvalue;
                    }));
                    regrowInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                    regrowInput.InputField.characterLimit = 9;
                    regrowInput.InputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;*/

                    var BaseChildrenPanel = Settings.AddPanel(new Info("Children", 617, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseChildrenPanel.AddText(new Info("Text", 0, 600, 550, 200), "Children").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;


                    BaseChildrenPanel.AddButton(new Info("CreateNewBloon", 0, 375, 450, 150), VanillaSprites.GreenBtnLong, new System.Action(() =>
                    {
                        MenuManager.instance.buttonClickSound.Play("ClickSounds");
                        CreateChildrenPopup();
                    }))
                    .AddText(new Info("Text", 0, 0, 350, 100), "Add Children", 100).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    ChildrenPanel = BaseChildrenPanel.AddScrollPanel(new Info("ChildrenScrollPanel", 0, -200, 500, 900), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, 50, 50);
                    UpdateChildPanel();
                    break;
                case EditorPanel.Behaviors:

                    var BaseBehaviorPanel = Settings.AddPanel(new Info("BaseBehaviorsPanel", -617, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseBehaviorPanel.AddText(new Info("Text", 0, 600, 550, 200), "Add Behaviors").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BaseBehaviorPanel.AddButton(new Info("AddTrigger", 0, 300, 450, 200), VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        MenuManager.instance.buttonClickSound.Play("ClickSounds");
                        BehaviorHelper.ShowAddBehaviorPopup(SelectedBloon, BehaviorType.Trigger, new Action(() => { UpdateBehaviorPanel(); }));
                    })).AddText(new Info("Text", 0, 0, 400, 150), "Add Trigger");

                    BaseBehaviorPanel.AddButton(new Info("AddAction", 0, 0, 450, 200), VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        MenuManager.instance.buttonClickSound.Play("ClickSounds");
                        BehaviorHelper.ShowAddBehaviorPopup(SelectedBloon, BehaviorType.Action, new Action(() => { UpdateBehaviorPanel(); }));
                    })).AddText(new Info("Text", 0, 0, 400, 150), "Add Action");

                    BaseBehaviorPanel.AddButton(new Info("AddBehavior", 0, -300, 450, 200), VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        MenuManager.instance.buttonClickSound.Play("ClickSounds");
                        BehaviorHelper.ShowAddBehaviorPopup(SelectedBloon, BehaviorType.Behavior, new Action(() => { UpdateBehaviorPanel(); }));
                    })).AddText(new Info("Text", 0, 0, 400, 150), "Add Behavior");

                    var addbehaviorpanel = Settings.AddPanel(new Info("idfk", 305f, 0, 1175, 1400), VanillaSprites.MainBGPanelBlue);
                    BehaviorsPanel = addbehaviorpanel.AddScrollPanel(new Info("BehaviorScrollPanel", 0, 0, 1075, 1300), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, 50, 50);

                    UpdateBehaviorPanel();

                    Behaviors.Button.interactable = false;
                    break;
                case EditorPanel.Spawning:
                    Spawning.Button.interactable = false;
                    var BaseAddBloonGroupPanel = Settings.AddPanel(new Info("BaseBloonGroup", -617, 0, 566, 1400), VanillaSprites.MainBGPanelBlue);
                    BaseAddBloonGroupPanel.AddText(new Info("Text", 0, 600, 550, 200), "Bloon Group").GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BaseAddBloonGroupPanel.AddButton(new Info("CreateSingleRoundGroup", 0, 375, 450, 200), VanillaSprites.GreenBtnLong, new System.Action(() =>
                    {
                        MenuManager.instance.buttonClickSound.Play("ClickSounds");
                        SelectedBloon.BloonRounds.Add(new CustomBloonRound());
                        UpdateRoundPanel();
                    }))
                    .AddText(new Info("Text", 0, 0, 350, 150), "Add Single Round Group", 100).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    BaseAddBloonGroupPanel.AddButton(new Info("CreateMultiRoundGroup", 0, 125, 450, 200), VanillaSprites.GreenBtnLong, new System.Action(() =>
                    {
                        MenuManager.instance.buttonClickSound.Play("ClickSounds");
                        SelectedBloon.BloonRounds.Add(new CustomBloonRound()
                        { IsMultiRound = true });
                        UpdateRoundPanel();
                    }))
                   .AddText(new Info("Text", 0, 0, 350, 150), "Add Multi Round Group", 100).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;

                    //313.3
                    var addbloonpanel = Settings.AddPanel(new Info("BaseBloonGroup", 305f, 0, 1175, 1400), VanillaSprites.MainBGPanelBlue);
                    RoundSpawnPanel = addbloonpanel.AddScrollPanel(new Info("BaseBloonGroup", 0, 0, 1075, 1300), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, 50, 50);

                    UpdateRoundPanel();
                    break;
                    
            }
        }

        public void UpdateVisuals()
        {
            Bloonimage.Image.color = SelectedBloon.Color;
            if (SelectedBloon.Decal1.Type != DecalType.None)
            {
                Decal1.SetActive(true);
                var names = GetSpriteNames(SelectedBloon.Decal1.Type);
                Decal1.Image.SetSprite(GetSprite<BloonFactoryMod>(names.Item1));
            }
            else
            {
                Decal1.SetActive(false);
            }

            if (SelectedBloon.Decal2.Type != DecalType.None)
            {
                Decal2.SetActive(true);
                var names = GetSpriteNames(SelectedBloon.Decal2.Type);
                Decal2.Image.SetSprite(GetSprite<BloonFactoryMod>(names.Item1));
            }
            else
            {
                Decal2.SetActive(false);
            }

            Decal1.transform.localPosition = new Vector3(OffsetPerPixel * (SelectedBloon.Decal1.GetOffsetX() - 64), OffsetPerPixel * (SelectedBloon.Decal1.GetOffsetY() - 64), 0);
            Decal2.transform.localPosition = new Vector3(OffsetPerPixel * (SelectedBloon.Decal2.GetOffsetX() - 64), OffsetPerPixel * (SelectedBloon.Decal2.GetOffsetY() - 64), 0);

            Decal1.Image.color = SelectedBloon.Decal1.Color;
            Decal2.Image.color = SelectedBloon.Decal2.Color;
        }
        public void CreateChildrenPopup()
        {
            ModHelperDropdown dropdown = null!;

            PopupScreen.instance.SafelyQueue(p =>
            {
                List<string> bloons = GetBloons();

                p.ShowPopup(PopupScreen.Placement.menuCenter,
                    "Add Children", "Choose the child bloon.",
                    new Action(() =>
                    {
                        string selectedbloon = bloons[dropdown.Dropdown.value];

                        PopupScreen.instance.SafelyQueue(screen =>
                        {
                            PopupScreen.instance.SafelyQueue(screen => screen.ShowSetNamePopup("Add Children", "How many child bloons to spawn?", new Action<string>(name =>
                            {
                                if (!string.IsNullOrEmpty(name))
                                {
                                    SelectedBloon.BloonChildren.Add(new CustomBloonChild() { Amount = int.Parse(name), BloonName = selectedbloon });
                                    UpdateChildPanel();
                                }
                            }), null));
                            PopupScreen.instance.SafelyQueue(screen => screen.ModifyField(tmpInputField =>
                            {
                                tmpInputField.textComponent.font = Fonts.Btd6FontBody;
                                tmpInputField.characterLimit = 4;
                                tmpInputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
                            }));
                        });

                    }), "Confirm", null, "Cancel", Popup.TransitionAnim.Scale, instantClose: true);

                TaskScheduler.ScheduleTask(() =>
                {
                    dropdown = p.GetFirstActivePopup().bodyObj.AddModHelperPanel(new Info("BloonsPanel", 400, 700))
                        .AddDropdown(new Info("Filter",
                                421.5F * 1.5f, 150F * 1.5f, new Vector2(.5f, 0.2f)), bloons, 600 , null, VanillaSprites.BlueInsertPanelRound, 70
                        );

                    healthInput.GetComponent<Mask>().enabled = false;
                    healthInput.GetComponent<Mask>().enabled = true;
                    damageInput.GetComponent<Mask>().enabled = false;
                    damageInput.GetComponent<Mask>().enabled = true;
                    speedInput.GetComponent<Mask>().enabled = false;
                    speedInput.GetComponent<Mask>().enabled = true;
                    cashdropInput.GetComponent<Mask>().enabled = false;
                    cashdropInput.GetComponent<Mask>().enabled = true;

                    TaskScheduler.ScheduleTask(() =>
                    {
                        p.GetFirstActivePopup().bodyObj.transform.localPosition = new Vector3(0, 50, 0);
                    });
                }, () => p.GetFirstActivePopup()?.bodyObj is not null);
            });
            
        }
        public List<string> GetBloons()
        {
            List<string> bloons = new List<string>();

            foreach (var bloon in Game.instance.model.bloonsByName)
            {
                bloons.Add(bloon.Key);
            }
            return bloons;
        }
        public void UpdateChildPanel()
        {
            ChildrenPanel.ScrollContent.transform.DestroyAllChildren();
            foreach (var children in SelectedBloon.BloonChildren)
            {
                ChildrenPanel.AddScrollContent(CreateChildrenPanel(children));
            }
        }
        public void UpdateRoundPanel()
        {
            RoundSpawnPanel.ScrollContent.transform.DestroyAllChildren();
            foreach (var round in SelectedBloon.BloonRounds)
            {
                RoundSpawnPanel.AddScrollContent(CreateRoundPanel(round));
            }
        }
        public void UpdateBehaviorPanel()
        {
            BehaviorsPanel.ScrollContent.transform.DestroyAllChildren();
            foreach (var behavior in SelectedBloon.BloonBehaviors)
            {
                var panel = CustomBloonBehavior.BehaviorByType[behavior.GetType()].CreatePanel(behavior, SelectedBloon);
                panel.AddButton(new Info("DeleteButton", 475, panel.initialInfo.Height / 2, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { SelectedBloon.BloonBehaviors.Remove(behavior); UpdateBehaviorPanel(); }));
                BehaviorsPanel.AddScrollContent(panel);
            }

        }
        public ModHelperPanel CreateRoundPanel(CustomBloonRound round)
        {
            var panel = ModHelperPanel.Create(new Info("RoundPanel", 0, 0, 975, 400), VanillaSprites.MainBGPanelBlue);

            panel.AddText(new Info("RoundNumberText", -313, 125, 250, 80), round.IsMultiRound ? "Start Round:" : "Round:", 100, TextAlignmentOptions.MidlineLeft).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var roundInput = panel.AddInputField(new Info("RoundNumberInput", -63, 125, 150, 70), $"{round.StartRound}", VanillaSprites.BlueInsertPanelRound, new Action<string>(roundText =>
            {
                round.StartRound = int.Parse(roundText);
            }), 100, TMP_InputField.CharacterValidation.Digit);
            roundInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            roundInput.InputField.characterLimit = 3;

            roundInput.SetActive(false);
            roundInput.SetActive(true);

            if (round.IsMultiRound)
            {
                panel.AddText(new Info("RoundNumberText", -313, 50, 250, 80), "End Round:", 100, TextAlignmentOptions.MidlineLeft).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                var roundEndInput = panel.AddInputField(new Info("RoundNumberInput", -63, 50, 150, 70), $"{round.EndRound}", VanillaSprites.BlueInsertPanelRound, new Action<string>(roundText =>
                {
                    round.EndRound = int.Parse(roundText);
                }), 100, TMP_InputField.CharacterValidation.Digit);
                roundEndInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
                roundEndInput.InputField.characterLimit = 3;

                roundEndInput.SetActive(false);
                roundEndInput.SetActive(true);
            }
            
            panel.AddText(new Info("AmountText", 137, 125, 200, 70), $"Amount:", 100, TextAlignmentOptions.MidlineLeft).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var amountInput = panel.AddInputField(new Info("RoundAmountInput", 337, 125, 150, 70), $"{round.Amount}", VanillaSprites.BlueInsertPanelRound, new Action<string>(roundText =>
            {
                round.Amount = int.Parse(roundText);
            }), 100, TMP_InputField.CharacterValidation.Digit);
            amountInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            amountInput.InputField.characterLimit = 3;

            amountInput.SetActive(false);
            amountInput.SetActive(true);

            panel.AddText(new Info("RoundSpacingText", -313, -125, 250, 80), "Spacing", 100, TextAlignmentOptions.MidlineLeft).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            var spacingInput = panel.AddInputField(new Info("RoundSpacingInput", -63, -125, 150, 70), $"{round.Spacing}", VanillaSprites.BlueInsertPanelRound, new Action<string>(roundText =>
            {
                round.Spacing = float.Parse(roundText);
            }), 100, TMP_InputField.CharacterValidation.Decimal);
            spacingInput.Text.GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            spacingInput.InputField.characterLimit = 5;

            spacingInput.SetActive(false);
            spacingInput.SetActive(true);


            panel.AddButton(new Info("Delete", 437, -150, 100, 100), VanillaSprites.AddRemoveBtn, new System.Action(() =>
            {
                SelectedBloon.BloonRounds.Remove(round);
                UpdateRoundPanel();
            }));

            return panel;
        }
        public ModHelperPanel CreateChildrenPanel(CustomBloonChild children)
        {
            var panel = ModHelperPanel.Create(new Info("ChildrenPanel", 0, 0, 450, 250), VanillaSprites.MainBGPanelBlue);

            panel.AddText(new Info("BloonName", 0, 50, 350, 100), children.BloonName, 100, TextAlignmentOptions.MidlineLeft).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            panel.AddText(new Info("BloonAmount", -100, -50, 150, 100), $"{children.Amount}x", 100, TextAlignmentOptions.MidlineLeft).GetComponent<NK_TextMeshProUGUI>().enableAutoSizing = true;
            panel.AddButton(new Info("Delete", 150, -50, 100, 100), VanillaSprites.AddRemoveBtn, new System.Action(() =>
            {
                SelectedBloon.BloonChildren.Remove(children);
                UpdateChildPanel();
            }));
            return panel;
        }
    }
    public enum EditorPanel
    {
        Visuals,
        Stats,
        Behaviors,
        Spawning
    }
}
