using MenuCore.Behaviours;
using MenuCore.Patches;
using Nick;
using SMU.Reflection;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MenuCore
{
    class SelectorHandler
    {
        internal static GameObject clone;

        internal static GameObject CreateSelector(string optionID, string optionName, List<string> selections, int firstItem, SelectorType typeOfSelector, Action<string, int> changeValAction)
        {
            var SelectorObj = GameObject.Instantiate(clone, clone.transform.parent);
            SelectorObj.name = "moddedSelector_" + optionName;
            SelectorObj.SetActive(true);

            var menuFloat = SelectorObj.GetComponent<MenuUpdateFloat>();
            menuFloat.SetField<MenuUpdateFloat, float>("_minVal", 1);
            menuFloat.SetField<MenuUpdateFloat, float>("_maxVal", 3);
            menuFloat.SetField<MenuUpdateFloat, float>("_step", 1);
            menuFloat.InvokeMethod<MenuUpdateFloat>("SetValue", 2);

            var buttonTransform = SelectorObj.transform.Find("Button");
            buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = optionName;
            var textTransform = buttonTransform.Find("textvalue");
            var textMesh = textTransform.GetComponent<TextMeshProUGUI>();

            var customSelectorObj = SelectorObj.AddComponent<CustomSelector>();
            if (typeOfSelector == SelectorType.Center) {
                customSelectorObj.prefix = optionName + ": ";
                buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = String.Empty;
                textMesh.alignment = TextAlignmentOptions.Center;
                textTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(1300, 100);
                textMesh.text = customSelectorObj.prefix + selections[firstItem];

                GameObject.Instantiate(MainOptionsScreen_MenuOpen.baseGameSelect.transform.Find("LeftArrow").gameObject, SelectorObj.transform);
                GameObject.Instantiate(MainOptionsScreen_MenuOpen.baseGameSelect.transform.Find("RightArrow").gameObject, SelectorObj.transform);
            }
            else if(typeOfSelector == SelectorType.RightSide)
            {
                textMesh.alignment = TextAlignmentOptions.Right;
                textMesh.margin = new Vector4(0, 0, 40, 0);
                textTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(850, 100);
                customSelectorObj.prefix = String.Empty;
                buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = optionName;
                textMesh.text = selections[firstItem];
            }
            customSelectorObj.selections = selections;
            customSelectorObj.valueChangeAction += changeValAction;
            customSelectorObj.selectedIndex = firstItem;
            customSelectorObj.selectorType = typeOfSelector;
            customSelectorObj.optionID = optionID;
            return SelectorObj;
        }
    }
}
