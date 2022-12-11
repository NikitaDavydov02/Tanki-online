using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour {
    //[SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    public AudioSource towerAudioSource;
    [SerializeField]
    public AudioClip towerClip;
    public float rotSpeed = 10f;
    public float lastRot { get; private set; }
    private float _rotY;
    private bool alive;
    public bool started = false;
    public bool garage = true;
    // Use this for initialization
    void Start()
    {
        towerAudioSource.volume = 40;
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (garage)
            return;
        //
        if (!alive && reactiveTarget.alive)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        alive = reactiveTarget.alive;
        if (reactiveTarget.alive)
        {
            if (Managers.UserInterfaceManager.camArtaZoom && Managers.UserInterfaceManager.inArtaZoom)
            {
                Vector3 rot = transform.eulerAngles;
                rot.y = Managers.UserInterfaceManager.TowerAngle;
                transform.eulerAngles = rot;
            }
            if (Managers.UserInterfaceManager.inArtaZoom)
                return;
            float horInput = Input.GetAxis("Rotate");
            if (horInput != 0)
            {
                _rotY = horInput * rotSpeed;
            }
            else
            {
                _rotY = Input.GetAxis("Mouse X") * rotSpeed;
            }
            transform.Rotate(0, _rotY, 0);
            if(_rotY!=0&&!started)
            PlaySound();
            if (_rotY == 0 && started)
            {
                towerAudioSource.Stop();
                started = false;
            }
        }
    }
    public void NoGarage()
    {
        garage = false;
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
    }

    public void PlaySound()
    {
        if (_rotY != 0 && !started)
        {
            started = true;
            towerAudioSource.clip = towerClip;
            towerAudioSource.Play();
        }
    }
    void Awake()
    {
        Messenger.AddListener(GameEvent.NOGARAGE, NoGarage);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.NOGARAGE, NoGarage);
    }
}
