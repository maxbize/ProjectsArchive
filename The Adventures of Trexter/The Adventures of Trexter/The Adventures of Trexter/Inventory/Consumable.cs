using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Adventures_of_Trexter.Inventory
{
    public class Consumable : Item
    {
        public class HealthPotion
        {
            //Random rnd = new Random();

            public string Name = "small health potion";
            public int HealthPotionsInv;
            public int HealAmount;
            public int MinHealAmount = 30;
            public int MaxHealAmount = 50;
            public int Price = 35;

            public void DisplayStats()
            {
                System.Console.WriteLine("Name: " + Name);
                System.Console.WriteLine("Heals between " + MinHealAmount + " and " + MaxHealAmount + " health.");
                System.Console.WriteLine("Price: " + Price + " gold\n");
            }

            public void AddHealthPotion()
            {
                Game.player.HealthPotion.HealthPotionsInv++;
            }
            public void UseHealthPotion()
            {
                if (Game.player.HealthPotion.HealthPotionsInv > 0)
                {
                    Game.player.HealthPotion.HealthPotionsInv--;
                    HealAmount = Game.Generator.Next(MinHealAmount, MaxHealAmount);
                    if (HealAmount + Game.player.Life > Game.player.MAX_PLAYER_HEALTH)
                    {
                        HealAmount = Game.player.MAX_PLAYER_HEALTH - Game.player.Life;
                    }
                    Game.player.Life += HealAmount;
                    System.Console.WriteLine("The potion's soothing liquid heals you for " + HealAmount + " life!");
                    if ( Game.player.Life > Game.player.MAX_PLAYER_HEALTH )
                    {
                        Game.player.Life = Game.player.MAX_PLAYER_HEALTH;
                    }
                    System.Console.WriteLine("You now have " + Game.player.Life + " health.\n");
                }
                else
                {
                    System.Console.WriteLine("You don't have any health potions to use.");
                }
            }
        }
    }
}
