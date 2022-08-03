using System;

namespace Zong
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ZongGame game = new ZongGame())
            {
                game.Run();
            } 
        }
    }
#endif
}

