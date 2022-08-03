using UnityEngine;

public class Grapher1 : MonoBehaviour {
	
	public enum functionOption {
		linear,
		exponential,
		parabolic,
		sine
	}
	
	private delegate float functionDelegate(float x);
	private static functionDelegate[] functionDelegates = {
		linear,
		exponential,
		parabola,
		Sine
	};
	
	public functionOption function;
	public int resolution = 100;
	private int currentResolution;
	
	private ParticleSystem.Particle[] points;
	
	void Start() {
		createPoints();
	}
	
	private static float linear(float x) {
		return x;
	}
	
	private static float exponential(float x) {
		return x * x;
	}
	
	private static float parabola(float x) {
		x = 2f * x - 1f;
		return x * x;
	}
	
	private static float Sine(float x) {
		return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);	
	}
	
	private void createPoints() {
		if (resolution < 2) {
			resolution = 2;	
		}
		functionDelegate f = functionDelegates[(int)function];
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		float increment = 1f / (resolution - 1);
		for (int i = 0; i < resolution; i++) {
			float x = i * increment;
			points[i].position = new Vector3(x, f(x), 0f);
			points[i].color = new Color(x, f(x), 0f);
			points[i].size = 0.1f;
		}
	}
	
	void Update() {
		createPoints();
		particleSystem.SetParticles(points, points.Length);	
	}

}
