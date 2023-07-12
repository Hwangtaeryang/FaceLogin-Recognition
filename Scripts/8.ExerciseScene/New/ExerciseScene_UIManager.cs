using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExerciseScene_UIManager : MonoBehaviour
{
    public static ExerciseScene_UIManager instance { get; private set; }


    public Text upExerciseNameText; //�ֻ��� � �̸� �ؽ�Ʈ
    public Text exerciseNameText;   //�����ǳ� � �̸� �ؽ�Ʈ
    public Text startWeekText;  //�����ǳ� ���� �ؽ�Ʈ
    public Text startExerciseNumberText;    //�����ǳ� Ƚ�� �ؽ�Ʈ
    public Text startTimerText;

    public GameObject startPanel;   //�����ǳ�
    public GameObject videoPanel;   //�����ĳ�
    public GameObject myImage;  //���̹��� ȭ��
    public GameObject resultPanel;  //���ȭ�� �ǳ�
    public GameObject restPanel;    //�޽Ľð� �ȳ�
    public GameObject lastResultPanel;  //���� �ƿ� �� ������ �� �������� ���ȭ��
    public GameObject endCountDownObj;  //���� 5��ī��Ʈ�ٿ� ������Ʈ
    public GameObject restNextButtonPanel;  //���½ð� ����� ���� ��ư �ִ� �ǳ�

    public Image startTimeRotateImg;    //�����ǳ� 5�� Ÿ�̸�
    public Image videoTimeRotatImg; //����5�ʳ������� Ÿ�̸�

    public Image endAvergeImg;  //� ������ ��� ���ۿ� ���� ��� ���
    public Sprite[] allClassSprite;   //��� �̹���
    public Text aText;  //A��� ����
    public Text bText;
    public Text cText;
    public Text reportText; //����Ʈ �ؽ�Ʈ

    public Button playBtn;
    public Button pauseBtn;

    public Text videoNameText;  //������� ���� �ؽ�Ʈ
    public Slider videoSlider;  //���� �����̴�
    public Text videoTimeText;  //���� �ð� �ؽ�Ʈ
    public Text EndCountVideoNameText;  //5��ī��Ʈ�ٿ� ������� �ؽ�Ʈ

    public Text resultNameText; //���ȭ�� �������� �ؽ�Ʈ
    //public Text resultAvergeText;   //���ȭ�� ��� ��� �ؽ�Ʈ
    public Image resultAvergeImg;   //���ȭ�� ��� ��� �̹���

    public bool lastMotionState;    //�������������� ����

    int motionCount = 0;    //���° ������ �ߴ��� ī��Ʈ



    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        //� ���� �� �ؽ�Ʈ ���� �ʱ�ȭ
        ExerciseScene_Manager.instance.StartPanelTextInit(upExerciseNameText, exerciseNameText, startWeekText, startExerciseNumberText);
        
        //5��ī��Ʈ
        ExerciseTimer.instance.StartTime(5, startTimerText, startPanel);
    }

    
    void Update()
    {
        //ī��Ʈ �ٿ� 5�ʰ� �����ٸ�
        if(ExerciseTimer.instance.startExercise.Equals(true))
        {
            NextExerciseButtonClick();
        }

        if(startPanel.activeSelf.Equals(true))
            startTimeRotateImg.transform.Rotate(new Vector3(0, 0, -5f * Time.deltaTime * 20));
        else if(endCountDownObj.activeSelf.Equals(true))
            videoTimeRotatImg.transform.Rotate(new Vector3(0, 0, -5f * Time.deltaTime * 20));
    }


    //��� ��ư
    public void PlayButtonClick()
    {
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);

        if (restPanel.activeSelf.Equals(false))
            VideoHandler.instance.PauseVideo(); //�Ͻ�����
        else
            Time.timeScale = 0;
    }

    //�Ͻ����� ��ư
    public void PauseButtonClick()
    {
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);

        if (restPanel.activeSelf.Equals(false))
            VideoHandler.instance.PlayVideo();  //���
        else
            Time.timeScale = 1;
    }

    //���� � Ŭ�� ��ư �̺�Ʈ
    public void NextExerciseButtonClick()
    {
        //Debug.Log("----���� �ȵ����°��� ???");
        //��� ������ ������ �ƴ� ��� - ExerciseContentData.cs���� �˷���
        if (lastMotionState.Equals(false))
        {
           //Debug.Log("���� �ȵ����°��� ???");
            ExerciseTimer.instance.startExercise = false;
            ExerciseTimer.instance.restTimeState = false;

            //����Ʈ �ڽĵ� �ʱ�ȭ
            ExerciseScene_Manager.instance.NarrationListDelete();

            //� ������ �Ҷ� üũ�Ѱ� Ƚ���� ���� ���Դ��� üũ�� ���Ѱ͵� - �ʱ�ȭ
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "");
            PlayerPrefs.SetString("EP_MotionCheckBothBad", "");
            PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "");
            PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "");
            PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "");
            PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", 0);
            PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", 0);
            PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", 0);
            PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", 0);
            PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", 0);
            PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", 0);

            restNextButtonPanel.SetActive(false);
            resultPanel.SetActive(false);   //���ȭ�� ��Ȱ��ȭ
            restPanel.SetActive(false); //�޽�ȭ�� ��Ȱ��ȭ
            videoPanel.SetActive(true); //����ȭ�� Ȱ��ȭ
            myImage.SetActive(true);    //�����ȭ�� Ȱ��ȭ

            //���° ������ �ߴ��� ī��Ʈ���ش�.
            motionCount += 1;
            PlayerPrefs.SetInt("EP_PlayCount", motionCount);

            //Debug.Log("EP_PlayCount " + PlayerPrefs.GetInt("EP_PlayCount"));

            string exerciseType = "";
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
                exerciseType = "�ٷ¿��ü";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
                exerciseType = "�ٷ¿��ü";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
                exerciseType = "�����";

            int videoMaxTimer = 0;  //������� �ð� �ʱ�ȭ

            //���� ��� �̸� �����ֱ� ���� �Լ� ���ܿ�
            ExerciseContentData.instacne.ExerciseVideoNameShow(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType,
                PlayerPrefs.GetInt("EP_PlayCount"), videoNameText, restPanel, videoPanel, myImage);
            EndCountVideoNameText.text = videoNameText.text;    //ī��Ʈ�ٿ� �ؽ�Ʈ���� ������� �̸� �־���

            //�����̴��� ��� �ð� ����
            if (upExerciseNameText.text.Equals("���� �"))
            {
                //�޽Ľð� �ش��� �� �ð��� �ٸ���.
                if (restPanel.activeSelf.Equals(true))
                {
                    if (PlayerPrefs.GetInt("EP_AllWeekNumber") <= 4)
                        videoMaxTimer = 200;
                    else if (PlayerPrefs.GetInt("EP_AllWeekNumber") > 4 && PlayerPrefs.GetInt("EP_AllWeekNumber") <= 8)
                        videoMaxTimer = 150;
                    else if (PlayerPrefs.GetInt("EP_AllWeekNumber") > 8 && PlayerPrefs.GetInt("EP_AllWeekNumber") <= 12)
                        videoMaxTimer = 120;
                }
                else
                    videoMaxTimer = 60;
            }
            else if (upExerciseNameText.text.Equals("�ٷ� � ��ü"))
            {
                videoMaxTimer = 20;
            }
            else if (upExerciseNameText.text.Equals("�ٷ� � ��ü"))
            {
                videoMaxTimer = 18;
            }

            //Debug.Log("�̰� �ι� ������ ????");
            //���� �ִ�ð�, �����̴���, �ð��� ������, ���� ���°���� 
            ExerciseTimer.instance.PlayingTime(videoMaxTimer, videoSlider, videoTimeText);
        }
        else
        {
            //Debug.Log("?????? ???");
            //��� ������ �����̸� ���� ��� ��հ� ���� �����Ͱ� ���;���
            ExerciseTimer.instance.startExercise = false;

            //����Ʈ �ڽĵ� �ʱ�ȭ
            ExerciseScene_Manager.instance.NarrationListDelete();

            //� ������ �Ҷ� üũ�Ѱ� Ƚ���� ���� ���Դ��� üũ�� ���Ѱ͵� - �ʱ�ȭ
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "");
            PlayerPrefs.SetString("EP_MotionCheckBothBad", "");
            PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "");
            PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "");
            PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "");
            PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", 0);
            PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", 0);
            PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", 0);
            PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", 0);
            PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", 0);
            PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", 0);

            resultPanel.SetActive(false);   //���ȭ�� ��Ȱ��ȭ
            restPanel.SetActive(false); //�޽�ȭ�� ��Ȱ��ȭ
            videoPanel.SetActive(false); //����ȭ�� ��Ȱ��ȭ
            myImage.SetActive(false);    //�����ȭ�� ��Ȱ��ȭ
            lastResultPanel.SetActive(true);    //��� ������ �� ������ ���� ����� �����ִ� ȭ�� Ȱ��ȭ 

            //���° ������ �ߴ��� ī��Ʈ���ش�.
            motionCount = 0;
            PlayerPrefs.SetInt("EP_PlayCount", motionCount);

            //Debug.Log("EP_PlayCount " + PlayerPrefs.GetInt("EP_PlayCount"));

            string exerciseType = "";
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
                exerciseType = "�ٷ¿��ü";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
                exerciseType = "�ٷ¿��ü";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
                exerciseType = "�����";

            int videoMaxTimer = 0;  //������� �ð� �ʱ�ȭ

            LastResultPenelInit();
        }
        
    }

    //���ȭ�鿡 �ִ� ������ �ʱ�ȭ(��, ������Ʈ)
    public void ResultPenelInit()
    {
        ExerciseScene_Manager.instance.ResultPanelDataUpdata(resultNameText, resultAvergeImg);
    }


    //���� ���� ������ �� ��� ���ۿ� ���� ���� ���ȭ�鿡 ���� ����
    public void LastResultPenelInit()
    {
        float allAvrage = ExerciseScene_Manager.instance.LastResultPanelDataUpdata(aText, bText, cText);
        ExerciseScene_Manager.instance.LastResultPanelDataReportUpdata(reportText);
        Debug.Log("2�� :  "+allAvrage);

        if (allAvrage >= 16f)
            endAvergeImg.sprite = allClassSprite[0];    //a
        else if (allAvrage < 16f && allAvrage >= 14f)
            endAvergeImg.sprite = allClassSprite[1];    //a-
        else if (allAvrage < 14f && allAvrage >= 12f)
            endAvergeImg.sprite = allClassSprite[2];    // b+
        else if (allAvrage < 12f && allAvrage >= 10f)
            endAvergeImg.sprite = allClassSprite[3];    //b
        else if (allAvrage < 10 && allAvrage >= 8f)
            endAvergeImg.sprite = allClassSprite[4];    //b-
        else if (allAvrage < 8f && allAvrage >= 6f)
            endAvergeImg.sprite = allClassSprite[5];    //c+
        else if (allAvrage < 6f && allAvrage >= 5f)
            endAvergeImg.sprite = allClassSprite[6]; //c
        else
            endAvergeImg.sprite = allClassSprite[7];

    }

    public void ReportPageGoButtonClick()
    {
        ExerciseScene_Manager.instance.LastDataSaveUpdata();    //��� ���� �ϳ��ϳ� ��� ������Ʈ
        ExerciseScene_Manager.instance.UserInfoLastSaveUpdata();    //����� ������ ������ ������Ʈ ��Ű��
        PlayerPrefs.SetString("EP_CErrorClassGroup", "");
        SceneManager.LoadScene("5.UserExercise");
    }

    //������ ��ư Ŭ�� ��
    public void ExerciseGoOutButtonClick()
    {
        SceneSoundCtrl.instance.MainBGM_Sound();
        if (videoPanel.activeSelf.Equals(true))
            VideoHandler.instance.PauseVideo(); //�Ͻ�����
        else
            Time.timeScale = 0;
    }

    //��� ��ư Ŭ�� ��
    public void ExercisePopupCloaseButtonClick()
    {
        if (videoPanel.activeSelf.Equals(true))
            VideoHandler.instance.PlayVideo();  //���
        else
            Time.timeScale = 1;
    }

    public void ExerciseStop()
    {
        SceneSoundCtrl.instance.MainBGM_Sound();
        SceneManager.LoadScene("5.UserExercise");
    }
}
