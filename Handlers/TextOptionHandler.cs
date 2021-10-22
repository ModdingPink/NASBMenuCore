using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MenuCore
{
    class TextOptionHandler
    {
        public static GameObject baseGameTitle;
        public static GameObject createText(string optionID, string text)
        {
            GameObject SliderObj = GameObject.Instantiate(baseGameTitle, baseGameTitle.transform.parent);
            SliderObj.name = "moddedSlider_" + optionID;
            SliderObj.SetActive(true);
            SliderObj.GetComponent<TextMeshProUGUI>().text = text;
            return SliderObj;
        }
    }
}
