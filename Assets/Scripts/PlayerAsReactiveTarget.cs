using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAsReactiveTarget : MonoBehaviour {
    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    CameraScript cam;
    [SerializeField]
    public AudioClip hitClip;
    [SerializeField]
    public AudioSource hitAudioSource;
    public float[] xSpawnPoints;
    public float[] zSpawnPoints;
    public  float maxHealth;
    public float health = 100f;
    public bool alive;
    //public bool alive { get; private set; }
    private float spawnX;
    private float spawnZ;
    private bool camerHadAlreadyFly = false;
    private bool activeDoubleBlow = false;
    private float timeToDestroy = 5f;
    private float timeBeforeDestroy = 0;
    public float burningSpeed = 0f;
    public bool garage = true;
    // Use this for initialization
    void Start()
    {
        //
        StartCoroutine(Alive());
    }
    public IEnumerator Alive()
    {
        yield return new WaitForSeconds(0.2f);
        alive = true;
        Managers.Tank.alive = true;
        maxHealth = health;
        healthSlider.value = healthSlider.maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        if (burningSpeed != 0)
        {
            SelfDamage(burningSpeed);
        }
        if (burningSpeed != 0)
        {
            burningSpeed -= 0.03f * Time.deltaTime;
            if (burningSpeed < 0)
                burningSpeed = 0;
        }
        if (timeBeforeDestroy != 0)
        {
            Managers.UserInterfaceManager.Destroy(true, timeBeforeDestroy);
            timeBeforeDestroy -= Time.deltaTime;
            if (timeBeforeDestroy <= 0)
            {
                timeBeforeDestroy = 0;
                Damage(1000);
                Managers.UserInterfaceManager.Destroy(false, 100);
            }
        }
        if (!alive)
        {
            healthSlider.value = 0;
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            timeBeforeDestroy = timeToDestroy;
            Managers.UserInterfaceManager.Destroy(true, timeBeforeDestroy);
        }
    }
    public void SelfDamage(float damage)
    {
        if (!alive)
            return;
        if (!activeDoubleBlow)
            health -= damage;
        else
            health -= (damage / 2);
        healthSlider.value = (health / maxHealth) * healthSlider.maxValue;
        if (health <= 0 && GetComponent<Player>() && !camerHadAlreadyFly)
        {
            Messenger.Broadcast(GameEvent.SHOOT);
            Managers.Game.PlayerKilled();
            alive = false;
            Managers.Tank.alive = false;
            camerHadAlreadyFly = true;
            int index = Random.Range(0, xSpawnPoints.Length);
            spawnX = xSpawnPoints[index];
            spawnZ = zSpawnPoints[index];
            cam.Destroy(index);
        }
    }

    public void Damage(float damage)
    {
        if (!alive)
            return;
        if (!activeDoubleBlow)
            health -= damage;
        else
            health -= (damage / 2);
        PlayHitSound();
        healthSlider.value = (health / maxHealth) * healthSlider.maxValue;
        if (health <= 0 && GetComponent<Player>() && !camerHadAlreadyFly)
        {
            Messenger.Broadcast(GameEvent.SHOOT);
            Managers.Game.PlayerKilled();
            alive = false;
            Managers.Tank.alive = false;
            camerHadAlreadyFly = true;
            int index = Random.Range(0, xSpawnPoints.Length);
            spawnX = xSpawnPoints[index];
            spawnZ = zSpawnPoints[index];
            cam.Destroy(index);
        }
    }
    public void ReplaceTank()
    {
        transform.position = new Vector3(spawnX, 1.67f, spawnZ);
        alive = true;
        Managers.Tank.alive = true;
        camerHadAlreadyFly = false;
        health = maxHealth;
        healthSlider.value = healthSlider.maxValue;
        Messenger.Broadcast(GameEvent.PLAYER_ALIVE);
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_TAKE_DOUBLEARMOR, SetDoubleArmorActive);
        Messenger.AddListener(GameEvent.PLAYER_DOUBLEARMOR_DISACTIVE, SetDoubleArmorDisactive);
        Messenger.AddListener(GameEvent.PLAYER_TAKE_KIT, SetKitActive);
        Messenger.AddListener(GameEvent.GAME_OVER, GameOver);
        Messenger.AddListener(GameEvent.GAME_STARTED, GameStarted);
        Messenger.AddListener(GameEvent.PLAYER_ALIVE, EmptyListener);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_DOUBLEARMOR, SetDoubleArmorActive);
        Messenger.RemoveListener(GameEvent.PLAYER_DOUBLEARMOR_DISACTIVE, SetDoubleArmorDisactive);
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_KIT, SetKitActive);
        Messenger.RemoveListener(GameEvent.GAME_OVER, GameOver);
        Messenger.RemoveListener(GameEvent.GAME_STARTED, GameStarted);
        Messenger.RemoveListener(GameEvent.PLAYER_ALIVE, EmptyListener);
    }

    private void GameOver()
    {
        alive = false;
        Managers.Tank.alive = false;
    }
    private void GameStarted()
    {
        camerHadAlreadyFly = true;
        int index = Random.Range(0, xSpawnPoints.Length);
        spawnX = xSpawnPoints[index];
        spawnZ = zSpawnPoints[index];
        cam.Destroy(index);
    }
    private void SetDoubleArmorActive()
    {
        activeDoubleBlow = true;
        Debug.Log("Must work!");
    }
    private void SetDoubleArmorDisactive()
    {
        activeDoubleBlow = false;
    }
    private void SetKitActive()
    {
        health += 100;
        if (health > maxHealth)
            health = maxHealth;
        healthSlider.value = (health / maxHealth) * healthSlider.maxValue;
    }

    private void PlayHitSound()
    {

        if (hitClip != null)
            hitAudioSource.PlayOneShot(hitClip);
    }
    public void Burning(float burningSpeed)
    {
        this.burningSpeed = burningSpeed;
    }

    public void EmptyListener()
    {

    }
}
