using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public bool isDestroyer = false;
    public string spawnType;
    SpawnManagerr managerSpawn;
    public bool spawned = false;

    public GameObject myCreated;

    void Awake()
    {
        managerSpawn = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerr>();

        if (!spawned)
        {
            managerSpawn.allSpawners.Add(gameObject);
            managerSpawn.roomWaitLine.Add(gameObject);
        }
    }

    public void SpawnRoom()
    {
        if (!spawned)
        {
            GameObject room;

            if (managerSpawn.roomCount + 1 <= managerSpawn.maxRooms)
            {
                room = Instantiate(managerSpawn.returnRandRoom(spawnType), transform.position, Quaternion.identity);
            }
            else
            {
                room = Instantiate(managerSpawn.returnClosedRoom(spawnType), transform.position, Quaternion.identity);
            }
            myCreated = room;
            room.GetComponent<Room>().myCreator = gameObject;
            room.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpawnRoom")
        {
            if (collision.GetComponent<Spawn>().isDestroyer)
                DestroyMe();

            if (managerSpawn.allSpawners.Contains(collision.gameObject))
                if (managerSpawn.allSpawners.IndexOf(gameObject) > managerSpawn.allSpawners.IndexOf(collision.gameObject)) 
                    DestroyMe();

            {
                /*
                GameObject emptyRoom = Instantiate(managerSpawn.emptyRoom, transform.position, Quaternion.identity);
                emptyRoom.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
                emptyRoom.GetComponent<Room>().myCreator = gameObject;
                myCreated = emptyRoom;
                */
            }
        }
    }

    public void DestroyMe()
    {
        if(managerSpawn.roomWaitLine.Contains(gameObject))
            managerSpawn.roomWaitLine.Remove(gameObject);
        if(managerSpawn.allSpawners.Contains(gameObject))
            managerSpawn.allSpawners.Remove(gameObject);

        Destroy(gameObject);
    }

}

