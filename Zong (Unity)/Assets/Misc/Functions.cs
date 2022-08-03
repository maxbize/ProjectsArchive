// This file is for miscellaneous functions

using UnityEngine;
using System.Collections.Generic;

public static class Functions
{

    public static Color randomColor()
    {
        Color color = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f));
        return color;
    }

    public static bool cellInList(List<Int3> cellList, Int3 cell)
    {
        foreach (Int3 listCell in cellList)
        {
            if (listCell.X == cell.X &&
                listCell.Y == cell.Y &&
                listCell.Z == cell.Z)
            {
                return true;
            }
        }
        return false;
    }
}