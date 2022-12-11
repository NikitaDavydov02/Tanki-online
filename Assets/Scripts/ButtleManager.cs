using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManager : MonoBehaviour {
    public static int enemyCount { get; private set; }
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        enemyCount = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewEnemyCount(int count)
    {
        enemyCount = count;
    }
}
