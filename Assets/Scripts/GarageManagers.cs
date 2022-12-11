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
public class GarageManagers : MonoBehaviour {
    public static AudioManager Audio { get; private set; }
    public static BoxManager Box { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static GameManager Game { get; private set; }
    public static UserInterfaceManager UserInterfaceManager { get; private set; }
    public static EnemyManager Enemy { get; private set; }
    public static GarageUIManager GarageUserInterfaceManager { get; private set; }
    private List<IGameManager> _startSequence;
    void Awake()
    {
        Audio = GetComponent<AudioManager>();
        Box = GetComponent<BoxManager>();
        UserInterfaceManager = GetComponent<UserInterfaceManager>();
        Inventory = GetComponent<InventoryManager>();
        Game = GetComponent<GameManager>();
        Enemy = GetComponent<EnemyManager>();
        GarageUserInterfaceManager = GetComponent<GarageUIManager>();
        _startSequence = new List<IGameManager>();
        _startSequence.Add(Audio);
        _startSequence.Add(Box);
        _startSequence.Add(Inventory);
        _startSequence.Add(Game);
        _startSequence.Add(UserInterfaceManager);
        _startSequence.Add(Enemy);
        _startSequence.Add(GarageUserInterfaceManager);
        StartCoroutine(StartupManagers());
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            if (manager != null)
                manager.Startup();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;
            foreach (IGameManager manager in _startSequence)
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
    }
}
