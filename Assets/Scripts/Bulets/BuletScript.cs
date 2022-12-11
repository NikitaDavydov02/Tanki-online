using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuletScript : MonoBehaviour {
    public float speed = 1f;
    public float damage = 50f;
    public float force = 10f;
    public float shootRadius = 1f;
    public float timeOfLife = 2f;
    private float time = 0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        Debug.Log(time);
        if (time >= timeOfLife)
        {
            Debug.Log("Destroy!");
            Destroy(this.gameObject);
            return;
        }
        transform.Translate(speed * Time.deltaTime,0,0);
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
            Destroy(this.gameObject);
            return;
        }
    }
}
