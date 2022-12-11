using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(BoxManager))]
[RequireComponent(typeof(UserInterfaceManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(EnemyManager))]
[RequireComponent(typeof(GarageUIManager))]
[RequireComponent(typeof(TankManager))]
public class Managers : MonoBehaviour {
    public static AudioManager Audio { get; private set; }
    public static BoxManager Box { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static GameManager Game { get; private set; }
    public static UserInterfaceManager UserInterfaceManager { get; private set; }
    public static EnemyManager Enemy { get; private set; }
    public static TankManager Tank { get; private set; }
    private List<IGameManager> _startSequence;
    void Awake(){
        Debug.Log("Awake!");
        //SaveManager.InstantiateTank();
        Audio = GetComponent<AudioManager>();
        Box = GetComponent<BoxManager>();
        UserInterfaceManager = GetComponent<UserInterfaceManager>();
        Inventory = GetComponent<InventoryManager>();
        Game = GetComponent<GameManager>();
        Enemy = GetComponent<EnemyManager>();
        Tank = GetComponent<TankManager>();
        _startSequence = new List<IGameManager>();
        _startSequence.Add(Audio);
        _startSequence.Add(Box);
        _startSequence.Add(Inventory);
        _startSequence.Add(Game);
        _startSequence.Add(UserInterfaceManager);
        _startSequence.Add(Enemy);
        _startSequence.Add(Tank);
        StartCoroutine(StartupManagers());
        Tank.InstantiateTank(SaveManager.gunIndex, SaveManager.corpuseIndex);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator StartupManagers()
    {
        Debug.Log("Startup!");
        foreach(IGameManager manager in _startSequence)
        {
            if(manager!=null)
                manager.Startup();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;
            foreach(IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }
            if (numReady > lastReady)
                Debug.Log("Progress: " + numReady + "/" + numModules);
            yield return null;
        }
        Debug.Log("All managers started up!");
        Messenger.Broadcast(GameEvent.NOGARAGE);
        Debug.Log("Broad");
    }
}
