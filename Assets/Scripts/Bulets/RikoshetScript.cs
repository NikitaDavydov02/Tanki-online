using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikoshetScript : MonoBehaviour {
    public float speed = 1f;
    public float damage = 50f;
    public float force = 10f;
    public float shootRadius = 1f;
    public float timeOfLife = 2f;
    private float time = 0f;
    private Vector3 direction;
    [SerializeField]
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        direction = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= timeOfLife)
        {
            Destroy(this.gameObject);
            return;
        }
        //transform.Translate(speed * Time.deltaTime, 0, 0);
        if (direction.magnitude == 0)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance >= 1)
                return;
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
                //base.SendSound(reactiveTargetScript, null);
                reactiveTargetScript.Damage(damage);
                Collider[] hitColliers = Physics.OverlapSphere(hit.point, shootRadius);
                foreach (Collider hitCollider in hitColliers)
                {
                    Rigidbody rigidbody = hitCollider.gameObject.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.velocity = -hit.normal * force;
                    }
                }
                Destroy(this.gameObject);
            }
            else
            {
                if (direction.magnitude == 0)
                {
                    direction = ray.direction;
                }

                Vector3 normal = hit.normal;

                if(normal==new Vector3(0, 0, 1)|| normal == new Vector3(0, 0, -1))
                {
                    direction.z = -direction.z;
                }
                if (normal == new Vector3(1, 0, 0) || normal == new Vector3(-1, 0, 0))
                {
                    direction.x = -direction.x;
                }
                audioSource.Play();
            }
        }
    }
}
