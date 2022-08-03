using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Zong
{
    public struct Int3
    {
        public int X;
        public int Y;
        public int Z;

        public Int3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Allow people to cast int3's as vector3's
        public static implicit operator Vector3(Int3 int3)
        {
            return new Vector3(int3.X,int3.Y,int3.Z);
        }

        public static Int3 operator -(Int3 i3_1, Int3 i3_2)
        {
            return new Int3(i3_1.X - i3_2.X, i3_1.Y - i3_2.Y, i3_1.Z - i3_2.Z);
        }

        public static Int3 operator +(Int3 i3_1, Int3 i3_2)
        {
            return new Int3(i3_1.X + i3_2.X, i3_1.Y + i3_2.Y, i3_1.Z + i3_2.Z);
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
    }
}
