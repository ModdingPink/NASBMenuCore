using MenuCore.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore
{
    public class CustomOptionsMenuHandler
    {
        internal static List<CustomOptionsMenu> menus = new List<CustomOptionsMenu>();

        private static List<GameObject> currentMenuObjects = new List<GameObject>();

        private static int currentSelectedMenu;

        public static void InitiateMenu(Action method) => MainOptionsScreen_MenuOpen.menuCreationAction += method;

        public static void CreateMenuTab(CustomOptionsMenu menu) => menus.Add(menu);

        internal static void MenuHasChanged(string ID, int menuChangedTo)
        {
            currentSelectedMenu = menuChangedTo;
            DestroyObjects();
            CreateObjects(menus[menuChangedTo]);
        }

        internal static MenuItem GetMenuItem(string itemName) => menus[currentSelectedMenu].menuItems.FirstOrDefault(x => x.optionID == itemName);

        private static void DestroyObjects()
        {
            if (currentMenuObjects.Count == 0) return;
            foreach (var currentObject in currentMenuObjects)
            {
                GameObject.Destroy(currentObject);
            }
            currentMenuObjects.Clear();
        }

        internal static void CreateOriginalMenuCoreObject()
        {
            var newMenu = new CustomOptionsMenu();
            newMenu.menuName = "MenuCore";
            newMenu.CreateText("optionText", "Install mods from");
            newMenu.CreateText("optionText2", "nasb.thunderstore.io!");
            menus.Add(newMenu);
        }

        internal static void CreateInitialSelector(string ID, int firstVal)
        {
            var listOfMenus = new List<string>();
            foreach (var menu in menus)
                listOfMenus.Add(menu.menuName);

            var selector = SelectorHandler.CreateSelector("MenuCoreModHandler", "Mod<b>", listOfMenus, firstVal, SelectorType.Center, OnSelectorChanged);
            var button = selector.transform.Find("Button");

            button.Find("Unfocused").GetComponent<Image>().color = new Color(1f, 0.41f, 0.7f);
            button.Find("textvalue").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }

        private static void OnSelectorChanged(string ID, int item) => MenuHasChanged(ID, item);

        private static void CreateObjects(CustomOptionsMenu menu)
        {
            foreach (var menuItem in menu.menuItems)
            {
                switch(menuItem.typeOfItem)
                {
                    case ItemType.Selector:
                        var selector = menuItem as Selector;
                        currentMenuObjects.Add(SelectorHandler.CreateSelector(selector.optionID, selector.optionName, selector.selections, (int)selector.startValue, selector.typeOfSelector, selector.action));
                        break;
                    case ItemType.Slider:
                        var slider = menuItem as Slider;
                        currentMenuObjects.Add(SliderHandler.CreateSlider(slider.optionID, slider.optionName, slider.rangeMin, slider.rangeMax, slider.increment, slider.startValue, slider.action));
                        break;
                    case ItemType.Text:
                        currentMenuObjects.Add(TextOptionHandler.CreateText(menuItem.optionID, menuItem.optionName));
                        break;
                }
            }
        } 

    }
}
