using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Zong
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelManager : Microsoft.Xna.Framework.GameComponent
    {
        // Random var to use for the game
        public static Random rnd { get; private set; }
        public static int LEVEL_SIZE = 4;
        int maxBricksPossible;
        int density;
        
        // List which tells us which cells are open. Used to parse through level faster
        //  -Makes creating dense levels faster
        //  -Makes creating large, not dense levels much slower
        List<Int3> vacantCells = new List<Int3>();

        // List of assemblies created
        List<Assembly> assemblyList = new List<Assembly>();
        
        // Pass to children
        ModelManager modelManager;

        // Used to test speed of different methods
        Stopwatch stopwatch = new Stopwatch();

        public LevelManager(Game game, ModelManager modelMngr)
            : base(game)
        {
            modelManager = modelMngr;
            rnd = new Random();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void createLevel()
        {
            LEVEL_SIZE += 2;
            //levelArray = new bool[LEVEL_SIZE, LEVEL_SIZE, LEVEL_SIZE];

            Int3 start;
            resetVacancies();
            int count = 0;

            while (levelDensity() < density)
            {
                count += 1;
                start = vacantCells[rnd.Next(vacantCells.Count - 1)];

                Assembly assem = (new Assembly(Functions.randomColor(),
                    start, vacantCells, modelManager, assemblyList.Count));
                assemblyList.Add(assem);
                removeVacancies(assem.getOccupied());
                
            }
            Console.WriteLine("Final Count tick: " + count);
        }

        private int levelDensity()
        {
            int density = (int)(100 - (float)vacantCells.Count / maxBricksPossible * 100);
            return density;
        }

        private void removeVacancies(List<Int3> cells)
        {
            foreach (Int3 cell in cells)
            {
                vacantCells.Remove(cell);
            }
        }

        private void resetVacancies()
        {
            vacantCells = new List<Int3>();
            int type = rnd.Next(0, 3);
            density = 35;

            // Creates Cube-like level
            if (type == 0)
            {
                for (int i = 0; i < LEVEL_SIZE; i++)
                {
                    for (int j = 0; j < LEVEL_SIZE; j++)
                    {
                        for (int k = 0; k < LEVEL_SIZE; k++)
                        {
                            vacantCells.Add(new Int3(i, j, k));
                        }
                    }
                }
            }
            
            // Spherical level
            else if (type == 1)
            {
                Int3 center = new Int3(LEVEL_SIZE / 2, LEVEL_SIZE / 2, LEVEL_SIZE / 2);
                int radius = LEVEL_SIZE / 2;
                for (int i = 0; i < LEVEL_SIZE; i++)
                {
                    for (int j = 0; j < LEVEL_SIZE; j++)
                    {
                        for (int k = 0; k < LEVEL_SIZE; k++)
                        {
                            Int3 cell = new Int3(i, j, k);
                            if ((cell - center).Length() <= radius)
                            {
                                vacantCells.Add(new Int3(i, j, k));
                            }
                        }
                    }
                }
            }

            // Scripted level
            else if (type == 2)
            {
                type = rnd.Next(0, 1);
                if (type == 0) // Hollow cube
                {
                    density = 80;
                    int edge = LEVEL_SIZE - 1;
                    for (int i = 0; i < LEVEL_SIZE; i++)
                    {
                        for (int j = 0; j < LEVEL_SIZE; j++)
                        {
                            for (int k = 0; k < LEVEL_SIZE; k++)
                            {
                                if (i == 0 || j == 0 || k == 0 ||
                                    i == edge || j == edge || k == edge)
                                {
                                    vacantCells.Add(new Int3(i, j, k));
                                }
                            }
                        }
                    }
                }
            }
            maxBricksPossible = vacantCells.Count;
        }
    }
}
