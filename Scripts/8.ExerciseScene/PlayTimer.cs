using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimer : MonoBehaviour
{
    public static PlayTimer instance { get; private set; }

    public GameObject endCountDownObj;  //���� ������ 5���� ī��Ʈ�ٿ�
    public GameObject videoPanel;   //���������� ȭ��
    public GameObject myImage;  //���� ȭ��
    public GameObject resultPanel;  //���ȭ��

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
            exerciseType = "�ٷ¿��ü";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            exerciseType = "�ٷ¿��ü";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            exerciseType = "�����";
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
            //Debug.Log("���� ?  " + exercixeCurrTime);


            if (exercixeCurrTime >= _maxTime - 5)
            {
                endCountDownObj.SetActive(true);
                endCountDownObj.transform.GetChild(1).GetComponent<Text>().text = (_maxTime - (int)exercixeCurrTime).ToString();
            }

            if(exercixeCurrTime >= _maxTime)
            {
                VideoHandler.instance.StopVideo();
                videoPanel.SetActive(false);    //���������� ȭ�� ��Ȱ��ȭ
                myImage.SetActive(false);   //����� ��Ȱ��ȭ
                resultPanel.SetActive(true);    //���ȭ�� Ȱ��ȭ
            }
        }
    }


}
