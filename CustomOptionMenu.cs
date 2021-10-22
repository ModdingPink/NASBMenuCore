using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuCore
{
    public enum itemType
    {
        Selector,
        Slider,
        Text
    }
    public enum SelectorType
    {
        RightSide,
        Center
    }
    public class MenuItem {
        

        public itemType typeOfItem;
        public string optionID;
        public string optionName;
        public float rangeMin;
        public float rangeMax;
        public float increment;
        public float startValue;
        public Action<string, float> action;
        public SelectorType typeOfSelector;
        public List<string> selections;
    }

    public class CustomOptionMenu
    {
        public List<MenuItem> menuList = new List<MenuItem>();
        public string menuName;
        public void createSlider(string optionID, string optionName, float rangeMin, float rangeMax, float increment, float startValue, Action<string, float> changeValAction) {
            MenuItem menuItem = new MenuItem();
            menuItem.typeOfItem = itemType.Slider;
            menuItem.optionID = optionID;
            menuItem.optionName = optionName;
            menuItem.rangeMin = rangeMin;
            menuItem.rangeMax = rangeMax;
            menuItem.increment = increment;
            menuItem.startValue = startValue;
            menuItem.action = changeValAction;
            menuList.Add(menuItem);
        }

        public void createSelector(string optionID, string optionName, List<string> selections, int firstItem, SelectorType typeOfSelector, Action<string, float> changeValAction)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.typeOfItem = itemType.Selector;
            menuItem.optionName = optionName;
            menuItem.optionID = optionID;
            menuItem.startValue = firstItem;
            menuItem.typeOfSelector = typeOfSelector;
            menuItem.action = changeValAction;
            menuItem.selections = selections;
            menuList.Add(menuItem);
        }

        public void createText(string optionID, string title)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.optionName = title;
            menuItem.optionID = title;
            menuItem.typeOfItem = itemType.Text;
            menuList.Add(menuItem);
        }

    }
}
