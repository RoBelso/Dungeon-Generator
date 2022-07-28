using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResizeTest : MonoBehaviour
{
    public bool doStuff = true;
    SpawnManagerr managerSpawn;
    Camera cameraComp;
    float highestTilePos;  float righestTilePos;  float lowestTilePos;  float leftestTilePos;
    UI uiScript;

    void Start()
    {
        managerSpawn = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerr>();
        cameraComp = gameObject.GetComponent<Camera>();
        uiScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            ToggleOnOff();

    }

    void ToggleOnOff()
    {
        doStuff = !doStuff;

        uiScript.ToggleCameraImage(doStuff);

        if (!doStuff)
        {
            transform.position = new Vector3(0, 0, -1);
            cameraComp.orthographicSize = 47.5f;
        }
        else
            UpdateCamera(highestTilePos, righestTilePos, lowestTilePos, leftestTilePos);
    }

    public void UpdateCamera(float highestTile, float righestTile, float lowestTile, float leftestTile)
    {
        highestTilePos = highestTile;
        righestTilePos = righestTile;
        lowestTilePos = lowestTile;
        leftestTilePos = leftestTile;

        if (!doStuff) return;

        Vector3 camPos = new Vector3((leftestTilePos + righestTilePos) / 2, (highestTilePos + lowestTilePos) / 2, -1);
        UpdateCameraPos(camPos);

        float camSizeX = Mathf.Abs(leftestTilePos) + Mathf.Abs(righestTilePos);
        float camSizeY = Mathf.Abs(highestTilePos) + Mathf.Abs(lowestTilePos);
        float camSize;
        if (camSizeX > camSizeY)
            camSize = camSizeX;
        else
            camSize = camSizeY;

        UpdateCameraSize(camSize);
    }

    void UpdateCameraSize(float size)
    {
        if (size - (size / 3) < 29.3)
            cameraComp.orthographicSize = 29.3f;
        else
            cameraComp.orthographicSize = size - (size / 3);
    }

    void UpdateCameraPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
