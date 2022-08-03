using UnityEngine;
using System.Collections.Generic;

public struct Level {
    public List<Vector3> markers;
    public List<Wave> waves;

    public Level(List<Vector3> levelMarkers, List<Wave> waves)
    {
        markers = levelMarkers;
        this.waves = waves;
    }
}
