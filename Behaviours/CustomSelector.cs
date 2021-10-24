using System;
using System.Collections.Generic;
using UnityEngine;
using SMU.Events;

namespace MenuCore.Behaviours
{
    class CustomSelector : MonoBehaviour
    {

        public Action<string, int> valueChangeAction;

        public List<string> selections;

        public int selectedIndex = 0;

        public string prefix = String.Empty;

        public string optionID = String.Empty;
         
        public SelectorType selectorType;

        public string SetValue(float val)
        {
            // The slider is set to 2 with a step of 1, if its moved left, the new value is 2-1, which is 1
            if (val == 1)
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = selections.Count - 1;
            }
            else if (val == 3)
            {
                selectedIndex++;
                if (selectedIndex > selections.Count - 1) selectedIndex = 0;
            }

            var menuItem = CustomOptionsMenuHandler.GetMenuItem(optionID);
            if (menuItem != null) menuItem.startValue = selectedIndex;

            valueChangeAction.InvokeAll(optionID, selectedIndex); // invoke the action we input into createSelector
            return selections[selectedIndex];
        }
    }
}
