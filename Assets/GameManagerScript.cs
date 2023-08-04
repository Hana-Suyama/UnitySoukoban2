using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //îzóÒÇÃêÈåæ
    //int[] map;

    //void PrintArray()
    //{
    //    string debugText = "";
    //    for(int i = 0; i < map.Length; i++)
    //    {
    //        debugText += map[i].ToString() + ", ";
    //    }
    //    Debug.Log(debugText);
    //}

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (map[moveTo.y, moveTo.x] == 4) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool ResetNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (map[moveTo.y, moveTo.x] == 4) { return false; }

        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject wallPrefab;
    
    public GameObject clearText;

    int[,] map;
    GameObject[,] field;

    // Start is called before the first frame update
    void Start()
    {

       // public static void SetResolution(1920, 1080, false);

        map = new int[,]
        {
            {4,4,4,4,4,4,4 },
            {4,0,0,0,0,0,4 },
            {4,0,3,1,3,0,4 },
            {4,0,0,2,0,0,4 },
            {4,0,2,3,2,0,4 },
            {4,0,0,0,0,0,4 },
            {4,4,4,4,4,4,4 },
        };
       
        field = new GameObject[map.GetLength(0),map.GetLength(1)];

        string debugText = "";
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y,x] == 1)
                {
                    field[y,x] = Instantiate(
                    playerPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                    boxPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }
                if (map[y, x] == 3)
                {
                    Instantiate(
                    goalPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0.01f),
                    Quaternion.identity
                    );
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(
                    wallPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0.01f),
                    Quaternion.identity
                    );
                }
            }
        }
        Debug.Log(debugText);
        //îzóÒÇÃé¿ë‘ÇÃçÏê¨Ç∆èâä˙âª
        //map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };
        //PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (field[2, 3] == null || field[2, 3].tag == "Box")
            {
                Vector2Int playerIndex = GetPlayerIndex();
                ResetNumber("player", playerIndex, new Vector2Int(3, 2));
                //Debug.Log(field[2, 3].tag);
            }

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (field[y, x] == null) { continue; }
                    if (field[y, x].tag == "Box")
                    {
                        if (field[4, 2] == null)
                        {
                            ResetNumber("box", new Vector2Int(x, y), new Vector2Int(2, 4));
                        }else if (field[3, 3] == null)
                        {
                            ResetNumber("box", new Vector2Int(x, y), new Vector2Int(3, 3));
                        }else if (field[4, 4] == null)
                        {
                            ResetNumber("box", new Vector2Int(x, y), new Vector2Int(4, 4));
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(1, 0));
            //PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(-1, 0));
            //PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(0, -1));
            //PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(0, 1));
            //PrintArray();
        }
        if (IsCleard())
        {
            //Debug.Log("Clear!");
            clearText.SetActive(true);
        }
    }
    bool IsCleard()
    {
        //Vector2Intå^ÇÃâ¬ïœí∑îzóÒÇÃçÏê¨
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //äiî[èÍèäÇ©î€Ç©Çîªíf
                if (map[y, x] == 3)
                {
                    //äiî[èÍèäÇÃÉCÉìÉfÉbÉNÉXÇçTÇ¶ÇƒÇ®Ç≠
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //óvëfêîÇÕgoals.CountÇ≈éÊìæ
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //àÍÇ¬Ç≈Ç‡î†Ç™ñ≥Ç©Ç¡ÇΩÇÁèåèñ¢íBê¨
                return false;
            }
        }
        return true;
    }
}
