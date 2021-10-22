using HarmonyLib;
using Nick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore
{
    class CreateMenuCustomButton
    {

        [HarmonyPatch(typeof(MainMenuScreen), nameof(MainMenuScreen.MenuOpen)), HarmonyPostfix]
        private static void openMenu(MainMenuScreen __instance, MenuSystem sys)
        {
            if (__instance == null || __instance.name != "mainmenu(Clone)") return;

            Transform currentCanvas = __instance.gameObject.transform.Find("Canvas");

            Transform buttonContainer = currentCanvas.Find("MainContainer").Find("Buttons");
            GameObject orgButton = buttonContainer.Find("OptionsButtonPC").gameObject;
            GameObject OptionsButton = GameObject.Instantiate<UnityEngine.GameObject>(orgButton, buttonContainer);
            GameObject moddedOptionsButton = GameObject.Instantiate<UnityEngine.GameObject>(orgButton, buttonContainer);

            OptionsButton.name = "OptionsButtonPC";
            moddedOptionsButton.name = "ModdedOptionsButton";

            Transform moddedPivot = moddedOptionsButton.transform.Find("Pivot");
            Transform optionsPivot = OptionsButton.transform.Find("Pivot");

            Transform textObj = moddedPivot.Find("Text");
            textObj.Find("Text").GetComponent<TextMeshProUGUI>().text = "Mod Settings";
            textObj.Find("Shadow").GetComponent<TextMeshProUGUI>().text = "Mod Settings";
            textObj.Find("Outline").GetComponent<TextMeshProUGUI>().text = "Mod Settings";

            Texture2D texture = ImageHandler.LoadImageFromEmbeddedResource("MenuCore.Images.MenuImage.png", 552, 524);
            Texture2D textureBorder = ImageHandler.LoadImageFromEmbeddedResource("MenuCore.Images.Border.png", 552, 524);
            Texture2D textureModIcon = ImageHandler.LoadImageFromEmbeddedResource("MenuCore.Images.modicon.png", 552, 524);

            Image moddedBGImg = moddedPivot.Find("Background").GetComponent<Image>();
            Image moddedBorderImg = moddedPivot.Find("Outline").GetComponent<Image>();
            Image moddedModIcon = moddedPivot.Find("Icon").GetComponent<Image>();


            Image optionsBGImg = optionsPivot.Find("Background").GetComponent<Image>();
            Image optionsBorderImg = optionsPivot.Find("Outline").GetComponent<Image>();

            optionsBGImg.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            optionsBorderImg.sprite = Sprite.Create(textureBorder, new Rect(0.0f, 0.0f, textureBorder.width, textureBorder.height), new Vector2(0.5f, 0.5f), 100.0f);

            moddedBGImg.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            moddedBorderImg.sprite = Sprite.Create(textureBorder, new Rect(0.0f, 0.0f, textureBorder.width, textureBorder.height), new Vector2(0.5f, 0.5f), 100.0f);
            moddedModIcon.sprite = Sprite.Create(textureModIcon, new Rect(0.0f, 0.0f, textureModIcon.width, textureModIcon.height), new Vector2(0.5f, 0.5f), 100.0f);

            moddedModIcon.gameObject.transform.localScale = new Vector3(0.625f, 0.589625f, 1f);
            moddedBGImg.color = new Color(0.3f, 0.4f, 1f);

            moddedBGImg.gameObject.transform.localScale = new Vector3(0.5f, 1f, 1f);
            moddedBorderImg.gameObject.transform.localScale = new Vector3(0.52f, 1.12f, 1f);
            optionsBGImg.gameObject.transform.localScale = new Vector3(0.5f, 1f, 1f);
            optionsBorderImg.gameObject.transform.localScale = new Vector3(0.52f, 1.12f, 1f);

            moddedOptionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(970, 231);
            OptionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(970, 231);

            Canvas canvasItem = currentCanvas.gameObject.GetComponent<Canvas>();
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

            SwitchMenuScreen switchMenu = moddedOptionsButton.GetComponent<SwitchMenuScreen>();
            switchMenu.switchScreenId = "modded_options";

        }

    }
}
