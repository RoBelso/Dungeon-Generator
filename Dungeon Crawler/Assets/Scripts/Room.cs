using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [Header("Set Variables")]
    public GameObject background;

    [Header("In Game")]
    public GameObject myCreator;

    [Header("About Room")]
    public bool isSpawnRoom = false;
    public bool isClosedRoom = false;
    public bool isBossRoom = false;
    public bool isTreasureRoom = false;
    public bool isTrapRoom = false;

    [Header("Set Details")]
    public bool alreadySpawnRoom = false;

    [Header("Components")]
    SpawnManagerr managerSpawn;

    void Start()
    {
        managerSpawn.roomCount++;
    }

    private void Awake()
    {
        managerSpawn = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerr>();
        managerSpawn.allRooms.Add(gameObject);
        if (!isClosedRoom)
            SetDefaultColor();
    }

    public void DestroyMe()
    {
        managerSpawn.allRooms.Remove(gameObject);
        managerSpawn.roomCount--;
        Destroy(gameObject);
    }

    public void SetDefaultColor()
    {
        background.GetComponent<Tilemap>().color = managerSpawn.defaultColor;
    }

    public void MakeBossRoom()
    {
        if (isBossRoom == true) return;

        isBossRoom = true;

        background.GetComponent<Tilemap>().color = managerSpawn.bossRoomColor;
    }

    public void MakeSpawnRoom()
    {
        if(alreadySpawnRoom) return;
        alreadySpawnRoom = true;

        isSpawnRoom = true;

        GameObject destroyer = Instantiate(managerSpawn.destroyPoint, transform.position, Quaternion.identity);
        destroyer.transform.parent = transform;

        background.GetComponent<Tilemap>().color = managerSpawn.spawnRoomColor;
    }
}
