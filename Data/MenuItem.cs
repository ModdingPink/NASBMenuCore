using System;

namespace MenuCore
{
    public abstract class MenuItem
    {
        public ItemType typeOfItem;
        public string optionID;
        public string optionName;
        
        public float startValue;
        public Action action;
    }
}
