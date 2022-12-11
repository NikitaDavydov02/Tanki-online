using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friz : BaseGun {

    private GameObject _flashHit;
    private GameObject _lightHit;
    public float shootAngle = 40f;
    public float speedOfShooting = 2f;
    public float delta = 1f;
    public float distance = 5f;
    public float activitySpeed = 0.05f;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    FrizFireEffect fireEffect;
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
        if (Input.GetButton("Fire3") && timeFromLastShot > 0 && reactiveTarget.alive)
        {
            fireEffect.Effect();
            base.PlaySound();
            timeFromLastShot -= speedOfShooting * Time.deltaTime;
            if (timeFromLastShot <= 0)
            {
                timeFromLastShot = 0;
            }
            Vector3 direction;
            direction = transform.up;
            direction.x -= delta;
            Ray leftRay = new Ray(transform.position, direction);
            direction = transform.up;
            direction.x += delta;
            Ray rightRay = new Ray(transform.position, direction);
            direction = transform.up;
            direction.z += delta;
            Ray topRay = new Ray(transform.position, direction);
            direction = transform.up;
            direction.z -= delta;
            Ray bottomRay = new Ray(transform.position, direction);
            Ray ray = new Ray(transform.position, transform.up);
            List<Ray> rays = new List<Ray>();
            rays.Add(leftRay);
            rays.Add(rightRay);
            rays.Add(topRay);
            rays.Add(bottomRay);
            rays.Add(ray);
            foreach (Ray r in rays)
            {
                RaycastHit hit;
                if (Physics.Raycast(r, out hit))
                {
                    if (hit.distance <= distance)
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
                            Debug.Log("Sound sended!");
                            reactiveTargetScript.Activity(activitySpeed * Time.deltaTime);
                            reactiveTargetScript.Damage(damage * Time.deltaTime);
                        }
                    }
                }
            }
        }
        if (Input.GetButtonUp("Fire3")||timeFromLastShot==0)
        {
            fireEffect.StopEffect();
        }
        timeFromLastShot += Time.deltaTime;
        if (timeFromLastShot > timeOfRecharge)
            timeFromLastShot = timeOfRecharge;
        Managers.UserInterfaceManager.UpdateRechargeSlider((timeFromLastShot / timeOfRecharge) * Managers.UserInterfaceManager.maxValue);
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
