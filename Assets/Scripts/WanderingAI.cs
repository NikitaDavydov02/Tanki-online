using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour {

    public float speed = 10f;
    public float rotSpeed = 10f;
    public float buffer = 5f;
    public float angle { get; private set; }
    private bool _stop = false;
    [SerializeField]
    AudioClip engineClip;
    [SerializeField]
    AudioSource engineAudioSource;
    ReactiveTarget rt;
    // Use this for initialization
    void Start () {
        angle = 0f;
        engineAudioSource.volume = 0.05f;
        engineAudioSource.clip = engineClip;
        engineAudioSource.Play();
        rt = GetComponent<ReactiveTarget>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!_stop)
        {
            //Debug.Log("I'm going. Angle: " + angle);
            if (angle == 0)
            {
                transform.Translate(0, 0, speed * rt.activity * Time.deltaTime);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 1f, out hit))
                {
                    //Debug.Log(hit.distance);
                    if (buffer >= hit.distance)
                    {
                        angle = Random.Range(-110, 110);
                    }
                }
            }
            else
            {
                if (angle > 0)
                {
                    transform.Rotate(0, rotSpeed * rt.activity * Time.deltaTime, 0);
                    angle -= rotSpeed * Time.deltaTime;
                    if (angle < 0)
                        angle = 0;
                }
                else
                {
                    transform.Rotate(0, -rotSpeed * rt.activity * Time.deltaTime, 0);
                    angle += rotSpeed * Time.deltaTime;
                    if (angle > 0)
                        angle = 0;
                }
            }
        }
	}

    public void Stop(bool stop)
    {
        _stop = stop;
    }
}
