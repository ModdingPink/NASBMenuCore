using System;
using System.Collections.Generic;

namespace MenuCore
{
    class Selector : MenuItem
    {
        public new int startValue;
        public new Action<string, int> action;
        public SelectorType typeOfSelector;
        public List<string> selections;
    }
}
