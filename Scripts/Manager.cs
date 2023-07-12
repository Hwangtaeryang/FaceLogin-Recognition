using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{


    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //snapCam.CallTakeSnapShot();
            ScreenShotHandler.TakeScreenShot_Static(1000, 1100);
        }
    }

    
}
