using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MenuCore
{
    class CustomSliderMonoBehaviour : MonoBehaviour
    {

        public Action<string, float> valChangeAction;
        //The action which we input into the createSlider
        public string menuItemName;
        public void setVal(float value) {
            MenuItem menuItem = CustomOptionMenuHandler.getMenuItem(menuItemName);
            if (menuItem != null) menuItem.startValue = value;

            try
            {
                valChangeAction.Invoke(menuItemName, value); //invoke the action we input into createSlider
            }
            catch
            {
                MenuCore.LogInfo("Nothing to invoke");
            }
        }

    }
}
