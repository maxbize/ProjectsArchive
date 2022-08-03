using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    // set in editor
    public int maxHealth;
    public float timeToHeal;
    public GameObject shield;
    public float minShieldAlpha;
    public float maxShieldAlpha;
    public GameObject deathExplosionGrey;
    public GameObject deathExplosionRed;
    public GameObject shieldDamageEffect;
    public GameObject shieldExplosionEffect;
    public float colorChangeRate;
    public AudioClip damagedShieldSound;
    public AudioClip noShieldSound;


    private Color greyColor = new Color(1, 1, 1);
    [HideInInspector]
    public Color redColor2 = new Color(1, (float)60 / 255, (float)60 / 255);

    private float healTimer = 0;
    private int health;
    private Renderer myRenderer;
    [HideInInspector]
    public Color targetColor;
    private AudioSource myAs;

	// Use this for initialization
	void Start () {
        targetColor = greyColor;
        myRenderer = GetComponent<Renderer>();
        Init();
        myAs = GetComponent<AudioSource>();
	}

    public void Init() {
        health = maxHealth;
        UpdateShield();
    }
	
	// Update is called once per frame
	void Update () {
        if (health != maxHealth) {
            healTimer += Time.deltaTime;
        }
        if (healTimer > timeToHeal) {
            healTimer = 0;
            ChangeHealth(1);
        }
        myRenderer.material.color = Color.Lerp(myRenderer.material.color, targetColor, colorChangeRate);
	}

    public void RegisterHit(GameObject offender) {
        ChangeHealth(-1);
        ShieldDamageEffect(offender);
        ShieldDamageSound();
    }

    private void ChangeHealth(int amount) {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateShield();
        if (health <= 0) {
            Die();
        }
    }

    private void UpdateShield() {
        float t = (float)(health - 1) / (float)maxHealth;
        float alpha = Mathf.Lerp(minShieldAlpha, maxShieldAlpha, t) / (float)255;
        if (health == 1) {
            alpha = 0;
        }
        Color newColor = Color.Lerp(Color.red, Color.green, t);
        newColor.a = alpha;
        shield.GetComponent<Renderer>().material.color = newColor;
    }

    public void ShieldDamageEffect(GameObject offender) {
        if (health > 1) {
            Vector3 pos = transform.position + (offender.transform.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(pos - transform.position);
            Instantiate(shieldDamageEffect, pos, rot);
        }
        else if (health > 0) {
            Instantiate(shieldExplosionEffect, transform.position, Quaternion.LookRotation(Vector3.up));
        }
    }

    public void ShieldDamageSound() {
        myAs.pitch = Random.Range(0.9f, 1.0f);
        if (health > 1) {
            myAs.PlayOneShot(damagedShieldSound);
        }
        else if (health > 0) {
            myAs.PlayOneShot(noShieldSound);        
        }
    }

    private void Die() {
        Instantiate(GetDeathExplosion(), transform.position, transform.rotation);
        FindObjectOfType<StoryManager>().RegisterHeroDeath();
        gameObject.SetActive(false);
    }

    private GameObject GetDeathExplosion() {
        return targetColor == redColor2 ? deathExplosionRed : deathExplosionGrey;
    }
}
