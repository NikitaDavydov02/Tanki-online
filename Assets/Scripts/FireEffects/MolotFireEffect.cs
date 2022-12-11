using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotFireEffect : FireEffect {
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Effect()
    {
        StartCoroutine(GunEffect());
    }

    public override IEnumerator GunEffect()
    {
        _flashGun = Instantiate(fireGunPrefab) as GameObject;
        _flashGun.transform.position = transform.position;
        _lightGun = Instantiate(lightGunPrefab) as GameObject;
        _lightGun.transform.position = transform.position;
        Vector3 rot = transform.parent.parent.eulerAngles;
        _flashGun.transform.eulerAngles = rot;
        yield return new WaitForSeconds(0.1f);
        Destroy(_lightGun);
        Destroy(_flashGun);
    }
}
