using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGun : MonoBehaviour {

    public float force = 10f;
    [SerializeField]
    public GameObject fireHitPrefab;
    [SerializeField]
    public GameObject lightHitPrefab;
    [SerializeField]
    public Slider rechargeSlider;
    [SerializeField]
    public AudioClip shootClip;
    [SerializeField]
    public AudioSource audioSource;
    [SerializeField]
    public AudioClip hitClip;
    public float timeOfRecharge = 5.0f;
    public float timeFromLastShot;
    public float damage = 50f;
    public float onceDamage;
    public float doubleDamage;
    //public GunType gunType;
    // Use this for initialization
    void Start()
    {
        timeFromLastShot = timeOfRecharge;
        onceDamage = damage;
        doubleDamage = onceDamage * 2;
        Debug.Log(onceDamage + "   " + doubleDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  virtual IEnumerator HitEffect(Vector3 pos)
    {
        return null;
    }

    public virtual IEnumerator GunEffect()
    {
        return null;
    }
    public void PlaySound(bool twince = false)
    {
        if (twince)
        {
            audioSource.PlayOneShot(shootClip);
            return;
        }
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(shootClip);
    }
    

    public void SendSound(ReactiveTarget target = null, PlayerAsReactiveTarget player = null)
    {
        if (target != null)
        {
            target.hitClip = hitClip;
        }
        if (player != null)
        {
            player.hitClip = hitClip;
        }
    }
}
