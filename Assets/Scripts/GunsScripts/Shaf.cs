using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaf : BaseGun {
    private GameObject _flashHit;
    private GameObject _lightHit;
    public float shootRadius = 3f;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    ShaftFireEffect fireEffect;
    private float pause = 0;
    public float pauseSpeed = 2f;
    private bool recharged = false;
    public float vertSpeed = 0.2f;
    private float angle = 0f;
    public bool garage = true;
    // Use this for initialization
    void Start()
    {
        pause = 0;
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (garage)
            return;
        //
        if (Managers.UserInterfaceManager.inZoom)
        {
            //Debug.Log("Shaftknow!");
            float vertInput = Input.GetAxis("Vertical");
            float _rotX = 0f;
            if (vertInput != 0)
            {
                _rotX = vertInput * vertSpeed;
            }
            angle += -_rotX;
            Managers.UserInterfaceManager.ChangeDeltaAngle(angle);
            if (angle < 5f && angle > -2)
               transform.Rotate(angle, 0, 0);
        }
        //
        if (!Managers.UserInterfaceManager.inZoom)
        {
            transform.Rotate(-angle, 0, 0);
            angle = 0;
        }
        if (!reactiveTarget.alive)
        {
            Managers.UserInterfaceManager.UpdateRechargeSlider(0);
        }
        if (Input.GetButton("Fire3") && recharged)
        {
            if (timeFromLastShot <= 0)
            {
                timeFromLastShot = 0;
            }
            timeFromLastShot -= pauseSpeed * Time.deltaTime;
            pause += Time.deltaTime;
        }
        if (Input.GetButtonUp("Fire3") && recharged && reactiveTarget.alive)
        {
            Messenger.Broadcast(GameEvent.SHOOT);
            recharged = false;
            base.PlaySound();
            timeFromLastShot = 0f;
            Ray ray = new Ray(transform.position, transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //StartCoroutine(HitEffect(hit.point));
                fireEffect.Effect();
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
                    reactiveTargetScript.Damage(damage*pause);
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
        if (!recharged)
            timeFromLastShot += Time.deltaTime;
        if (timeFromLastShot >= timeOfRecharge)
        {
            timeFromLastShot = timeOfRecharge;
            recharged = true;
            pause = 0;
        }
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
        Managers.UserInterfaceManager.camZoom = true;
        pause = 0;
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
    }
}
