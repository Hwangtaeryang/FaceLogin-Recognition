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
    public bool restTimeState;  //���� �ð����¿���

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

    //���� ī��Ʈ �ٿ� - � ó�� ���� ���� �� 5�� ī��Ʈ �ٿ�
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
                _startPanel.SetActive(false);   //�����ǳ� ��Ȱ��ȭ
                startExercise = true;   //����� ī��Ʈ�ٿ� ��
            }
        }
    }



    public void PlayingTime(float _maxTime, Slider _slider, Text _videoTimeText)
    {
        StartCoroutine(_PlayingTime(_maxTime, _slider, _videoTimeText));
    }

    IEnumerator _PlayingTime(float _maxTime, Slider _slider, Text _videoTimeText)
    {
        Debug.Log("���� ����̳� ������ ?  " + _maxTime);
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
            

            //�����̼� ������ �üũ�ϱ� ����
            if (currTime > PlayerPrefs.GetInt("EP_NarratoinTime") && currTime < PlayerPrefs.GetInt("EP_NarratoinTime") +2)
                ExerciseScene_Manager.instance.startMotionState = true; //� üũ ����

            if (currTime >= _maxTime - 5)
            {
                playVideoBarObj.SetActive(false);   //������ �÷��̺����� ��Ȱ��ȭ
                endCountDownObj.SetActive(true);
                endCountDownObj.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = (_maxTime - (int)currTime).ToString();

                //���� ������ 3~4�� ������ üũ �����ʴ´�.
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
                    playVideoBarObj.SetActive(true);   //������ �÷��̺����� Ȱ��ȭ
                    endCountDownObj.SetActive(false);
                    videoPanel.SetActive(false);    //���������� ȭ�� ��Ȱ��ȭ
                    myImage.SetActive(false);   //����� ��Ȱ��ȭ
                    resultPanel.SetActive(true);    //���ȭ�� Ȱ��ȭ
                    ExerciseScene_UIManager.instance.ResultPenelInit();
                }
                else
                {
                    //restTimeState = true;
                    playVideoBarObj.SetActive(true);   //������ �÷��̺����� Ȱ��ȭ
                    restNextButtonPanel.SetActive(true);
                    endCountDownObj.SetActive(false);

                    //if(restTimeState.Equals(true))
                    //    ExerciseScene_UIManager.instance.NextExerciseButtonClick();
                }
            }
        }
    }

    
}
