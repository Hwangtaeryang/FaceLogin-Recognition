using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseSceneUI_Manager : MonoBehaviour
{
    public static ExerciseSceneUI_Manager instance { get; private set; }

    public Text upTitleText;
    public Text titleText;
    public Text weekText;
    public Text startTimerText;

    public GameObject startPanel;   //ù ���� ȭ��
    public GameObject videoPanel;   //���󳪿��� ȭ��
    public GameObject myImage;  //�� ���ȭ��
    public GameObject resultPanel;  //���ȭ��

    public Button playBtn;
    public Button pauseBtn;
    public Text videoNameText;  //���� �̸� �ؽ�Ʈ
    public Text videoTimerText;
    public Slider sliderTime;
    public Text bodyCountdownText;  //����ȭ�� ī��Ʈ�ٿ� ���� ������ 5����

    int count = 0;
    int restCount = 0;
    int sixCount = 0;

    public bool lastExerciseState;  //������ ����� ����

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        //ExerciseSceneManager.instance.StartPanelTextInit(upTitleText, titleText, weekText);

        //���� �� 5�� �Ŀ� �ȭ������ ����
        PlayTimer.instance.StartTimer(5, startTimerText, startPanel);
    }

    // Update is called once per frame
    void Update()
    {
        //ExerciseTimer();
        //�ش� � �ð���ŭ �����̴��ٰ� �����̰�, � �ð��� �ؽ�Ʈ�� ���´�.
        if(PlayTimer.instance.exerciseStart.Equals(true))
        {
            NextExerciseButtonClick();
            //PlayCoolTime.instance.ExercisePlayingTimer(60f, sliderTime, videoTimerText);
        }
            
    }


    //��� ��ư
    public void PlayButtonClick()
    {
        VideoHandler.instance.PauseVideo(); //�Ͻ�����
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
    }

    //�Ͻ����� ��ư
    public void PauseButtonClick()
    {
        VideoHandler.instance.PlayVideo();  //���
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

    //� ���� �ð�
    //public void ExerciseTimer()
    //{
    //    VideoHandler.instance.ExcerciseTime(videoTimerText);
    //}

    //���� �ϳ� ������ ���� ���ȭ�鿡 �ִ� ������ư Ŭ�� �� �̺�Ʈ
    public void NextExerciseButtonClick()
    {
        PlayTimer.instance.exerciseStart = false;

        restCount += 1;

        //������ ��� �ƴϸ� 
        if (lastExerciseState.Equals(false))
        {
            if(restCount.Equals(6) || restCount.Equals(10) || restCount.Equals(14))
            {

            }
            else
            {
                resultPanel.SetActive(false);
                videoPanel.SetActive(true);
                myImage.SetActive(true);

                count += 1; //Ŭ�� �� �÷���
                PlayerPrefs.SetInt("EP_PlayCount", count);
                int maxtimer = 0;

                if (upTitleText.text.Equals("���� �"))
                    maxtimer = 60;
                else if (upTitleText.text.Equals("�ٷ� � ��ü"))
                    maxtimer = 20;
                else if (upTitleText.text.Equals("�ٷ� � ��ü"))
                    maxtimer = 18;

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

                ExerciseAnnouncementData.instacne.ExerciseName(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType,
                            PlayerPrefs.GetInt("EP_PlayCount"), videoNameText);
                VideoHandler.instance.PlayVideo();

                PlayTimer.instance.PlayingTimer(maxtimer, sliderTime, videoTimerText);
            }
        }
        else
        {
            sixCount += 1;  //6ȸ �ߴ��� ī����

            Debug.Log("��������̿����ϴ�.");
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

            if(exerciseType.Equals("�����"))
            {
                //���� � �� - ���� ��� ���� ó������ ȭ�� ���;���.
            }
            else
            {
                count = 0;
                //� 6ȸ�ݺ�
                if (sixCount != 6)
                {
                    resultPanel.SetActive(false);
                    videoPanel.SetActive(true);
                    myImage.SetActive(true);

                    count += 1; //Ŭ�� �� �÷���
                    PlayerPrefs.SetInt("EP_PlayCount", count);
                    int maxtimer = 0;

                    if (upTitleText.text.Equals("�ٷ� � ��ü"))
                        maxtimer = 20;
                    else if (upTitleText.text.Equals("�ٷ� � ��ü"))
                        maxtimer = 18;

                    string exerciseType2 = "";
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

                    ExerciseAnnouncementData.instacne.ExerciseName(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType2,
                                PlayerPrefs.GetInt("EP_PlayCount"), videoNameText);
                    VideoHandler.instance.PlayVideo();

                    PlayTimer.instance.PlayingTimer(maxtimer, sliderTime, videoTimerText);
                }
                else
                {
                    //6ȸ � �� - ó���� ȭ���� ���;���.
                }
            }
        }
        
    }
}
