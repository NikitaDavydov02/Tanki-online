using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField]
    Transform target;
    [SerializeField]
    PlayerAsReactiveTarget player;
    //[SerializeField]
    //Smoki smoki;
    public float[] xSpawnPoints;
    public float[] zSpawnPoints;
    public float[] ySpawnPoints;
    public float rotSpeed=2f;
    private float _rotY;
    private Vector3 _offset;
    public float RIPVertSpeed = 0.1f;
    public float RIPHorSpeed = 0.1f;
    public float spawnX;
    public float spawnZ;
    private bool goingUp;
    private bool goingDown;
    private bool goingHorizontal;
    private int index;
    private bool zoom = false;
    private bool artaZoom = false;
    private Vector3 _zoomOffset;
    public Vector3 movementToSpawnPoint;
    private float usual = 0;
    private Camera cam;
    [SerializeField]
    GameObject pricel;
    public float translateSpeed = 2f;

    private bool firstTime = false;
    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        usual = cam.fieldOfView;
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position;
        transform.Translate(new Vector3(0, -3.25f, 8.31f));
        _zoomOffset = target.position - transform.position;
        transform.Translate(new Vector3(0, 3.25f, -8.31f));
        Debug.Log(_zoomOffset);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!zoom && Managers.UserInterfaceManager.camZoom && !Managers.UserInterfaceManager.camArtaZoom)
            {
                InZoom();
            }
            else
            {
                OutZoom();
            }
            if (!artaZoom && Managers.UserInterfaceManager.camArtaZoom)
            {
                InArtaZoom();
            }
            else
            {
                OutArtaZoom();
            }
        }
	}

    public void OutZoom()
    {
        Managers.UserInterfaceManager.ChangeZoom(false);
        zoom = false;
        cam.fieldOfView = usual;
        Vector3 rot = transform.localEulerAngles;
        float y = transform.localEulerAngles.y;
        transform.localEulerAngles = new Vector3(0, y, 0);
        transform.Translate(new Vector3(0, 3.25f, -8.31f));
        Managers.UserInterfaceManager.DontShowPricel();
    }

    public void InZoom()
    {
        Debug.Log("Zoom1");
        Managers.UserInterfaceManager.ChangeZoom(true);
        zoom = true;
        cam.fieldOfView = 20;
        //Vector3 rot = transform.localEulerAngles;
        //float y = transform.localEulerAngles.y;
        //transform.localEulerAngles = new Vector3(0, y, 0);
        //transform.Translate(new Vector3(0, -3.25f, 8.31f));
        //Debug.Log(transform.localEulerAngles);
        //Managers.UserInterfaceManager.ShowPricel();
        //Debug.Log("RotY: " + _rotY);
    }

    public void InArtaZoom()
    {
        Managers.UserInterfaceManager.ChangeArtaZoom(true);
        artaZoom = true;
        Vector3 rot = transform.localEulerAngles;
        Vector3 pos = Managers.UserInterfaceManager.currentPricelPoint;
        Managers.UserInterfaceManager.targetPricelPoint = Managers.UserInterfaceManager.currentPricelPoint;
        pos.y = 25;
        transform.position = pos;
        transform.localEulerAngles = new Vector3(90, 0, 0);
        Managers.UserInterfaceManager.ShowPricel();
    }

    public void OutArtaZoom()
    {
        Managers.UserInterfaceManager.ChangeArtaZoom(false);
        artaZoom = false;
        Managers.UserInterfaceManager.DontShowPricel();
        _rotY = Managers.UserInterfaceManager.TowerAngle;
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
        Managers.UserInterfaceManager.DontShowLine();
    }

    void LateUpdate()
    {
        Debug.Log(transform.localEulerAngles);
        if (!player.alive)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            //Debug.Log(goingUp);
            if (goingUp)
            {
                transform.position = new Vector3(x, y + (RIPVertSpeed * Time.deltaTime), z);
                if (transform.position.y >= 10)
                {
                    goingUp = false;
                    goingDown = false;
                    goingHorizontal = true;
                }
            }
            if (goingHorizontal)
            {
                //Debug.Log(movementToSpawnPoint);
                transform.position = new Vector3(x + (movementToSpawnPoint.x * RIPHorSpeed * Time.deltaTime), y, z + (movementToSpawnPoint.z * RIPHorSpeed * Time.deltaTime));
                if (TimeToStop())
                {
                    transform.position = new Vector3(xSpawnPoints[index], y, zSpawnPoints[index]);
                    goingUp = false;
                    goingDown = true;
                    goingHorizontal = false;
                }
            }
            if (goingDown)
            {
                transform.position = new Vector3(x, y + (-RIPVertSpeed * Time.deltaTime), z);
                if (transform.position.y <= ySpawnPoints[index])
                {
                    transform.position = new Vector3(xSpawnPoints[index], ySpawnPoints[index], zSpawnPoints[index]);
                    goingUp = false;
                    goingDown = false;
                    goingHorizontal = false;
                    player.ReplaceTank();
                    _rotY = transform.eulerAngles.y;
                    _offset = target.position - transform.position;
                }
            }
            if (Mathf.Abs(transform.localEulerAngles.y) > 1)
            {
                float rotX = transform.localEulerAngles.x;
                float rotZ = transform.localEulerAngles.z;
                float rotY;
                if (transform.localEulerAngles.y > 180)
                   rotY = transform.localEulerAngles.y + 1f;
                else
                    rotY = transform.localEulerAngles.y - 1f;
                transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);
            }
            return;
        }
        //
        if (!artaZoom)
        {
            Debug.Log("RotBeforeY: " + _rotY);
            float horInput = Input.GetAxis("Rotate");
            if (horInput != 0)
            {
                _rotY += horInput * rotSpeed;
            }
            else
            {
                _rotY += Input.GetAxis("Mouse X") * rotSpeed;
            }
            _rotY += Input.GetAxis("Horizontal");
            Debug.Log("RotY: " + _rotY);
        }
        if (!zoom && !artaZoom)
        {
            Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
            transform.position = target.position - (rotation * _offset);
            transform.LookAt(target);
        }
        else
        {
            if (zoom)
            {
                Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
                transform.position = target.position - (rotation * _zoomOffset);
                transform.LookAt(target);
                //transform.Rotate(-5.35f, 180, 0);

                //Vector3 rot = transform.localEulerAngles;
                //rot.z = 0;
                //transform.localEulerAngles = rot;
                //transform.localEulerAngles = target.eulerAngles;
            }
            if (artaZoom)
            {
                float vertInput = Input.GetAxis("Mouse Y");
                float horInput = Input.GetAxis("Mouse X");
                Vector3 movement = new Vector3(horInput, vertInput, 0);
                transform.Translate(movement);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    Managers.UserInterfaceManager.targetPricelPoint = hit.point;
                    //Debug.Log(hit.point);
                }
            }
        }
        if (Managers.UserInterfaceManager.camArtaZoom)
        {
            pricel.transform.position = Managers.UserInterfaceManager.currentPricelPoint;
        }
        //
    }

    private bool TimeToStop()
    {
        if (Mathf.Abs(spawnX - transform.position.x) <= 0.5 && Mathf.Abs(spawnZ - transform.position.z) <= 0.5)
        {
            return true;
        }
        return false;
    }

    public void Destroy(int index)
    {
        if (artaZoom)
        {
            OutArtaZoom();
        }
        this.index = index;
        spawnX = xSpawnPoints[index];
        spawnZ = zSpawnPoints[index];
        movementToSpawnPoint = new Vector3(spawnX - transform.position.x, 0, spawnZ - transform.position.z);
        _offset = new Vector3(0, 0, 0);
        _rotY = transform.eulerAngles.y;
        Debug.Log(movementToSpawnPoint);
        goingUp = true;
        goingDown = false;
        goingHorizontal = false;
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.SHOOT, OutZoom);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SHOOT, OutZoom);
    }
}
