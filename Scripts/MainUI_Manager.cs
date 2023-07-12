using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI_Manager : MonoBehaviour
{
    public static MainUI_Manager instance { get; private set; }

    public Text textGuide_L;  //안내멘트
    public Text textGuide_R;    
    public Text[] textPos;    //포지션

    public GameObject resultPanel;  //결과 화면
    public Text resultText; //결과 텍스트

    public Flexibility_Exercise flexi_exercise; //율동운동 스크립트

    public bool checkStart; //움직임 체크 시작 여부

    public float totalGoolTime = 0;  //완벽하게 운동을 한 시간
    bool totalGoolState;    //완벽하게 운동한 시간 카운트 시작 여부

    public float time_L = 0;    //운동을 하고 있는 시간- 왼
    public float time_R = 0;   //운동을 하고 있는 시간- 오

    bool wrist_L_Move, wrist_R_Move;    //손목 움직임 여부
    int wrist_L_Before, wrist_R_Before; //손목 전에 움직인 각도
    int wrist_L_Curr, wrist_R_Curr; //손목 지금 움직인 각도
    int wrist_L_Gap, wrist_R_Gap;   //손목 움직인 갭

    bool shoulder_L_Move, shoulder_R_Move;      //어깨 움직임 여부
    int shoulder_L_Before, shoulder_R_Before;   //어깨 전에 움직인 각도
    int shoulder_L_Curr, shoulder_R_Curr;   //어깨 지금 움직인 각도
    int shoulder_L_Gap, shoulder_R_Gap; //어깨 움직인 갭

    public Text timeText;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        PlayerPrefs.SetString("FKR_WorkOutEvnet", "");
        Debug.Log("====== " + PlayerPrefs.GetString("FKR_WorkOutEvnet"));
        StartCoroutine(_StartFitnees());
        
    }

    private void Update()
    {
        if(checkStart.Equals(true))
        {
            time_L += Time.deltaTime;
            time_R += Time.deltaTime;
        }
        

        if(flexi_exercise.totalGoolState.Equals(true))
        {
            totalGoolTime += Time.deltaTime;
            //Debug.Log("타임 : " + totalGoolTime);
        }
            
    }

    public void WorkOutEvnetReport()
    {
        StartCoroutine(_WorkOutEvnetReport());
    }

    //다음 운동 소개 - 두번째 운동부터
    IEnumerator _WorkOutEvnetReport()
    {
        checkStart = false;
        textGuide_L.text = "너무 잘하셨습니다.";
        textGuide_R.text = "";

        yield return new WaitForSecondsRealtime(3f);

        textGuide_L.text = "이제 다음 운동을 시작하겠습니다.";
        textGuide_R.text = "";

        yield return new WaitForSecondsRealtime(3f);

        //textGuide.text = "율동운동 1. 손목털기 시작!";
        WorkOutRecipeStartEvent();  //운동 처방 이름 

        yield return new WaitForSecondsRealtime(3f);

        Video_WorkOUtRecipeShow();  //운동 처방 비디오
        //VideoHandler.instance.PlayVideo(0);
        textGuide_L.text = "화면에 나오는 운동을 보고 따라해보세요.";
        textGuide_R.text = "";

        yield return new WaitForSecondsRealtime(2f);
        checkStart = true;

        time_L = 0; time_R = 0; //타임 초기화
        wrist_L_Move = false; wrist_R_Move = false;
        shoulder_L_Move = false; shoulder_R_Move = false;
        CoolTime.instance.currCoolTime = 0f;   //타임 초기화
        totalGoolTime = 0;  //초기화
    }

    //운동처방 시작 멘트 - 제일 처음 
    IEnumerator _StartFitnees()
    {
        time_L = 0; time_R = 0; //타임 초기화
        textGuide_L.text = "안녕하세요. 운동처방에 오신걸 환영합니다.";
        textGuide_R.text = "";

        yield return new WaitForSecondsRealtime(3f);

        textGuide_L.text = "지금부터 운동을 시작하겠습니다.";
        textGuide_R.text = "";

        yield return new WaitForSecondsRealtime(3f);

        //textGuide.text = "율동운동 1. 손목털기 시작!";
        WorkOutRecipeStartEvent();  //운동 처방 이름 

        yield return new WaitForSecondsRealtime(3f);

        //VideoHandler.instance.PlayVideo(0);
        Video_WorkOUtRecipeShow();  //운동 처방 비디오
        textGuide_L.text =  "화면에 나오는 운동을 보고 따라해보세요.";
        textGuide_R.text = "";

        yield return new WaitForSecondsRealtime(2f);
        checkStart = true;
    }

    
    
    //운동 결과 화면
    public void ResultPanelShow()
    {
        StartCoroutine(_ResultPanelShow());
    }

    IEnumerator _ResultPanelShow()
    {
        resultPanel.SetActive(true);    //결과화면 활성화

        if (CoolTime.instance.maxCoolTime.Equals(60))
        {
            if (totalGoolTime >= 55f)
                resultText.text = "S";
            else if (totalGoolTime < 55f && totalGoolTime >= 40f)
                resultText.text = "A";
            else if (totalGoolTime < 40f && totalGoolTime >= 30f)
                resultText.text = "B";
            else if (totalGoolTime < 30f && totalGoolTime >= 20f)
                resultText.text = "C";
            else if (totalGoolTime < 20f)
                resultText.text = "D";
        }
        else if (CoolTime.instance.maxCoolTime.Equals(30))
        {
            if (totalGoolTime >= 25f)
                resultText.text = "S";
            else if (totalGoolTime < 25f && totalGoolTime >= 20f)
                resultText.text = "A";
            else if (totalGoolTime < 20f && totalGoolTime >= 15f)
                resultText.text = "B";
            else if (totalGoolTime < 15f && totalGoolTime >= 10f)
                resultText.text = "C";
            else if (totalGoolTime < 10f)
                resultText.text = "D";
        }

        yield return new WaitForSeconds(5f);

        resultPanel.SetActive(false);
    }



    //운동처방종료 말해주기
    public void WorkOutRecipeStartEvent()
    {
        if(PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") || PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1"))
        {
            textGuide_L.text = "율동운동 1. 손목털기 시작!";
            PlayerPrefs.SetString("FKR_WorkOutEvnet", "1_1");   //프리팹에 아무것도 없음. 저장해줌.
            //Debug.Log("운동 저장?  " + PlayerPrefs.GetString("FKR_WorkOutEvnet"));
        }
        else if(PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
            textGuide_L.text = "율동운동 2. 팔 앞으로 뻗기 시작!";
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))
            textGuide_L.text = "율동운동 3. 손목 꺾기";
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_4"))
            textGuide_L.text = "율동운동 4. 손목 돌리기";
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_5"))
            textGuide_L.text = "율동운동 5. 어깨 돌리기";
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_6"))
            textGuide_L.text = "율동운동 6. 팔을 위로 올리기/내리기";
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_7"))
            textGuide_L.text = "율동운동 7. 팔을 앞으로 올리기/내리기";
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_8"))
            textGuide_L.text = "율동운동 8. 팔을 옆으로 올리기/내리기";
    }

    //운동처방 해당 비디오 출력
    public void Video_WorkOUtRecipeShow()
    {
        if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") || PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1"))
        {
            //VideoHandler.instance.PlayVideo(0);
        }
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
        {
            //VideoHandler.instance.PlayVideo(1);
        }
        else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))
        {
            //VideoHandler.instance.PlayVideo(2);
        }
    }

    //해당 운동 선택 시작
    //각도, 관절 인덱스 값, 방향(음수,양수 - 위아래[관절에 따라 방향값이 틀림])
    public void ExerciseCheicePlay(float _angle, int _index, float _way)
    {
        //운동 체크가 시작 되었다면
        if(checkStart.Equals(true))
        {
            //율동 운동 - 손목털기
            if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1"))
            {
                if (_index.Equals(6))
                {
                    flexi_exercise.ShakeWristMove(_index, _angle, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
                else if (_index.Equals(7))
                {
                    flexi_exercise.ShakeWristMove(_index, _angle, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
            }
            //율동 운동 - 팔 앞으로 뻗기
            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
            {
                if (_index.Equals(4))
                {
                    flexi_exercise.StretchArmsForward(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
                else if (_index.Equals(5))
                {
                    flexi_exercise.StretchArmsForward(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
            }
            //율동 운동 - 손목 꺽기
            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))
            {

            }
            //율동 운동 - 손목 돌리기
            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_4"))
            {
                if (_index.Equals(6))
                {
                    flexi_exercise.TurningWristMove(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
                else if (_index.Equals(7))
                {
                    flexi_exercise.TurningWristMove(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
            }
            //율동 운동 - 어깨 돌리기
            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_5"))
            {
                if (_index.Equals(4))
                {
                    flexi_exercise.TurningShoulders(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
                else if (_index.Equals(5))
                {
                    flexi_exercise.TurningShoulders(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
                }
            }
            //율동 운동 - 한팔 위로 올리기/내리기
            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_6"))
            {
                flexi_exercise.OneArm_UpDown();
            }
        }
    }

    //public void PosionText(float _angle, int _index, float _way)
    //{
    //    Debug.Log(" - - ?  ? ? ? ?");
    //    if (_index.Equals(0))
    //        textPos[0].text = "(왼)팔꿈치 좌표: " + _index + " 각도: " + (int)_angle;
    //    else if (_index.Equals(1))
    //        textPos[1].text = "(오)팔꿈치 좌표: " + _index + " 각도: " + (int)_angle;
    //    else if (_index.Equals(2))
    //        textPos[2].text = "(왼)무릎 좌표: " + _index + " 각도: " + (int)_angle;
    //    else if (_index.Equals(3))
    //        textPos[3].text = "(오)무릎 좌표: " + _index + " 각도: " + (int)_angle;
    //    else if (_index.Equals(4))
    //    {
    //        textPos[4].text = "(왼)어깨 좌표: " + _index + " 각도: " + (int)_angle;
    //        //Debug.Log("(왼)_rot " + _way);

    //        if (checkStart.Equals(true))
    //        {
    //            if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))    //팔 앞으로 뻗기
    //                flexi_exercise.StretchArmsForward(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
    //            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))   //어깨돌리기
    //                flexi_exercise.TurningShoulders(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
    //        }
                
    //    }
    //    else if (_index.Equals(5))
    //    {
    //        textPos[5].text = "(오)어깨 좌표: " + _index + " 각도: " + (int)_angle;
    //        //Debug.Log("(오)_rot " + _way);
    //        if (checkStart.Equals(true))
    //        {
    //            if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))    //팔 앞으로 뻗기
    //                flexi_exercise.StretchArmsForward(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
    //            else if (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))   //어깨 돌리기
    //                flexi_exercise.TurningShoulders(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
    //        }
                
    //    }
    //    else if (_index.Equals(6))
    //    {
    //        textPos[6].text = "(왼)손목 좌표: " + _index + " 각도: " + (int)_angle;

    //        if (checkStart.Equals(true))
    //        {
    //            //손목 털기
    //            //flexi_exercise.ShakeWristMove(_index, _angle, textGuide_L, textGuide_R, checkStart, time_L, time_R);

    //            //손목 돌리기
    //            flexi_exercise.TurningWristMove(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
    //        }
    //    } 
    //    else if (_index.Equals(7))
    //    {
    //        textPos[7].text = "(오)손목 좌표: " + _index + " 각도: " + (int)_angle;

    //        if (checkStart.Equals(true))
    //        {
    //            //손목 털기
    //            //flexi_exercise.ShakeWristMove(_index, _angle, textGuide_L, textGuide_R, checkStart, time_L, time_R);

    //            //손목 돌리기
    //            flexi_exercise.TurningWristMove(_index, _angle, _way, textGuide_L, textGuide_R, checkStart, time_L, time_R);
    //        } 
    //    }

    //    //잘했을 때 운동 멘트 함수
    //    flexi_exercise.ExerciseWell(textGuide_L, textGuide_R, checkStart);

    //    ////팔목
    //    //if (wrist_L_Move.Equals(true) && wrist_R_Move.Equals(true) && checkStart.Equals(true))
    //    //{
    //    //    totalGoolState = true;  //완벽한 운동 카운트 시작
    //    //    textGuide_L.text = "아주 잘하고 있어요^^!";
    //    //    textGuide_R.text = "굳굳! 힘내세요!";
    //    //}
    //    ////어깨
    //    //if(shoulder_L_Move.Equals(true) && shoulder_R_Move.Equals(true) && checkStart.Equals(true))
    //    //{
    //    //    totalGoolState = true;  //완벽한 운동 카운트 시작
    //    //    textGuide_L.text = "아주 잘하고 있어요^^!";
    //    //    textGuide_R.text = "굳굳! 힘내세요!";
    //    //}
            
    //}

    ////율동운동 - 손목 털기 확인 함수

    //public void ShakeWristMove(int _index, float _angle)
    //{
    //    Debug.Log("손목털기");
    //    if(_index.Equals(6) && checkStart.Equals(true) &&(PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") || 
    //        PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1")))
    //    {
    //        wrist_L_Curr = (int)_angle;
    //        wrist_L_Gap = wrist_L_Before - wrist_L_Curr;

    //        if((Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 20))
    //        {
    //            //Debug.Log("wrist_L_Gap " + Mathf.Abs(wrist_L_Gap) + "   " + time_L);
    //            if (time_L >= 1.5f)
    //            {
    //                wrist_L_Move = false;
    //                totalGoolState = false;  //완벽한 운동 카운트 끝
    //                textGuide_L.text = "왼손을 흔들어주세요!!";
    //            }
    //            wrist_L_Before = wrist_L_Curr;
    //        }
    //        else if(Mathf.Abs(wrist_L_Gap) >= 20)
    //        {
    //            //Debug.Log("wrist_L_Gap " + Mathf.Abs(wrist_L_Gap) + "   " + time_L);
    //            textGuide_L.text = "";
    //            time_L = 0;
    //            wrist_L_Move = true;
    //            wrist_L_Before = wrist_L_Curr;
    //        }
    //    }

    //    if (_index.Equals(7) && checkStart.Equals(true) &&(PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") ||
    //        PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1")))
    //    {
    //        wrist_R_Curr = (int)_angle;
    //        wrist_R_Gap = wrist_R_Before - wrist_R_Curr;

    //        if ((Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 20))
    //        {
    //            if (time_R >= 1.5f)
    //            {
    //                wrist_R_Move = false;
    //                totalGoolState = false;  //완벽한 운동 카운트 끝
    //                textGuide_R.text = "오른손을 흔들어주세요!!";
    //            }
    //            wrist_R_Before = wrist_R_Curr;
    //        }
    //        else if (Mathf.Abs(wrist_L_Gap) >= 20)
    //        {
    //            textGuide_R.text = "";
    //            time_R = 0;
    //            wrist_R_Move = true;
    //            wrist_R_Before = wrist_R_Curr;
    //        }
    //    }
    //}

    ////율동 운동 - 팔 앞으로 뻗기 함수
    ////_way - 방향 : 양수/위 , 음수/아래 (왼)
    ////_way - 방향 : 양수/아래, 음수/위 (오)
    //public void StretchArmsForward(int _index, float _angle, float _way)
    //{
    //    Debug.Log("팔앞으로뻗기");
    //    //왼쪽
    //    if (_index.Equals(4) && checkStart.Equals(true) &&PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
    //    {
    //        shoulder_L_Curr = (int)_angle;

    //        if(_way < 0)    //음수:아래
    //        {
    //            shoulder_L_Move = false;
    //            totalGoolState = false;  //완벽한 운동 카운트 끝
    //            textGuide_L.text = "왼팔을 올려주세요.";
    //        }
    //        else //양수:위
    //        {
    //            if (shoulder_L_Curr >= 80f && shoulder_L_Curr <= 125f)
    //            {
    //                time_L = 0;
    //                shoulder_L_Move = true;
    //                textGuide_L.text = "";
    //            }
    //            else if (shoulder_L_Curr < 80f)
    //            {
    //                if (time_L >= 2f)
    //                {
    //                    shoulder_L_Move = false;
    //                    totalGoolState = false;  //완벽한 운동 카운트 끝
    //                    textGuide_L.text = "왼팔을 조금 더 올려주세요.";
    //                }
    //            }
    //            else if (shoulder_L_Curr > 125f)
    //            {
    //                if (time_L >= 2f)
    //                {
    //                    shoulder_L_Move = false;
    //                    totalGoolState = false;  //완벽한 운동 카운트 끝
    //                    textGuide_L.text = "왼팔을 조금 더 내려주세요.";
    //                }
    //            }
    //        }
    //    }

    //    //오른쪽
    //    if (_index.Equals(5) && checkStart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
    //    {
    //        shoulder_R_Curr = (int)_angle;

    //        if (_way > 0)    //양수:아래
    //        {
    //            shoulder_R_Move = false;
    //            totalGoolState = false;  //완벽한 운동 카운트 끝
    //            textGuide_R.text = "오른팔을 올려주세요.";
    //        }
    //        else
    //        {
    //            if (shoulder_R_Curr >= 80f && shoulder_R_Curr <= 125f)
    //            {
    //                time_R = 0;
    //                shoulder_R_Move = true;
    //                textGuide_R.text = "";
    //            }
    //            else if (shoulder_R_Curr < 80f)
    //            {
    //                if (time_R >= 2f)
    //                {
    //                    shoulder_R_Move = false;
    //                    totalGoolState = false;  //완벽한 운동 카운트 끝
    //                    textGuide_R.text = "오른팔을 조금 더 올려주세요.";
    //                }
    //            }
    //            else if (shoulder_R_Curr > 125f)
    //            {
    //                if (time_R >= 2f)
    //                {
    //                    shoulder_R_Move = false;
    //                    totalGoolState = false;  //완벽한 운동 카운트 끝
    //                    textGuide_R.text = "오른팔을 조금 더 내려주세요.";
    //                }
    //            }
    //        } 
    //    }
    //}


    ////율동 운동 - 어깨 돌리기 함수
    //public void TurningShoulders(int _index, float _angle, float _way)
    //{
    //    Debug.Log("어깨돌리기");
    //    //왼쪽
    //    if (_index.Equals(4) && checkStart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))
    //    {
    //        shoulder_L_Curr = (int)_angle;
    //        shoulder_L_Gap = shoulder_L_Before - shoulder_L_Curr;

    //        if(Mathf.Abs(shoulder_L_Gap).Equals(shoulder_L_Curr) || Mathf.Abs(shoulder_L_Gap) < 17)
    //        {
    //            if(time_L >= 2f)
    //            {
    //                shoulder_L_Move = false;
    //                totalGoolState = false;  //완벽한 운동 카운트 끝
    //                textGuide_L.text = "왼쪽 어깨를 돌려주세요.";
    //            }
    //            shoulder_L_Before = shoulder_L_Curr;
    //        }
    //        else if(Mathf.Abs(shoulder_L_Gap) >= 17)
    //        {
    //            time_L = 0;
    //            shoulder_L_Move = true;
    //            shoulder_L_Before = shoulder_L_Curr;
    //            textGuide_L.text = "";
    //        }
    //    }

    //    //오른쪽
    //    if (_index.Equals(4) && checkStart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_3"))
    //    {
    //        shoulder_R_Curr = (int)_angle;
    //        shoulder_R_Gap = shoulder_R_Before - shoulder_R_Curr;

    //        if (Mathf.Abs(shoulder_R_Gap).Equals(shoulder_R_Curr) || Mathf.Abs(shoulder_R_Gap) < 17)
    //        {
    //            if (time_L >= 2f)
    //            {
    //                shoulder_R_Move = false;
    //                totalGoolState = false;  //완벽한 운동 카운트 끝
    //                textGuide_R.text = "오른쪽 어깨를 돌려주세요.";
    //            }
    //            shoulder_R_Before = shoulder_R_Curr;
    //        }
    //        else if (Mathf.Abs(shoulder_R_Gap) >= 17)
    //        {
    //            time_L = 0;
    //            shoulder_R_Move = true;
    //            shoulder_R_Before = shoulder_R_Curr;
    //            textGuide_R.text = "";
    //        }
    //    }
    //}
}
