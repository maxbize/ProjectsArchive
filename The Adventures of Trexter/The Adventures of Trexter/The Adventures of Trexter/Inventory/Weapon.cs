using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Adventures_of_Trexter.Inventory
{
    public class Weapon : Item, IStatable
    {

        public int Damage { get; set; }

        private static string[] arrWeaponPre = { "blue ", "red ", "golden ", "cursed ", "fire ", "iron " };
        private static string[] arrWeaponNames = { "walking stick", "dagger", "sword", "longsword", "crossbow", "bow" };
        private static string[] arrWeaponAdjectives = { " of the dead", " of the moon", " of the bear", " of the darkness" };
        //private Random rnd = new Random();

        public Weapon(String name, int damage, int price)
        {
            this.Name = name;
            this.Damage = damage;
            this.Price = price;
        }

        public Weapon(int level)
        {

            string p = arrWeaponPre[Game.Generator.Next(0, arrWeaponPre.Length)];
            string n = arrWeaponNames[Game.Generator.Next(0, arrWeaponNames.Length)];
            string s = arrWeaponAdjectives[Game.Generator.Next(0, arrWeaponAdjectives.Length)];

            if (level > 5)
            {
                this.Name = p + n + s;
            }
            else if (level > 2 && level < 6)
            {
                this.Name = p + n;
            }
            else
            {
                this.Name = n;
            }

            this.Damage = Game.Generator.Next(10);
            this.DexterityBonus = Game.Generator.Next(0, level);
            this.StrengthBonus = Game.Generator.Next(0, level);
            this.HealthBonus = Game.Generator.Next(0, level * 3);
            this.Level = level;
            this.Price = this.Damage * level * 10 + this.DexterityBonus * 20 + this.StrengthBonus * 20 + Game.Generator.Next(1, 10);
        }

        public int GetWeaponDamage()
        {
            int totalDmg = 0;
            for (int i = 0; i < this.Level; i++) //Weapon dice damage
            {
                totalDmg += Game.Generator.Next(1, 7);
            }
            totalDmg += this.Damage;
            return totalDmg;
        }

        public void DisplayDamagePossible()
        {
            int minDamage = this.Level + this.Damage;
            int maxDamage = this.Level * 6 + this.Damage;
            System.Console.WriteLine("Damage: " + minDamage + " to " + maxDamage);
        }

        public void DisplayStats()
        {
            System.Console.WriteLine();            
            System.Console.WriteLine("Name: " + this.Name);
            DisplayDamagePossible();
            if (this.DexterityBonus > 0 )
                System.Console.WriteLine("Dexterity Bonus: " + this.DexterityBonus);
            if (this.StrengthBonus > 0 )
                System.Console.WriteLine("Strength Bonus: " + this.StrengthBonus);
            if (this.HealthBonus > 0 )
                System.Console.WriteLine("Health Bonus: " + this.HealthBonus);
            System.Console.WriteLine("Price: " + this.Price + "g");
        }


    }
}
