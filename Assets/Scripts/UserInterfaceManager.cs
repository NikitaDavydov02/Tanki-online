using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    [SerializeField]
    Text goldText;
    [SerializeField]
    Image kitImage;
    [SerializeField]
    Image doubleBlowImage;
    [SerializeField]
    Image doubleArmorImage;
    [SerializeField]
    Image accelerationImage;

    [SerializeField]
    Text kitStaticText;
    [SerializeField]
    Text doubleBlowStaticText;
    [SerializeField]
    Text doubleArmorStaticText;
    [SerializeField]
    Text accelerationStaticText;
    [SerializeField]
    Text mineStaticText;
    [SerializeField]
    Text killText;
    [SerializeField]
    Text timeText;
    [SerializeField]
    Image gameResultFone;
    [SerializeField]
    Text killResultText;
    [SerializeField]
    Text killedResultText;
    [SerializeField]
    Text kristalsResultText;
    [SerializeField]
    Image destroyImage;
    [SerializeField]
    Text destroyText;
    private float lastTime;
    public float goldTime = 25f;
    private int kill = 0;
    [SerializeField]
    Image pricel;
    [SerializeField]
    public Slider rechargeSlider;
    [SerializeField]
    Transform greenLine;
    [SerializeField]
    Text cristalText;
    public float maxValue { get; private set; }

    public bool camZoom=false;
    public bool camArtaZoom = false;
    public bool inZoom { get; private set; }
    public bool inArtaZoom { get; private set; }
    public float deltaAngle { get; private set; }
    public Vector3 currentPricelPoint;
    public Vector3 targetPricelPoint;
    public float TowerAngle = 0;

    public void Startup()
    {
        maxValue = rechargeSlider.maxValue;
        inZoom = false;
        Debug.Log("UIManager stating...");
        status = ManagerStatus.Started;
        if (Managers.Game.garage)
            return;
        goldText.gameObject.SetActive(false);
        accelerationImage.gameObject.SetActive(false);
        kitImage.gameObject.SetActive(false);
        doubleArmorImage.gameObject.SetActive(false);
        doubleBlowImage.gameObject.SetActive(false);
        destroyImage.gameObject.SetActive(false);
        pricel.gameObject.SetActive(false);
        greenLine.gameObject.SetActive(false);
        DontShowResult();
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
                DontShowGold();
            }
        }
        cristalText.text = SaveManager.cristals.ToString();
    }

    public void ShowGold()
    {
        goldText.gameObject.SetActive(true);
        lastTime = 25;
    }
    public void DontShowGold()
    {
        goldText.gameObject.SetActive(false);
    }

    public void ShowDinamicInventoryImage(BoxType image)
    {
        if (image == BoxType.Acceleration)
            accelerationImage.gameObject.SetActive(true);
        else if (image == BoxType.Kit)
            kitImage.gameObject.SetActive(true);
        else if (image == BoxType.DoubleArmor)
            doubleArmorImage.gameObject.SetActive(true);
        else
            doubleBlowImage.gameObject.SetActive(true);
    }
    public void DontShowDinamicInventoryImage(BoxType image)
    {
        if (image == BoxType.Acceleration)
            accelerationImage.gameObject.SetActive(false);
        else if (image == BoxType.Kit)
            kitImage.gameObject.SetActive(false);
        else if (image == BoxType.DoubleArmor)
            doubleArmorImage.gameObject.SetActive(false);
        else
            doubleBlowImage.gameObject.SetActive(false);
    }

    public void UpdateItem(BoxType item, int count)
    {
        if (item == BoxType.Kit)
        {
            kitStaticText.text = count.ToString();
        }
        else if (item == BoxType.DoubleArmor)
        {
            doubleArmorStaticText.text = count.ToString();
        }
        else if (item == BoxType.DoubleBlow)
        {
            doubleBlowStaticText.text = count.ToString();
        }
        else if (item == BoxType.Acceleration)
        {
            accelerationStaticText.text = count.ToString();
        }
        else if (item == BoxType.Mine)
        {
            mineStaticText.text = count.ToString();
        }
    }
    
    public void UpdateKillText()
    {
        kill++;
        killText.text = kill.ToString();
    }

    public void ShowTime(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds - (minutes * 60);
        timeText.text = minutes + ":" + seconds;
    }

    public void ShowResult(int kill, int killed, int kristals)
    {
        if (Managers.Game.garage)
            return;
        gameResultFone.gameObject.SetActive(true);
        killResultText.text = kill.ToString();
        killedResultText.text = killed.ToString();
        kristalsResultText.text = kristals.ToString();
    }

    public void DontShowResult()
    {
        gameResultFone.gameObject.SetActive(false);
    }

    public void Destroy(bool show, float time)
    {
        if (show)
        {
            if (!destroyImage.gameObject.active)
            {
                destroyImage.gameObject.SetActive(true);
            }
            int minutes = (int)time / 60;
            int seconds = (int)time - (minutes * 60);
            destroyText.text = "До самоуничтожения" + "\r\n" + "осталось:\r\n" + minutes + ":" + "0" + seconds;
        }
        else
        {
            destroyImage.gameObject.SetActive(false);
        }
    }

    public void ShowPricel()
    {
        pricel.gameObject.SetActive(true);
        Debug.Log("Set");
    }

    public void DontShowPricel()
    {
        pricel.gameObject.SetActive(false);
    }

    public void ChangeZoom(bool value)
    {
        inZoom = value;
    }

    public void ChangeArtaZoom(bool value)
    {
        inArtaZoom = value;
    }
    public void ChangeDeltaAngle(float value)
    {
        deltaAngle = value;
    }
    // private void TrueGarage()
    //{
    //    garage = true;
    //}
    void Awake()
    {
        //Messenger.AddListener(GameEvent.GARAGE, TrueGarage);
    }
    void OnDerstroy()
    {
        //Messenger.RemoveListener(GameEvent.GARAGE, TrueGarage);
    }

    public void UpdateRechargeSlider(float value)
    {
        rechargeSlider.value = value;
    }

    public void NavigateToGarage()
    {
        SaveManager.SaveGame();
        Application.LoadLevel("Garage");
    }

    public void ShowLine(float l, Vector3 pos, float angle)
    {
        Vector3 scale = greenLine.transform.localScale;
        scale.z = l;
        greenLine.localScale = scale;
        Vector3 rot = greenLine.localEulerAngles;
        rot.y = angle;
        greenLine.transform.localEulerAngles = rot;
        greenLine.transform.position = pos;
        greenLine.gameObject.SetActive(true);
    }
    public void DontShowLine()
    {
        greenLine.gameObject.SetActive(false);
    }
}
