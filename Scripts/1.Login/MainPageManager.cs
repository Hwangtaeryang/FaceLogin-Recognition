using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPageManager : MonoBehaviour
{


    private void Start()
    {
        //string path = System.IO.Path.Combine(Application.streamingAssetsPath);
        //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "'\'Zoom";
        //Debug.Log(path);
    }

    public void LoginButtonClick()
    {
        SceneManager.LoadScene("1.Login");
    }

    public void GuestButtonClick()
    {
        SceneManager.LoadScene("5.GuestExercise");
    }
}
