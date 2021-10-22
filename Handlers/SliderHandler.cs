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
    class SliderHandler
    {

        public static GameObject exampleSlider;
        public static GameObject createSlider(string optionID, string optionName, float rangeMin, float rangeMax, float increment, float startValue, Action<string, float> changeValAction)
        {
            GameObject SliderObj = GameObject.Instantiate(exampleSlider, exampleSlider.transform.parent);
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
            CustomSliderMonoBehaviour customSliderObj = SliderObj.AddComponent<CustomSliderMonoBehaviour>();
            customSliderObj.valChangeAction += changeValAction;
            customSliderObj.menuItemName = optionID;
            return SliderObj;
        }
    }
}
