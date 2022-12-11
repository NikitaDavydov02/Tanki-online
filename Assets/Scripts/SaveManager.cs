using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour {
    public static int gunIndex = 1;
    public static int corpuseIndex = 1;
    public static int cristals = 0;
    private static string filename;
    private static Dictionary<int, int> avaliableItems = new Dictionary<int, int>();

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
    }
	
	// Update is called once per frame
	void Update () {
	}

    public static void SaveGame()
    {
        Debug.Log("Saving:");
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("cristals", cristals);
        for (int i = 0; i < 16; i++)
        {
            if (!gamestate.ContainsKey((i + 1).ToString()) && avaliableItems.ContainsKey(i + 1))
            {
                gamestate.Add((i + 1).ToString(), avaliableItems[i + 1]);
            }
        }
        foreach(string s in gamestate.Keys)
        {
            Debug.Log(s + "," + gamestate[s]);
        }
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public static void LoadGameState()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("No save game.");
            return;
        }
        Debug.Log("Loading");
        Dictionary<string, object> gamestate;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();
        cristals = (int)gamestate["cristals"];
        Debug.Log("cristals," + cristals);
        avaliableItems = new Dictionary<int, int>();
        //Если хочешь по умолчанию раскоментируй это.
        //avaliableItems.Add(1, 0);
        //avaliableItems.Add(10, 0);
        for (int i = 0; i < 16; i++)
        {
            if(gamestate.ContainsKey((i + 1).ToString()) && !avaliableItems.ContainsKey(i + 1))
            {
                avaliableItems.Add(i + 1, (int)gamestate[(i + 1).ToString()]);
                Debug.Log(i + 1 + "," + avaliableItems[i + 1]);
            }
        }
    }
}
