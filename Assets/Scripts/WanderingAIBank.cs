using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAIBank : BaseTower {
    private float angle;
    //public float rotSpeed = 10f;
    private WanderingAI wanderingAI;
    [SerializeField]
    private ReactiveTarget enemyReactiveTarget;
	// Use this for initialization
	void Start () {
        angle = 0;
        wanderingAI = transform.parent.GetComponent<WanderingAI>();
	}
	
	// Update is called once per frame
	void Update () {
        if (angle != 0)
        {
            if (angle > 0)
            {
                transform.Rotate(0, rotSpeed * Time.deltaTime * enemyReactiveTarget.activity, 0);
                angle -= rotSpeed * Time.deltaTime;
                if (angle < 0)
                {
                    angle = 0;
                    wanderingAI.Stop(false);
                }
            }
            else
            {
                transform.Rotate(0, -rotSpeed * Time.deltaTime * enemyReactiveTarget.activity, 0);
                angle += rotSpeed * Time.deltaTime;
                if (angle > 0)
                {
                    angle = 0;
                    wanderingAI.Stop(false);
                }
            }
        }
        else
        {
            started = false;
            towerAudioSource.Stop();
        }
    }

    public void RotateBank(float angle)
    {
        if(this.angle == 0)
        {
            started = true;
            towerAudioSource.clip = towerClip;
            towerAudioSource.Play();
            if (angle > 180)
            {
                this.angle = -(360 - angle);
            }
            else
            {
                this.angle = angle;
            }
            wanderingAI.Stop(true);
        }
    }
}
