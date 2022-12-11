using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveTarget : MonoBehaviour {
    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    public AudioClip hitClip;
    [SerializeField]
    public AudioSource hitAudioSource;
    private float maxHealth;
    public float health = 100f;
    public float activitySpeed = 0.02f;
    public float burningSpeed = 0f;
    public bool alive { get; private set; }
    public float activity { get; private set; }
	// Use this for initialization
	void Start () {
        alive = true;
        activity = 1;
        maxHealth = health;
        healthSlider.value = healthSlider.maxValue;
    }
	
	// Update is called once per frame
	void Update () {
        if (burningSpeed != 0)
        {
            Damage(burningSpeed);
        }
        if (activity != 1)
        {
            activity += activitySpeed * Time.deltaTime;
            if (activity > 1)
                activity = 1;
        }
        if (burningSpeed != 0)
        {
            burningSpeed -= 0.03f * Time.deltaTime;
            if (burningSpeed<0)
                burningSpeed = 0;
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        //PlayHitSound();
        healthSlider.value = (health / maxHealth) * healthSlider.maxValue;
        //if (health <= 0 && !GetComponent<Player>())
        if (health <= 0 && alive)
        {
            alive = false;
            StartCoroutine(Destroy());
        }
    }

    public void Activity(float delta)
    {
        activity -= delta;
        if (activity < 0.2f)
        {
            activity = 0.2f;
        }
    }

    public void Burning(float burningSpeed)
    {
        this.burningSpeed = burningSpeed;
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        Managers.Game.PlayerKill();
    }

    private void PlayHitSound()
    {
        if (hitClip != null)
            hitAudioSource.PlayOneShot(hitClip);
    }
}
