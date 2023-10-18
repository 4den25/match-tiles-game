using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartLevel : MonoBehaviour
{
    public UnityEvent StartEvent;
    static bool isStartScreenMovingDown = false;
    static bool isStartScreenMovingUp = false;
    bool isTryAgainScreenMovingDown = false;
    bool isTryAgainScreenMovingUp = false;
    bool callTryAgainScreen = false;
    public static bool justLose = false;
    public static bool clearTileForLosing = false;
    [SerializeField] AudioClip startSound;

    CreateTile createTile;
    Timer timer;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (StartEvent == null)
        {
            StartEvent = new UnityEvent();
        }
        createTile = GameObject.FindGameObjectWithTag("Floor").GetComponent<CreateTile>();
        StartEvent.AddListener(createTile.StartGame);

        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.timer <= 0 && callTryAgainScreen == true)
        {
            callTryAgainScreen = false;
            MoveTryAgainScreenDown();
        }

        if (isStartScreenMovingUp == true)
        {
            GameObject.Find("StartScreen").transform.position = Vector3.MoveTowards(GameObject.Find("StartScreen").transform.position, new Vector3(-0.4f, 0.6f, 10f), 10f * Time.deltaTime);
        }
        if (Vector3.Distance(GameObject.Find("StartScreen").transform.position, new Vector3(-0.4f, 0.6f, 10f)) < 0.01f)
        {
            isStartScreenMovingUp = false;
        }

        if (isStartScreenMovingDown == true)
        {
            GameObject.Find("StartScreen").transform.position = Vector3.MoveTowards(GameObject.Find("StartScreen").transform.position, new Vector3(-0.4f, 0.6f, -0.3f), 10f * Time.deltaTime);
        }
        if (Vector3.Distance(GameObject.Find("StartScreen").transform.position, new Vector3(-0.4f, 0.6f, -0.3f)) < 0.01f)
        {
            isStartScreenMovingDown = false;
        }

        if (isTryAgainScreenMovingDown == true)
        {
            GameObject.Find("TryAgainScreen").transform.position = Vector3.MoveTowards(GameObject.Find("TryAgainScreen").transform.position, new Vector3(-0.4f, 0.6f, -0.3f), 10f * Time.deltaTime);
        }
        if (Vector3.Distance(GameObject.Find("TryAgainScreen").transform.position, new Vector3(-0.4f, 0.6f, -0.3f)) < 0.01f)
        {
            isTryAgainScreenMovingDown = false;
        }

        if (isTryAgainScreenMovingUp == true)
        {
            GameObject.Find("TryAgainScreen").transform.position = Vector3.MoveTowards(GameObject.Find("TryAgainScreen").transform.position, new Vector3(-0.4f, 0.6f, 10f), 10f * Time.deltaTime);
        }
        if (Vector3.Distance(GameObject.Find("TryAgainScreen").transform.position, new Vector3(-0.4f, 0.6f, 10f)) < 0.01f)
        {
            isTryAgainScreenMovingUp = false;
        }
    }

    void OnMouseUpAsButton()
    {
        audioSource.PlayOneShot(startSound);
        if (justLose == false)
        {
            MoveStartScreenUp();
        }
        else
        {
            MoveTryAgainScreenUp();
        }
        StartCoroutine(StartLevelCoroutine());
        timer.ResetTimer();
        callTryAgainScreen = true;
    }

    public void MoveStartScreenUp()
    {
        isStartScreenMovingUp = true;
    }

    public void MoveStartScreenDown()
    {
        StartCoroutine(MoveStartScreenDownCoroutine());
    }

    public IEnumerator MoveStartScreenDownCoroutine()
    {
        yield return new WaitForSeconds(2f);
        isStartScreenMovingDown = true;
    }

    public void MoveTryAgainScreenDown()
    {
        isTryAgainScreenMovingDown = true;
        justLose = true;
    }
    public void MoveTryAgainScreenUp()
    {
        foreach (var tile in createTile.tiles)
        {
            tile.transform.position = new Vector3(7, 0, 0);
        }
        foreach (var tile in SelectTile.selectedTiles)
        {
            tile.GetComponent<Tile>().isSelected = false;
        }
        foreach (var slot in SelectTile.slots)
        {
            slot.isEmpty = true;
        }
        SelectTile.selectedTiles.Clear();
        createTile.tiles.Clear();
        clearTileForLosing = true;

        //foreach (var slot in SelectTile.slots)
        //{
        //    slot.isEmpty = true;
        //}
        //foreach (var tile in createTile.tiles)
        //{
        //    Destroy(tile);
        //}
        isTryAgainScreenMovingUp = true;
        justLose = false;
    }

    IEnumerator StartLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Trigger");
        StartEvent.Invoke();
    }
}
