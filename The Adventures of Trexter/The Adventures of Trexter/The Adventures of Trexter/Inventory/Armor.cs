using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Adventures_of_Trexter.Inventory
{
    public class Armor : Item, IStatable
    {
        public int Protection { get; set; }

        private static string[] arrArmorNames = { "Wooden Shield", "Iron boots", "Chainmail", "Knight's armor" };
        //private Random rnd = new Random();

        public Armor(int level)
        {
            this.Name = arrArmorNames[Game.Generator.Next(1, arrArmorNames.Length)];
            this.Price = this.Protection * level * 10 + this.DexterityBonus * 20 + this.StrengthBonus * 20 + Game.Generator.Next(1, 10);
            this.DexterityBonus = Game.Generator.Next(0, level);
            this.StrengthBonus = Game.Generator.Next(0, level);
            this.HealthBonus = Game.Generator.Next(0, level * 3);
            this.Protection = Game.Generator.Next(0, level);
            this.Level = level;
        }

        public void DisplayStats()
        {
            System.Console.WriteLine();            
            System.Console.WriteLine("Name: " + this.Name);
            System.Console.WriteLine("Protection: " + this.Protection + " armor");
            if (this.DexterityBonus > 0)
                System.Console.WriteLine("Dexterity Bonus: " + this.DexterityBonus);
            if (this.StrengthBonus > 0)
                System.Console.WriteLine("Strength Bonus: " + this.StrengthBonus);
            if (this.HealthBonus > 0)
                System.Console.WriteLine("Health Bonus: " + this.HealthBonus);
            System.Console.WriteLine("Price: " + this.Price + "g");
        }
    }
}
