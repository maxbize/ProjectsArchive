using UnityEngine;

public class Grapher3 : MonoBehaviour {
	
	public enum functionOption {
		linear,
		exponential,
		parabolic,
		sine,
		ripple
	}
	
	private delegate float functionDelegate(Vector3 p, float t);
	private static functionDelegate[] functionDelegates = {
		linear,
		exponential,
		parabola,
		sine,
		ripple
	};
	
	public functionOption function;
	public int resolution = 10;
	
	private ParticleSystem.Particle[] points;
	
	void Start() {
		createPoints();
	}
	
	private static float linear (Vector3 p, float t) {
		return 1f - p.x - p.y - p.z + 0.5f * Mathf.Sin(t);
	}
	
	private static float exponential (Vector3 p, float t) {
		return 1f - p.x * p.x - p.y * p.y - p.z * p.z + 0.5f * Mathf.Sin(t);
	}
	
	private static float parabola (Vector3 p, float t){
		p.x = 2f * p.x - 1f;
		p.z = 2f * p.z - 1f;
		return 1f - p.x * p.x - p.z * p.z + 0.5f * Mathf.Sin(t);
	}

	private static float ripple (Vector3 p, float t){
		float squareRadius =
			(p.x - 0.5f) * (p.x - 0.5f) +
			(p.y - 0.5f) * (p.y - 0.5f) +
			(p.z - 0.5f) * (p.z - 0.5f);
		return Mathf.Sin(4 * Mathf.PI * squareRadius - 2f * t);
	}
	
	private static float sine(Vector3 p, float t) {
		return 0.50f +
			0.25f * Mathf.Sin(4 * Mathf.PI * p.x + 4 * t) * Mathf.Sin(2 * Mathf.PI * p.z + t) +
			0.10f * Mathf.Cos(3 * Mathf.PI * p.x + 5 * t) * Mathf.Cos(5 * Mathf.PI * p.z + 3 * t) +
			0.15f * Mathf.Sin(Mathf.PI * p.x + 0.6f * t);
	}	
	
	private void createPoints() {
		if (resolution < 2) {
			resolution = 2;	
		}
		if (resolution > 40) {
			resolution = 40;
		}
		functionDelegate f = functionDelegates[(int)function];
		points = new ParticleSystem.Particle[resolution * resolution * resolution];
		float increment = 1f / (resolution - 1);
		float t = Time.timeSinceLevelLoad;
		int i = 0;
		for (int x = 0; x < resolution; x++) {
			for (int y = 0; y < resolution; y++) {
				for (int z = 0; z < resolution; z++) {
					Vector3 p = new Vector3(x, y, z) * increment;
					points[i].position = new Vector3(p.x, p.y, p.z);
					points[i].color = new Color(p.x, p.y, p.z, f(p,t));
					points[i].size = 0.1f;
					i++;
				}
			}
		}
	}
	
	void Update() {
		createPoints();
		particleSystem.SetParticles(points, points.Length);	
	}

}
