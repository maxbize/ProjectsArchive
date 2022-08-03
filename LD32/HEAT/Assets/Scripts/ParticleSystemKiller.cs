using UnityEngine;
using System.Collections;

public class ParticleSystemKiller : MonoBehaviour {

    private float timer = 0;
    private float timeToDeath;

    [HideInInspector]
    public Vector3 velocity;

	// Use this for initialization
	void Start () {
        timeToDeath = GetComponent<ParticleSystem>().startLifetime + GetComponent<ParticleSystem>().duration;

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource) {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        if (timer > timeToDeath) {
            Destroy(gameObject);
        }
	}
}
