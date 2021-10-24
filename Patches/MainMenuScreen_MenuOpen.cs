using HarmonyLib;
using Nick;
using SMU.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore.Patches
{
    [HarmonyPatch(typeof(MainMenuScreen), nameof(MainMenuScreen.MenuOpen))]
    class MainMenuScreen_MenuOpen
    {
        private static void Postfix(MainMenuScreen __instance, MenuSystem sys)
        {
            Plugin.LogInfo("Creating Menu Button!");

            if (__instance == null || __instance.name != "mainmenu(Clone)") return;

            var currentCanvas = __instance.gameObject.transform.Find("Canvas");

            var buttonContainer = currentCanvas.Find("MainContainer").Find("Buttons");
            var orgButton = buttonContainer.Find("OptionsButtonPC").gameObject;
            var OptionsButton = GameObject.Instantiate<UnityEngine.GameObject>(orgButton, buttonContainer);
            // this is really dumb but it works lmao; parenting to the online button makes it follow the animations
            var moddedOptionsButton = GameObject.Instantiate<UnityEngine.GameObject>(orgButton, GameObject.Find("test/menu/mainmenu(Clone)/Canvas/MainContainer/Buttons/OnlineButton").transform);

            OptionsButton.name = "OptionsButtonPC";
            moddedOptionsButton.name = "ModdedOptionsButton";

            var moddedPivot = moddedOptionsButton.transform.Find("Pivot");
            var optionsPivot = OptionsButton.transform.Find("Pivot");

            var textContent = moddedPivot.Find("Text").GetComponent<MenuTextContent>();
            textContent.SetString("Mod Settings");

            var sprite = ImageHelper.LoadSpriteFromResources("MenuCore.Images.MenuImage.png");
            var spriteBorder = ImageHelper.LoadSpriteFromResources("MenuCore.Images.Border.png");
            var spriteModIcon = ImageHelper.LoadSpriteFromResources("MenuCore.Images.modicon.png");

            var moddedBGImg = moddedPivot.Find("Background").GetComponent<Image>();
            var moddedBorderImg = moddedPivot.Find("Outline").GetComponent<Image>();
            var moddedModIcon = moddedPivot.Find("Icon").GetComponent<Image>();

            var optionsBGImg = optionsPivot.Find("Background").GetComponent<Image>();
            var optionsBorderImg = optionsPivot.Find("Outline").GetComponent<Image>();

            optionsBGImg.sprite = sprite;
            optionsBorderImg.sprite = spriteBorder;

            moddedBGImg.sprite = sprite;
            moddedBorderImg.sprite = spriteBorder;
            moddedModIcon.sprite = spriteModIcon;

            moddedModIcon.gameObject.transform.localScale = new Vector3(0.625f, 0.589625f, 1f);
            moddedBGImg.color = new Color(0.3f, 0.4f, 1f);

            moddedBGImg.gameObject.transform.localScale = new Vector3(0.5f, 1f, 1f);
            moddedBorderImg.gameObject.transform.localScale = new Vector3(0.52f, 1.12f, 1f);
            optionsBGImg.gameObject.transform.localScale = new Vector3(0.5f, 1f, 1f);
            optionsBorderImg.gameObject.transform.localScale = new Vector3(0.52f, 1.12f, 1f);

            moddedOptionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(970, 231);
            OptionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(970, 231);

            var canvasItem = currentCanvas.gameObject.GetComponent<Canvas>();
            canvasItem.pixelPerfect = true;
            canvasItem.renderMode = RenderMode.WorldSpace;
            canvasItem.gameObject.transform.position = new Vector3(0f, 0f, -5f);
            canvasItem.gameObject.transform.localScale = new Vector3(0.0025f, 0.0025f, 1f);

            moddedOptionsButton.transform.position = new Vector3(-1.325f, -1.75f, -5f);
            moddedModIcon.gameObject.transform.position = new Vector3(-1.1161f, -2.0388f, -5f);

            canvasItem.renderMode = RenderMode.ScreenSpaceCamera;
            canvasItem.gameObject.transform.localScale = new Vector3(0.4167f, 0.4167f, 0.4167f);
            canvasItem.gameObject.transform.position = new Vector3(800f, 450f, 0f);

            GameObject.DestroyImmediate(orgButton);

            var switchMenu = moddedOptionsButton.GetComponent<SwitchMenuScreen>();
            switchMenu.switchScreenId = "modded_options";
        }
    }
}
