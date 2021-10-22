using HarmonyLib;
using Nick;
using SMU.Reflection;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore
{
    class CustomMenuPatches
    {

        public static event Action<string, int> menuCompleteAction;

        public static Action menuCreationAction;


        public static GameObject baseGameSelect;

        [HarmonyPatch(typeof(MainOptionsScreen), nameof(MainOptionsScreen.MenuOpen)), HarmonyPostfix]
        private static void opencustomMenu(MainOptionsScreen __instance, MenuSystem sys)
        {
            string screenName = sys.CurrentScreen();
            if (!screenName.StartsWith("modded_")) return;

            //since we're reusing the base game options menu for any menu, we have to delete all the objects we dont want

            __instance.name = screenName;

            Transform currentCanvas = __instance.gameObject.transform.Find("Canvas");
            Transform mainContainer = currentCanvas.Find("MainContainer");

            Transform menuTransform = mainContainer.Find("menu");

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

            TextOptionHandler.baseGameTitle = optionsModalTransform.Find("SoundTitle").gameObject;
            TextOptionHandler.baseGameTitle.name = "baseGameTitle";
            TextOptionHandler.baseGameTitle.SetActive(false);

            optionsModalTransform.Find("MasterSlider").name = "exampleSlider";

            Canvas canvasItem = currentCanvas.gameObject.GetComponent<Canvas>();
            canvasItem.pixelPerfect = true;
            canvasItem.renderMode = RenderMode.WorldSpace;
            canvasItem.gameObject.transform.position = new Vector3(0f, 0f, -5.32f);
            canvasItem.gameObject.transform.localScale = new Vector3(0.0025f, 0.0025f, 1f);


            string[] listOfImages = { "Megalon", "Pogbie", "Goku", "Jenny", "SadCat" };
            var random = new System.Random();

            Texture2D texture = ImageHandler.LoadImageFromEmbeddedResource("MenuCore.Images." + listOfImages[random.Next(listOfImages.Length)] + ".png", 1546, 2016);
            mainContainer.Find("Decoration").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            baseGameSelect.SetActive(false);

            SliderHandler.exampleSlider = optionsModalTransform.Find("exampleSlider").gameObject;
            SliderHandler.exampleSlider.SetActive(false);


            SelectorHandler.exampleSelector = GameObject.Instantiate(SliderHandler.exampleSlider, SliderHandler.exampleSlider.transform.parent);
            SelectorHandler.exampleSelector.name = "exampleSelector";
            SelectorHandler.exampleSelector.SetActive(false);
            SelectorHandler.exampleSelector.transform.Find("Button").Find("slider").gameObject.SetActive(false);
            SelectorHandler.exampleSelector.transform.Find("Button").Find("marker").gameObject.SetActive(false);
            //SelectorHandler.exampleSelector2.SetActive(false);
            CustomOptionMenuHandler.menus.Clear();
            menuCreationAction.Invoke();
            if (CustomOptionMenuHandler.menus.Count == 0) CustomOptionMenuHandler.createOriginalMenuCoreObject();
            menuCompleteAction.Invoke("", 0);
        }


        [HarmonyPatch(typeof(MenuUpdateFloat), nameof(MenuUpdateFloat.ConsumeInput)), HarmonyPostfix]
        private static void floatChange(MenuUpdateFloat __instance, MenuInputEvent ev, bool __result)
        {
           
            if (__instance.name.StartsWith("moddedSlider_"))
            {
                if (__result)
                {
                    CustomSliderMonoBehaviour customSliderObj = __instance.GetComponent<CustomSliderMonoBehaviour>();
                    var val = __instance.GetField<MenuUpdateFloat, MenuValueSource<float>.Tag>("srcTag").Value;
                    customSliderObj.setVal(val);
                    __instance.transform.Find("Button").Find("textvalue").GetComponent<TextMeshProUGUI>().text = val.ToString();
                    __instance.transform.Find("SelectionSound").GetComponent<ButtonSFXTrigger>().PlaySound();
                }
            }
            else if (__instance.name.StartsWith("moddedSelector_"))
            {
                if (__result)
                {
                    var val = __instance.GetField<MenuUpdateFloat, MenuValueSource<float>.Tag>("srcTag").Value;
                    CustomSelectorMonoBehaviour customSelectorObj = __instance.GetComponent<CustomSelectorMonoBehaviour>();
                    __instance.InvokeMethod<MenuUpdateFloat>("SetValue", 2);
                    string newItem = customSelectorObj.valChange(val);
                    if (customSelectorObj.prefix == "") {
                        __instance.transform.Find("Button").Find("textvalue").GetComponent<TextMeshProUGUI>().text = newItem;
                    }
                    else{
                        __instance.transform.Find("Button").Find("textvalue").GetComponent<TextMeshProUGUI>().text = customSelectorObj.prefix + newItem;
                    }

                    __instance.transform.Find("SelectionSound").GetComponent<ButtonSFXTrigger>().PlaySound();
                }/* 
                MenuAction.ActionButton _decBtn = __instance.GetField<MenuUpdateFloat, MenuAction.ActionButton>("_decBtn");
                MenuAction.ActionButton _incBtn = __instance.GetField<MenuUpdateFloat, MenuAction.ActionButton>("_incBtn");
                if (MenuAction.CheckButton(_decBtn, ev, true))
                {
                }
                if (MenuAction.CheckButton(_incBtn, ev, true))
                {
                }*/
            }

        }

    }
}
