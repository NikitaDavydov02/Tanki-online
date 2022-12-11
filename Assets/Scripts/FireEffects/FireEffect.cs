﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour {
    public GameObject _flashGun;
    public GameObject _lightGun;
    [SerializeField]
    public GameObject fireGunPrefab;
    [SerializeField]
    public GameObject lightGunPrefab;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public virtual void Effect()
    {
        StartCoroutine(GunEffect());
    }

    public virtual IEnumerator GunEffect()
    {
        //_flashGun = Instantiate(fireGunPrefab) as GameObject;
        //_flashGun.transform.position = transform.position;
        //_lightGun = Instantiate(lightGunPrefab) as GameObject;
        //_lightGun.transform.position = transform.position;
        //yield return new WaitForSeconds(0.1f);
        //Destroy(_lightGun);
        //yield return new WaitForSeconds(3f);
        //Destroy(_flashGun);
        yield break;
    }
}
