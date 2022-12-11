using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molot : BaseGun {
    public float shootAngle = 40f;
    public float delta = 1f;
    public float distance = 7f;
    public float shootRadius = 3f;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    MolotFireEffect fireEffect;
    public int barabanCount = 3;
    private int currentCount = 3;
    public float timeOfBarabanRecharge = 10f;
    private float rechargeTime=0f;
    private float targetTime;
    public bool garage = true;
    // Use this for initialization
    void Start()
    {
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
        targetTime = timeOfBarabanRecharge;
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
        if (Input.GetButtonDown("Fire3") && timeFromLastShot >= targetTime && reactiveTarget.alive && currentCount > 0)
        {
            fireEffect.Effect();
            base.PlaySound();
            timeFromLastShot = 0;
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
                            reactiveTargetScript.Damage(damage);
                        }
                        Collider[] hitColliers = Physics.OverlapSphere(hit.point, shootRadius);
                        foreach (Collider hitCollider in hitColliers)
                        {
                            Rigidbody rigidbody = hitCollider.gameObject.GetComponent<Rigidbody>();
                            if (rigidbody != null)
                            {
                                rigidbody.velocity = -hit.normal * force;
                            }
                        }
                    }
                }
            }
            timeFromLastShot = 0;
            currentCount--;
            if (currentCount == 0)
            {
                targetTime = timeOfBarabanRecharge;
            }
            else
            {
                targetTime = timeOfRecharge;
            }
        }
        timeFromLastShot += Time.deltaTime;
        if (timeFromLastShot > targetTime)
        {
            timeFromLastShot = targetTime;
            if (targetTime == timeOfBarabanRecharge)
            {
                currentCount = barabanCount;
            }
        }
        Managers.UserInterfaceManager.UpdateRechargeSlider((timeFromLastShot / targetTime) * Managers.UserInterfaceManager.maxValue);
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_TAKE_DOUBLEBLOW, SetDoubleBlowActive);
        Messenger.AddListener(GameEvent.PLAYER_DOUBLEBLOW_DISACTIVE, SetDoubleBlowDisactive);
        Messenger.AddListener(GameEvent.PLAYER_ALIVE, ReloadGun);
        Messenger.AddListener(GameEvent.NOGARAGE, NoGarage);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_DOUBLEBLOW, SetDoubleBlowActive);
        Messenger.RemoveListener(GameEvent.PLAYER_DOUBLEBLOW_DISACTIVE, SetDoubleBlowDisactive);
        Messenger.RemoveListener(GameEvent.PLAYER_ALIVE, ReloadGun);
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
    public void ReloadGun()
    {
        currentCount = 0;
        targetTime = timeOfBarabanRecharge;
        timeFromLastShot = 0;
    }
    public void NoGarage()
    {
        garage = false;
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
    }

}
