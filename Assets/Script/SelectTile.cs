using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class SelectTile : MonoBehaviour
{
    //public static int destroyedTilesCount;
    public static List<GameObject> selectedTiles = new List<GameObject>();
    List<bool> needToSort = new List<bool>() { false, false, false, false, false, false, false };

    public Vector3 newNextTarget;
    Vector3 scaleChange = new Vector3(-0.04f, -0.02f, -0.04f);
    public static List<Slots> slots = new List<Slots>()
    {
        new Slots()
        {
            target = new Vector3(-1.65f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 0,
        },
        new Slots()
        {
            target = new Vector3(-1.10f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 1,
        },
        new Slots()
        {
            target = new Vector3(-0.55f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 2,
        },
        new Slots()
        {
            target = new Vector3(0f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 3,
        },
        new Slots()
        {
            target = new Vector3(0.55f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 4,
        },
        new Slots()
        {
            target = new Vector3(1.1f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 5,
        },
        new Slots()
        {
            target = new Vector3(1.65f, 0.2f, -3.2f),
            isEmpty = true,
            slotIndex = 6,
        },
    };
    bool isMovingToSelectedBar = false;
    bool isMovingOutSelectedBar = false;
    Vector3 backToTable;
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip matchSound;

    CreateTile createTile;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        createTile = GameObject.FindGameObjectWithTag("Floor").GetComponent<CreateTile>();
    }


    // Update is called once per frame
    void Update()
    {
        //ghép được 3 cái phá nó đi thì sort lại mấy cục dưới bar
        //move 1 cái ra khỏi bar cũng sort lại
        if (selectedTiles.Count != 0)
        {
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                if (needToSort[i] == true)
                {
                    SortTiles(i);
                }
            }
        }

        //cái này không move theo kiểu sort đươc, vì chọn tới cục thứ 3 giống nhau
        //thì list selected bị set về 0 mất
        if (isMovingToSelectedBar == true)
        {
            MakeTileSmaller();
            MovingToSelectedBar();
        }

        if (Vector3.Distance(transform.position, newNextTarget) < 0.001f && isMovingToSelectedBar == true)
        {
            StopMovingToSelectedBar();
        }

        if (isMovingOutSelectedBar == true)
        {
            MakeTileBigger();
            MoveToTable();
        }

        if (Vector3.Distance(transform.position, backToTable) < 0.001f && isMovingOutSelectedBar == true)
        {
            StopMovingToTable();
        }
    }

    void MovingToSelectedBar()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        //gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        transform.position = Vector3.MoveTowards(transform.position, newNextTarget, 5f * Time.deltaTime);
    }

    void MakeTileSmaller()
    {
        while (transform.localScale.x > 0.6f)
        {
            transform.localScale += scaleChange;
        }
    }

    void StopMovingToSelectedBar()
    {
        isMovingToSelectedBar = false;
        //gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        gameObject.transform.position = newNextTarget;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void MakeTileBigger()
    {
        while (transform.localScale.x < 1f)
        {
            transform.localScale += -scaleChange;
        }
    }

    void MoveToTable()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        transform.position = Vector3.MoveTowards(transform.position, backToTable, 5f * Time.deltaTime);
    }

    void StopMovingToTable()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        gameObject.transform.position = backToTable;
        isMovingOutSelectedBar = false;
    }


    void OnMouseUpAsButton()
    {
        Debug.Log(selectSound.name);
        audioSource.PlayOneShot(selectSound);
        //chọn cục trên bàn, nếu bar chưa full 7 cục thì cục đấy đổi thuộc tính isSelected thành true
        //và add cục đấy vào list selected
        if (gameObject.GetComponent<Tile>().isSelected == false)
        {
            if (selectedTiles.Count == 7)
            {
                return;
            }

            gameObject.GetComponent<Tile>().isSelected = true;
            selectedTiles.Add(gameObject);
            Debug.Log("Select");

            //chọn slot tiếp theo còn trống trong bar cho cục bay tới
            //và set tileIndex của cục = slotIndex của slot được chọn luôn
            newNextTarget = slots[selectedTiles.Count - 1].target;

            //cho cục bay tới slot đã chọn
            isMovingToSelectedBar = true;
        }
        else
        {
            //nếu chọn cục dưới bar thì đổi thuộc tính isSelected thành false,
            //move nó ra khỏi list selected
            gameObject.GetComponent<Tile>().isSelected = false;
            selectedTiles.Remove(gameObject);
            Debug.Log("UnSelect");

            //random một vị trí trên bàn cho cục bay về
            backToTable = new Vector3((float)GetRandomNumber(-1.4, 1.4), 1, (float)GetRandomNumber(-2, 2.35));

            //cho cục bay về
            isMovingOutSelectedBar = true;

            //sau khi move 1 cục back về bàn,
            //set tất cả slot trong list 7 slots dưới bar thành isEmpty = true
            //set lại  tileIndex  của các cục còn trong list selected
            //slot có slotIndex = tileIndex sẽ set isEmpty = false
            //cho sort lại các cục dưới bar
            sortTileIndex(false);
        }

        //mỗi lần chọn sẽ check xem có 3 cục trong list selected giống nhau k
        int sameTileCount = 0;
        foreach (var selectedTile in selectedTiles)
        {
            if (selectedTile.gameObject.tag == gameObject.tag)
            {
                sameTileCount++;
            }
        }

        //nếu có 3 cục trong bar giống nhau thì tìm 3 tất cả cục có tag giống cục vừa chọn trên bàn
        //trong tất cả cục đó, cục nào isSelected = true thì throw
        if (sameTileCount == 3)
        {
            audioSource.PlayOneShot(matchSound);
            GameObject[] sameTile = GameObject.FindGameObjectsWithTag(gameObject.tag);
            foreach (var tile in sameTile)
            {
                if (tile.GetComponent<Tile>().isSelected == true)
                {
                    StartCoroutine(ThrowTileCoroutine(tile));
                    selectedTiles.Remove(tile);
                    createTile.tiles.Remove(tile);
                }
            }

            //sau khi phá 3 cục, set tất cả slot trong list 7 slots dưới bar thành isEmpty = true
            //set lại  tileIndex  của các cục còn trong list selected
            //slot có slotIndex = tileIndex sẽ set isEmpty = false
            //cho sort lại các cục dưới bar
            sortTileIndex(true);
        }
    }

    //này dùng để  get vị trí slot cho cục bay tới
    Vector3 GetTheTarget(GameObject tile)
    {
        Debug.Log(tile.GetComponent<Tile>().tileIndex); //noted
        if (tile.GetComponent<Tile>().tileIndex == -1)
        {
            tile.GetComponent<Tile>().tileIndex = 0;
        }
        Slots slot = slots.Find(t => t.slotIndex == tile.GetComponent<Tile>().tileIndex);
        return slot.target;
    }

    //này dùng để xếp cục vào slot có slotIndex = tileIndex của cục đó
    void SortTiles(int i)
    {
        if (selectedTiles.Count != 0)
        {
            selectedTiles[i].GetComponent<Rigidbody>().useGravity = false;

            selectedTiles[i].transform.position = Vector3.MoveTowards(selectedTiles[i].transform.position, GetTheTarget(SelectTile.selectedTiles[i]), 5f * Time.deltaTime);
            if (Vector3.Distance(selectedTiles[i].transform.position, GetTheTarget(selectedTiles[i])) < 0.001f)
            {
                needToSort[i] = false;
                selectedTiles[i].GetComponent<Rigidbody>().useGravity = true;
                selectedTiles[i].transform.rotation = new Quaternion(0, 0, 0, 0);
                selectedTiles[i].transform.position = GetTheTarget(selectedTiles[i]);
            }
        }
    }

    //get randomnumber cho số kiểu double
    double GetRandomNumber(double minimum, double maximum)
    {
        var random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    void sortTileIndex(bool SortAfterMatchThreeTiles)
    {
        foreach (var slot in slots) { slot.isEmpty = true; }
        if (selectedTiles.Count != 0)
        {
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                selectedTiles[i].GetComponent<Tile>().tileIndex = i;
                slots[i].isEmpty = false;
                if (SortAfterMatchThreeTiles == true)
                {
                    StartCoroutine(SortTilesCoroutine(i));
                    needToSort[i] = true;
                }
                else
                {
                    needToSort[i] = true;
                }
            }
        }
    }

    IEnumerator SortTilesCoroutine(int i)
    {
        yield return new WaitForSeconds(2.1f);
        needToSort[i] = true;
    }

    IEnumerator ThrowTileCoroutine(GameObject tile)
    {
        yield return new WaitForSeconds(2f);
        tile.transform.position = new Vector3(100, -100, 0);
        tile.GetComponent<Rigidbody>().isKinematic = false;
        tile.GetComponent<Rigidbody>().detectCollisions = false;
    }
}
