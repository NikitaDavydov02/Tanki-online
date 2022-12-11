using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    public int playerKill = 0;
    public int playerWasKilled = 0;
    public float timeOfGame = 60f;
    private float currentTimeOfGame;
    public float pause = 10f;
    private float currentPause;
    private int kristals = 0;
    public bool garage;
    public void Startup()
    {
        garage = false;
        Debug.Log("Game manager starting....");
        status = ManagerStatus.Started;
        currentTimeOfGame = timeOfGame;
        currentPause = pause;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (garage)
        {
            return;
        }
        if (currentTimeOfGame != 0)
            currentTimeOfGame -= Time.deltaTime;
        if (currentTimeOfGame == 0 && currentPause != 0)
        {
            currentPause -= Time.deltaTime;
            if (currentPause <= 0)
            {
                playerKill = 0;
                playerWasKilled = 0;
                currentPause = pause;
                currentTimeOfGame = timeOfGame;
                Messenger.Broadcast(GameEvent.GAME_STARTED);
                //Application.LoadLevel("Promzona");
                Managers.UserInterfaceManager.DontShowResult();
            }
        }
        if (currentTimeOfGame < 0)
        {
            currentTimeOfGame = 0;
            kristals = playerKill * 100;
            Messenger.Broadcast(GameEvent.GAME_OVER);
            Managers.UserInterfaceManager.ShowResult(playerKill, playerWasKilled, kristals);
            SaveManager.cristals += kristals;
        }
        Managers.UserInterfaceManager.ShowTime(currentTimeOfGame);
    }

    public void PlayerKill()
    {
        playerKill++;
        Managers.UserInterfaceManager.UpdateKillText();
    }

    public void PlayerKilled()
    {
        playerWasKilled++;
    }
}
