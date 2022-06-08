using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using The_Adventures_of_Trexter.Character;
using The_Adventures_of_Trexter.Inventory;

namespace The_Adventures_of_Trexter.World
{
    public class Map
    {
        public static int totalRows;
        public static int totalcolumns;
        /// <summary>
        /// Walls | + -
        /// Starting Point S
        /// Ending Point E
        /// Enemy X
        /// Treasure T
        /// Shop $
        /// Boss B
        /// </summary>
        public static List<string> CurrentLevel = new List<string>();
        public static List<Position> BacktrackList = new List<Position>();
        public static string[] Level1 = new String[]{
            
            // 10 x 10 square
            "+E-------+",
            "|B---    |",
            "|X    $| |",
            "|------+ |",
            "|TX     X|",
            "|X  +----|",
            "|-- +----|",
            "|-   B  X|",
            "|$ X|S|XT|",
            "+--------+",
 

            //Nicolas's crappy map that was 66x20
           //"+----------------------------------------------------------------+",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|                                                                |",
           //"|------------+                                                   |",
           //"| $         T|    +--------+                                     |",
           //"|    X       |    |T       |                                     |",
           //"|---------+  +----+ X      +----------+--------------------------|",
           //"|                          TX         |                          |",
           //"|   ------+  -----+ +-------+   |     |                          |",
           //"|    X    |     X | |XXXXXXX|   |                                |",
           //"|       T |   X T |X|S|T $|XXXXX|     |                        E |",
           //"+---------+-------+-+-+---------+-----+--------------------------+"       

        };

        public bool CanGoWest(Position position)
        {
            String c = Level1[position.Y].Substring(position.X - 1, 1);
            if (c == "|" || c == "+" || c == "-")
            {
                return false;
            }
            return true;
        }

        public bool CanGoEast(Position position)
        {
            String c = Level1[position.Y].Substring(position.X + 1, 1);
            if (c == "|" || c == "+" || c == "-")
            {
                return false;
            }
            return true;
        }

        public bool CanGoNorth(Position position)
        {
            String c = Level1[position.Y - 1].Substring(position.X, 1);
            if (c == "|" || c == "+" || c == "-")
            {
                return false;
            }
            return true;
        }

        public bool CanGoSouth(Position position)
        {
            String c = Level1[position.Y + 1].Substring(position.X, 1);
            if (c == "|" || c == "+" || c == "-")
            {
                return false;
            }
            return true;
        }


        public Position GetStartingLocation()
        {
            bool found = false;
            int nbLine = 0;
            while (!found && nbLine < Level1.Length)
            {
                if (Level1[nbLine].IndexOf("S") > 0)
                {
                    Position p = new Position();
                    p.X = Level1[nbLine].IndexOf("S");
                    p.Y = nbLine;
                    return p;
                }
                nbLine++;
            }
            return null;
        }

        public bool IsEndLocation(Position location)
        {
            if (Level1[location.Y].Substring(location.X, 1) == "E")
            {
                Console.WriteLine("You made it to the exit of the dungeon. There would be another, even infinite levels, if the creators add random level generation. Thanks for playing!");
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetLocationChar(Position location)
        {
            return Level1[location.Y].Substring(location.X, 1);
        }

        public void PlaceObject(Position position, string p)
        {
            String line = Level1[position.Y];
            Level1[position.Y] = line.Substring(0, position.X) + p + line.Substring(position.X + 1);
            
        }

        public List<Being> FillEnemiesList()
        {
            List<Being> enemiesList = new List<Being>();
            //Random rnd = new Random();

            for (int y = 0; y < Level1.Length; y++)
            {
                String line = Level1[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (GetLocationChar(new Position(x, y)) == "X")
                    {
                        Being enemy = new Being(Game.Generator.Next(1, 3));
                        enemy.Location = new Position(x, y);
                        enemiesList.Add(enemy);
                    }
                    if (GetLocationChar(new Position(x, y)) == "B")
                    {
                        Being enemy = new Being(5);
                        enemy.Location = new Position(x, y);
                        enemiesList.Add(enemy);
                    }
                }
            }

            return enemiesList;
        }

        //Find all shops and store
        public List<Shop> FillShopsList()
        {
            List<Shop> shopList = new List<Shop>();
            //Random rnd = new Random();

            for (int y = 0; y < Level1.Length; y++)
            {
                String line = Level1[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (GetLocationChar(new Position(x, y)) == "$")
                    {
                        Shop shop = new Shop(1);
                        shop.Location = new Position(x, y);
                        shopList.Add(shop);
                    }
                }
            }
            return shopList;
        }

        // Max's attempt at an algorithm to generate a random map that will change with each level
        public static void GenerateMap()
        {
            CurrentLevel = new List<string>();
            BacktrackList = new List<Position>();
            GenerateMazeWalls();
            BreakInnerWalls();
            RemoveMarkers();
        }

        public static void GenerateMazeWalls()
        {
            totalRows = Game.Generator.Next(10, 14);
            totalcolumns = Game.Generator.Next(5, 7); //Generate a random amount of space (columns) for each row

            string middleMazeRow = "";
            string solidMazeRow = new string('+', totalcolumns * 2 - 1);

            for (int currentcolumn = 0; currentcolumn < totalcolumns; currentcolumn++)
            {
                middleMazeRow = middleMazeRow + "+ ".ToString();
            }

            for (int currentRow = 0; currentRow < totalRows; currentRow++) // Go through all of the generated rows
            {
                if (currentRow % 2 == 0)
                {
                    CurrentLevel.Add(solidMazeRow);
                }
                else
                {
                    CurrentLevel.Add(middleMazeRow);
                }
            }
            if (totalRows % 2 == 0)
            {
                CurrentLevel.Add(solidMazeRow); // Needed in case there are an odd number of rows
            }

        }

        public static void BreakInnerWalls()
        {
            bool validStart = false;
            Position mazeCursor = new Position();

            while (!validStart)
            {
                int randomRow = Game.Generator.Next(2, totalRows - 2);
                int randomColumn = Game.Generator.Next(2, totalcolumns - 2);
                mazeCursor = new Position(randomColumn, randomRow);
                string loc = CurrentLevel[randomRow].Substring(randomColumn, 1);
                if (loc == " ")
                {
                    string line = CurrentLevel[randomRow];
                    CurrentLevel[randomRow] = line.Substring(0, randomColumn) + "S" + line.Substring(randomColumn + 1);
                    validStart = true;
                    BacktrackList.Add(mazeCursor);
                }
            }

            while (BacktrackList.Count != 0)
            {
                // Check number of possibilities for backtracking
                int possibilities = 0;
                if (CanGoNorthTest(mazeCursor))
                    possibilities++;
                if (CanGoEastTest(mazeCursor))
                    possibilities++;
                if (CanGoSouthTest(mazeCursor))
                    possibilities++;
                if (CanGoWestTest(mazeCursor))
                    possibilities++;

                if (possibilities == 2) // This isn't the source of the inf loop error
                {
                    Position currentPosition = new Position(mazeCursor.X, mazeCursor.Y);
                    BacktrackList.Add(currentPosition);
                }

                //Backtrack if you hit a dead end
                if (possibilities == 0)
                {
                    int thisPositionNumber = Game.Generator.Next(0, BacktrackList.Count);
                    Position thisPosition = new Position(mazeCursor.X, mazeCursor.Y);
                    mazeCursor = BacktrackList[thisPositionNumber];
                    BacktrackList.RemoveAt(thisPositionNumber);

                    if (BacktrackList.Count == 2)
                    {
                        string end = CurrentLevel[mazeCursor.Y];
                        CurrentLevel[mazeCursor.Y] = end.Substring(0, mazeCursor.X) + "E" + end.Substring(mazeCursor.X + 1);
                    }
                }


                int nextTest = Game.Generator.Next(1, 5);
                switch (nextTest)
                {
                    case 1:
                        if (CanGoNorthTest(mazeCursor))
                        {
                            BreakNorthWall(mazeCursor);
                            mazeCursor.Y -= 2;
                            MarkCell(mazeCursor);
                        }   
                        break;
                    case 2:
                        if (CanGoEastTest(mazeCursor))
                        {
                            BreakEastWall(mazeCursor);
                            mazeCursor.X += 2;
                            MarkCell(mazeCursor);
                        }
                        break;
                    case 3:
                        if (CanGoSouthTest(mazeCursor))
                        {
                            BreakSouthWall(mazeCursor);
                            mazeCursor.Y += 2;
                            MarkCell(mazeCursor);
                        }
                        break;
                    case 4:
                        if (CanGoWestTest(mazeCursor))
                        {
                            BreakWestWall(mazeCursor);
                            mazeCursor.X -= 2;
                            MarkCell(mazeCursor);                            
                        }
                        break;
                }
            }
        }

        public static bool CanGoWestTest(Position position)
        {
            if (position.X - 1 != 0)
            {
                String c = CurrentLevel[position.Y].Substring(position.X - 2, 1);
                if (c == " ")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGoEastTest(Position position)
        {
            if (position.X + 2 < totalcolumns*2 - 1)
            {
                String c = CurrentLevel[position.Y].Substring(position.X + 2, 1);
                if (c == " ")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGoNorthTest(Position position)
        {
            if (position.Y - 2 > 0)
            {
                String c = CurrentLevel[position.Y - 2].Substring(position.X, 1);
                if (c == " ")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGoSouthTest(Position position)
        {
            if (position.Y + 2 < totalRows)
            {
                String c = CurrentLevel[position.Y + 2].Substring(position.X, 1);
                if (c == " ")
                {
                    return true;
                }
            }
            return false;
        }

        private static void BreakNorthWall(Position position)
        {
            String line = CurrentLevel[position.Y - 1];
            CurrentLevel[position.Y - 1] = line.Substring(0, position.X) + " " + line.Substring(position.X + 1);
        }

        private static void BreakEastWall(Position position)
        {
            String line = CurrentLevel[position.Y];
            CurrentLevel[position.Y] = line.Substring(0, position.X + 1) + " " + line.Substring(position.X + 2);
        }

        private static void BreakSouthWall(Position position)
        {
            String line = CurrentLevel[position.Y + 1];
            CurrentLevel[position.Y + 1] = line.Substring(0, position.X) + " " + line.Substring(position.X + 1);
        }

        private static void BreakWestWall(Position position)
        {
            String line = CurrentLevel[position.Y];
            CurrentLevel[position.Y] = line.Substring(0, position.X - 1) + " " + line.Substring(position.X);
        }

        private static void MarkCell(Position position)
        {
            String line = CurrentLevel[position.Y];
            CurrentLevel[position.Y] = line.Substring(0, position.X) + "*" + line.Substring(position.X + 1);
        }

        public static void RemoveMarkers()
        {
            for (int i = 0; i < CurrentLevel.Count; i++)
            {
                CurrentLevel[i] = CurrentLevel[i].Replace('*', ' ');
            }
        }
    }
}
