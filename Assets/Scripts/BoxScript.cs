using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {
    public float speed = -0.01f;
    public BoxType type;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y > 1)
            transform.Translate(0, speed, 0);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WanderingAI>() != null)
        {
            return;
        }
        if (other.gameObject.GetComponent<PlayerAsReactiveTarget>())
        {
            Destroy(this.gameObject);
            if (type == BoxType.Acceleration)
            {
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_ACCELERATION);
                Managers.Inventory.AddDinamicInventroyItem(BoxType.Acceleration);
            }
            if (type == BoxType.DoubleBlow)
            {
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_DOUBLEBLOW);
                Managers.Inventory.AddDinamicInventroyItem(BoxType.DoubleBlow);
            }
            if (type == BoxType.DoubleArmor)
            {
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_DOUBLEARMOR);
                Managers.Inventory.AddDinamicInventroyItem(BoxType.DoubleArmor);
            }
            if (type == BoxType.Kit)
            {
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_KIT);
            }
        }
    }
}
