using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour {
    public float damage = 25f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEntered(Collider other)
    {
        Debug.Log("Collider!");
        ReactiveTarget target = other.gameObject.transform.parent.GetComponent<ReactiveTarget>();
        if (target != null)
        {
            Debug.Log("Enemy!");
        }
        else
        {
            Debug.Log("No!");
        }
        Destroy(this.gameObject);
    }
}
