using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcan : BaseGun {
    private GameObject _flashHit;
    private GameObject _lightHit;
    public float shootRadius = 3f;
    public float speedOfShooting = 2f;
    public float burningSpeed = 0.3f;
    public float selfDamage = 15f;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    FrizFireEffect fireEffect;
    public float timeOfFiring = 0f;
    public bool garage = true;
    // Use this for initialization
    void Start()
    {
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (garage)
            return;
        if (!reactiveTarget.alive)
        {
            Managers.UserInterfaceManager.UpdateRechargeSlider(0);
        }
        if (Input.GetButton("Fire3") && timeFromLastShot >0 && reactiveTarget.alive)
        {
            timeFromLastShot -= speedOfShooting * Time.deltaTime;
            timeOfFiring += Time.deltaTime;
            if (timeFromLastShot <= 0)
            {
                timeFromLastShot = 0;
            }
            base.PlaySound();
            fireEffect.Effect();
            Ray ray = new Ray(transform.position, transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ReactiveTarget reactiveTargetScript = null;
                try
                {
                    reactiveTargetScript = hit.collider.gameObject.transform.parent.GetComponent<ReactiveTarget>();
                }
                catch
                {

                }
                if (reactiveTargetScript != null)
                {
                    base.SendSound(reactiveTargetScript, null);
                    reactiveTargetScript.Damage(damage * Time.deltaTime);
                }
            }
        }
        if (Input.GetButtonUp("Fire3") || timeFromLastShot == 0||!reactiveTarget.alive)
        {
            fireEffect.StopEffect();
            timeOfFiring = 0;
        }
        if (timeOfFiring > 5)
        {
            reactiveTarget.SelfDamage(selfDamage * Time.deltaTime);
            reactiveTarget.Burning(burningSpeed);
        }
        timeFromLastShot += Time.deltaTime;
        if (timeFromLastShot > timeOfRecharge)
            timeFromLastShot = timeOfRecharge;
        Managers.UserInterfaceManager.UpdateRechargeSlider((timeFromLastShot / timeOfRecharge) * Managers.UserInterfaceManager.maxValue);
    }

    public override IEnumerator HitEffect(Vector3 pos)
    {
        _flashHit = Instantiate(fireHitPrefab) as GameObject;
        _flashHit.transform.position = pos;
        _lightHit = Instantiate(lightHitPrefab) as GameObject;
        _lightHit.transform.position = pos;
        yield return new WaitForSeconds(0.1f);
        Destroy(_lightHit);
        yield return new WaitForSeconds(3f);
        Destroy(_flashHit);
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_TAKE_DOUBLEBLOW, SetDoubleBlowActive);
        Messenger.AddListener(GameEvent.PLAYER_DOUBLEBLOW_DISACTIVE, SetDoubleBlowDisactive);
        Messenger.AddListener(GameEvent.NOGARAGE, NoGarage);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_DOUBLEBLOW, SetDoubleBlowActive);
        Messenger.RemoveListener(GameEvent.PLAYER_DOUBLEBLOW_DISACTIVE, SetDoubleBlowDisactive);
        Messenger.RemoveListener(GameEvent.NOGARAGE, NoGarage);
    }
    public void SetDoubleBlowActive()
    {
        damage = doubleDamage;
    }
    public void SetDoubleBlowDisactive()
    {
        damage = onceDamage;
    }
    public void NoGarage()
    {
        garage = false;
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
    }
}
