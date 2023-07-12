using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseTimer : MonoBehaviour
{
    public static ExerciseTimer instance { get; private set; }

    float currTime;
    public bool startExercise;
    public bool restTimeState;  //쉬는 시간상태여부

    public GameObject playVideoBarObj;
    public GameObject endCountDownObj;
    public GameObject videoPanel;
    public GameObject myImage;
    public GameObject resultPanel;
    public GameObject restPanel;
    public GameObject restNextButtonPanel;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    //시작 카운트 다운 - 운동 처음 시작 햇을 때 5초 카운트 다운
    public void StartTime(float _maxTime, Text _startTimerText, GameObject _startPanel)
    {
        StartCoroutine(_StartTime(_maxTime, _startTimerText, _startPanel));
    }

    IEnumerator _StartTime(float _maxTime, Text _startTimerText, GameObject _startPanel)
    {
        currTime = 0;
        yield return new WaitForSeconds(1f);

        currTime = _maxTime;

        while(currTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currTime--;

            if (currTime <= 5)
                _startTimerText.text = currTime.ToString();

            if (currTime.Equals(0))
            {
                _startPanel.SetActive(false);   //시작판넬 비활성화
                startExercise = true;   //운동시작 카운트다운 끝
            }
        }
    }



    public void PlayingTime(float _maxTime, Slider _slider, Text _videoTimeText)
    {
        StartCoroutine(_PlayingTime(_maxTime, _slider, _videoTimeText));
    }

    IEnumerator _PlayingTime(float _maxTime, Slider _slider, Text _videoTimeText)
    {
        Debug.Log("여긴 몇번이나 들어오나 ?  " + _maxTime);
        SceneSoundCtrl.instance.MainBGM_SoundPuase();
        currTime = 0;
        endCountDownObj.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);

        while (currTime <= _maxTime)
        {
            yield return new WaitForSecondsRealtime(0f);
            currTime += Time.deltaTime;
            _slider.value = currTime / _maxTime;

            //Debug.Log("currTime : " + currTime);
            TimeSpan timeSpan = TimeSpan.FromSeconds(currTime);
            string timer = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
            _videoTimeText.text = timer;
            //yield return null;
            

            //내래이션 끝나고 운동체크하기 위함
            if (currTime > PlayerPrefs.GetInt("EP_NarratoinTime") && currTime < PlayerPrefs.GetInt("EP_NarratoinTime") +2)
                ExerciseScene_Manager.instance.startMotionState = true; //운동 체크 시작

            if (currTime >= _maxTime - 5)
            {
                playVideoBarObj.SetActive(false);   //기존의 플레이비디오바 비활성화
                endCountDownObj.SetActive(true);
                endCountDownObj.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = (_maxTime - (int)currTime).ToString();

                //영상 끝나기 3~4초 전부터 체크 하지않는다.
                if(currTime >= _maxTime - 4f)
                    ExerciseScene_Manager.instance.startMotionState = false;
            }

            if(currTime >= _maxTime)
            {
                Debug.Log("currTime " + currTime);
                SceneSoundCtrl.instance.MainBGM_Sound();

                if (restPanel.activeSelf.Equals(false))
                {
                    VideoHandler.instance.StopVideo();
                    playVideoBarObj.SetActive(true);   //기존의 플레이비디오바 활성화
                    endCountDownObj.SetActive(false);
                    videoPanel.SetActive(false);    //비디오나오는 화면 비활성화
                    myImage.SetActive(false);   //내모습 비활성화
                    resultPanel.SetActive(true);    //결과화면 활성화
                    ExerciseScene_UIManager.instance.ResultPenelInit();
                }
                else
                {
                    //restTimeState = true;
                    playVideoBarObj.SetActive(true);   //기존의 플레이비디오바 활성화
                    restNextButtonPanel.SetActive(true);
                    endCountDownObj.SetActive(false);

                    //if(restTimeState.Equals(true))
                    //    ExerciseScene_UIManager.instance.NextExerciseButtonClick();
                }
            }
        }
    }

    
}
