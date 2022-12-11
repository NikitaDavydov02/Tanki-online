using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAIGun : BaseGun {

    //public float force = 10f;
    private GameObject _flashHit;
    private GameObject _lightHit;
    [SerializeField]
    ReactiveTarget reactiveTarget;
    [SerializeField]
    FireEffect fireEffect;
    public float shootRadius = 3f;
    private bool _shoot = false;
    // Use this for initialization
    void Start()
    {
        timeFromLastShot = timeOfRecharge;
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayS = new Ray(transform.position, transform.up);
        RaycastHit hitS;
        if(Physics.Raycast(rayS,out hitS))
        {
            GameObject xHitObject = hitS.transform.gameObject;
            try
            {
                if (xHitObject.GetComponent<Player>())
                {
                    _shoot = true;
                }
            }
            catch
            {

            }
        }
        if (_shoot && timeFromLastShot >= timeOfRecharge && reactiveTarget.alive)
        {
            base.PlaySound();
            _shoot = false;
            timeFromLastShot = 0f;
            Ray ray = new Ray(transform.position, transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                StartCoroutine(HitEffect(hit.point));
                fireEffect.Effect();
                PlayerAsReactiveTarget reactiveTargetScript = null;
                try
                {
                    reactiveTargetScript = hit.collider.gameObject.GetComponent<PlayerAsReactiveTarget>();
                }
                catch
                {

                }
                if (reactiveTargetScript != null)
                {
                    base.SendSound(null, reactiveTargetScript);
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
        timeFromLastShot += Time.deltaTime;
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
}
