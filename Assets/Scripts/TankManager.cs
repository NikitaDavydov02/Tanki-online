using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    private GameObject bank;
    private GameObject corpus;
    [SerializeField]
    private GameObject tank;
    public List<GameObject> banksPrefabs;
    public List<GameObject> corpusesPrefabs;
    public bool alive;
    [SerializeField]
    public PlayerAsReactiveTarget playerAsReactiveTarget;
    public void Startup()
    {
        Debug.Log("Tank mamager starting...");
        status = ManagerStatus.Started;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InstantiateTank(int gunIndex, int corpuseIndex)
    {
        //gunIndex = 10;
        //corpuseIndex = 6;
        GameObject lbank = Instantiate(banksPrefabs[gunIndex - 1]) as GameObject;
        lbank.transform.position = new Vector3(-40, 1.725f,-30);
        if (gunIndex == 10)
        {
            lbank.transform.position = new Vector3(-40, 1.526f, -30);
        }
        GameObject lcorpus = Instantiate(corpusesPrefabs[corpuseIndex - 1]) as GameObject;
        if (corpuseIndex == 6)
            lcorpus.transform.position = new Vector3(-40, 1f, -30.693f);
        else
            lcorpus.transform.position = new Vector3(-40, 1f, -30f);
        Debug.Log(lbank);
        Debug.Log(tank);
        lbank.transform.SetParent(tank.transform);
        lcorpus.transform.SetParent(tank.transform);
    }
}
