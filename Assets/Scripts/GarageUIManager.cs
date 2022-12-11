using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageUIManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    [SerializeField]
    Text discription;
    [SerializeField]
    Text title;
    [SerializeField]
    Image selectedImage;
    [SerializeField]
    Text cristalText;
    private int selectedItem = 1;
    private float selectedCoordinate = 89f;
    public List<string> discriptions = new List<string>();
    public List<string> titles = new List<string>();
    private GameObject bank;
    private GameObject corpus;
    public List<GameObject> banksPrefabs;
    public List<GameObject> corpusesPrefabs;
    private int selectedIsGun = -1;
    private int selectedIsCorpus = -1;
    private Vector3 bankCoordinates;
    private Dictionary<int, int> avaliableItems = new Dictionary<int, int>();

    public void Startup()
    {
        Debug.Log("GarageUIManager starting...");
        status = ManagerStatus.Started;
    }

    // Use this for initialization
    void Start () {
        discription.text = "";
        title.text = "";
        selectedIsGun = 1;
        Upgrade();
        selectedIsCorpus = 1;
        selectedIsGun = -1;
        Upgrade();
        selectedIsCorpus = -1;
        SaveManager.gunIndex = 1;
        SaveManager.corpuseIndex = 1;
        SaveManager.LoadGameState();
        //selectedIsGun = 0;
    }
	
	// Update is called once per frame
	void Update () {
        cristalText.text = SaveManager.cristals.ToString();
    }

    public void Click(int item)
    {
        float delta = (item - selectedItem) * 187;
        selectedItem = item;
        selectedImage.transform.Translate(delta, 0, 0);
        discription.text = discriptions[item - 1];
        title.text = titles[item - 1];
        if (selectedItem <= 9)
        {
            selectedIsGun = selectedItem;
            selectedIsCorpus = -1;
        }
        if (selectedItem > 9 && selectedItem <= 15)
        {
            selectedIsCorpus = selectedItem - 9;
            selectedIsGun = -1;
        }
        if (selectedItem == 16)
        {
            selectedIsGun = 10;
            selectedIsCorpus = -1;
            Debug.Log(selectedIsGun);
            Debug.Log(selectedIsCorpus);
        }
    }
    public void Upgrade()
    {
        if (selectedIsGun >= 0)
        {
            Destroy(bank);
            bank = Instantiate(banksPrefabs[selectedIsGun - 1]) as GameObject;
            bank.transform.position = new Vector3(0, 1.725f, 0);
            if (selectedIsGun == 10)
            {
                bank.transform.position = new Vector3(0, 1.526f, 0);
            }
            SaveManager.gunIndex = selectedIsGun;
        }
        if (selectedIsCorpus>=0)
        {
            Destroy(corpus);
            corpus = Instantiate(corpusesPrefabs[selectedIsCorpus - 1]) as GameObject;
            if (selectedIsCorpus == 6)
                corpus.transform.position = new Vector3(0, 1f, -0.693f);
            else
                corpus.transform.position = new Vector3(0, 1f, 0f);
            SaveManager.corpuseIndex = selectedIsCorpus;
        }
    }

    public void InButtle()
    {
        //Managers.Game.garage = false;
        Application.LoadLevel("Promzona");
    }
}
