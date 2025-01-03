using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    private List<BoxType> dinamicInventory = new List<BoxType>();
    public Dictionary<BoxType, int> staticInventory = new Dictionary<BoxType, int>();

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");
        status = ManagerStatus.Started;
    }

    // Use this for initialization
    void Start () {
        staticInventory.Add(BoxType.Kit, 100);
        staticInventory.Add(BoxType.DoubleArmor, 100);
        staticInventory.Add(BoxType.DoubleBlow, 100);
        staticInventory.Add(BoxType.Acceleration, 100);
        staticInventory.Add(BoxType.Mine, 100);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (staticInventory[BoxType.Kit] > 0)
            {
                RemoveStaticInventroyItem(BoxType.Kit);
                Managers.UserInterfaceManager.UpdateItem(BoxType.Kit, staticInventory[BoxType.Kit]);
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_KIT);
                //AddDinamicInventroyItem(BoxType.Kit);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (staticInventory[BoxType.DoubleArmor] > 0)
            {
                RemoveStaticInventroyItem(BoxType.DoubleArmor);
                Managers.UserInterfaceManager.UpdateItem(BoxType.DoubleArmor, staticInventory[BoxType.DoubleArmor]);
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_DOUBLEARMOR);
                AddDinamicInventroyItem(BoxType.DoubleArmor);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (staticInventory[BoxType.DoubleBlow] > 0)
            {
                RemoveStaticInventroyItem(BoxType.DoubleBlow);
                Managers.UserInterfaceManager.UpdateItem(BoxType.DoubleBlow, staticInventory[BoxType.DoubleBlow]);
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_DOUBLEBLOW);
                AddDinamicInventroyItem(BoxType.DoubleBlow);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (staticInventory[BoxType.Acceleration] > 0)
            {
                RemoveStaticInventroyItem(BoxType.Acceleration);
                Managers.UserInterfaceManager.UpdateItem(BoxType.Acceleration, staticInventory[BoxType.Acceleration]);
                Messenger.Broadcast(GameEvent.PLAYER_TAKE_ACCELERATION);
                AddDinamicInventroyItem(BoxType.Acceleration);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (staticInventory[BoxType.Mine] > 0)
            {
                RemoveStaticInventroyItem(BoxType.Mine);
                Managers.UserInterfaceManager.UpdateItem(BoxType.Mine, staticInventory[BoxType.Mine]);
                Messenger.Broadcast(GameEvent.PLAYER_PUT_MINE);
            }
        }
    }

    public void AddDinamicInventroyItem(BoxType item)
    {
        if (!dinamicInventory.Contains(item))
        {
            dinamicInventory.Add(item);
            Managers.UserInterfaceManager.ShowDinamicInventoryImage(item);
        }
    }
    public void RemoveDinamicInventroyItem(BoxType item)
    {
        if (dinamicInventory.Contains(item))
        {
            dinamicInventory.Remove(item);
            Managers.UserInterfaceManager.DontShowDinamicInventoryImage(item);
        }
    }

    public void AddStaticInventroyItem(BoxType item)
    {
        if (!staticInventory.ContainsKey(item))
        {
            staticInventory.Add(item, 1);
        }
        else
        {
            staticInventory[item]++;
        }
    }
    public void RemoveStaticInventroyItem(BoxType item)
    {
        if (staticInventory[item] > 0)
        {
            staticInventory[item]--;
        }
    }
}
