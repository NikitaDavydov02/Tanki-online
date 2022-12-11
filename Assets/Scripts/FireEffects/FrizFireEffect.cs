using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrizFireEffect : FireEffect {

    // Use this for initialization
    void Start()
    {
        _lightGun = Instantiate(lightGunPrefab) as GameObject;
        _lightGun.transform.position = transform.position;
        _flashGun = Instantiate(fireGunPrefab) as GameObject;
        _flashGun.transform.position = transform.position;
        _lightGun.SetActive(false);
        _flashGun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Effect()
    {
        _lightGun.transform.position = transform.position;
        _flashGun.transform.position = transform.position;
        Vector3 rot = transform.parent.parent.eulerAngles;
        _flashGun.transform.eulerAngles = rot;
        _lightGun.SetActive(true);
        _flashGun.SetActive(true);
    }

    public void StopEffect()
    {
        _lightGun.SetActive(false);
        _flashGun.SetActive(false);
    }
}
