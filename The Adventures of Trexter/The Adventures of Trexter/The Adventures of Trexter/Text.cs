using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Adventures_of_Trexter
{
        /// <summary>
        /// Class that handles some ANSI art 
        /// </summary>
        public static class Text
        {
            public static ConsoleColor ArrayBorderColor = ConsoleColor.DarkGray;
            public static ConsoleColor ArrayHeaderColor = ConsoleColor.Magenta;
            public static ConsoleColor ArrayValueColor = ConsoleColor.Red;
            public static int ArrayLeftPadding = 10;

            private static string[] cols;

            /// <summary>
            /// Adds some fancy colors to text
            /// Use: Text.Print("Hey you [super] dude, what is [up]", ConsoleColor.Blue, ConsoleColor.Magenta)
            /// </summary>
            /// <param name="text"></param>
            public static void Print(object text, params ConsoleColor[] colors)
            {
                string str = text.ToString();
                int sepIdx = str.IndexOf('[');
                if (sepIdx > -1 && colors.Length > 0)
                {
                    Console.Write(str.Substring(0, sepIdx));
                    int sepIdx2 = str.IndexOf(']');
                    ConsoleColor oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = colors[0];
                    Console.Write(str.Substring(sepIdx + 1, sepIdx2 - sepIdx - 1));
                    Console.ForegroundColor = oldColor;
                    str = str.Substring(sepIdx2 + 1);
                    List<ConsoleColor> newCols = colors.ToList<ConsoleColor>();
                    newCols.RemoveAt(0);
                    Text.Print(str, newCols.ToArray());
                }
                else
                {
                    Console.Write(str);
                }
            }

            /// <summary>
            /// Adds some fancy colors to text
            /// Use: Text.Print("Hey you [super] dude, what is [up]", ConsoleColor.Blue, ConsoleColor.Magenta)
            /// </summary>
            /// <param name="text"></param>
            public static void PrintLine(object text, params ConsoleColor[] colors)
            {
                string str = text.ToString();
                int sepIdx = str.IndexOf('[');
                if (sepIdx > -1)
                {
                    Console.Write(str.Substring(0, sepIdx));
                    int sepIdx2 = str.IndexOf(']');
                    ConsoleColor oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = colors[0];
                    Console.Write(str.Substring(sepIdx + 1, sepIdx2 - sepIdx - 1));
                    Console.ForegroundColor = oldColor;
                    str = str.Substring(sepIdx2 + 1);
                    List<ConsoleColor> newCols = colors.ToList<ConsoleColor>();
                    newCols.RemoveAt(0);
                    Text.PrintLine(str, newCols.ToArray());
                }
                else
                {
                    Console.WriteLine(str);
                }
            }

            public static void ArrayStart(params string[] cols)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Text.cols = cols;
                if (cols.Length > 0)
                {
                    Console.Write(new string(' ', ArrayLeftPadding));
                    Console.ForegroundColor = ArrayBorderColor;
                    Console.Write("+");
                    // draw top border
                    foreach (string col in cols)
                    {
                        Console.Write(new string('-', col.Length));
                        Console.Write("+");
                    }
                    Console.WriteLine();
                    Console.Write(new string(' ', ArrayLeftPadding));
                    Console.Write("|");
                    // draw header
                    foreach (string col in cols)
                    {
                        Console.ForegroundColor = ArrayHeaderColor;
                        Text.Print(col);
                        Console.ForegroundColor = ArrayBorderColor;
                        Console.Write("|");
                    }
                    Console.WriteLine();
                    Console.Write(new string(' ', ArrayLeftPadding));
                    Console.ForegroundColor = ArrayBorderColor;
                    Console.Write("+");
                    // draw top border
                    foreach (string col in cols)
                    {
                        Console.Write(new string('-', col.Length));
                        Console.Write("+");
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = oldColor;
            }

            public static void ArrayFill(params string[] values)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                if (values.Length > 0)
                {
                    Console.ForegroundColor = ArrayBorderColor;
                    Console.Write(new string(' ', ArrayLeftPadding));
                    Console.Write("|");
                    // draw header
                    for (int i = 0; i < values.Length; i++)
                    {
                        Console.ForegroundColor = ArrayValueColor;
                        if (values[i].Length > cols[i].Length)
                        {
                            Text.Print(values[i].Substring(0, cols[i].Length));
                        }
                        else
                        {
                            Text.Print(values[i] + new string(' ', cols[i].Length - values[i].Length));
                        }

                        Console.ForegroundColor = ArrayBorderColor;
                        Console.Write("|");
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = oldColor;
            }


            public static void ArrayEnd()
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                if (cols.Length > 0)
                {
                    Console.Write(new string(' ', ArrayLeftPadding));
                    Console.ForegroundColor = ArrayBorderColor;
                    Console.Write("+");
                    foreach (string col in cols)
                    {
                        Console.Write(new string('-', col.Length));
                        Console.Write("+");
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = oldColor;
            }

        }
    }