using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelsaFireEffect : FireEffect {

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
        _lightGun = Instantiate(lightGunPrefab) as GameObject;
        _lightGun.transform.position = transform.position;
        yield return new WaitForSeconds(1f);
        Destroy(_lightGun);
        _flashGun = Instantiate(fireGunPrefab) as GameObject;
        _flashGun.transform.position = transform.position;
        Debug.Log(transform.parent.parent.eulerAngles);
        Vector3 rot = transform.parent.parent.eulerAngles;
        //rot.x += 90f;
        _flashGun.transform.eulerAngles = rot;
        yield return new WaitForSeconds(1f);
        Destroy(_flashGun);
    }
}
