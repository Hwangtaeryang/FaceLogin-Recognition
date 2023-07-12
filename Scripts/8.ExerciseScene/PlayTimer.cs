using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimer : MonoBehaviour
{
    public static PlayTimer instance { get; private set; }

    public GameObject endCountDownObj;  //영상 끝나기 5초전 카운트다운
    public GameObject videoPanel;   //비디오나오는 화면
    public GameObject myImage;  //내모슨 화면
    public GameObject resultPanel;  //결과화면

    int currTime;
    float exercixeCurrTime;
    public bool exerciseStart;




    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    public void StartTimer(int _maxTime, Text _timerText, GameObject _startPanel)
    {
        StartCoroutine(_StartTimer(_maxTime, _timerText, _startPanel));
    }


    IEnumerator _StartTimer(int _maxTime, Text _timerText, GameObject _startPanel)
    {
        yield return new WaitForSeconds(1f);

        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            exerciseType = "근력운동상체";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            exerciseType = "근력운동하체";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            exerciseType = "율동운동";
        }


        currTime = _maxTime;
        while (currTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currTime--;
            //Debug.Log("Time: " + currTime);
            if (currTime <= 5)
                _timerText.text = currTime.ToString();

            if (currTime.Equals(0))
            {
                _startPanel.SetActive(false);
                Debug.Log("????? " + exerciseType);
                exerciseStart = true;
            }
        }
    }


    public void PlayingTimer(int _maxTime, Slider _sliderTime, Text _videoTimeText)
    {
        StartCoroutine(_PlayingTimer(_maxTime, _sliderTime, _videoTimeText));
    }

    IEnumerator _PlayingTimer(int _maxTime, Slider _sliderTime, Text _videoTimeText)
    {
        exercixeCurrTime = 0;
        endCountDownObj.SetActive(false);

        while (exercixeCurrTime <= _maxTime)
        {
            exercixeCurrTime += Time.deltaTime;
            _sliderTime.value = exercixeCurrTime / _maxTime;

            TimeSpan timespan = TimeSpan.FromSeconds(exercixeCurrTime);
            string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
            //Debug.Log(" timer  " + timer);
            _videoTimeText.text = timer;
            yield return null;
            //Debug.Log("여기 ?  " + exercixeCurrTime);


            if (exercixeCurrTime >= _maxTime - 5)
            {
                endCountDownObj.SetActive(true);
                endCountDownObj.transform.GetChild(1).GetComponent<Text>().text = (_maxTime - (int)exercixeCurrTime).ToString();
            }

            if(exercixeCurrTime >= _maxTime)
            {
                VideoHandler.instance.StopVideo();
                videoPanel.SetActive(false);    //비디오나오는 화면 비활성화
                myImage.SetActive(false);   //내모습 비활성화
                resultPanel.SetActive(true);    //결과화면 활성화
            }
        }
    }


}
