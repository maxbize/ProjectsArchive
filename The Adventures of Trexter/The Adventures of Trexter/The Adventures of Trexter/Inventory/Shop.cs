using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The_Adventures_of_Trexter.World;
using The_Adventures_of_Trexter.Character;

namespace The_Adventures_of_Trexter.Inventory
{
    public class Shop
    {
        public Position Location { get; set; }
        //public List<Armor> Armor { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public Consumable.HealthPotion HealthPotion { get; set; }
        public int Level { get; set; }
        private Being visitor;

        public Shop( int level )
        {
            this.Level = level;
            //Random rnd = new Random();
            this.Weapon = new Weapon(level);
            this.Armor = new Armor(level);
            this.HealthPotion = new Consumable.HealthPotion();
        }    

        public void Shopping(Being player)
        {
            this.visitor = player;
            System.Console.WriteLine("Welcome to my humble shop!");
            System.Console.WriteLine("I have a piece of Armor, a Weapon, and a Consumable for sale.");
            System.Console.WriteLine("What would you like to see? Or would you like to sell your equipment?");
            string input = System.Console.ReadLine();
            input = input.ToUpper();
            while (input != "EXIT" && input != "LEAVE")
            {
                //IStatable currentItem;
                
                if (input == "WEAPON")
                {
                    Purchase(Game.player.Weapon, this.Weapon); // Neither game.player nor this.visitor seem to work
                }

                if (input == "ARMOR")
                {
                    Purchase(this.visitor.Armor, this.Armor);
                }

                if (input == "CONSUMABLE")
                {
                    HealthPotion.DisplayStats();
                    System.Console.WriteLine("Would you like to buy this " + HealthPotion.Name + "?");
                    string purchasing = System.Console.ReadLine();
                    purchasing = purchasing.ToUpper();
                    while (purchasing != "YES" && purchasing != "NO")
                    {
                        System.Console.WriteLine("I'm sorry could you repeat that please?");
                        purchasing = System.Console.ReadLine();
                        purchasing = purchasing.ToUpper();
                    }
                    if (purchasing == "YES")
                    {
                        if (Game.player.Gold >= HealthPotion.Price)
                        {
                            HealthPotion.AddHealthPotion();
                            Game.player.Gold -= HealthPotion.Price;
                            System.Console.WriteLine("Thanks for your money!");
                            System.Console.WriteLine("It looks like you still have " + Game.player.Gold + " gold you can give me");
                        }
                        else
                        {
                            System.Console.WriteLine("You don't have enough money. I have kids to feed ya know!");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Alright then what do you want to do?");
                    }
                }

                    input = System.Console.ReadLine();
                    input = input.ToUpper();
                
            }
        }

        private void Purchase(Item equippedItem, Item purchasedItem)
        {
            ((IStatable)purchasedItem).DisplayStats();
            System.Console.WriteLine("Would you like to buy this " + purchasedItem.Name + "?");
            string purchasing = System.Console.ReadLine();
            purchasing = purchasing.ToUpper();
            while (purchasing != "YES" && purchasing != "NO")
            {
                System.Console.WriteLine("I'm sorry could you repeat that please?");
                purchasing = System.Console.ReadLine();
                purchasing = purchasing.ToUpper();
            }
            if (purchasing == "YES")
            {
                if (Game.player.Gold >= purchasedItem.Price)
                {
                    equippedItem = purchasedItem; // This line does not work as it should. It replaces a general inventory armor or weapon that is not being used. It should replace the player's weapon or armor.
                    Game.player.Gold -= purchasedItem.Price;
                    System.Console.WriteLine("Thanks for your money!");
                    System.Console.WriteLine("It looks like you still have " + Game.player.Gold + " gold you can give me");
                }
                else
                {
                    System.Console.WriteLine("You don't have enough money. I have kids to feed ya know!");
                }
            }
            else
            {
                System.Console.WriteLine("Alright then what do you want to do?");
            }
        }
        
    }
}
