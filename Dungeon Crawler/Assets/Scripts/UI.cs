using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public List<GameObject> children = new List<GameObject>();
    bool childrenActive = true;

    public GameObject imageCamera;
    public GameObject imageRoomSize;

    bool imageCameraActive = true;
    bool imageRoomSizeActive = true;

    private void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            childrenActive = !childrenActive;

            foreach (GameObject child in children)
            {
                child.SetActive(childrenActive);
            }
        }    
    }

    public void ToggleCameraImage(bool toggle)
    {
        imageCamera.SetActive(toggle);
    }

    public void ToggleRoomSizeImage(bool toggle)
    {
        imageRoomSize.SetActive(toggle);
    }
}
