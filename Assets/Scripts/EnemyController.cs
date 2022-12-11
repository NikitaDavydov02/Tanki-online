using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    GameObject enemyPrefab;
    private GameObject[] enemies;
    public int count = 2;
    private int realCount = 0;
    public float[] xCoordinatesOfSpawn;
    public float[] zCoordinatesOfSpawn;
    // Use this for initialization
    void Start () {
  //      enemies = new GameObject[count];
		//for(int i = 0; i < count; i++)
  //      {
  //          GameObject enemy = Instantiate(enemyPrefab) as GameObject;
  //          int index = Random.Range(0, xCoordinatesOfSpawn.Length - 1);
  //          enemy.transform.position = new Vector3(xCoordinatesOfSpawn[index], 1.13f, zCoordinatesOfSpawn[index]);
  //          enemies[i] = enemy;
  //      }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void EnemyDied()
    {
    }
}
