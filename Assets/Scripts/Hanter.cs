using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Hanter : MonoBehaviour {
    private CharacterController _charController;
    [SerializeField]
    PlayerAsReactiveTarget reactiveTarget;
    public float speed = 10f;

	// Use this for initialization
	void Start () {
        _charController = GetComponent<CharacterController>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (reactiveTarget.alive)
        {
            float deltaZ = Input.GetAxis("Vertical");
            Vector3 movement;
            if (deltaZ != 0)
            {
                movement = new Vector3(0, 0, deltaZ);
                movement *= Time.deltaTime;
                movement *= speed;
                movement = Vector3.ClampMagnitude(movement, speed);
                movement = transform.TransformDirection(movement);
                //transform.Translate(movement);
                _charController.Move(movement);
            }
            float rotY = Input.GetAxis("Horizontal");
            transform.Rotate(0, rotY, 0);
        }
	}
}
