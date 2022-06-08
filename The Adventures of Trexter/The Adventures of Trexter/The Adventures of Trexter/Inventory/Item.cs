using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Adventures_of_Trexter.Inventory
{
    public class Item
    {
        public String Name { get; set; }
        public int Price { get; set; }
        public int HealthBonus { get; set; }
        public int DexterityBonus { get; set; }
        public int StrengthBonus { get; set; }
        public int Level { get; set; }

    }
}
