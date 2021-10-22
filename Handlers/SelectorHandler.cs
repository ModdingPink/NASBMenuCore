using HarmonyLib;
using Nick;
using SMU.Reflection;
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
    class SelectorHandler
    {
      
        public static GameObject exampleSelector;

        public static GameObject createSelector(string optionID, string optionName, List<string> selections, int firstItem, SelectorType typeOfSelector, Action<string, float> changeValAction)
        {
            GameObject SelectorObj = GameObject.Instantiate(exampleSelector, exampleSelector.transform.parent);
            SelectorObj.name = "moddedSelector_" + optionName;
            SelectorObj.SetActive(true);

            MenuUpdateFloat menuFloat = SelectorObj.GetComponent<MenuUpdateFloat>();
            menuFloat.SetField<MenuUpdateFloat, float>("_minVal", 1);
            menuFloat.SetField<MenuUpdateFloat, float>("_maxVal", 3);
            menuFloat.SetField<MenuUpdateFloat, float>("_step", 1);
            menuFloat.InvokeMethod<MenuUpdateFloat>("SetValue", 2);

            Transform buttonTransform = SelectorObj.transform.Find("Button");
            buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = optionName;
            Transform textTransform = buttonTransform.Find("textvalue");
            TextMeshProUGUI textMesh = textTransform.GetComponent<TextMeshProUGUI>();

            CustomSelectorMonoBehaviour customSelectorObj = SelectorObj.AddComponent<CustomSelectorMonoBehaviour>();
            if (typeOfSelector == SelectorType.Center) {
                customSelectorObj.prefix = optionName + ": ";
                buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = "";
                textMesh.alignment = TextAlignmentOptions.Center;
                textTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(1300, 100);
                textMesh.text = customSelectorObj.prefix + selections[firstItem];

                GameObject.Instantiate(CustomMenuPatches.baseGameSelect.transform.Find("LeftArrow").gameObject, SelectorObj.transform);
                GameObject.Instantiate(CustomMenuPatches.baseGameSelect.transform.Find("RightArrow").gameObject, SelectorObj.transform);


            }
            else if(typeOfSelector == SelectorType.RightSide)
            {
                textMesh.alignment = TextAlignmentOptions.Right;
                textTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(850, 100);
                customSelectorObj.prefix = "";
                buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = optionName;
                textMesh.text = selections[firstItem];
            }
            customSelectorObj.selections = selections;
            customSelectorObj.valChangeAction += changeValAction;
            customSelectorObj.itemLoc = firstItem;
            customSelectorObj.selectorType = typeOfSelector;
            customSelectorObj.menuItemName = optionID;
            return SelectorObj;
            //customSelectorObj.setVal(firstItem);
        }
    }
}
