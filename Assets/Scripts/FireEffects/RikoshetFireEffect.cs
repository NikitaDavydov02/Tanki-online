using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikoshetFireEffect : FireEffect {

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
        Vector3 _rot = transform.parent.parent.eulerAngles;
        _rot.y -= 90f;
        _flashGun.transform.eulerAngles = _rot;
        yield break;
        //yield return new WaitForSeconds(15f);
        //Destroy(_flashGun);
    }
}
