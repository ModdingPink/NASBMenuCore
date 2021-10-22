using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nick;
using SMU.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore
{

    public class CustomOptionMenuHandler
    {

        public static List<CustomOptionMenu> menus = new List<CustomOptionMenu>();

        private static List<GameObject> currentMenuObjects = new List<GameObject>();

        private static int currentSelectedMenu;

        public static void initiateMenu(Action method){
            CustomMenuPatches.menuCreationAction += method;
        }

        public static void createMenuTab(CustomOptionMenu menu)
        {
            menus.Add(menu);
        }

        public static void menuHasChanged(string ID, int menuChangedTo)
        {
            currentSelectedMenu = menuChangedTo;
            destoryObjects();
            createObjects(menus[menuChangedTo]);
        }

        public static MenuItem getMenuItem(string itemName)
        {
            foreach(MenuItem menu in menus[currentSelectedMenu].menuList)
            {
                if(menu.optionID == itemName)
                {
                    return menu;
                }
            }
            return null;
        }

        private static void destoryObjects()
        {
            if (currentMenuObjects.Count == 0) return;
            foreach (var currentObject in currentMenuObjects)
            {
                GameObject.DestroyImmediate(currentObject);
            }
            currentMenuObjects.Clear();
        }

        public static void createOriginalMenuCoreObject()
        {
            CustomOptionMenu newMenu = new CustomOptionMenu();
            newMenu.menuName = "MenuCore";
            newMenu.createText("optionText", "Install mods from");
            newMenu.createText("optionText2", "nasb.thunderstore.io!");
            menus.Add(newMenu);
        }

        public static void createInitialSelector(string ID, int firstVal)
        {
            List<string> listOfMenus = new List<string>();
            foreach (var currentObject in CustomOptionMenuHandler.menus)
            {
                listOfMenus.Add(currentObject.menuName);
            }
            GameObject selector = SelectorHandler.createSelector("MenuCoreModHandler", "Mod<b>", listOfMenus, firstVal, SelectorType.Center, sliderChanged);
            selector.transform.Find("Button").Find("Unfocused").GetComponent<Image>().color = new Color(1f, 0.41f, 0.7f);
            selector.transform.Find("Button").Find("textvalue").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        private static void sliderChanged(string ID, float item)
        {
            CustomOptionMenuHandler.menuHasChanged(ID, (int)item);
        }

        private static void createObjects(CustomOptionMenu menu)
        {
            foreach (var menuItem in menu.menuList)
            {
                if(menuItem.typeOfItem == itemType.Selector)
                {
                    currentMenuObjects.Add(SelectorHandler.createSelector(menuItem.optionID, menuItem.optionName, menuItem.selections, (int)menuItem.startValue, menuItem.typeOfSelector, menuItem.action));
                }
                else if (menuItem.typeOfItem == itemType.Slider)
                {
                    currentMenuObjects.Add(SliderHandler.createSlider(menuItem.optionID, menuItem.optionName, menuItem.rangeMin, menuItem.rangeMax, menuItem.increment, menuItem.startValue, menuItem.action));
                }else if (menuItem.typeOfItem == itemType.Text)
                {
                    currentMenuObjects.Add(TextOptionHandler.createText(menuItem.optionID, menuItem.optionName));
                }
            }
        } 

    }
}
