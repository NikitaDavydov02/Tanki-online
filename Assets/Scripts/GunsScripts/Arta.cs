using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arta : BaseGun {
    private GameObject _flashHit;
    private GameObject _lightHit;
    public float shootRadius = 3f;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    ArtaFireEffect fireEffect;
    private float pause = 0;
    public float pauseSpeed = 2f;
    private bool recharged = false;
    public float vertSpeed = 0.2f;
    private float angle = 0f;
    public bool garage = true;
    public float gravitation = -20f;
    public float speed = -20f;
    [SerializeField]
    GameObject checkBulletPrefab;
    float checkBool = 0;
    float v;
    float g;
    // Use this for initialization
    void Start()
    {
        v = speed;
        g = Mathf.Abs(gravitation);
        pause = 0;
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (garage)
            return;
        float vertInput = Input.GetAxis("Mouse Y") * vertSpeed * Time.deltaTime;
        angle = transform.localEulerAngles.x - 90;
        if (!Managers.UserInterfaceManager.inArtaZoom)
        {
            if (angle <= 15 && angle >= -55f)
                transform.Rotate(vertInput, 0, 0);
            if (transform.localEulerAngles.x < 35f)
                transform.Rotate(2, 0, 0);
            if (transform.localEulerAngles.x > 105f)
                transform.Rotate(-2, 0, 0);
        }
        if (!Managers.UserInterfaceManager.inZoom)
        {
            //transform.Rotate(-angle, 0, 0);
            //angle = 0;
        }
        if (!reactiveTarget.alive)
        {
            Managers.UserInterfaceManager.UpdateRechargeSlider(0);
        }
        if ((Input.GetButtonDown("Fire3")||Input.GetKeyDown(KeyCode.Mouse0)) && timeFromLastShot >= timeOfRecharge && reactiveTarget.alive)
        {
            base.PlaySound();
            timeFromLastShot = 0f;
            fireEffect.Effect();
        }
        timeFromLastShot += Time.deltaTime;
        if (timeFromLastShot > timeOfRecharge)
            timeFromLastShot = timeOfRecharge;
        Managers.UserInterfaceManager.UpdateRechargeSlider((timeFromLastShot / timeOfRecharge) * Managers.UserInterfaceManager.maxValue);

        //Расчёт точки попадания
        //if (checkBool == 0)
        //{
        //    GameObject check;
        //    check = Instantiate(checkBulletPrefab) as GameObject;
        //    check.transform.position = transform.position;
        //    CheckBullet checkBullet = check.GetComponent<CheckBullet>();
        //    checkBullet.forward = transform.up;
        //    check.transform.eulerAngles = transform.parent.transform.eulerAngles;
        //}
        //checkBool++;
        //if (checkBool == 10)
        //{
        //    checkBool = 0;
        //}
        GameObject check;
        check = Instantiate(checkBulletPrefab) as GameObject;
        check.transform.position = transform.position;
        // CheckBullet checkBullet = check.GetComponent<CheckBullet>();
        //checkBullet.forward = transform.up;
        check.transform.eulerAngles = transform.parent.transform.eulerAngles;

        if (Managers.UserInterfaceManager.targetPricelPoint!= Managers.UserInterfaceManager.currentPricelPoint && Managers.UserInterfaceManager.inArtaZoom)
        {
            Vector3 now = Managers.UserInterfaceManager.currentPricelPoint;
            Vector3 must  = Managers.UserInterfaceManager.targetPricelPoint;
            float h = must.y - transform.position.y;
            float s = Mathf.Sqrt((transform.position.x - must.x) * (transform.position.x - must.x) + (transform.position.z - must.z) * (transform.position.z - must.z));
            float angleInRadians = Mathf.Acos((s / v) * Mathf.Sqrt((Mathf.Sqrt((v * v * v * v) - (2 * g * h * v * v) - (g * g * s * s)) - (g * h) + (v * v)) / (2 * ((h * h) + (s * s)))));
            float angleInDegrees = (angleInRadians * 180) / Mathf.PI;
            Vector3 rot = transform.localEulerAngles;
            rot.x = 90 - angleInDegrees;
            transform.localEulerAngles = rot;
            Vector3 target = Managers.UserInterfaceManager.targetPricelPoint;
            target.y = 0;
            float S = Mathf.Sqrt((target.x - transform.position.x) * (target.x - transform.position.x) + (target.z - transform.position.z) * (target.z - transform.position.z));
            float sin = (target.x - transform.position.x) / S;
            float cos = (target.z - transform.position.z) / S;
            float angle = 0;
            if (sin >= 0 && cos >= 0)
            {
                angle = (Mathf.Asin(sin) * 180) / Mathf.PI;
            }
            if (sin >= 0 && cos < 0)
            {
                angle = (Mathf.Acos(cos) * 180) / Mathf.PI;
            }
            if (sin < 0 && cos >= 0)
            {
                angle = -(Mathf.Acos(cos) * 180) / Mathf.PI;
            }
            if (sin < 0 && cos < 0)
            {
                angle = -(Mathf.Acos(cos) * 180) / Mathf.PI;
            }
            Managers.UserInterfaceManager.TowerAngle = angle;

            Vector3 vec = Managers.UserInterfaceManager.currentPricelPoint - transform.position;
            float dx = Managers.UserInterfaceManager.currentPricelPoint.x - transform.position.x;
            float dz = Managers.UserInterfaceManager.currentPricelPoint.z - transform.position.z;
            //Debug.Log("X: " + dx);
            //Debug.Log("Z: " + dz);
            float l = Mathf.Sqrt((dx * dx) + (dz * dz));
            //Debug.Log("L: " + l);
            float x = ((Managers.UserInterfaceManager.currentPricelPoint.x + transform.position.x) / 2);
            float z = ((Managers.UserInterfaceManager.currentPricelPoint.z + transform.position.z) / 2);
            //Debug.Log("Point(" + x + ";" + z + ")");
            Vector3 point = new Vector3(x, 7, z);
            //Managers.UserInterfaceManager.ShowLine(l, point, angle);
        }


        //if (Input.GetButtonUp("Fire3") && recharged && reactiveTarget.alive)
        //{
        //    Messenger.Broadcast(GameEvent.SHOOT);
        //    recharged = false;
        //    base.PlaySound();
        //    timeFromLastShot = 0f;
        //    Ray ray = new Ray(transform.position, transform.up);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        //StartCoroutine(HitEffect(hit.point));
        //        fireEffect.Effect();
        //        ReactiveTarget reactiveTargetScript = null;
        //        try
        //        {
        //            reactiveTargetScript = hit.collider.gameObject.transform.parent.GetComponent<ReactiveTarget>();
        //        }
        //        catch
        //        {

        //        }
        //        if (reactiveTargetScript != null)
        //        {
        //            base.SendSound(reactiveTargetScript, null);
        //            reactiveTargetScript.Damage(damage * pause);
        //        }
        //        Collider[] hitColliers = Physics.OverlapSphere(hit.point, shootRadius);
        //        foreach (Collider hitCollider in hitColliers)
        //        {
        //            Rigidbody rigidbody = hitCollider.gameObject.GetComponent<Rigidbody>();
        //            if (rigidbody != null)
        //            {
        //                rigidbody.velocity = -hit.normal * force;
        //            }
        //        }
        //    }
        //}
        //if (!recharged)
        //    timeFromLastShot += Time.deltaTime;
        //if (timeFromLastShot >= timeOfRecharge)
        //{
        //    timeFromLastShot = timeOfRecharge;
        //    recharged = true;
        //    pause = 0;
        //}
        //Managers.UserInterfaceManager.UpdateRechargeSlider((timeFromLastShot / timeOfRecharge) * Managers.UserInterfaceManager.maxValue);
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
        Managers.UserInterfaceManager.camArtaZoom = true;
        pause = 0;
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
    }
}
