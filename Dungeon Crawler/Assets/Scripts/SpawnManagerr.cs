using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerr : MonoBehaviour
{
    [Header("Variables")]
    public int maxRooms = 8;
    public float timeBetweenSpawns = 0.1f;
    public float timeBetweenDelete = 0.05f;
    public float timeUntilCheckBossRoom = 0.2f;
    public bool spawnRoomsWith3RdOpening = false;

    [Header("Rooms")]
    // Keep in Mind the first gameobject of each list will be the closed version of that room type
    public List<GameObject> roomsT = new List<GameObject>();
    public List<GameObject> roomsR = new List<GameObject>();
    public List<GameObject> roomsB = new List<GameObject>();
    public List<GameObject> roomsL = new List<GameObject>();

    public List<GameObject> roomsT3RdOpening = new List<GameObject>();
    public List<GameObject> roomsR3RdOpening = new List<GameObject>();
    public List<GameObject> roomsB3RdOpening = new List<GameObject>();
    public List<GameObject> roomsL3RdOpening = new List<GameObject>();

    public List<GameObject> allRooms = new List<GameObject>();
    
    public List<GameObject> allSpawners = new List<GameObject>();

    public GameObject emptyRoom;
    public GameObject destroyPoint;

    [Header("Room Details")]
    public Color defaultColor;
    public Color spawnRoomColor;
    public Color bossRoomColor;

    [Header("In Game")]
    public int roomCount = 0;

    [Header("Components")]
    CameraResizeTest cameraResize;
    UI uiScript;

    public bool doneCleaning = true;
    public bool doneSpawning = false;

    public List<GameObject> roomWaitLine = new List<GameObject>();

    public GameObject returnRandRoom(string roomType)
    {
        if (!spawnRoomsWith3RdOpening)
        {
            switch (roomType)
            {
                case "T":
                return roomsT[Random.Range(0, roomsT.Count)];
                case "R":
                return roomsR[Random.Range(0, roomsR.Count)];
                case "B":
                return roomsB[Random.Range(0, roomsB.Count)];
                case "L":
                return roomsL[Random.Range(0, roomsL.Count)];
                default:
                print("Bro you entered an invalid string for returnRandRoom in SpawnManager");
                return null;
            }
        }
        else
        {
            switch (roomType)
            {
                case "T":
                return roomsT3RdOpening[Random.Range(0, roomsT.Count)];
                case "R":
                return roomsR3RdOpening[Random.Range(0, roomsR.Count)];
                case "B":
                return roomsB3RdOpening[Random.Range(0, roomsB.Count)];
                case "L":
                return roomsL3RdOpening[Random.Range(0, roomsL.Count)];
                default:
                print("Bro you entered an invalid string for returnRandRoom in SpawnManager");
                return null;
            }
        }
    }

    public GameObject returnClosedRoom(string roomType)
    {
        switch (roomType)
        {
            case "T":
            return roomsT[0];
            case "R":
            return roomsR[0];
            case "B":
            return roomsB[0];
            case "L":
            return roomsL[0];
            default:
            print("Bro you entered an invalid string for returnClosedRoom in SpawnManager");
            return null;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnLine());
        cameraResize = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraResizeTest>();
        uiScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
    }

    public IEnumerator SpawnLine()
    {
        Spawn recentSpawner = null;
        yield return new WaitForSecondsRealtime(timeBetweenSpawns);
        while (roomWaitLine.Count != 0)
        {
            UpdateCameraSize();
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);

            if (roomWaitLine.Count != 0)
            {
                while (roomWaitLine[0] == null && roomWaitLine.Count != 0)
                {
                    print("During Loop");
                    roomWaitLine.Remove(roomWaitLine[0]);
                    yield return null;
                }
                if (roomWaitLine.Count != 0)
                {
                    recentSpawner = roomWaitLine[0].GetComponent<Spawn>();
                    recentSpawner.SpawnRoom();
                    roomWaitLine.Remove(roomWaitLine[0]);
                }
            }
        }
        doneSpawning = true;
        yield return new WaitForSecondsRealtime(timeUntilCheckBossRoom);
        FinishSpawning();
    }

    void FinishSpawning()
    {
        CalcBossRoom();
        RemoveAllSpawners();
        UpdateCameraSize();
    }

    public void ToggleRoomsWith3RDOpenings()
    {
        spawnRoomsWith3RdOpening = !spawnRoomsWith3RdOpening;
        uiScript.ToggleRoomSizeImage(spawnRoomsWith3RdOpening);
    }

    void UpdateCameraSize()
    {
        float upestTilePosY = allRooms[0].transform.position.y;
        float righestTilePosX = allRooms[0].transform.position.x;
        float lowestTilePosY = allRooms[0].transform.position.y;
        float leftestTilePosX = allRooms[0].transform.position.x;

        foreach (GameObject room in allRooms)
        {
            if(room.transform.position.y > upestTilePosY)
                upestTilePosY = room.transform.position.y;

            if(room.transform.position.x > righestTilePosX)
                righestTilePosX = room.transform.position.x;

             if(room.transform.position.y < lowestTilePosY)
                lowestTilePosY = room.transform.position.y;

            if(room.transform.position.x < leftestTilePosX)
                leftestTilePosX = room.transform.position.x;
        }

        cameraResize.UpdateCamera(upestTilePosY, righestTilePosX, lowestTilePosY, leftestTilePosX);

    }

    void CalcBossRoom()
    {
        GameObject lastRoomNotNull = null;

        foreach (GameObject room in allRooms)
        {
            if (room != null)
                lastRoomNotNull = room;
        }
        if (lastRoomNotNull != null)
            lastRoomNotNull.GetComponent<Room>().MakeBossRoom();
    }

    void RemoveAllSpawners()
    {
        foreach(GameObject spawner in GameObject.FindGameObjectsWithTag("SpawnRoom"))
        {
            spawner.GetComponent<Spawn>().DestroyMe();
        }    
    }

}
