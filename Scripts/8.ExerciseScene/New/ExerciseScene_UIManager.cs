using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExerciseScene_UIManager : MonoBehaviour
{
    public static ExerciseScene_UIManager instance { get; private set; }


    public Text upExerciseNameText; //최상위 운동 이름 텍스트
    public Text exerciseNameText;   //시작판넬 운동 이름 텍스트
    public Text startWeekText;  //시작판넬 주차 텍스트
    public Text startExerciseNumberText;    //시작판넬 횟수 텍스트
    public Text startTimerText;

    public GameObject startPanel;   //시작판넬
    public GameObject videoPanel;   //비디오파넬
    public GameObject myImage;  //내이미지 화면
    public GameObject resultPanel;  //결과화면 판넬
    public GameObject restPanel;    //휴식시간 팔넬
    public GameObject lastResultPanel;  //동작 아예 다 끝났을 때 보여지는 결과화면
    public GameObject endCountDownObj;  //동작 5초카운트다운 오브젝트
    public GameObject restNextButtonPanel;  //쉬는시간 다음운동 시작 버튼 있는 판넬

    public Image startTimeRotateImg;    //시작판넬 5초 타이머
    public Image videoTimeRotatImg; //동작5초남았을때 타이머

    public Image endAvergeImg;  //운동 끝나고 모든 동작에 관한 평균 등급
    public Sprite[] allClassSprite;   //등급 이미지
    public Text aText;  //A등급 갯수
    public Text bText;
    public Text cText;
    public Text reportText; //리포트 텍스트

    public Button playBtn;
    public Button pauseBtn;

    public Text videoNameText;  //재생비디오 제목 텍스트
    public Slider videoSlider;  //비디오 슬라이더
    public Text videoTimeText;  //비디오 시간 텍스트
    public Text EndCountVideoNameText;  //5초카운트다운 재생제목 텍스트

    public Text resultNameText; //결과화면 비디오제목 텍스트
    //public Text resultAvergeText;   //결과화면 평균 등급 텍스트
    public Image resultAvergeImg;   //결과화면 평균 등급 이미지

    public bool lastMotionState;    //마지막동작인지 상태

    int motionCount = 0;    //몇번째 동작을 했는지 카운트



    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        //운동 시작 시 텍스트 정보 초기화
        ExerciseScene_Manager.instance.StartPanelTextInit(upExerciseNameText, exerciseNameText, startWeekText, startExerciseNumberText);
        
        //5초카운트
        ExerciseTimer.instance.StartTime(5, startTimerText, startPanel);
    }

    
    void Update()
    {
        //카운트 다운 5초가 끝났다면
        if(ExerciseTimer.instance.startExercise.Equals(true))
        {
            NextExerciseButtonClick();
        }

        if(startPanel.activeSelf.Equals(true))
            startTimeRotateImg.transform.Rotate(new Vector3(0, 0, -5f * Time.deltaTime * 20));
        else if(endCountDownObj.activeSelf.Equals(true))
            videoTimeRotatImg.transform.Rotate(new Vector3(0, 0, -5f * Time.deltaTime * 20));
    }


    //재생 버튼
    public void PlayButtonClick()
    {
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);

        if (restPanel.activeSelf.Equals(false))
            VideoHandler.instance.PauseVideo(); //일시정지
        else
            Time.timeScale = 0;
    }

    //일시정지 버튼
    public void PauseButtonClick()
    {
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);

        if (restPanel.activeSelf.Equals(false))
            VideoHandler.instance.PlayVideo();  //재생
        else
            Time.timeScale = 1;
    }

    //다음 운동 클릭 버튼 이벤트
    public void NextExerciseButtonClick()
    {
        //Debug.Log("----여기 안들어오는거임 ???");
        //운동의 마지막 동작이 아닐 경우 - ExerciseContentData.cs에서 알려줌
        if (lastMotionState.Equals(false))
        {
           //Debug.Log("여기 안들어오는거임 ???");
            ExerciseTimer.instance.startExercise = false;
            ExerciseTimer.instance.restTimeState = false;

            //리스트 자식들 초기화
            ExerciseScene_Manager.instance.NarrationListDelete();

            //운동 동작을 할때 체크한거 횟수랑 뭐가 나왔는지 체크를 위한것들 - 초기화
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
            resultPanel.SetActive(false);   //결과화면 비활성화
            restPanel.SetActive(false); //휴식화면 비활성화
            videoPanel.SetActive(true); //비디오화면 활성화
            myImage.SetActive(true);    //내모습화면 활성화

            //몇번째 동작을 했는지 카운트해준다.
            motionCount += 1;
            PlayerPrefs.SetInt("EP_PlayCount", motionCount);

            //Debug.Log("EP_PlayCount " + PlayerPrefs.GetInt("EP_PlayCount"));

            string exerciseType = "";
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
                exerciseType = "근력운동상체";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
                exerciseType = "근력운동하체";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
                exerciseType = "율동운동";

            int videoMaxTimer = 0;  //비디오재생 시간 초기화

            //비디오 재생 이름 보여주기 위한 함수 땡겨옴
            ExerciseContentData.instacne.ExerciseVideoNameShow(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType,
                PlayerPrefs.GetInt("EP_PlayCount"), videoNameText, restPanel, videoPanel, myImage);
            EndCountVideoNameText.text = videoNameText.text;    //카운트다운 텍스트에서 비디오재생 이름 넣어줌

            //슬라이더바 재생 시간 설정
            if (upExerciseNameText.text.Equals("율동 운동"))
            {
                //휴식시간 해당할 때 시간은 다르다.
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
            else if (upExerciseNameText.text.Equals("근력 운동 상체"))
            {
                videoMaxTimer = 20;
            }
            else if (upExerciseNameText.text.Equals("근력 운동 하체"))
            {
                videoMaxTimer = 18;
            }

            //Debug.Log("이게 두번 들어오나 ????");
            //비디오 최대시간, 슬라이더바, 시간을 보여줌, 동작 몇번째인지 
            ExerciseTimer.instance.PlayingTime(videoMaxTimer, videoSlider, videoTimeText);
        }
        else
        {
            //Debug.Log("?????? ???");
            //운동의 마지막 동작이면 최종 운동의 평균과 운동결과 리포터가 나와야함
            ExerciseTimer.instance.startExercise = false;

            //리스트 자식들 초기화
            ExerciseScene_Manager.instance.NarrationListDelete();

            //운동 동작을 할때 체크한거 횟수랑 뭐가 나왔는지 체크를 위한것들 - 초기화
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

            resultPanel.SetActive(false);   //결과화면 비활성화
            restPanel.SetActive(false); //휴식화면 비활성화
            videoPanel.SetActive(false); //비디오화면 비활성화
            myImage.SetActive(false);    //내모습화면 비활성화
            lastResultPanel.SetActive(true);    //모든 동작이 다 끝나고 통합 결과를 보여주는 화면 활성화 

            //몇번째 동작을 했는지 카운트해준다.
            motionCount = 0;
            PlayerPrefs.SetInt("EP_PlayCount", motionCount);

            //Debug.Log("EP_PlayCount " + PlayerPrefs.GetInt("EP_PlayCount"));

            string exerciseType = "";
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
                exerciseType = "근력운동상체";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
                exerciseType = "근력운동하체";
            else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
                exerciseType = "율동운동";

            int videoMaxTimer = 0;  //비디오재생 시간 초기화

            LastResultPenelInit();
        }
        
    }

    //결과화면에 있는 정보를 초기화(즉, 업데이트)
    public void ResultPenelInit()
    {
        ExerciseScene_Manager.instance.ResultPanelDataUpdata(resultNameText, resultAvergeImg);
    }


    //최종 동작 끝났을 때 모든 동작에 관한 통합 결과화면에 정보 제공
    public void LastResultPenelInit()
    {
        float allAvrage = ExerciseScene_Manager.instance.LastResultPanelDataUpdata(aText, bText, cText);
        ExerciseScene_Manager.instance.LastResultPanelDataReportUpdata(reportText);
        Debug.Log("2차 :  "+allAvrage);

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
        ExerciseScene_Manager.instance.LastDataSaveUpdata();    //운동의 동작 하나하나 등급 업데이트
        ExerciseScene_Manager.instance.UserInfoLastSaveUpdata();    //사용자 정보에 데이터 업데이트 시키기
        PlayerPrefs.SetString("EP_CErrorClassGroup", "");
        SceneManager.LoadScene("5.UserExercise");
    }

    //나가기 버튼 클릭 시
    public void ExerciseGoOutButtonClick()
    {
        SceneSoundCtrl.instance.MainBGM_Sound();
        if (videoPanel.activeSelf.Equals(true))
            VideoHandler.instance.PauseVideo(); //일시정지
        else
            Time.timeScale = 0;
    }

    //취소 버튼 클릭 시
    public void ExercisePopupCloaseButtonClick()
    {
        if (videoPanel.activeSelf.Equals(true))
            VideoHandler.instance.PlayVideo();  //재생
        else
            Time.timeScale = 1;
    }

    public void ExerciseStop()
    {
        SceneSoundCtrl.instance.MainBGM_Sound();
        SceneManager.LoadScene("5.UserExercise");
    }
}
