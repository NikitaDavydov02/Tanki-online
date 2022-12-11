using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBullet : MonoBehaviour {
    public float speed = 1f;
    public Vector3 forward;
    private Vector3 curSpeed;
    public float gravitation = -20f;
    // Use this for initialization
    void Start () {
        curSpeed = forward * speed;
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y <= 0.5)
        {
            Destroy(this.gameObject);
            return;
        }
        Vector3 pos = transform.position;
        pos += curSpeed * Time.deltaTime;
        curSpeed.y += gravitation * Time.deltaTime;
        transform.position = pos;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() || other.gameObject.GetComponent<Arta>() || other.gameObject.GetComponent<ArtaBullet>())
        {
            return;
        }
        Vector3 d = Managers.UserInterfaceManager.currentPricelPoint - transform.position;
        if (Mathf.Abs(d.magnitude) > 2)
        {
            Managers.UserInterfaceManager.currentPricelPoint = transform.position;
        }
        Destroy(this.gameObject);
    }
}
