using System;
using System.Collections.Generic;

namespace MenuCore
{
    public class CustomOptionsMenu
    {
        public string menuName;
        public List<MenuItem> menuItems = new List<MenuItem>();

        /// <summary>
        /// Creates a slider
        /// </summary>
        /// <param name="optionID">ID for this slider. This will be included in the callback</param>
        /// <param name="optionName">Name that will be displayed alongside the slider.</param>
        /// <param name="rangeMin">Minimum value.</param>
        /// <param name="rangeMax">Maximum value.</param>
        /// <param name="increment">How far to increment each time the slider is changed.</param>
        /// <param name="selectedValue">Initial value for when the menu is opened.</param>
        /// <param name="changeValAction">Callback for when the value is changed. Provides the optionID and new value as a float.</param>
        public void CreateSlider(string optionID, string optionName, float rangeMin, float rangeMax, float increment, float selectedValue, Action<string, float> changeValAction)
        {
            var menuItem = new Slider
            {
                typeOfItem = ItemType.Slider,
                optionID = optionID,
                optionName = optionName,
                rangeMin = rangeMin,
                rangeMax = rangeMax,
                increment = increment,
                startValue = selectedValue,
                action = changeValAction
            };
            menuItems.Add(menuItem);
        }

        /// <summary>
        /// Create a selector to choose between a list of items
        /// </summary>
        /// <param name="optionID">ID for this selector. This will be included in the callback</param>
        /// <param name="optionName">Name that will be displayed alongside the selector.</param>
        /// <param name="selections">List of items to choose from.</param>
        /// <param name="selectedIndex">Index of the initial value when the menu loads.</param>
        /// <param name="typeOfSelector">Type of selector. Either right-aligned or center-aligned.</param>
        /// <param name="changeValAction">Callback for when the value is changed. Provides the optionID and new value as a float.</param>
        public void CreateSelector(string optionID, string optionName, List<string> selections, int selectedIndex, SelectorType typeOfSelector, Action<string, int> changeValAction)
        {
            var menuItem = new Selector
            {
                typeOfItem = ItemType.Selector,
                optionName = optionName,
                optionID = optionID,
                startValue = selectedIndex,
                typeOfSelector = typeOfSelector,
                action = changeValAction,
                selections = selections
            };
            menuItems.Add(menuItem);
        }

        /// <summary>
        /// Create a text element.
        /// </summary>
        /// <param name="optionID">ID for this text element.</param>
        /// <param name="text">The text to be displayed</param>
        public void CreateText(string optionID, string text)
        {
            var menuItem = new Text
            {
                optionName = text,
                optionID = optionID,
                typeOfItem = ItemType.Text
            };
            menuItems.Add(menuItem);
        }
    }
}
