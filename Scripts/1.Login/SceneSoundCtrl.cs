using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSoundCtrl : MonoBehaviour
{
    public static SceneSoundCtrl instance { get; private set; }


    AppSoundManager soundManager;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    void Start()
    {
        StartCoroutine(SoundManagerInit());
    }

    IEnumerator SoundManagerInit()
    {
        yield return new WaitForSeconds(0.5f);
        //soundManager = GameObject.Find("AppSoundManager").GetComponent<AppSoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSound()
    {
        soundManager.ClickSound();
    }

    public void ShootSound()
    {
        soundManager.ShootSound();
    }

    public void WindowTouchSound()
    {
        Debug.Log("???????");
        soundManager.WindowTouchSound();
    }

    public void KeyboardTouchSound()
    {
        soundManager.KeyboardTouchSound();
    }


    public void MainBGM_Sound()
    {
        soundManager.MainBGM_Sound();
    }

    public void MainBGM_SoundPuase()
    {
        soundManager.MainBGM_SoundPuase();
    }
}
