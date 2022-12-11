using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour {

    public float damage = 200f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            Debug.Log(hit.collider.gameObject);
            ReactiveTarget target = hit.collider.gameObject.transform.parent.GetComponent<ReactiveTarget>();
            if (target != null && hit.distance <= 1)
            {
                Debug.Log("Get!");
                target.Damage(damage);
                Destroy(this.gameObject);
            }
        }
	}
}
