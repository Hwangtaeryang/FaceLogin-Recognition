using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSoundManager : MonoBehaviour
{
    public static AppSoundManager instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;


    public AudioClip bgmMain_Clip;

    public AudioClip click_Clip;
    public AudioClip shoot_Clip;
    public AudioClip touch_Clip;
    public AudioClip keyboard_Clip;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else instance = this;

        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        MainBGM_Sound();
    }

    public void MainBGM_Sound()
    {
        bgmSource.clip = bgmMain_Clip;
        bgmSource.Play();
    }

    public void MainBGM_SoundPuase()
    {
        bgmSource.clip = bgmMain_Clip;
        bgmSource.Pause();
    }

    public void ClickSound()
    {
        sfxSource.clip = click_Clip;
        sfxSource.Play();
    }

    public void ShootSound()
    {
        sfxSource.clip = shoot_Clip;
        sfxSource.Play();
    }

    public void WindowTouchSound()
    {
        sfxSource.clip = touch_Clip;
        sfxSource.Play();
    }

    public void KeyboardTouchSound()
    {
        sfxSource.clip = keyboard_Clip;
        sfxSource.Play();
    }
}
