using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCorpus : MonoBehaviour {
    private CharacterController _charController;
    PlayerAsReactiveTarget reactiveTarget;
    [SerializeField]
    AudioClip engineClip;
    [SerializeField]
    AudioSource engineAudioSource;
    [SerializeField]
    GameObject minePrefab;
    private GameObject mine;
    public float speed = 10f;
    private bool alive;
    public float gravity = -9.8f;
    private float onceSpped;
    private float doubleSpeed;
    public float Health = 200f;
    public bool garage { get; private set; }
    //public float rotSpeed = 10f;

    // Use this for initialization
    void Start()
    {
        //\/1
        //reactiveTarget = Managers.Tank.playerAsReactiveTarget;
        garage = true;
        if (garage)
            return;
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (garage)
            return;
        if (!alive && Managers.Tank.alive)
        {
            transform.parent.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        alive = Managers.Tank.alive;
        if (Managers.Tank.alive)
        {
            if (Managers.UserInterfaceManager.inArtaZoom)
                return;
            float deltaZ = Input.GetAxis("Vertical");
            Vector3 movement;
            if (deltaZ != 0)
            {
                movement = new Vector3(0, gravity, deltaZ);
                movement *= Time.deltaTime;
                movement *= speed;
                movement = Vector3.ClampMagnitude(movement, speed);
                movement = transform.TransformDirection(movement);
                //transform.Translate(movement);
                if(!Managers.UserInterfaceManager.inZoom)
                    _charController.Move(movement);
            }
            float rotY = Input.GetAxis("Horizontal");
            transform.parent.transform.Rotate(0, rotY, 0);
        }
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_TAKE_ACCELERATION, SetAccelerationActive);
        Messenger.AddListener(GameEvent.PLAYER_ACCELERATION_DISACTIVE, SetAccelerationDisactive);
        Messenger.AddListener(GameEvent.PLAYER_PUT_MINE, PutMine);
        Messenger.AddListener(GameEvent.NOGARAGE, NoGarage);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_ACCELERATION, SetAccelerationActive);
        Messenger.RemoveListener(GameEvent.PLAYER_ACCELERATION_DISACTIVE, SetAccelerationDisactive);
        Messenger.RemoveListener(GameEvent.PLAYER_PUT_MINE, PutMine);
        Messenger.RemoveListener(GameEvent.NOGARAGE, NoGarage);
    }
    public void NoGarage()
    {
        Debug.Log("n");
        garage = false;
        //PlayerAsReactiveTarget rt = Managers.Tank.playerAsReactiveTarget;\
        reactiveTarget = Managers.Tank.playerAsReactiveTarget;
        Debug.Log(reactiveTarget);
        if (reactiveTarget != null)
        {
            reactiveTarget.maxHealth = Health;
            reactiveTarget.health = Health;
        }

        _charController = transform.parent.GetComponent<CharacterController>();
        engineAudioSource.clip = engineClip;
        engineAudioSource.Play();
        onceSpped = speed;
        doubleSpeed = onceSpped * 2;
    }
    private void SetAccelerationActive()
    {
        speed = doubleSpeed;
    }
    private void SetAccelerationDisactive()
    {
        speed = onceSpped;
    }

    private void PutMine()
    {
        mine = Instantiate(minePrefab) as GameObject;
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        float z = gameObject.transform.position.z;
        mine.transform.position = new Vector3(x, y - 0.35f, z);
    }
}
