using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotScript : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private int size = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            path += "screenshot ";
            path += System.Guid.NewGuid().ToString() + ".png";

            ScreenCapture.CaptureScreenshot(path, size);
        }
    }
}
