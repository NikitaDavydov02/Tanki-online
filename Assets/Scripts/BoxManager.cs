
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour, IGameManager {
    public float[] xSpawns;
    public float[] zSpawns;
    public float[] xGoldSpawns;
    public float[] zGoldSpawns;
    List<GameObject> boxes = new List<GameObject>();
    [SerializeField]
    GameObject acselerationBoxPrefab;
    [SerializeField]
    GameObject goldBoxPrefab;
    [SerializeField]
    GameObject doubleBlowBoxPrefab;
    [SerializeField]
    GameObject doubleArmorBoxPrefab;
    [SerializeField]
    GameObject kitBoxPrefab;
    public float time = 15;
    private float currentTime;
    private Dictionary<BoxType, float> collectedBoxes;
    public float activeTime = 30f;
    private bool timeToGlod = false;
    public float pauseBeforeGold = 45f;
    private float timeBeforeGold = 0;
    public int probabilityOfgold = 2000;

    public ManagerStatus status { get; private set; }

    // Use this for initialization
    void Start () {
        currentTime = 0;
        collectedBoxes = new Dictionary<BoxType, float>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Managers.Game.garage)
            return;
        float deltaTime = Time.deltaTime;
        if (timeBeforeGold != 0)
        {
            timeBeforeGold -= deltaTime;
            Debug.Log(timeBeforeGold);
            if (timeBeforeGold <= 0)
            {
                timeBeforeGold = -1;
            }
        }
        int v = Random.Range(0, probabilityOfgold);
        if (v == 1)
        {
            timeToGlod = true;
            Debug.Log("Time to gold!");
        }
        currentTime += deltaTime;
        int index = Random.Range(0, xSpawns.Length);
        bool canNotToFall = false;
        foreach (GameObject box1 in boxes)
        {
            if (box1 != null)
            {
                if (box1.transform.position.x == xSpawns[index] && box1.transform.position.z == zSpawns[index])
                {
                    canNotToFall = true;
                    break;
                }
            }
        }
        if (!canNotToFall && currentTime > time && !timeToGlod)
        {
            currentTime = 0;
            int indexOfType = Random.Range(0, 4);
            GameObject box = null;
            if (indexOfType == 0)
                box = Instantiate(acselerationBoxPrefab) as GameObject;
            if (indexOfType == 1)
                box = Instantiate(doubleBlowBoxPrefab) as GameObject;
            if(indexOfType == 2)
                box = Instantiate(doubleArmorBoxPrefab) as GameObject;
            if(indexOfType == 3)
                box = Instantiate(kitBoxPrefab) as GameObject;
            box.transform.position = new Vector3(xSpawns[index], 4, zSpawns[index]);
            boxes.Add(box);
        }
        for(int i = 0; i < 4; i++)
        {
            BoxType type = (BoxType)i;
            if (collectedBoxes.ContainsKey(type))
            {
                collectedBoxes[type] -= deltaTime;
                if (collectedBoxes[type] <= 0)
                {
                    collectedBoxes.Remove(type);
                    if (type == BoxType.Acceleration)
                    {
                        Messenger.Broadcast(GameEvent.PLAYER_ACCELERATION_DISACTIVE);
                        Managers.Inventory.RemoveDinamicInventroyItem(BoxType.Acceleration);
                    }
                    if (type == BoxType.DoubleBlow)
                    {
                        Messenger.Broadcast(GameEvent.PLAYER_DOUBLEBLOW_DISACTIVE);
                        Managers.Inventory.RemoveDinamicInventroyItem(BoxType.DoubleBlow);
                    }
                    if (type == BoxType.DoubleArmor)
                    {
                        Messenger.Broadcast(GameEvent.PLAYER_DOUBLEARMOR_DISACTIVE);
                        Managers.Inventory.RemoveDinamicInventroyItem(BoxType.DoubleArmor);
                    }
                }
            }
        }
        if (timeToGlod)
        {
            timeToGlod = false;
            Managers.Audio.PlayGold();
            Managers.UserInterfaceManager.ShowGold();
            timeBeforeGold = pauseBeforeGold;
        }
        if (timeBeforeGold == -1)
        {
            int goldIndex = Random.Range(0, xGoldSpawns.Length);
            GameObject goldBox = Instantiate(goldBoxPrefab) as GameObject;
            goldBox.transform.position = new Vector3(xGoldSpawns[goldIndex], 15, zGoldSpawns[goldIndex]);
            boxes.Add(goldBox);
            timeBeforeGold = 0;
        }
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_TAKE_ACCELERATION, SetAccelerationItemActive);
        Messenger.AddListener(GameEvent.PLAYER_TAKE_DOUBLEBLOW, SetDoubleBolwItemActive);
        Messenger.AddListener(GameEvent.PLAYER_TAKE_DOUBLEARMOR, SetDoubleArmorItemActive);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_ACCELERATION, SetAccelerationItemActive);
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_DOUBLEBLOW, SetDoubleBolwItemActive);
        Messenger.RemoveListener(GameEvent.PLAYER_TAKE_DOUBLEARMOR, SetDoubleArmorItemActive);
    }
    private void SetAccelerationItemActive()
    {
        if (collectedBoxes.ContainsKey(BoxType.Acceleration))
        {
            collectedBoxes[BoxType.Acceleration] = activeTime;
        }
        else
        {
            collectedBoxes.Add(BoxType.Acceleration, activeTime);
        }
    }
    private void SetDoubleBolwItemActive()
    {
        if (collectedBoxes.ContainsKey(BoxType.DoubleBlow))
        {
            collectedBoxes[BoxType.DoubleBlow] = activeTime;
        }
        else
        {
            collectedBoxes.Add(BoxType.DoubleBlow, activeTime);
        }
    }
    private void SetDoubleArmorItemActive()
    {
        if (collectedBoxes.ContainsKey(BoxType.DoubleArmor))
        {
            collectedBoxes[BoxType.DoubleArmor] = activeTime;
        }
        else
        {
            collectedBoxes.Add(BoxType.DoubleArmor, activeTime);
        }
    }

    public void Startup()
    {
        Debug.Log("Box manager starting...");
        status = ManagerStatus.Started;
    }
}
