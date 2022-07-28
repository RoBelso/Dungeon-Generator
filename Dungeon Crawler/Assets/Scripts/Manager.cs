using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject baseRoom;
    SpawnManagerr managerSpawn;


    private void Start()
    {
        managerSpawn = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerr>();
        SpawnBaseRoom(baseRoom);
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (managerSpawn.doneSpawning && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(ClearAllRooms());

        else if (managerSpawn.doneSpawning && (Input.GetKeyDown(KeyCode.Tab)
            || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.CapsLock)))
        {
            managerSpawn.ToggleRoomsWith3RDOpenings();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)
             || Input.GetKeyDown(KeyCode.Delete))
        {
            Application.Quit();
        }
    }

    IEnumerator ClearAllRooms()
    {
        managerSpawn.doneSpawning = false;
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            room.GetComponent<Room>().DestroyMe();
            yield return new WaitForSecondsRealtime(managerSpawn.timeBetweenDelete);
        }
        managerSpawn.roomCount = 0;
        SpawnBaseRoom(baseRoom);
        StartCoroutine(managerSpawn.SpawnLine());
    }

    void SpawnBaseRoom(GameObject baseRoom)
    {
        GameObject room = Instantiate(baseRoom, Vector2.zero, Quaternion.identity);
        room.transform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
        room.GetComponent<Room>().MakeSpawnRoom();
    }

}
