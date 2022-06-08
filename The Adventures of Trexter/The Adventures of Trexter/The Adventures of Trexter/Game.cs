using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The_Adventures_of_Trexter.Character;
using The_Adventures_of_Trexter.Inventory;
using The_Adventures_of_Trexter.World;

//    Updates that need to be made:
//  1. Player needs an inventory to be able to store objects
//      1a. Update the shop class to be able to buy items in your inv
//      1b. Update the treasure action to possibly add an item to your inv
//      1c. Add an "EQUIP" case
//  2. If someone flees from combat they should go back to their previous space
//  3. Allow items to be used in combat
//  4. Update combat with OneNote ideas
//  5. Create random level generation
//      4a. Bosses need to be level dependent (don't forget to update the writeline that says "IT'S A BOSS!!")
//  6. Possibly add a map to the shop
//  7. Add some color to this game
//

namespace The_Adventures_of_Trexter
{
    public class Game
    {
        public static Random Generator = new Random();
        public static Being player;
        private List<Being> enemies;
        private List<Item> treasures;
        private List<Shop> shops;
        private Map map;
        private const int MAX_TREASURE_GOLD = 300;
        //private Random rnd = new Random();

        public Game()
        {
            player = new Being();
            enemies = new List<Being>();
            treasures = new List<Item>();
            shops = new List<Shop>();
            map = new Map();
            player.Location = map.GetStartingLocation();
            player.Weapon = new Weapon(1);
            player.Armor = new Armor(1);
            player.HealthPotion = new Consumable.HealthPotion();
            player.Level = 0;
            player.BaseDexterity = 5;
            player.BaseStrength = 5;
            player.Life = 100;
            player.Gold = Game.Generator.Next(1,10);
            player.XP = 0;
            player.HealthLevelBonus = 0;
            player.HealthPotion.HealthPotionsInv = 0;
            player.DefensePercent = 0;
            player.Defense = 0;
            enemies = map.FillEnemiesList();
            shops = map.FillShopsList();
            
            Map.GenerateMap();
            PlayIntro();
            PlayGame();
            //System.Console.WriteLine("Starting location: " + player.Location.X.ToString() + " : " + player.Location.Y.ToString());
            
        }

        private void PlayIntro()
        {
            System.Console.WriteLine("THE ADVENTURES OF TREXTER - v.1.0");
            System.Console.WriteLine();
            System.Console.WriteLine("    by Max and Nicolas");
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("You are Trexter, a happy little boy living in a quiet farming village. You have no armor, no weapons, no friends, and a love for the smell of manure.");
            System.Console.WriteLine("One day you were helping your parents in the farm when the evil bandit lord and his marauders pillaged your village and killed everyone.");
            System.Console.WriteLine("You were knocked out trying to defend your beloved land and awake in the middle of nowhere with a thirst for vengeance! You found a basic weapon and armor. For instructions type 'help'");
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            LevelUp();
        }

        private void PlayGame()
        {
            while(!map.IsEndLocation(player.Location)){
                String loc = map.GetLocationChar(player.Location);
                UpdateStats();
                switch (loc)
                {
                    case "T":
                        PickUpTreasure();
                        break;
                    case "X":
                    case "B":
                        EncounterEnemy(player.Location);
                        break;
                    case "$":
                        EnterShop(player.Location);
                        break;

                }
                bool north = map.CanGoNorth(player.Location);
                bool south = map.CanGoSouth(player.Location);
                bool east = map.CanGoEast(player.Location);
                bool west = map.CanGoWest(player.Location);
                System.Console.Write("You can go ");
                if (north)
                {
                    System.Console.Write("(N)orth ");
                }
                if (east)
                {
                    System.Console.Write("(E)ast ");
                }
                if (west)
                {
                    System.Console.Write("(W)est ");
                }
                if (south)
                {
                    System.Console.Write("(S)outh ");
                }
                System.Console.Write("> ");
                string input = System.Console.ReadLine();
                System.Console.WriteLine();
                switch (input.ToUpper())
                {
                    case "N":
                        if (north)
                        {
                            player.Location.Y--;
                        }
                        break;
                    case "E":
                        if (east)
                        {
                            player.Location.X++;
                        }
                        break;
                    case "S":
                        if (south)
                        {
                            player.Location.Y++;
                        }
                        break;
                    case "W":
                        if (west)
                        {
                            player.Location.X--;
                        }
                        break;
                    case "STATUS":
                    case "STATS":
                    case "CHARACTER":
                    case "C":
                        PrintStatus();
                        break;
                    case "HELP":
                        PrintHelp();
                        break;
                    case "INVENTORY":
                    case "INV":
                        PrintInventory();
                        break;
                    case "USE":
                        UseConsumable();
                        break;
                    case "MAP":
                        PrintMap(Map.Level1);
                        break;
                    case "TEST":
                        PrintTestMap(Map.CurrentLevel);
                        break;
                    case "NEW":
                        Map.GenerateMap();
                        break;
                    default:
                        System.Console.WriteLine("You wish you could do that");
                        break;
                }
            }
        }

        

        private void UseConsumable()
        {
            System.Console.WriteLine("What would you like to use?");
            string requestedConsumable = System.Console.ReadLine();
            switch (requestedConsumable.ToUpper())
            {
                case "HEALTH POTION":
                case "HEALTH":
                case "POTION":
                    Game.player.HealthPotion.UseHealthPotion();
                    break;
            }
        }

        public void UpdateStats()
        {
            player.MAX_PLAYER_HEALTH = 100 + player.Armor.HealthBonus + player.Weapon.HealthBonus + player.HealthLevelBonus;
            if ( player.Life > player.MAX_PLAYER_HEALTH )
            {
                player.Life = player.MAX_PLAYER_HEALTH; // Just in case I have a logic error somewhere
            }
            player.Strength = player.BaseStrength + player.Armor.StrengthBonus + player.Weapon.StrengthBonus;
            player.Dexterity = player.BaseDexterity + player.Armor.DexterityBonus + player.Armor.DexterityBonus;
            if (player.XP > Being.arrXPRequired[player.Level-1]) // Check if player's exp is enough to level up, get XP required from array defined in Being class
            {
                LevelUp();
            }
        }

        public void PrintInventory()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Current inventory:");
            player.Weapon.DisplayStats();
            player.Armor.DisplayStats();
            if (player.HealthPotion.HealthPotionsInv > 0)
                System.Console.WriteLine("Health potions: " + player.HealthPotion.HealthPotionsInv);          
            
        }

        public void PrintHelp()
        {
            System.Console.WriteLine("The Adventures of Trexter is a text adventure (duh)");
            System.Console.WriteLine("Valid commands are:");
            System.Console.WriteLine("N / E / S / W to go North, East, South, or West, respectively");
            System.Console.WriteLine("Stats / Status to view player information");
            System.Console.WriteLine("Use to use any consumables you have");
            System.Console.WriteLine("Exit / Leave to leave a shop");
            System.Console.WriteLine("A / F to attack or flee from a battle, respectively");
            System.Console.WriteLine("Use your common sense to find some other commands");
        }

        private void EnterShop(Position loc)
        {

            Shop currentShop = shops.Find(delegate(Shop b)
            {
                return b.Location.X == loc.X && b.Location.Y == loc.Y;
            });
            currentShop.Shopping(player);
        }

        private void EncounterEnemy(Position loc)
        {

            Being currentEnemy = enemies.Find(delegate(Being b)
            {
                return b.Location.X == loc.X && b.Location.Y == loc.Y;
            });

            if (currentEnemy != null)
            {
                Console.WriteLine("-- ENEMY ENCOUNTERED !! --");
                Console.WriteLine();
                if (currentEnemy.Level > 3) //Right now bosses have to be level 5
                {
                    Text.Print("[IT'S A BOSS!!]\n", ConsoleColor.Red);
                }
                Console.WriteLine("You just encountered a level " + currentEnemy.Level + " " + currentEnemy.Name + "!");
                bool isCombatOver = false;
                while (!isCombatOver)
                {
                    Console.WriteLine("He has " + currentEnemy.Life + " life left.");
                    Console.WriteLine();
                    Console.WriteLine("You can (A)ttack or (F)lee : ");

                    System.Console.Write("> ");
                    string input = System.Console.ReadLine();
                    System.Console.WriteLine();
                    switch (input.ToUpper())
                    {
                        case "A":
                            if (AttackEnemy(currentEnemy))
                            {
                                isCombatOver = true;
                            }
                            else
                            {
                                GetAttackedBy(currentEnemy);
                            }
                            break;
                        case "F":
                            isCombatOver = true;
                            break;
                    }
                }
                //EncounterEnemy(loc);
            } 
        }

        private bool GetAttackedBy(Being currentEnemy)
        {
            int hitChance = 50; // If
            int dexterityDiff = currentEnemy.Dexterity - player.Dexterity; // Can be between -9 and 9
            hitChance += dexterityDiff * 5 + 30;
            int success = Game.Generator.Next(0, 100);
            if (hitChance >= success) // Succesful hit if Random number between 1 and 100 is less than hitChance
            {
                double Damage = 0;
                Damage += currentEnemy.Weapon.GetWeaponDamage(); //Get damage caused from the weapon
                Damage += (currentEnemy.Strength / 3); //Augment damage by 1 for every 3 str
                if (currentEnemy.CriticalHit())
                {
                    double damageMultiplier = (double)Game.Generator.Next(15, 21) / 10; // Damage x 1.5 to 2.0 (need cast because rnd generates an int)
                    Damage *= damageMultiplier;
                    Console.WriteLine("Critical hit! " + damageMultiplier + "x damage!!");
                }
                Damage -=  Damage * Game.player.GetDefensePercent(); // Subtract a percent amount defined in Defense 
                Console.WriteLine( currentEnemy.Name + " hit you for " + (int)Damage + "!");
                //if(dmg > currentEnemy.Armor)
                player.Life -= (int)Damage;
                if (player.Life < 1)
                {
                    Console.WriteLine("You were killed!");
                    ShowEnding();
                    return true;
                }
                else
                {
                    Console.WriteLine("You have " + player.Life + " life left.");
                }
            }
            else // Miss
            {
                Console.WriteLine(currentEnemy.Name + " missed!");
            }

            return false;            
        }

        private bool AttackEnemy(Being currentEnemy)
        {
            int hitChance = 50; 
            int dexterityDiff = player.Dexterity - currentEnemy.Dexterity; // Can be between -9 and 9
            hitChance += dexterityDiff * 5 + 30;
            int success = Game.Generator.Next(0, 100);
            if (hitChance >= success) // Succesful hit if Random number between 1 and 100 is less than hitChance
            {
                double Damage = 0;
                Damage += player.Weapon.GetWeaponDamage(); //Get damage caused from the weapon
                Damage += (player.Strength / 3); //Augment damage by 1 for every 3 str
                if (player.CriticalHit())
                {
                    double damageMultiplier = (double)Game.Generator.Next(15, 21) / 10; // Damage x 1.5 to 2.0
                    Damage *= damageMultiplier;
                    Console.WriteLine("Critical hit! " + damageMultiplier + "x damage!!");
                }
                Damage -= Damage * currentEnemy.GetDefensePercent(); // Subtract a percent amount defined in Defense
                Console.WriteLine("You hit your enemy for " + (int)Damage + " points!");
                currentEnemy.Life -= (int)Damage;
                //int dmg = player.Weapon.GetWeaponDamage();
                ////if(dmg > currentEnemy.Armor)
                //currentEnemy.Life -= dmg;
                if (currentEnemy.Life < 1)
                {
                    Console.WriteLine("You killed your enemy! You found " + currentEnemy.Gold + " gold.");
                    Console.WriteLine("You also gained " + currentEnemy.XP + " experience points.\n");
                    player.Gold += currentEnemy.Gold;
                    player.XP += currentEnemy.XP;
                    UpdateStats();
                    enemies.Remove(currentEnemy);
                    return true;
                }
            }
            else // Miss
            {
                Console.WriteLine("You missed!");
            }

            return false;
        }

        private void PrintStatus()
        {
            Console.WriteLine("You are level " + player.Level);
            Console.WriteLine("EXP: " + player.XP + " / " + Being.arrXPRequired[player.Level-1]);
            Console.WriteLine("Strength : " + player.Strength );
            Console.WriteLine("Dexterity : " + player.Dexterity);
            Console.WriteLine("Life : " + player.Life.ToString() + " / " + player.MAX_PLAYER_HEALTH);
            Console.WriteLine("Gold : " + player.Gold.ToString());
            Console.WriteLine();
        }

        public void PrintMap(string[] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                Console.WriteLine(map[i]);
            }
            Console.WriteLine();
        }

        public void PrintTestMap(List<string> map)
        {
            for (int row = 0; row < map.Count; row++)
            {
                Console.WriteLine(map[row]);
            }
            Console.WriteLine();
        }

        private void PickUpTreasure() //NEEDS UPDATE
        {
            int goldFound = Game.Generator.Next(MAX_TREASURE_GOLD);
            Console.WriteLine("-- TREASURE FOUND !! --");
            Console.WriteLine();
            Console.WriteLine("You just found " + goldFound.ToString() + " gold!");
            player.Gold += goldFound;

            int itemGet = Game.Generator.Next(1, 4); //1-3 are weps
            switch (itemGet)
            {
                case 1:
                case 2:
                case 3:
                System.Console.WriteLine("NEED TO UPDATE REALLY BADLY!");
                break;
            }
            map.PlaceObject(player.Location, " ");
        }

        private void LevelUp()
        {
            player.Level++;
            System.Console.WriteLine("You feel the spirit of adventure as the winds of fate blow around you!");
            for (int i = 0; i <= 3; i++)
            {
                System.Console.WriteLine("Would you like to upgrade your health, strength, or dexterity?");
                System.Console.WriteLine("You have " + (4 - i) + " upgrades remaining");
                string upgrade = System.Console.ReadLine();
                upgrade = upgrade.ToUpper();
                //while (upgrade != "HEALTH" && upgrade != "STRENGTH" && upgrade != "DEXTERITY") // Validating entry
                //{
                //    System.Console.WriteLine("Did you really think a farmboy could upgrade that stat?\n");
                //    upgrade = System.Console.ReadLine();
                //    upgrade = upgrade.ToUpper();
                //}
                switch (upgrade)
                {
                    case "HEALTH":
                    case "HP":
                        player.HealthLevelBonus += 15; //Add 15 to max health per upgrade
                        System.Console.WriteLine("Your max health has been augmented.\n");
                        break;
                    case "STRENGTH":
                    case "STR":
                        player.BaseStrength += 1;
                        System.Console.WriteLine("Your strength has been augmented.\n");
                        break;
                    case "DEXTERITY":
                    case "DEX":
                        player.BaseDexterity += 1;
                        System.Console.WriteLine("Your dexterity has been augmented.\n");
                        break;
                    default:
                        System.Console.WriteLine("Did you really think a farmboy could upgrade that stat?\n");
                        i--;
                        break;

                }
                UpdateStats();
            }
            player.Life = player.MAX_PLAYER_HEALTH; // Give the player full health
            System.Console.WriteLine("Your new stats are");
            PrintStatus();
        }

        private void ShowEnding()
        {
            if (player.Life > 0)
            {

            }
            else
            {
                Console.WriteLine("You miserably failed and your family was left unavenged!");
                Console.WriteLine("---  GAME OVER  ---");
            }

            Console.ReadLine();
            Environment.Exit(0);
        }


    }
}
