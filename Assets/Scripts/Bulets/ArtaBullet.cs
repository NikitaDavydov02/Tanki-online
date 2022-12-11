using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtaBullet : MonoBehaviour {
    public float speed = 1f;
    public float damage = 50f;
    public float force = 10f;
    public float shootRadius = 1f;
    public Vector3 forward;
    private Vector3 curSpeed;
    public float gravitation = -20f;
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip clip;
    [SerializeField]
    GameObject dimPrefab;
    private GameObject dim;
    // Use this for initialization
    void Start () {
        //Debug.Log(transform.forward);
        curSpeed = forward * speed;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos += curSpeed * Time.deltaTime;
        curSpeed.y += gravitation * Time.deltaTime;
        transform.position = pos;
        if (transform.position.y < 0)
        {
            Debug.Log("NeNorm destroy!");
            Destroy(this.gameObject);
            return;
        }
        //Ray ray = new Ray(transform.position, transform.right);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.distance >= 1)
        //        return;
        //    ReactiveTarget reactiveTargetScript = null;
        //    try
        //    {
        //        reactiveTargetScript = hit.collider.gameObject.transform.parent.GetComponent<ReactiveTarget>();
        //    }
        //    catch
        //    {

        //    }
        //    if (reactiveTargetScript != null)
        //    {
        //        //base.SendSound(reactiveTargetScript, null);
        //        reactiveTargetScript.Damage(damage);
        //    }
        //    Collider[] hitColliers = Physics.OverlapSphere(hit.point, shootRadius);
        //    foreach (Collider hitCollider in hitColliers)
        //    {
        //        Rigidbody rigidbody = hitCollider.gameObject.GetComponent<Rigidbody>();
        //        if (rigidbody != null)
        //        {
        //            rigidbody.velocity = -hit.normal * force;
        //        }
        //    }
        //    Destroy(this.gameObject);
        //    return;
        //}
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("AAA!"+transform.position);
        if (other.gameObject.GetComponent<Player>() || other.gameObject.GetComponent<Arta>()|| other.gameObject.GetComponent<CheckBullet>())
        {
            Debug.Log(other.gameObject);
            return;
        }
        dim = Instantiate(dimPrefab) as GameObject;
        dim.transform.position = transform.position;
        ReactiveTarget target = null;
        Collider[] hitColliers = Physics.OverlapSphere(transform.position, shootRadius);
        foreach (Collider hitCollider in hitColliers)
        {
            try
            {
                target = hitCollider.gameObject.transform.parent.transform.GetComponent<ReactiveTarget>();
            }
            catch
            {

            }
            if (target != null)
            {
                Vector3 different = target.transform.position - transform.position;
                float distance = Mathf.Abs(different.magnitude);
                float k = Mathf.Pow(0.7f, distance);
                float curDamage = k * damage;
                target.Damage(curDamage);
                //return;
            }
            Rigidbody rigidbody = hitCollider.gameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.velocity = -transform.forward * force;
            }
        }
        Debug.Log("Norm destroy!: "+other.gameObject);
        Destroy(this.gameObject);
    }
}
