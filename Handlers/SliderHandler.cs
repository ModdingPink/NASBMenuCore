using MenuCore.Behaviours;
using Nick;
using SMU.Reflection;
using System;
using TMPro;
using UnityEngine;

namespace MenuCore
{
    class SliderHandler
    {
        internal static GameObject clone;
        internal static GameObject CreateSlider(string optionID, string optionName, float rangeMin, float rangeMax, float increment, float startValue, Action<string, float> changeValAction)
        {
            GameObject SliderObj = GameObject.Instantiate(clone, clone.transform.parent);
            SliderObj.name = "moddedSlider_" + optionName;
            SliderObj.SetActive(true);
            MenuUpdateFloat menuFloat = SliderObj.GetComponent<MenuUpdateFloat>();
            menuFloat.SetField<MenuUpdateFloat, float>("_minVal", rangeMin);
            menuFloat.SetField<MenuUpdateFloat, float>("_maxVal", rangeMax);
            menuFloat.SetField<MenuUpdateFloat, float>("_step", increment);
            menuFloat.InvokeMethod<MenuUpdateFloat>("SetValue", startValue);
            Transform buttonTransform = SliderObj.transform.Find("Button");
            buttonTransform.Find("MasterText").GetComponent<TextMeshProUGUI>().text = optionName;
            Transform textTransform = buttonTransform.Find("textvalue");
            textTransform.GetComponent<TextMeshProUGUI>().text = startValue.ToString();
            CustomSlider customSliderObj = SliderObj.AddComponent<CustomSlider>();
            customSliderObj.valChangeAction += changeValAction;
            customSliderObj.menuItemName = optionID;
            return SliderObj;
        }
    }
}
