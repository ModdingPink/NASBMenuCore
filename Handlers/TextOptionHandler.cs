using TMPro;
using UnityEngine;

namespace MenuCore
{
    class TextOptionHandler
    {
        internal static GameObject clone;
        internal static GameObject CreateText(string optionID, string text)
        {
            GameObject SliderObj = GameObject.Instantiate(clone, clone.transform.parent);
            SliderObj.name = "moddedText_" + optionID;
            SliderObj.SetActive(true);
            SliderObj.GetComponent<TextMeshProUGUI>().text = text;
            return SliderObj;
        }
    }
}
