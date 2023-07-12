using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionCustomAlarm : MonoBehaviour
{
    public static MotionCustomAlarm instance { get; private set; }

    public ExerciseScene_Manager exerSceneManager;

    int wrist_L_Before, wrist_R_Before; //손목 전에 움직인 각도
    int wrist_L_Curr, wrist_R_Curr; //손목 지금 움직인 각도
    int wrist_L_Gap, wrist_R_Gap;   //손목 움직인 갭

    int shoulder_L_Before, shoulder_R_Before;   //어깨 전에 움직인 각도
    int shoulder_L_Curr, shoulder_R_Curr;   //어깨 지금 움직인 각도
    int shoulder_L_Gap, shoulder_R_Gap; //어깨 움직인 갭

    bool l_Motion_Good, r_Motion_Good;  //왼쪽 오른쪽 모션이 잘하고 있는 상태여부
    bool allAotion_Good;   //동작 잘한다! 여부
    bool l_MotionDown_Bad, l_MotionUp_Bad;  //왼쪽 모션 아래, 위 안좋다
    bool r_MotionDown_Bad, r_MotionUp_Bad;  //오른쪽모션 아래, 위 안좋다
    bool allSpin_Bad;   //돈다안돈다

    bool dir_State;  //방향 빈 껍데기(전 방향)
    bool dir_Up; //방향 위 상태(현재 방향)

    float voiceTimer;   //목소리 나오는 시간(한번 멘트할 때 3~4초의 텀을 주기위해)


    //테스트 좌표 확인용 텍스트
    public Text leftText;
    public Text rightText;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    void Start()
    {

    }


    void Update()
    {

    }

    string GoodNarrationSay()
    {
        string goolName = "";
        //1. 정말잘했어요  2.잘따라오고 있어요  3. 정말멋져요   4. 아주잘하고있어요,조금만더힘내요
        //5. 운동하는 모습이건강해보여요  6. 힘들면 운동을 멈추고 쉬어주세요  7.운동중 다치지 않게 주의해주세요.
        int number = Random.Range(1, 8);

        if (number.Equals(1)) goolName = "140 정말 잘했어요!";
        else if (number.Equals(2)) goolName = "141 잘 따라오고 있어요!";
        else if (number.Equals(3)) goolName = "142 정말 멋져요!";
        else if (number.Equals(4)) goolName = "144 아주 잘하고 있어요, 조금만 더 힘내봐요!";
        else if (number.Equals(5)) goolName = "145 운동하는 모습이 건강해보여요!";
        else if (number.Equals(6)) goolName = "148 힘들면 운동을 멈추고 쉬어주세요.";
        else if (number.Equals(7)) goolName = "152 운동중 다치지 않게 주의해주세요.";

        return goolName;
    }

    //float errVoiceTimer;
    //율동 운동 - 1. 손목 털기 확인 함수
    public void ShakeWristMove(int _index, float _angle)
    {
        //< 왼쪽 손목 >
        if (_index.Equals(6))
        {
            //Debug.Log("왼쪽 체크시작");
            wrist_L_Curr = (int)_angle;
            wrist_L_Gap = wrist_L_Before - wrist_L_Curr;

            //잘못하고 있다. 
            if ((Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 15))
            {
                //잘못한 동작이 2.5초 이상이 되면 수정하라는 내레이션이 나온다.
                if (exerSceneManager.missTime_L >= 4f)
                {
                    l_Motion_Good = false;
                    allAotion_Good = false; // 모션  틀렸다.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
            //잘하고 있다.
            else if (Mathf.Abs(wrist_L_Gap) >= 15)
            {
                if(exerSceneManager.successTime_L >= 3f)
                {
                    l_Motion_Good = true;
                    exerSceneManager.missTime_L = 0;    //잘하고 있으니 계속 0시간이라고 초기화
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
        }
        //< 오른쪾 손목 >
        if (_index.Equals(7))
        {
            //Debug.Log("오른쪽 체크시작");
            wrist_R_Curr = (int)_angle;
            wrist_R_Gap = wrist_R_Before - wrist_R_Curr;

            //잘못하고 있다. 
            if ((Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 15))
            {
                //잘못한 동작이 2.5초 이상이 되면 수정하라는 내레이션이 나온다.
                if (exerSceneManager.missTime_R >= 4f)
                {
                    r_Motion_Good = false;
                    allAotion_Good = false; // 모션  틀렸다.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
            //잘하고 있다.
            else if (Mathf.Abs(wrist_R_Gap) >= 15)
            {
                if(exerSceneManager.successTime_R >= 3f)
                {
                    r_Motion_Good = true;
                    exerSceneManager.missTime_R = 0;    //잘하고 있으니 계속 0시간이라고 초기화
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
        }
        WristNarrationSoundStart();
    }
    void WristNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else
        {
            allAotion_Good = false;
        }

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if(allAotion_Good.Equals(false))
        {
            //양쪽이 잘못했으면 손목털기
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "85 끝까지 동작을 유지하세요");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "끝까지 동작을 유지하세요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            //오른쪽이 잘못했으면
            else if (r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_1_오른쪽_손목을_털어주세요_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른쪽 손목을 털어주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
            //왼쪽이 잘못했으면
            else if (l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_0_왼쪽_손목을_털어주세요_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼쪽 손목을 털어주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 2. 팔 앞으로 뻗기 함수
    //_way - 방향 : 양수/위 , 음수/아래 (왼)
    //_way - 방향 : 양수/아래, 음수/위 (오)
    public void StretchArmsForward(int _index, float _angle, float _way)
    {
        //<왼쪽>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            leftText.text = "왼쪽어깨 " + (int)_angle + "  " + _way;
            if (_way < -230)    //음수 : 아래
            {
                if (exerSceneManager.missTime_L >= 3f)
                {
                    //왼팔을 올려주세요.
                    l_Motion_Good = false;
                    l_MotionDown_Bad = true;   //왼쪽 내려갔음
                    //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                    exerSceneManager.missTime_L = 0;    //잘하고 있어서 시간 0으로 초기화
                }
            }
            //양수 : 위
            else
            {
                if (shoulder_L_Curr >= 120f)// && shoulder_L_Curr <= 155f)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                        //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 120f)
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔 더 올려주세요
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;   //왼쪽 내려갔음
                        //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
                //else if (shoulder_L_Curr > 155f)
                //{
                //    if (exerSceneManager.missTime_L >= 3f)
                //    {
                //        //왼팔 더 내려주세요
                //        l_Motion_Good = false;
                //        l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                //        l_MotionUp_Bad = true; //왼쪽 올라갔음.
                //        exerSceneManager.missTime_L = 0;    //체크를 했으니깐 초기화 0
                //        exerSceneManager.successTime_L = 0;
                //    }
                //}
            }
        }

        //<오른쪽>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            rightText.text = "오른쪽어깨 " + (int)_angle + "  " + _way;
            //if (_way > 0)    //양수 : 아래
            //{
            //    if (exerSceneManager.missTime_R >= 3f)
            //    {
            //        오른팔을 올려주세요.
            //        r_Motion_Good = false;
            //        r_MotionDown_Bad = true;   //오른쪽 내려갔음
            //        r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
            //        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
            //    }
            //}
            //양수 : 위
            //else
            {
                if (shoulder_R_Curr >= 125f)// && shoulder_R_Curr <= 150f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 125f)
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔 더 올려주세요
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;   //오른쪽 내려갔음
                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                //else if (shoulder_R_Curr > 150f)
                //{
                //    if (exerSceneManager.missTime_R >= 3f)
                //    {
                //        //오른팔 더 내려주세요
                //        r_Motion_Good = false;
                //        r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                //        r_MotionUp_Bad = true; //오른쪽 올라갔음.
                //        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                //        exerSceneManager.successTime_R =0;
                //    }
                //}
            }
        }
        StretchArmsNarrationSoundStart();
    }
    void StretchArmsNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if ((l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15))
            {
                //양쪽 다 잘못하고 있음.
                SoundMaixerManager.instance.PlayNarration("율동운동", "86 팔을 어깨까지 올려주세요.");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 어깨까지 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
            {
                //왼쪽팔 더 올려야함
                SoundMaixerManager.instance.PlayNarration("율동운동", "87 왼쪽팔을 어깨높이까지 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼쪽팔을 어깨높이까지 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //오른쪽팔 더 올려야함.
                SoundMaixerManager.instance.PlayNarration("율동운동", "89 오른팔을 어깨높이까지 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 어깨높이까지 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }

    string[] timeArr;
    //율동 운동 - 3. 손목 꺾기
    //_way - 방향 : 손목 위로 꺾기(양수), 손목 아래로 꺾기(음수) (오)
    public void WristBreak(int _index, float _angle, float _way, Text _videoTimeText)
    {

        //if(_index.Equals(6))
        //{
        //    rightText.text = "오른쪽손목 " + _angle + "  " + _way;
        //}

        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //오른쪽 손목 검증 시간 구간
        if ((videoTimer >= 25 && videoTimer <= 30) || (videoTimer >= 38 && videoTimer <= 42) ||
            (videoTimer >= 49 && videoTimer <= 53))
        {
            exerSceneManager.missTime_L = 0;
            //오른쪽 손목 꺾기
            if (_index.Equals(7))
            {
                rightText.text = "오른쪽손목 " + _angle + "  " + _way;
                wrist_R_Curr = (int)_angle;
                if (_way > 0)//양수 손목 위로 꺾음
                {
                    if (wrist_R_Curr >= 120f)    //손목 잘 꺾었음
                    {
                        if(exerSceneManager.successTime_L >= 3f)
                        {
                            r_Motion_Good = true;
                            l_Motion_Good = true;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                    else
                    {
                        //손목을 꺾으세요.
                        if (exerSceneManager.missTime_R > 3f)
                        {
                            r_Motion_Good = false;
                            l_Motion_Good = true;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
                else
                {
                    //손목을 꺾으세요.
                    if (exerSceneManager.missTime_R > 3f)
                    {
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
            }
        }
        else if ((videoTimer >= 32 && videoTimer <= 37) || (videoTimer >= 44 && videoTimer <= 48) ||
            (videoTimer >= 54 && videoTimer <= 58))
        {
            exerSceneManager.missTime_R = 0;
            //왼쪽 손목 꺾기
            if (_index.Equals(6))
            {
                leftText.text = "왼쪽손목 " + _angle + "   " + _way;

                wrist_L_Curr = (int)_angle;

                if (_way > -40)  //왼쪽 손목 위로 꺾음
                {
                    if (wrist_L_Curr >= 120f)
                    {
                        if(exerSceneManager.successTime_R >= 3f)
                        {
                            l_Motion_Good = true;
                            r_Motion_Good = true;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                    else
                    {
                        //손목을 꺾으세요.
                        if (exerSceneManager.missTime_L > 3f)
                        {
                            l_Motion_Good = false;
                            r_Motion_Good = true;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
                else
                {
                    //손목을 꺾으세요.
                    if (exerSceneManager.missTime_L > 3f)
                    {
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_R = 0;
                    }
                }
            }
        }
        WristBreakNarrationSoundStart();
    }
    void WristBreakNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 10)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(true) && voiceTimer >= 10)
            {
                //왼손을 돌려주세요
                SoundMaixerManager.instance.PlayNarration("율동운동", "왼쪽 손목을 꺾어주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼쪽 손목을 꺾어주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && l_Motion_Good.Equals(true) && voiceTimer >= 10)
            {
                //오른손으로 돌려주세요
                SoundMaixerManager.instance.PlayNarration("율동운동", "오른쪽 손목을 꺾어주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른쪽 손목을 꺾어주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 4. 손목 돌리기
    //_way - 방향 : 양수/위 , 음수/아래 (왼)
    //_way - 방향 : 양수/아래, 음수/위 (오)
    public void TurningWristMove(int _index, float _angle, float _way)
    {
        //<왼쪽>
        if (_index.Equals(6))
        {
            if (_way > 0)
                dir_Up = true;  //양수 방향 위
            else
                dir_Up = false; //음수 방향 아래

            wrist_L_Curr = (int)_angle;
            wrist_L_Gap = wrist_L_Before - wrist_L_Curr;
            leftText.text = "왼손목 " + (int)_angle + "  " + _way + " 갭 " + Mathf.Abs(wrist_L_Gap);

            if (Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 10)
            {
                if (exerSceneManager.missTime_L > 3f)
                {
                    //왼쪽 어깨 돌려주세용
                    l_Motion_Good = false;  //움직이지 않는다.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
            else if (Mathf.Abs(wrist_L_Gap) >= 15)
            {
                //if(exerSceneManager.successTime_L >= 3f)
                {
                    //굳굳
                    l_Motion_Good = true;  //움직이다.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
        }

        //<오른쪽>
        if (_index.Equals(7))
        {
            if (_way > 0)
                dir_Up = true;  //양수 방향 위
            else
                dir_Up = false; //음수 방향 아래

            wrist_R_Curr = (int)_angle;
            wrist_R_Gap = wrist_R_Before - wrist_R_Curr;
            rightText.text = "오손목 " + (int)_angle + "  " + _way + " 갭 " + Mathf.Abs(wrist_R_Gap);

            if (Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 10)
            {
                if (exerSceneManager.missTime_R > 3f)
                {
                    //오른쪽 어깨 돌려주세용
                    r_Motion_Good = false;  //움직이지 않는다.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
            else if (Mathf.Abs(wrist_R_Gap) >= 15)
            {
                //if(exerSceneManager.successTime_R >= 3f)
                {
                    //굳굳
                    r_Motion_Good = true;  //움직이다.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
        }
        TurningWristNarrationSoundStart();
    }
    void TurningWristNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                //양쪽다 돌려주세요.
                SoundMaixerManager.instance.PlayNarration("율동운동", "85 끝까지 동작을 유지하세요");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "끝까지 동작을 유지하세요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //왼손을 돌려주세요
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_2_왼쪽_손목을_돌려주세요_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼쪽 손목을 돌려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && l_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //오른손으로 돌려주세요
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_3_오른쪽_손목을_돌려주세요_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른쪽 손목을 돌려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 5. 어깨돌리기 함수
    public void TurningShoulders(int _index, float _angle)
    {
        //왼쪽 어깨
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            shoulder_L_Gap = shoulder_L_Before - shoulder_L_Curr;
            leftText.text = "오손목 " + (int)_angle + " 갭 " + Mathf.Abs(shoulder_L_Gap);

            if (Mathf.Abs(shoulder_L_Gap).Equals(shoulder_L_Curr) || Mathf.Abs(shoulder_L_Gap) < 5)
            {
                if (exerSceneManager.missTime_L > 3f)
                {
                    //왼쪽 어깨 돌려주세용
                    l_Motion_Good = false;  //움직이지 않는다.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                shoulder_L_Before = shoulder_L_Curr;
            }
            else if (Mathf.Abs(shoulder_L_Gap) >= 10)
            {
                //if(exerSceneManager.successTime_L >= 3f)
                {
                    //굳굳
                    l_Motion_Good = true;  //움직이다.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                shoulder_L_Before = shoulder_L_Curr;
            }
        }

        //오른쪽 어깨
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            shoulder_R_Gap = shoulder_R_Before - shoulder_R_Curr;
            rightText.text = "오손목 " + (int)_angle + " 갭 " + Mathf.Abs(shoulder_R_Gap);

            if (Mathf.Abs(shoulder_R_Gap).Equals(shoulder_R_Curr) || Mathf.Abs(shoulder_R_Gap) < 5)
            {
                if (exerSceneManager.missTime_R > 3f)
                {
                    //오른쪽 어깨 돌려주세용
                    r_Motion_Good = false;  //움직이지 않는다.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                shoulder_R_Before = shoulder_R_Curr;
            }
            else if (Mathf.Abs(shoulder_R_Gap) >= 10)
            {
                //if(exerSceneManager.successTime_R >= 3f)
                {
                    //굳굳
                    r_Motion_Good = true;  //움직이다.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                shoulder_R_Before = shoulder_R_Curr;
            }
        }
        TurningShouldersNarrationSoundStart();
    }
    void TurningShouldersNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "85 끝까지 동작을 유지하세요");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "끝까지 동작을 유지하세요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //왼쪽어깨 돌리기
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_4_왼쪽_어깨를_돌려주세요_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼쪽_어깨를_돌려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && l_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //오른쪽어깨 돌리기
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_5_오른쪽_어깨를_돌려주세요_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른쪽_어깨를_돌려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 6.팔을 위로 올리기/내리기
    public void ArmUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //오른쪽 팔 검증 시간 구간
        if ((videoTimer >= 16 && videoTimer <= 19) || (videoTimer >= 25 && videoTimer <= 27) ||
            (videoTimer >= 33 && videoTimer <= 36) || (videoTimer >= 40 && videoTimer <= 43) ||
            (videoTimer >= 49 && videoTimer <= 51))
        {
            //<오른쪽>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + _angle + "  " + _way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔을 올려주세요.
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125f)
                    {
                        r_Motion_Good = true;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                    }
                }
            }
        }
        else if ((videoTimer >= 20 && videoTimer <= 23) || (videoTimer >= 29 && videoTimer <= 31) ||
            (videoTimer >= 37 && videoTimer <= 39) || (videoTimer >= 44 && videoTimer <= 46) ||
            (videoTimer >= 52 && videoTimer <= 54))
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + _angle + "  " + _way;
                if (_way < 0) //음수 : 아래
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔을 올려줴숑.
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        l_Motion_Good = true;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
            }
        }
        ArmUpDownNarrationSoundStart();
    }
    void ArmUpDownNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(true) && l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 7. 팔을 앞으로 올리기/내리기
    public void ArmFrontUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //오른쪽 팔 검증 시간 구간
        if ((videoTimer >= 15 && videoTimer <= 18) || (videoTimer >= 23 && videoTimer <= 26) ||
            (videoTimer >= 32 && videoTimer <= 35) || (videoTimer >= 39 && videoTimer <= 42) ||
            (videoTimer >= 46 && videoTimer <= 48))
        {
            //<오른쪽>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + _angle + "  " + _way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔을 올려주세요.
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125f)
                    {
                        r_Motion_Good = true;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                    }
                }
            }
        }
        else if ((videoTimer >= 20 && videoTimer <= 22) || (videoTimer >= 28 && videoTimer <= 30) ||
            (videoTimer >= 35 && videoTimer <= 37) || (videoTimer >= 42 && videoTimer <= 44) ||
            (videoTimer >= 49 && videoTimer <= 51))
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + _angle + "  " + _way;
                if (_way < 0) //음수 : 아래
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔을 올려줴숑.
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        l_Motion_Good = true;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
            }
        }

        ArmFrontUpDownSoundStart();
    }
    void ArmFrontUpDownSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(true) && l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 8. 팔을 옆으로 올리기/내리기
    public void ArmSideUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //오른쪽 팔 검증 시간 구간
        if ((videoTimer >= 15 && videoTimer <= 22) || (videoTimer >= 28 && videoTimer <= 32) ||
            (videoTimer >= 38 && videoTimer <= 41) || (videoTimer >= 48 && videoTimer <= 52))
        {
            //<오른쪽>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + _angle + "  " + _way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔을 올려주세요.
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 160f)
                    {
                        r_Motion_Good = true;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                    }
                }
            }
        }
        else if ((videoTimer >= 23 && videoTimer <= 27) || (videoTimer >= 34 && videoTimer <= 37) ||
            (videoTimer >= 43 && videoTimer <= 47) || (videoTimer >= 53 && videoTimer <= 57))
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + _angle + "  " + _way;
                if (_way < 0) //음수 : 아래
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔을 올려줴숑.
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr >= 140f)
                    {
                        l_Motion_Good = true;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
            }
        }

        ArmSideUpDownSoundStart();
    }
    void ArmSideUpDownSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(true) && l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 9. 양팔을 위로 올리기/내리기
    public void BothArmUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);



        //양팔이 위로 올라가 있어야하는 구간
        if ((videoTimer >= 15 && videoTimer <= 20) || (videoTimer >= 24 && videoTimer <= 29) ||
            (videoTimer >= 33 && videoTimer <= 37) || (videoTimer >= 41 && videoTimer <= 45) ||
            (videoTimer >= 50 && videoTimer <= 54))
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way < -230) //230보다 작으면 차렷//음수 : 아래
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔을 올려줴숑.
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;    // 왼팔이 내려갓다.
                        //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        if(exerSceneManager.successTime_L >= 3f)
                        {
                            l_Motion_Good = true;
                            l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                                                        //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //오른쪽
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔을 올려주세요.
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;    // 오른팔이 내려갓다.
                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125)
                    {
                        if(exerSceneManager.successTime_R >= 3f)
                        {
                            r_Motion_Good = true;
                            r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                                                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
            }
        }
        else
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way < -230f) //230보다 작으면 차렷//음수 : 아래
                {
                    if (exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionUp_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                                                  //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    //if (shoulder_L_Curr > 120f)
                    {
                        if (exerSceneManager.missTime_L >= 3f)
                        {
                            //왼팔을 올려줴숑.
                            l_Motion_Good = false;
                            l_MotionUp_Bad = true;    // 왼팔이 내려갓다.
                                                      //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //오른쪽
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionUp_Bad = false;   //오른쪽 내려가서 자세좋음
                                                  //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    //if (shoulder_R_Curr >= 120f)
                    {
                        if (exerSceneManager.missTime_R >= 3f)
                        {
                            //오른팔을 올려주세요.
                            r_Motion_Good = false;
                            r_MotionUp_Bad = true;    // 오른팔이 내려가지않음 안좋음
                                                      //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0고 있어서 시간 0으로 초기화
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
            }
        }
        BothArmUpDownNarrationSoundStart(videoTimer);
    }
    void BothArmUpDownNarrationSoundStart(float _videoTimer)
    {
        voiceTimer += Time.deltaTime;
        //errVoiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if ((_videoTimer >= 15 && _videoTimer <= 20) || (_videoTimer >= 24 && _videoTimer <= 29) ||
            (_videoTimer >= 33 && _videoTimer <= 37) || (_videoTimer >= 41 && _videoTimer <= 45) ||
            (_videoTimer >= 50 && _videoTimer <= 54))
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                r_Motion_Good = false; l_Motion_Good = false; allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //양팔이 내려와있다. 둘다 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "106 팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //왼팔이 내려와있다. 왼팔 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 위로 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //오른팔이 내려왔다. 오른팔 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 위로 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
        else 
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                //SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                //PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
                //PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                r_Motion_Good = false; l_Motion_Good = false; allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                Debug.Log("1여기 안들어오나 ???????");
                if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //양팔이 올라가있다. 둘다 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "팔을 아래로 내려주세요-");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //왼팔이 올라가있다. 왼팔 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "98 왼팔을 아래로 내려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(false) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //오른팔이 올라가있다. 오른팔 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "99 오른팔을 아래로 내려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }

    }


    //율동 운동 - 10. 양팔을 앞으로 올리기/내리기
    public void BothArmFrontUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //양팔이 앞으로 올라가 있어야하는 구간
        if ((videoTimer >= 14 && videoTimer <= 17) || (videoTimer >= 22 && videoTimer <= 25) ||
            (videoTimer >= 27 && videoTimer <= 31) || (videoTimer >= 35 && videoTimer <= 38) ||
            (videoTimer >= 41 && videoTimer <= 45) || (videoTimer >= 49 && videoTimer <= 52) ||
            (videoTimer >= 53 && videoTimer <= 56))
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way < -230) //230보다 작으면 차렷//음수 : 아래
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔을 올려줴숑.
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;    // 왼팔이 내려갓다.
                        //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        if(exerSceneManager.successTime_L >= 3f)
                        {
                            l_Motion_Good = true;
                            l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                                                        //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //오른쪽
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔을 올려주세요.
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;    // 오른팔이 내려갓다.
                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125)
                    {
                        if(exerSceneManager.successTime_R >= 3f)
                        {
                            r_Motion_Good = true;
                            r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                                                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }

            }
        }
        else
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way < -230f) //230보다 작으면 차렷//음수 : 아래
                {
                    if (exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionUp_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                                                  //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    //if (shoulder_L_Curr > 120f)
                    {
                        if (exerSceneManager.missTime_L >= 3f)
                        {
                            //왼팔을 올려줴숑.
                            l_Motion_Good = false;
                            l_MotionUp_Bad = true;    // 왼팔이 내려갓다.
                                                      //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //오른쪽
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //양수 : 아래
                {
                    if (exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionUp_Bad = false;   //오른쪽 내려가서 자세좋음
                                                  //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    //if (shoulder_R_Curr >= 120f)
                    {
                        if (exerSceneManager.missTime_R >= 3f)
                        {
                            //오른팔을 올려주세요.
                            r_Motion_Good = false;
                            r_MotionUp_Bad = true;    // 오른팔이 내려가지않음 안좋음
                                                      //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                            exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0고 있어서 시간 0으로 초기화
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
            }
        }

        BothArmFrontUpDownNarrationSoundStart(videoTimer);
    }
    void BothArmFrontUpDownNarrationSoundStart(float _videoTimer)
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if ((_videoTimer >= 14 && _videoTimer <= 17) || (_videoTimer >= 22 && _videoTimer <= 25) ||
            (_videoTimer >= 27 && _videoTimer <= 31) || (_videoTimer >= 35 && _videoTimer <= 38) ||
            (_videoTimer >= 41 && _videoTimer <= 45) || (_videoTimer >= 49 && _videoTimer <= 52) ||
            (_videoTimer >= 53 && _videoTimer <= 56))
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //양팔이 내려와있다. 둘다 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "106 팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //왼팔이 내려와있다. 왼팔 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 위로 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //오른팔이 내려왔다. 오른팔 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 위로 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
        else
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                //SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                //PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
                //PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                Debug.Log("2여기 안들어오나 ???????");
                if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //양팔이 올라가있다. 둘다 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "팔을 아래로 내려주세요-");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //왼팔이 올라가있다. 왼팔 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "98 왼팔을 아래로 내려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(false) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //오른팔이 올라가있다. 오른팔 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "99 오른팔을 아래로 내려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
    }


    //율동 운동 - 11.양팔을 옆으로 올리기/내리기
    public void BothArmSideUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //양팔이 옆으로 올라가 있어야하는 구간
        if ((videoTimer >= 13 && videoTimer <= 16) || (videoTimer >= 20 && videoTimer <= 23) ||
            (videoTimer >= 26 && videoTimer <= 29) || (videoTimer >= 34 && videoTimer <= 37) ||
            (videoTimer >= 40 && videoTimer <= 43) || (videoTimer >= 47 && videoTimer <= 50) ||
            (videoTimer >= 53 && videoTimer <= 57))
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (shoulder_L_Curr >= 160f)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                                                    //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 160f)
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔 더 올려주세요
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;   //왼쪽 내려갔음
                                                   // l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
            }
            //<오른쪽>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
                if (shoulder_R_Curr >= 160f)//&& shoulder_R_Curr <= 145f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                                                    //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 160f)
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔 더 올려주세요
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;   //오른쪽 내려갔음
                                                   //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
            }
        }
        else
        {
            //<왼쪽>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;

                if (shoulder_L_Curr >= 160f)//만세
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        l_Motion_Good = false;
                        l_MotionUp_Bad = true;   //왼쪽올라가있음
                        exerSceneManager.missTime_L = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 160f)//차렷
                {
                    if (exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionUp_Bad = false;   //왼쪽 내려갔음
                        exerSceneManager.missTime_L = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
            }
            //오른쪽
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;

                if (shoulder_R_Curr >= 160f)//&& shoulder_R_Curr <= 145f) //만세
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        r_Motion_Good = false;
                        r_MotionUp_Bad = true;   //오른쪽 내려가지않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 160f)//차렷
                {
                    if (exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionUp_Bad = false;   //오른쪽 내려갔음
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
            }
        }

        BothArmSideUpDownNarrationSoundStart(videoTimer);
    }
    void BothArmSideUpDownNarrationSoundStart(float _videoTimer)
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if ((_videoTimer >= 13 && _videoTimer <= 16) || (_videoTimer >= 20 && _videoTimer <= 23) ||
            (_videoTimer >= 26 && _videoTimer <= 29) || (_videoTimer >= 34 && _videoTimer <= 37) ||
            (_videoTimer >= 40 && _videoTimer <= 43) || (_videoTimer >= 47 && _videoTimer <= 50) ||
            (_videoTimer >= 53 && _videoTimer <= 57))
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //양팔이 내려와있다. 둘다 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "106 팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //왼팔이 내려와있다. 왼팔 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 위로 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //오른팔이 내려왔다. 오른팔 올려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 위로 올려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
        else 
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                //SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                //PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
                //PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                Debug.Log("3여기 안들어오나 ???????");
                if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //양팔이 올라가있다. 둘다 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "팔을 아래로 내려주세요-");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //왼팔이 올라가있다. 왼팔 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "98 왼팔을 아래로 내려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(false) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //오른팔이 올라가있다. 오른팔 내려라!
                    SoundMaixerManager.instance.PlayNarration("율동운동", "99 오른팔을 아래로 내려주세요");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 아래로 내려주세요.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
    }


    //율동 운동 - 12. 오른쪽 팔 돌리기
    public void RightArmSpin(int _index, float _angle, float _way)
    {
        //<오른쪽>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            shoulder_R_Gap = shoulder_R_Before - shoulder_R_Curr;
            rightText.text = "오른쪽어깨 " + _angle + "  " + _way + "갭 " + Mathf.Abs(shoulder_R_Gap);
            //if (_way > 0)    //양수 : 아래
            //{
            //    if (exerSceneManager.missTime_R >= 3f)
            //    {
            //        //오른팔을 올려주세요.
            //        r_Motion_Good = false;
            //        allSpin_Bad = false;
            //        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
            //    }
            //    shoulder_R_Before = shoulder_R_Curr;
            //}
            //else
            {
                if (shoulder_R_Curr >= 160f)
                {
                    if (Mathf.Abs(shoulder_R_Gap).Equals(shoulder_R_Curr) || Mathf.Abs(shoulder_R_Gap) < 5)
                    {
                        if (exerSceneManager.missTime_R > 3f)
                        {
                            //오른쪽 어깨 돌려주세용
                            r_Motion_Good = false;  //움직이지 않는다.
                            allSpin_Bad = true;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                        shoulder_R_Before = shoulder_R_Curr;
                    }
                    else if (Mathf.Abs(shoulder_R_Gap) >= 8)
                    {
                        //if(exerSceneManager.successTime_R >= 3f)
                        {
                            //굳굳
                            r_Motion_Good = true;  //움직이다.
                            allSpin_Bad = false;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                        shoulder_R_Before = shoulder_R_Curr;
                    }
                }
                else
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔을 올려주세요.
                        r_Motion_Good = false;
                        allSpin_Bad = false;
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                    shoulder_R_Before = shoulder_R_Curr;
                }
            }
        }
        RightArmSpinNarrationSoundStart();
    }
    void RightArmSpinNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (r_Motion_Good.Equals(true) && allSpin_Bad.Equals(false))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(false) && allSpin_Bad.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && allSpin_Bad.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_5_오른쪽_어깨를_돌려주세요_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 어깨를 돌려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 13. 왼쪽 팔 돌리기
    public void LeftArmSpin(int _index, float _angle, float _way)
    {
        //<왼쪽>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            shoulder_L_Gap = shoulder_L_Before - shoulder_L_Curr;
            leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way + "갭 " + Mathf.Abs(shoulder_L_Gap);
            //if (_way < 0) //음수 : 아래
            //{
            //    if (exerSceneManager.missTime_L >= 3f)
            //    {
            //        //왼팔을 올려줴숑.
            //        l_Motion_Good = false;
            //        r_Motion_Good = true;
            //        exerSceneManager.missTime_L = 0;
            //    }
            //}
            //else
            {
                if (shoulder_L_Curr >= 160)
                {
                    if (Mathf.Abs(shoulder_L_Gap).Equals(shoulder_L_Curr) || Mathf.Abs(shoulder_L_Gap) < 5)
                    {
                        if (exerSceneManager.missTime_L > 3f)
                        {
                            //왼쪽 어깨 돌려주세용
                            l_Motion_Good = false;  //움직이지 않는다.
                            allSpin_Bad = true;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                        shoulder_L_Before = shoulder_L_Curr;
                    }
                    else if (Mathf.Abs(shoulder_L_Gap) >= 10)
                    {
                        //if(exerSceneManager.successTime_L >= 3f)
                        {
                            //굳굳
                            l_Motion_Good = true;  //움직이다.
                            allSpin_Bad = false;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                        shoulder_L_Before = shoulder_L_Curr;
                    }
                }
                else
                {
                    if (exerSceneManager.missTime_L > 3f)
                    {
                        //왼쪽 어깨 돌려주세용
                        l_Motion_Good = false;  //움직이지 않는다.
                        allSpin_Bad = false;
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                    shoulder_L_Before = shoulder_L_Curr;
                }
            }
        }
        LeftArmSpinNarrationSoundStart();
    }
    void LeftArmSpinNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && allSpin_Bad.Equals(false))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && allSpin_Bad.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(false) && allSpin_Bad.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("율동운동", "audio_4_왼쪽_어깨를_돌려주세요_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼팔을 어깨를 돌려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 14. 양팔 위로 올리기
    public void BothArmUp(int _index, float _angle, float _way)
    {
        //<왼쪽>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
            if (_way > 0) //음수 : 아래
            {
                if (exerSceneManager.missTime_L >= 4f)
                {
                    //왼팔을 올려줴숑.
                    l_Motion_Good = false;
                    l_MotionDown_Bad = true;    // 왼팔이 내려갓다.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
            }
            else
            {
                //if (shoulder_L_Curr > 125)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        //굳
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                //else
                //{
                //    l_Motion_Good = false;
                //    l_MotionDown_Bad = true;   //왼쪽 내려가지않음. 자세좋음
                //    exerSceneManager.missTime_L = 0;
                //    exerSceneManager.successTime_L = 0;
                //}
            }
        }
        //<오른쪽>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
            if (_way < 0)    //음수 -40보다 크면: 아래
            {
                if (exerSceneManager.missTime_R >= 3f)
                {
                    //오른팔을 올려주세요.
                    r_Motion_Good = false;
                    r_MotionDown_Bad = true;    // 오른팔이 내려갓다.
                    exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                    exerSceneManager.successTime_R = 0;
                }
            }
            else
            {
                //if (shoulder_R_Curr >= 125f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                //else
                //{
                //    //오른팔을 올려주세요.
                //    r_Motion_Good = false;
                //    r_MotionDown_Bad = true;    // 오른팔이 내려갓다.
                //    exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                //    exerSceneManager.successTime_R = 0;
                //}
            }
        }
        BothArmUpNarrationSoundStart();
    }
    void BothArmUpNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //양팔 내려갓다
                SoundMaixerManager.instance.PlayNarration("율동운동", "106 팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
            {
                //왼팔 내려갔다
                SoundMaixerManager.instance.PlayNarration("율동운동", "96 왼팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "왼팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //오른팔 내려갔다
                SoundMaixerManager.instance.PlayNarration("율동운동", "97 오른팔을 위로 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "오른팔을 위로 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //율동 운동 - 15. 양팔 옆으로 벌리기
    public void BothArmSideUp(int _index, float _angle, float _way)
    {
        //<왼쪽>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            leftText.text = "왼쪽어깨 " + (int)_angle + "  " + (int)_way;
            //if (_way < 0)    //음수 : 아래
            //{
            //    if (exerSceneManager.missTime_L >= 3f)
            //    {
            //        //왼팔을 올려주세요.
            //        l_Motion_Good = false;
            //        l_MotionDown_Bad = true;   //왼쪽 내려갔음
            //        l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
            //        exerSceneManager.missTime_L = 0;    //잘하고 있어서 시간 0으로 초기화
            //    }
            //}
            ////양수 : 위
            //else
            {
                if (shoulder_L_Curr >= 150f)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                                                    //l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 150f)
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //왼팔 더 올려주세요
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;   //왼쪽 내려갔음
                                                   // l_MotionUp_Bad = false; //왼쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_L = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
                //else if (shoulder_L_Curr > 145f)
                //{
                //    if (exerSceneManager.missTime_L >= 3f)
                //    {
                //        //왼팔 더 내려주세요
                //        l_Motion_Good = false;
                //        l_MotionDown_Bad = false;   //왼쪽 내려가지않음. 자세좋음
                //        //l_MotionUp_Bad = true; //왼쪽 올라갔음.
                //        exerSceneManager.missTime_L = 0;    //체크를 했으니깐 초기화 0
                //    }
                //}
            }
        }
        //<오른쪽>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            rightText.text = "오른쪽어깨 " + (int)_angle + "  " + (int)_way;
            //if (_way > 0)    //양수 : 아래
            //{
            //    if (exerSceneManager.missTime_R >= 3f)
            //    {
            //        //오른팔을 올려주세요.
            //        r_Motion_Good = false;
            //        r_MotionDown_Bad = true;   //오른쪽 내려갔음
            //        r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
            //        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
            //    }
            //}
            ////양수 : 위
            //else
            {
                if (shoulder_R_Curr >= 150f)//&& shoulder_R_Curr <= 145f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                                                    //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //잘하고 있어서 시간 0으로 초기화
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 150f)
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //오른팔 더 올려주세요
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;   //오른쪽 내려갔음
                        //r_MotionUp_Bad = false; //오른쪽 올라가지 않음. 자세좋음
                        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                //else if (shoulder_R_Curr > 145f)
                //{
                //    if (exerSceneManager.missTime_R >= 3f)
                //    {
                //        //오른팔 더 내려주세요
                //        r_Motion_Good = false;
                //        r_MotionDown_Bad = false;   //오른쪽 내려가지않음. 자세좋음
                //        r_MotionUp_Bad = true; //오른쪽 올라갔음.
                //        exerSceneManager.missTime_R = 0;    //체크를 했으니깐 초기화 0
                //    }
                //}
            }
        }

        BothArmSideUpNarrationSoundStart();
    }
    void BothArmSideUpNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //왼쪽 오른쪽 모션을 둘다 잘하고 있다.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // 모션 너무 잘하고 있다.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "잘하고있어요.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if ((l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15))
            {
                //양쪽 다 잘못하고 있음.
                SoundMaixerManager.instance.PlayNarration("율동운동", "86 팔을 어깨까지 올려주세요.");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "팔을 어깨까지 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
            {
                //왼쪽팔 더 올려야함
                SoundMaixerManager.instance.PlayNarration("율동운동", "87 왼쪽팔을 어깨높이까지 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "왼쪽팔을 어깨높이까지 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //오른쪽팔 더 올려야함.
                SoundMaixerManager.instance.PlayNarration("율동운동", "89 오른팔을 어깨높이까지 올려주세요");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "오른팔을 어깨높이까지 올려주세요.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }
}
