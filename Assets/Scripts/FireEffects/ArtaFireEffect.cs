using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtaFireEffect : FireEffect
{
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
        ArtaBullet artaBullet = _flashGun.GetComponent<ArtaBullet>();
        artaBullet.forward = transform.parent.transform.up;
        //Vector3 _rot = transform.parent.parent.eulerAngles;
        //_rot.y -= 90f;
        //Debug.Log(transform.parent.localEulerAngles.x);
        //_rot.x = transform.parent.localEulerAngles.x;
        //_flashGun.transform.eulerAngles = _rot;
        _flashGun.transform.eulerAngles = transform.parent.transform.eulerAngles;
        _lightGun = Instantiate(lightGunPrefab) as GameObject;
        _lightGun.transform.position = transform.position;
        yield return new WaitForSeconds(3f);
        Destroy(_lightGun);
        yield break;
    }
}
