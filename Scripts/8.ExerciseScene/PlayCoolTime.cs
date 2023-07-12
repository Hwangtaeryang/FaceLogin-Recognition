using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCoolTime : MonoBehaviour
{
    public static PlayCoolTime instance { get; private set; }


    public Text startPanelTimeText; //�����ǳڿ� �ִ� Ÿ�̸��ؽ�Ʈ
    public Text videoNameText;  //���� ���� �̸� �ؽ�Ʈ


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
                //_videoPanel.SetActive(true);
                //_myImage.SetActive(true);
                _startPanel.SetActive(false);
                Debug.Log("????? " + exerciseType);
                //ExerciseAnnouncementData.instacne.ExerciseName(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType,
                //    PlayerPrefs.GetInt("EP_PlayCount"), videoNameText);
                //VideoHandler.instance.PlayVideo();
                exerciseStart = true;
            }
        }
    }


    //���� �� �����ǳ� ��Ȱ��ȭ, ����/��ȭ�� Ȱ��ȭ
    public void PlayStartTimer(int _maxTime, Text _timerText, GameObject _videoPanel, GameObject _myImage, GameObject _startPanel)
    {
        Debug.Log("�ȵ���?");
        StartCoroutine(_PlayStartTimer(_maxTime, _timerText, _videoPanel, _myImage, _startPanel));
    }

    IEnumerator _PlayStartTimer(int _maxTime, Text _timerText, GameObject _videoPanel, GameObject _myImage, GameObject _startPanel)
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
        while(currTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currTime--;
            //Debug.Log("Time: " + currTime);
            if(currTime <= 5)
                _timerText.text = currTime.ToString();

            if(currTime.Equals(0))
            {
                _videoPanel.SetActive(true);
                _myImage.SetActive(true);
                _startPanel.SetActive(false);
                Debug.Log("????? " + exerciseType);
                ExerciseAnnouncementData.instacne.ExerciseName(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType, 
                    PlayerPrefs.GetInt("EP_PlayCount"), videoNameText);
                VideoHandler.instance.PlayVideo();
                exerciseStart = true;
            }
        }
    }



    //��ϰ� ���� �� Ÿ�̸�, ��ǳ� ��Ȱ��ȭ - ���ȭ�� Ȱ��ȭ
    public void PlayingTimer(Slider _sliderTime, Text _timer, int _maxTime, Text _timerText, GameObject _videoPanel, GameObject _myImage, GameObject _resultPanel)
    {
        Debug.Log("�ȵ���?");
        StartCoroutine(_PlayingTimer(_sliderTime, _timer, _maxTime, _timerText, _videoPanel, _myImage, _resultPanel));
    }

    IEnumerator _PlayingTimer(Slider _sliderTime, Text _timer, int _maxTime, Text _timerText, GameObject _videoPanel, GameObject _myImage, GameObject _resultPanel)
    {
        while (exercixeCurrTime <= _maxTime)
        {
            exercixeCurrTime += Time.deltaTime;
            //exercixeCurrTime += 0.1f;
            _sliderTime.value = exercixeCurrTime / _maxTime;

            TimeSpan timespan = TimeSpan.FromSeconds(exercixeCurrTime);
            string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
            //Debug.Log(" timer  " + timer);
            _timer.text = timer;
            yield return null;

            if (exercixeCurrTime <= 5)
            {
                
                _timerText.text = currTime.ToString();
            }


            if (exercixeCurrTime >= _maxTime)
            {
                exerciseStart = false;
                VideoHandler.instance.StopVideo();
                _videoPanel.SetActive(false);
                _myImage.SetActive(false);
                _resultPanel.SetActive(true);
            }
        }



        //yield return new WaitForSeconds(1f);
        //currTime = _maxTime;
        //while (currTime > 0)
        //{
        //    yield return new WaitForSecondsRealtime(1f);
        //    --currTime;
        //    Debug.Log("Time: " + currTime);
        //    if (currTime <= 5)
        //    {
        //        _timerText.gameObject.SetActive(true);
        //        _timerText.text = currTime.ToString();
        //    }


        //    if (currTime.Equals(0))
        //    {
        //        _videoPanel.SetActive(false);
        //        _myImage.SetActive(false);
        //        _resultPanel.SetActive(true);
        //    }
        //}
    }


    //����(��ð�) Ÿ�̸�
    public void ExercisePlayingTimer(float _maxTime, Slider _sliderTime, Text _timer)
    {
        //exercixeCurrTime += Time.deltaTime;
        //_sliderTime.value = exercixeCurrTime / _maxTime;

        //TimeSpan timespan = TimeSpan.FromSeconds(exercixeCurrTime);
        //string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
        ////Debug.Log(" timer  " + timer);
        //_timer.text = timer;

        //if (exercixeCurrTime >= _maxTime)
        //{
        //    exerciseStart = false;
        //    VideoHandler.instance.StopVideo();
        //}
        StartCoroutine(_ExercisePlayingTimer(_maxTime, _sliderTime, _timer));
    }

    IEnumerator _ExercisePlayingTimer(float _maxTime, Slider _sliderTime, Text _timer)
    {
        //yield return null;

        Debug.Log("exercixeCurrTime " + exercixeCurrTime);
        while(exercixeCurrTime <= _maxTime)
        {
            exercixeCurrTime += Time.deltaTime;
            //exercixeCurrTime += 0.1f;
            _sliderTime.value = exercixeCurrTime / _maxTime;

            TimeSpan timespan = TimeSpan.FromSeconds(exercixeCurrTime);
            string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
            //Debug.Log(" timer  " + timer);
            _timer.text = timer;
            yield return null;

            if (exercixeCurrTime >= _maxTime)
            {
                exerciseStart = false;
                VideoHandler.instance.StopVideo();
            }
        }
    }





}
