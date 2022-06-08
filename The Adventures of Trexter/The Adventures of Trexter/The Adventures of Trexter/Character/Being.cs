using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The_Adventures_of_Trexter.Inventory;
using The_Adventures_of_Trexter.World;

namespace The_Adventures_of_Trexter.Character
{
    public class Being
    {
        public static int MIN_ENEMY_STARTING_LIFE = 10;
        public static int MAX_ENEMY_STARTING_LIFE = 30;
        public int MAX_PLAYER_HEALTH = 100;

        private static string[] arrEnemyNames = new string[]{ "Ork", "Goblin", "Bandit", "Squirrel" };
        private static string[] arrEnemyAdjectives = new string []{ " of darkness", " of fear", " from the devil", " the chicken chaser", " the ERASOR"};
        public static int[] arrXPRequired = new int[] { 100, 300, 500, 1000 };

        public string Name { get; set; }
        public int Life { get; set; }
        public int HealthLevelBonus { get; set; }
        public int Strength { get; set; }
        public int BaseStrength { get; set; }
        public int Dexterity { get; set; }
        public int BaseDexterity { get; set; }
        public int Gold { get; set; }
        public Armor Armor { get; set; }
        public Weapon Weapon { get; set; }
        public Consumable.HealthPotion HealthPotion { get; set; }
        public Position Location { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public double DefensePercent { get; set; } // Percent damage modifier that reduces damage.
        public int Defense { get; set; } // Variable used in DefensePercent

        public Being() { }

        //Enemies
        public Being(int level)
        {
            //Random rnd = new Random();
            this.Name = arrEnemyNames[Game.Generator.Next(0, arrEnemyNames.Length)];
            if (level > 3)
            {
                this.Name += arrEnemyAdjectives[Game.Generator.Next(0, arrEnemyAdjectives.Length)];
            }
            this.Life = level * 10 + Game.Generator.Next(0, 12);
            this.Strength = level * 2 + Game.Generator.Next(0, 10);
            this.Dexterity = level * 2 + Game.Generator.Next(0, 10);
            this.Gold = level * 25 + Game.Generator.Next(1, 30);
            this.Weapon = new Weapon(level);
            this.XP = level * 10 + Game.Generator.Next(0, 12);
            this.Level = level;
            this.Armor = new Armor(1); // Needed or else DefensePercent has an unhandled exception
        }

        public int GetBeingDamage()
        {
            int totalDamage = 0;

            return totalDamage;
        }

        public double GetDefensePercent()
        {
            if (this.Armor.Protection > 0 || this.Strength > 3) // If the being has 0 defense, the function cannot evaluate (ln 0 = neg infinity)
            {
                this.Defense = this.Armor.Protection + (this.Strength / 4); // Defense points are based on armor and STR
                this.DefensePercent = Math.Log(((double)this.Defense / 3) + 1) * 15 / 100;  // ln(x/3+1)*0.15            
                return this.DefensePercent;
            }
            else
            {
                return DefensePercent = 0;
            }
        }

        // Function checks if attack is critical hit
        public bool CriticalHit()
        {
            //Random rnd = new Random();
            double critChance = Math.Log(this.Dexterity / 3 + 1) * 8; //ln(x/3+1)*0.08 Generates a crit chance based on DEX that peaks in the 20's
            double succesfulCrit = Game.Generator.Next(0, 100);
            if (critChance > (double)succesfulCrit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
