using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtaDamage : MonoBehaviour {
    public float damage = 100f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("rrrr");
        ReactiveTarget target = other.gameObject.transform.parent.GetComponent<ReactiveTarget>();
        if (target != null)
        {
            target.Damage(damage);
            //Destroy(this.gameObject);
        }
    }
}
