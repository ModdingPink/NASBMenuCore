using System;

namespace MenuCore
{
    class Slider : MenuItem
    {
        public float rangeMin;
        public float rangeMax;
        public float increment;

        public new Action<string, float> action;
    }
}
