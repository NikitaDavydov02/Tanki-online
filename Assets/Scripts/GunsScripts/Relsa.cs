using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relsa : BaseGun {
    private GameObject _flashHit;
    private GameObject _lightHit;
    public float shootRadius = 3f;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    RelsaFireEffect fireEffect;
    public bool garage = true;
    // Use this for initialization
    void Start()
    {
        //timeFromLastShot = timeOfRecharge;
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
        if (Input.GetButtonDown("Fire3") && timeFromLastShot >= timeOfRecharge && reactiveTarget.alive)
        {
            fireEffect.Effect();
            base.PlaySound();
            StartCoroutine(Shot());
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

    public IEnumerator Shot()
    {
        yield return new WaitForSeconds(1f);
        timeFromLastShot = 0f;
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //StartCoroutine(HitEffect(hit.point));
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
