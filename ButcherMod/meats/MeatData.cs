using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalHusbandryMod.meats
{
    public class MeatData
    {
        public List<MeatItem> Meat;

        public MeatData()
        {
            this.Meat = new List<MeatItem>()
            {
                new MeatItem("Beef", 639, 100, 15),
                new MeatItem("Pork", 640, 1250, 30),
                new MeatItem("Chicken", 641, 250, 15),
                new MeatItem("Duck", 642, 800, 20),
                new MeatItem("Rabbit", 643, 2500, 20),
                new MeatItem("Mutton", 644, 650, 20),
            };
        }

        public MeatItem getMeatItem(int meatIndex)
        {
            MeatItem item = this.Meat.FirstOrDefault(o => o.Index == meatIndex);

            if (item == null)
            {
                throw new ArgumentException("Invalid Meat");
            }

            return item;
        }
    }
}
