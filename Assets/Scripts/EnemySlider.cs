using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlider : MonoBehaviour {
    Transform cam;
	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles = cam.localEulerAngles;
	}
}
