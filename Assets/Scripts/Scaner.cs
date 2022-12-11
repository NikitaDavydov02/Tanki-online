using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour {

    public float stap = 1.0f;
    private float angle = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray xRay = new Ray(transform.position, transform.forward);
        RaycastHit xHit;
        if (Physics.Raycast(xRay, out xHit))
        {
            GameObject xHitObject = xHit.transform.gameObject;
            //if (xHitObject.transform.parent != null)
            //{
                //if (xHitObject.transform.parent.GetComponent<Player>())
                if (xHitObject.GetComponent<Player>())
                {
                    WanderingAIBank s = transform.parent.GetComponent<WanderingAIBank>();
                    if (s != null)
                    {
                        transform.localEulerAngles = new Vector3(0, 0, 0);
                        if (angle > 10 || angle < 10)
                        {
                            s.RotateBank(angle);
                            angle = 0;
                        }
                        //s.gameObject.transform.Rotate(0, angle, 0, Space.World);
                        //Debug.Log(s.gameObject.transform.localEulerAngles);
                    }
                }
            //}
        }
        if (angle > 360)
        {
            angle = 0;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        angle += stap;
        transform.Rotate(0, stap, 0);
    }
}
