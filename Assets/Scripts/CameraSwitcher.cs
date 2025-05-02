using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Array of camera objects
    private int currentCameraIndex = 0; // Index of the currently active camera

    void Start()
    {
        // Ensure only the first camera is active at the start
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }

    void Update()
    {
        // Check if the "V" key is pressed
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Deactivate the current camera
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Move to the next camera index
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

            // Activate the new current camera
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }
}