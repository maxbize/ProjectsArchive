using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utilities
{

    public static List<Int3> dirs = genDirs();

    public static List<Int3> genDirs() {
        return new List<Int3>() {
        new Int3(1, 0, 0),
        new Int3(0, 1, 0),
        new Int3(0, 0, 1),
        new Int3(-1, 0, 0),
        new Int3(0, -1, 0),
        new Int3(0, 0, -1)
    };
    }

    // Generates the dirs list but in a random order
    public static List<Int3> genRandomDirs() {
        List<Int3> orderedDirs = genDirs();
        List<Int3> rndDirs = new List<Int3>();

        // We could generalize this, but this is the only place it's needed
        for (int i = 0; i < 6; i++) {
            int idx = Random.Range(0, 6 - i);
            rndDirs.Add(orderedDirs[idx]);
            orderedDirs.RemoveAt(idx);
        }

        return rndDirs;
    }

    // Using this to fix gymbal lock. Pretty bad, but works with our 90 degree angles =)
    public static Vector3 roundedV3(Vector3 v) {
        return new Vector3(
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z)
            );
    }

}
