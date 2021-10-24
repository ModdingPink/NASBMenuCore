using System;
using UnityEngine;
using SMU.Events;

namespace MenuCore.Behaviours
{
    class CustomSlider : MonoBehaviour
    {
        public Action<string, float> valChangeAction;
        //The action which we input into the createSlider
        public string menuItemName;
        public void SetValue(float value)
        {
            var menuItem = CustomOptionsMenuHandler.GetMenuItem(menuItemName);
            if (menuItem != null) menuItem.startValue = value;

            valChangeAction.InvokeAll(menuItemName, value); //invoke the action we input into createSlider
        }

    }
}
