using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCameraScript : MonoBehaviour {
    public float rotSpeed = 5f;
    public float aheadSpeed = 5f;
    public float max;
    public float min;
    private float distance = 0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float horInput = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        transform.parent.transform.Rotate(0, horInput, 0);
        transform.LookAt(transform.parent.transform);
        float aheadInput = Input.GetAxis("Mouse ScrollWheel") * aheadSpeed * Time.deltaTime;
        distance += aheadInput;
        if (distance > min && distance < max)
            transform.Translate(0, 0, aheadInput);
        else
        {
            distance -= aheadInput;
        }
    }
}
