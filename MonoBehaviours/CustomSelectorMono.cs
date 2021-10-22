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
    class CustomSelectorMonoBehaviour : MonoBehaviour
    {

        public Action<string, float> valChangeAction;

        public List<string> selections;

        public int itemLoc = 0;

        public string prefix = "";

        public string menuItemName = "";
         
        public SelectorType selectorType;

        public string valChange(float val)
        {
            //The slider is set to 2 with a step of 1, if its moved left, the new value is 2-1, which is 1
            if (val == 1)
            {
                itemLoc -= 1;
                if (itemLoc < 0) itemLoc = selections.Count - 1;
            }
            else if (val == 3)
            {
                itemLoc += 1;
                if (itemLoc > selections.Count - 1) itemLoc = 0;
            }

            MenuItem menuItem = CustomOptionMenuHandler.getMenuItem(menuItemName);
            if (menuItem != null) menuItem.startValue = (int)itemLoc;

            valChangeAction.Invoke(menuItemName, itemLoc); //invoke the action we input into createSelector
            return selections[itemLoc];
        }
    }
}
