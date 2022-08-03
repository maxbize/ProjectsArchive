using UnityEngine;
using System.Collections;

public struct Int3
{

    public int x;
    public int y;
    public int z;

    public Int3(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    static public implicit operator Vector3(Int3 i) {
        return new Vector3(i.x, i.y, i.z);
    }

    static public implicit operator Int3(Vector3 v) {
        return new Int3(
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z)
            );
    }


    public static Int3 operator *(Int3 i, int n) {
        return new Int3(
            i.x * n,
            i.y * n,
            i.z * n);
    }

    public static Int3 operator +(Int3 i1, Int3 i2) {
        return new Int3(
            i1.x + i2.x,
            i1.y + i2.y,
            i1.z + i2.z);
    }

    public static Int3 operator -(Int3 i1, Int3 i2) {
        return new Int3(
            i1.x - i2.x,
            i1.y - i2.y,
            i1.z - i2.z);
    }

    public static bool operator ==(Int3 i1, Int3 i2) {
        if (i1.x == i2.x &&
            i1.y == i2.y &&
            i1.z == i2.z) {
            return true;
        }
        return false;
    }

    public static bool operator !=(Int3 i1, Int3 i2) {
        if (i1.x != i2.x ||
            i1.y != i2.y ||
            i1.z != i2.z) {
            return true;
        }
        return false;
    }

    public override string ToString() {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}
