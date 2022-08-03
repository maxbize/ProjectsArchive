using UnityEngine;
using System.Collections.Generic;

public static class LevelData
{
    static List<Level> levels = new List<Level> {
		new Level( 
            new List<Vector3> { // level 0
		        new Vector3(150, 0, 450),
		        new Vector3(150, 0, 150),
		        new Vector3(-250, 0, 150),
		        new Vector3(-250, 0, -250),
		        new Vector3(450, 0, -250)
                },
            new List<Wave> {
                new Wave(0, 0, 0, false, 0, 0),
                new Wave(200f, 100f, 20, false, 100, 0.5f),
                new Wave(200f, 100f, 20, false, 100, 0.5f),
                new Wave(200f, 100f, 20, false, 100, 0.5f),
                new Wave(200f, 100f, 20, false, 100, 0.5f),
                }
            )
	};

    public static Level GetLevel(int index)
    {
        return levels[index];
    }
}
