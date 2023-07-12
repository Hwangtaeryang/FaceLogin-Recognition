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

    public GameObject startPanel;   //첫 시작 화면
    public GameObject videoPanel;   //영상나오는 화면
    public GameObject myImage;  //내 모습화면
    public GameObject resultPanel;  //결과화면

    public Button playBtn;
    public Button pauseBtn;
    public Text videoNameText;  //영상 이름 텍스트
    public Text videoTimerText;
    public Slider sliderTime;
    public Text bodyCountdownText;  //비디오화면 카운트다운 영상 끝나기 5초전

    int count = 0;
    int restCount = 0;
    int sixCount = 0;

    public bool lastExerciseState;  //마지막 운동인지 여부

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        //ExerciseSceneManager.instance.StartPanelTextInit(upTitleText, titleText, weekText);

        //시작 시 5초 후에 운동화면으로 변경
        PlayTimer.instance.StartTimer(5, startTimerText, startPanel);
    }

    // Update is called once per frame
    void Update()
    {
        //ExerciseTimer();
        //해당 운동 시간만큼 슬라이더바가 움직이고, 운동 시간이 텍스트에 나온다.
        if(PlayTimer.instance.exerciseStart.Equals(true))
        {
            NextExerciseButtonClick();
            //PlayCoolTime.instance.ExercisePlayingTimer(60f, sliderTime, videoTimerText);
        }
            
    }


    //재생 버튼
    public void PlayButtonClick()
    {
        VideoHandler.instance.PauseVideo(); //일시정지
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
    }

    //일시정지 버튼
    public void PauseButtonClick()
    {
        VideoHandler.instance.PlayVideo();  //재생
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

    //운동 영상 시간
    //public void ExerciseTimer()
    //{
    //    VideoHandler.instance.ExcerciseTime(videoTimerText);
    //}

    //동작 하나 끝나고 나서 결과화면에 있는 다음버튼 클릭 시 이벤트
    public void NextExerciseButtonClick()
    {
        PlayTimer.instance.exerciseStart = false;

        restCount += 1;

        //마지막 운동이 아니면 
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

                count += 1; //클릭 수 플러스
                PlayerPrefs.SetInt("EP_PlayCount", count);
                int maxtimer = 0;

                if (upTitleText.text.Equals("율동 운동"))
                    maxtimer = 60;
                else if (upTitleText.text.Equals("근력 운동 상체"))
                    maxtimer = 20;
                else if (upTitleText.text.Equals("근력 운동 하체"))
                    maxtimer = 18;

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

                ExerciseAnnouncementData.instacne.ExerciseName(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType,
                            PlayerPrefs.GetInt("EP_PlayCount"), videoNameText);
                VideoHandler.instance.PlayVideo();

                PlayTimer.instance.PlayingTimer(maxtimer, sliderTime, videoTimerText);
            }
        }
        else
        {
            sixCount += 1;  //6회 했는지 카운팅

            Debug.Log("마지막운동이였씁니다.");
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

            if(exerciseType.Equals("율동운동"))
            {
                //율동 운동 끝 - 율동 운동에 관한 처방전이 화면 나와야함.
            }
            else
            {
                count = 0;
                //운동 6회반복
                if (sixCount != 6)
                {
                    resultPanel.SetActive(false);
                    videoPanel.SetActive(true);
                    myImage.SetActive(true);

                    count += 1; //클릭 수 플러스
                    PlayerPrefs.SetInt("EP_PlayCount", count);
                    int maxtimer = 0;

                    if (upTitleText.text.Equals("근력 운동 상체"))
                        maxtimer = 20;
                    else if (upTitleText.text.Equals("근력 운동 하체"))
                        maxtimer = 18;

                    string exerciseType2 = "";
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

                    ExerciseAnnouncementData.instacne.ExerciseName(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType2,
                                PlayerPrefs.GetInt("EP_PlayCount"), videoNameText);
                    VideoHandler.instance.PlayVideo();

                    PlayTimer.instance.PlayingTimer(maxtimer, sliderTime, videoTimerText);
                }
                else
                {
                    //6회 운동 끝 - 처방전 화면이 나와야함.
                }
            }
        }
        
    }
}
