using UnityEngine;


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
        return new Vector3(int3.X, int3.Y, int3.Z);
    }

    public static Int3 operator -(Int3 i3_1, Int3 i3_2)
    {
        return new Int3(i3_1.X - i3_2.X, i3_1.Y - i3_2.Y, i3_1.Z - i3_2.Z);
    }

    public static Int3 operator +(Int3 i3_1, Int3 i3_2)
    {
        return new Int3(i3_1.X + i3_2.X, i3_1.Y + i3_2.Y, i3_1.Z + i3_2.Z);
    }

    public static bool operator ==(Int3 i3_1, Int3 i3_2)
    {
        if (i3_1.X == i3_2.X &&
            i3_1.Y == i3_2.Y &&
            i3_1.Z == i3_2.Z)
        {
            return true;
        }
        return false;
    }

    public static bool operator !=(Int3 i3_1, Int3 i3_2)
    {
        if (i3_1.X != i3_2.X ||
            i3_1.Y != i3_2.Y ||
            i3_1.Z != i3_2.Z)
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public double Length()
    {
        return Mathf.Sqrt(X * X + Y * Y + Z * Z);
    }

    public override string ToString()
    {
        return "X: " + X + ",\t Y:" + Y + ",\t Z: " + Z + "\n";
    }
}
