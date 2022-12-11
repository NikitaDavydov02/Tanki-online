using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    [SerializeField]
    AudioSource sound2D;
    [SerializeField]
    AudioClip goldClip;
    private float lastTime;
    public float goldTime = 25f;

    public void Startup()
    {
        Debug.Log("Audio manager starting...");
        status = ManagerStatus.Started;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (lastTime != 0)
        {
            lastTime -= Time.deltaTime;
            if (lastTime <= 0)
            {
                lastTime = 0;
                StopPlay();
            }
        }
	}

    public void PlayGold()
    {
        if (Managers.Game.garage)
            return;
        sound2D.clip = goldClip;
        sound2D.Play();
        lastTime = 25;
    }
    public void StopPlay()
    {
        if (Managers.Game.garage)
            return;
        sound2D.Stop();
    }
}
