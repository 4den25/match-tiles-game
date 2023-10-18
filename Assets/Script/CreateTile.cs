using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CreateTile : MonoBehaviour
{
    StartLevel startLevel;
    public int currentLevel = 1;
    [SerializeField] public TMP_Text level;

    #region Tiles Prefab
    [SerializeField] public GameObject zeroonePrefab;
    [SerializeField] public GameObject zombiePrefab;
    [SerializeField] public GameObject arkonePrefab;
    [SerializeField] public GameObject saberPrefab;
    [SerializeField] public GameObject reviPrefab;
    [SerializeField] public GameObject vicePrefab;
    [SerializeField] public GameObject geatPrefab;
    [SerializeField] public GameObject gotchardPrefab;

    [SerializeField] public GameObject kuugaPrefab;
    [SerializeField] public GameObject agitoPrefab;
    [SerializeField] public GameObject ryukiPrefab;
    [SerializeField] public GameObject faizPrefab;
    [SerializeField] public GameObject bladePrefab;
    [SerializeField] public GameObject hibikiPrefab;
    [SerializeField] public GameObject kabutoPrefab;
    [SerializeField] public GameObject denoPrefab;
    [SerializeField] public GameObject kivaPrefab;
    [SerializeField] public GameObject decadePrefab;

    [SerializeField] public GameObject wPrefab;
    [SerializeField] public GameObject oooPrefab;
    [SerializeField] public GameObject fourzePrefab;
    [SerializeField] public GameObject wizardPrefab;
    [SerializeField] public GameObject gaimPrefab;
    [SerializeField] public GameObject drivePrefab;
    [SerializeField] public GameObject ghostPrefab;
    [SerializeField] public GameObject exaidPrefab;
    [SerializeField] public GameObject buildPrefab;
    [SerializeField] public GameObject zioPrefab;
    #endregion

    public List<GameObject> tiles = new List<GameObject>(); //list những cục đã tạo
    GameObject tile;
    bool createTileYet = false;
    Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        startLevel = GameObject.FindGameObjectWithTag("StartBtn").GetComponent<StartLevel>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    public void StartGame()
    {
        timer.UnFreezeTimer();
        //load file level vào tileNames, bắt đầu tạo cục
        string[] tileNames = File.ReadAllLines(CreateLevelFilePath(currentLevel));
        InstantiateTiles(tileNames);
        createTileYet = true;
        StartLevel.clearTileForLosing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //startLevel.StartEvent.AddListener(StartGame);
        //sau khi tạo cục, set lại rotation của cục để cục k nằm úp mặt xuống
        if (timer.timer > 85)
        {
            foreach (var tile in tiles)
            {
                tile.GetComponent<Transform>().rotation = new Quaternion(tile.transform.rotation.x, tile.transform.rotation.y, tile.transform.rotation.z, Quaternion.identity.w);
            }
        }

        //nếu số cục đã hủy = số cục đã tạo, load màn mới
        if (tiles.Count == 0 && createTileYet == true && StartLevel.clearTileForLosing == false)
        {
            createTileYet = false;
            Debug.Log("tong " + tiles.Count);
            timer.FreezeTimer();
            Debug.Log("You win");
            LoadNewLevel();
            startLevel.MoveStartScreenDown();
        }
    }

    //tạo đường dẫn tới file level
    string CreateLevelFilePath(int i)
    {
        string levelFilePath = "Assets/Resources/Level" + i.ToString() + ".txt";
        return levelFilePath;
    }

    //load level mới
    void LoadNewLevel()
    {
        if (currentLevel < 3)
        {
            tiles.Clear();
            currentLevel++;
            level.text = "Level " + currentLevel;
        }
        else
        {
            Debug.Log("Finish the game");
        }
    }

    public double GetRandomNumber(double minimum, double maximum)
    {
        var random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    //tạo các cục
    public void InstantiateTiles(string[] tileNames)
    {
        foreach (var tileName in tileNames)
        {
            if (tileName == "Zeroone")
            {
                CreateThreeTiles(zeroonePrefab);
            }
            else if (tileName == "Zombie")
            {
                CreateThreeTiles(zombiePrefab);
            }
            else if (tileName == "Arkone")
            {
                CreateThreeTiles(arkonePrefab);
            }
            else if (tileName == "Saber")
            {
                CreateThreeTiles(saberPrefab);
            }
            else if (tileName == "Revi")
            {
                CreateThreeTiles(reviPrefab);
            }
            else if (tileName == "Vice")
            {
                CreateThreeTiles(vicePrefab);
            }
            else if (tileName == "Geat")
            {
                CreateThreeTiles(geatPrefab);
            }
            else if (tileName == "Gotchard")
            {
                CreateThreeTiles(gotchardPrefab);
            }
            else if (tileName == "Kuuga")
            {
                CreateThreeTiles(kuugaPrefab);
            }
            else if (tileName == "Agito")
            {
                CreateThreeTiles(agitoPrefab);
            }
            else if (tileName == "Ryuki")
            {
                CreateThreeTiles(ryukiPrefab);
            }
            else if (tileName == "Faiz")
            {
                CreateThreeTiles(faizPrefab);
            }
            else if (tileName == "Blade")
            {
                CreateThreeTiles(bladePrefab);
            }
            else if (tileName == "Hibiki")
            {
                CreateThreeTiles(hibikiPrefab);
            }
            else if (tileName == "Kabuto")
            {
                CreateThreeTiles(kabutoPrefab);
            }
            else if (tileName == "Deno")
            {
                CreateThreeTiles(denoPrefab);
            }
            else if (tileName == "Kiva")
            {
                CreateThreeTiles(kivaPrefab);
            }
            else if (tileName == "Decade")
            {
                CreateThreeTiles(decadePrefab);
            }
            else if (tileName == "W")
            {
                CreateThreeTiles(wPrefab);
            }
            else if (tileName == "OOO")
            {
                CreateThreeTiles(oooPrefab);
            }
            else if (tileName == "Fourze")
            {
                CreateThreeTiles(fourzePrefab);
            }
            else if (tileName == "Wizard")
            {
                CreateThreeTiles(wizardPrefab);
            }
            else if (tileName == "Gaim")
            {
                CreateThreeTiles(gaimPrefab);
            }
            else if (tileName == "Drive")
            {
                CreateThreeTiles(drivePrefab);
            }
            else if (tileName == "Ghost")
            {
                CreateThreeTiles(ghostPrefab);
            }
            else if (tileName == "Exaid")
            {
                CreateThreeTiles(exaidPrefab);
            }
            else if (tileName == "Build")
            {
                CreateThreeTiles(buildPrefab);
            }
            else if (tileName == "Zio")
            {
                CreateThreeTiles(zioPrefab);
            }
        }
    }

    //tạo ba cục cho một tên trong file level và add cục vào list cục đã tạo
    public void CreateThreeTiles(GameObject prefab)
    {
        for (int i = 0; i < 3; i++)
        {
            tile = Instantiate(prefab, new Vector3((float)GetRandomNumber(-1.4, 1.4), 1, (float)GetRandomNumber(-2, 2.35)), Quaternion.identity);
            tiles.Add(tile);
        }
    }

    //public void RemoveDestroyedTile(GameObject tile)
    //{
    //    tiles.Remove(tile);
    //}
}
