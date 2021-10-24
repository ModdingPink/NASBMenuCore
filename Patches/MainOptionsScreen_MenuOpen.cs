using HarmonyLib;
using Nick;
using SMU.Events;
using SMU.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore.Patches
{
    [HarmonyPatch(typeof(MainOptionsScreen), nameof(MainOptionsScreen.MenuOpen))]
    class MainOptionsScreen_MenuOpen
    {
        public static event Action<string, int> MenuCompleteAction;

        public static Action menuCreationAction;

        public static GameObject baseGameSelect;

        private static void Postfix(MainOptionsScreen __instance, MenuSystem sys)
        {
            var screenName = sys.CurrentScreen();
            if (!screenName.StartsWith("modded_")) return;

            //since we're reusing the base game options menu for any menu, we have to delete all the objects we dont want

            __instance.name = screenName;

            var currentCanvas = __instance.gameObject.transform.Find("Canvas");
            var mainContainer = currentCanvas.Find("MainContainer");

            var menuTransform = mainContainer.Find("menu");

            GameObject.Destroy(menuTransform.Find("Title").gameObject);
            GameObject.Destroy(menuTransform.Find("NavBar").gameObject);
            GameObject.Destroy(menuTransform.Find("creditsbutton").gameObject);

            menuTransform.Find("TitlePC").GetComponent<TextMeshProUGUI>().text = screenName.Replace("_", " ");

            Transform optionsModalTransform = menuTransform.Find("optionsModal");
            GameObject.Destroy(optionsModalTransform.Find("MusicSlider").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("SfxSlider").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("NarratorSlider").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("GraphicsTitle").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("qualityButton").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("antialiasingButton").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("resolutionButton").gameObject);
            GameObject.Destroy(optionsModalTransform.Find("fullscreenButton").gameObject);

            baseGameSelect = optionsModalTransform.Find("languageButton").gameObject;
            baseGameSelect.name = "baseGameSelect";

            TextOptionHandler.clone = optionsModalTransform.Find("SoundTitle").gameObject;
            TextOptionHandler.clone.name = "baseGameTitle";
            TextOptionHandler.clone.SetActive(false);

            optionsModalTransform.Find("MasterSlider").name = "exampleSlider";

            var canvasItem = currentCanvas.gameObject.GetComponent<Canvas>();
            canvasItem.pixelPerfect = true;
            canvasItem.renderMode = RenderMode.WorldSpace;
            canvasItem.gameObject.transform.position = new Vector3(0f, 0f, -5.32f);
            canvasItem.gameObject.transform.localScale = new Vector3(0.0025f, 0.0025f, 1f);

            string[] listOfImages = { "Megalon", "Pogbie", "Goku", "Jenny", "SadCat" };

            var texture = ImageHelper.LoadTextureFromResources("MenuCore.Images." + listOfImages[UnityEngine.Random.Range(0, listOfImages.Length)] + ".png");
            mainContainer.Find("Decoration").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            baseGameSelect.SetActive(false);

            SliderHandler.clone = optionsModalTransform.Find("exampleSlider").gameObject;
            SliderHandler.clone.SetActive(false);

            SelectorHandler.clone = GameObject.Instantiate(SliderHandler.clone, SliderHandler.clone.transform.parent);
            SelectorHandler.clone.name = "exampleSelector";
            SelectorHandler.clone.SetActive(false);
            SelectorHandler.clone.transform.Find("Button").Find("slider").gameObject.SetActive(false);
            SelectorHandler.clone.transform.Find("Button").Find("marker").gameObject.SetActive(false);
            //SelectorHandler.exampleSelector2.SetActive(false);
            CustomOptionsMenuHandler.menus.Clear();
            menuCreationAction.InvokeAll();
            if (CustomOptionsMenuHandler.menus.Count == 0) CustomOptionsMenuHandler.CreateOriginalMenuCoreObject();
            MenuCompleteAction.InvokeAll(String.Empty, 0);
        }
    }
}
