using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalHusbandryMod.meats
{
    public class MeatItem
    {
        public string Name;
        public int Index;
        public int Price;
        public int Edibility;

        public MeatItem(string name, int index, int price, int edibility)
        {
            Name = name;
            Index = index;
            Price = price;
            Edibility = edibility;
        }
    }
}
