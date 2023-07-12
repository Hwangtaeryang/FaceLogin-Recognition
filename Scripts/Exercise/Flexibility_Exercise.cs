using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flexibility_Exercise : MonoBehaviour
{
    public static Flexibility_Exercise instance { get; private set; }


    public MainUI_Manager mainUIManager;
    public GameObject[] btn_Obj;  //버튼

    public Transform exercise_1_6_Pos;  //율동운동 1-6 위치

    public bool totalGoolState;    //완벽하게 운동한 시간 카운트 시작 여부

    bool wrist_L_Move, wrist_R_Move;    //손목 움직임 여부
    int wrist_L_Before, wrist_R_Before; //손목 전에 움직인 각도
    int wrist_L_Curr, wrist_R_Curr; //손목 지금 움직인 각도
    int wrist_L_Gap, wrist_R_Gap;   //손목 움직인 갭

    bool shoulder_L_Move, shoulder_R_Move;
    int shoulder_L_Before, shoulder_R_Before;
    int shoulder_L_Curr, shoulder_R_Curr;
    int shoulder_L_Gap, shoulder_R_Gap;

    bool binState;  //방향 빈 껍데기(전 방향)
    bool dir_Up; //방향 위 상태(현재 방향)


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    //잘했을 때 운동 멘트 함수

    public void ExerciseWell(Text _guideL, Text _guideR, bool _checkstart)
    {
        //팔목
        if (wrist_L_Move.Equals(true) && wrist_R_Move.Equals(true) && _checkstart.Equals(true))
        {
            totalGoolState = true;  //완벽한 운동 카운트 시작
            _guideL.text = "아주 잘하고 있어요^^!";
            _guideR.text = "굳굳! 힘내세요!";
        }

        //어깨
        if (shoulder_L_Move.Equals(true) && shoulder_R_Move.Equals(true) && _checkstart.Equals(true))
        {
            totalGoolState = true;  //완벽한 운동 카운트 시작
            _guideL.text = "아주 잘하고 있어요^^!";
            _guideR.text = "굳굳! 힘내세요!";
        }
    }

    //율동운동 - 1. 손목 털기 확인 함수
    public void ShakeWristMove(int _index, float _angle, Text _guideL, Text _guideR, bool _checkstart, float _timeL, float _timeR)
    {
        Debug.Log("손목털기");
        if (_index.Equals(6) && _checkstart.Equals(true) && (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") ||
            PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1")))
        {
            wrist_L_Curr = (int)_angle;
            wrist_L_Gap = wrist_L_Before - wrist_L_Curr;

            if ((Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 20))
            {
                //Debug.Log("wrist_L_Gap " + Mathf.Abs(wrist_L_Gap) + "   " + time_L);
                if (_timeL >= 1.5f)
                {
                    wrist_L_Move = false;
                    totalGoolState = false;  //완벽한 운동 카운트 끝
                    _guideL.text = "왼손을 흔들어주세요!!";
                }
                wrist_L_Before = wrist_L_Curr;
            }
            else if (Mathf.Abs(wrist_L_Gap) >= 20)
            {
                Debug.Log("wrist_L_Gap " + Mathf.Abs(wrist_L_Gap) + "   " + _timeL);
                _guideL.text = "";
                mainUIManager.time_L = 0;
                wrist_L_Move = true;
                wrist_L_Before = wrist_L_Curr;
            }
        }

        if (_index.Equals(7) && _checkstart.Equals(true) && (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") ||
            PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_1")))
        {
            wrist_R_Curr = (int)_angle;
            wrist_R_Gap = wrist_R_Before - wrist_R_Curr;

            if ((Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 20))
            {
                if (_timeR >= 1.5f)
                {
                    wrist_R_Move = false;
                    totalGoolState = false;  //완벽한 운동 카운트 끝
                    _guideR.text = "오른손을 흔들어주세요!!";
                }
                wrist_R_Before = wrist_R_Curr;
            }
            else if (Mathf.Abs(wrist_L_Gap) >= 20)
            {
                _guideR.text = "";
                mainUIManager.time_R = 0;
                wrist_R_Move = true;
                wrist_R_Before = wrist_R_Curr;
            }
        }
    }


    //율동 운동 - 2. 팔 앞으로 뻗기 함수
    //_way - 방향 : 양수/위 , 음수/아래 (왼)
    //_way - 방향 : 양수/아래, 음수/위 (오)
    public void StretchArmsForward(int _index, float _angle, float _way, Text _guideL, Text _guideR, bool _checkstart, float _timeL, float _timeR)
    {
        Debug.Log("팔앞으로뻗기");
        //왼쪽
        if (_index.Equals(4) && _checkstart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
        {
            shoulder_L_Curr = (int)_angle;

            if (_way < 0)    //음수:아래
            {
                shoulder_L_Move = false;
                totalGoolState = false;  //완벽한 운동 카운트 끝
                _guideL.text = "왼팔을 올려주세요.";
            }
            else //양수:위
            {
                if (shoulder_L_Curr >= 75f && shoulder_L_Curr <= 125f)
                {
                    mainUIManager.time_L = 0;
                    shoulder_L_Move = true;
                    _guideL.text = "";
                }
                else if (shoulder_L_Curr < 75f)
                {
                    if (_timeL >= 2f)
                    {
                        shoulder_L_Move = false;
                        totalGoolState = false;  //완벽한 운동 카운트 끝
                        _guideL.text = "왼팔을 조금 더 올려주세요.";
                    }
                }
                else if (shoulder_L_Curr > 125f)
                {
                    if (_timeL >= 2f)
                    {
                        shoulder_L_Move = false;
                        totalGoolState = false;  //완벽한 운동 카운트 끝
                        _guideL.text = "왼팔을 조금 더 내려주세요.";
                    }
                }
            }
        }

        //오른쪽
        if (_index.Equals(5) && _checkstart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_2"))
        {
            shoulder_R_Curr = (int)_angle;

            if (_way > 0)    //양수:아래
            {
                shoulder_R_Move = false;
                totalGoolState = false;  //완벽한 운동 카운트 끝
                _guideR.text = "오른팔을 올려주세요.";
            }
            else
            {
                if (shoulder_R_Curr >= 75f && shoulder_R_Curr <= 125f)
                {
                    mainUIManager.time_R = 0;
                    shoulder_R_Move = true;
                    _guideR.text = "";
                }
                else if (shoulder_R_Curr < 75f)
                {
                    if (_timeR >= 2f)
                    {
                        shoulder_R_Move = false;
                        totalGoolState = false;  //완벽한 운동 카운트 끝
                        _guideR.text = "오른팔을 조금 더 올려주세요.";
                    }
                }
                else if (shoulder_R_Curr > 125f)
                {
                    if (_timeR >= 2f)
                    {
                        shoulder_R_Move = false;
                        totalGoolState = false;  //완벽한 운동 카운트 끝
                        _guideR.text = "오른팔을 조금 더 내려주세요.";
                    }
                }
            }
        }
    }

    
    //율동 운동 - 4. 손목 돌리기
    //_way - 방향 : 양수/위 , 음수/아래 (왼)
    //_way - 방향 : 양수/아래, 음수/위 (오)
    public void TurningWristMove(int _index, float _angle, float _way, Text _guideL, Text _guideR, bool _checkstart, float _timeL, float _timeR)
    {
        Debug.Log("손목 돌리기");
        if (_index.Equals(6) && _checkstart.Equals(true) && (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") ||
            PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_4")))   //순서에 맞게 변경 1_1
        {
            if (_way > 0)
                dir_Up = true;  //양수 방향 위
            else
                dir_Up = false; //음수 방향 아래

            wrist_L_Curr = (int)_angle;
            wrist_L_Gap = wrist_L_Before - wrist_L_Curr;

            if ((Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 10))
            {
                //Debug.Log("타임 : " + wrist_L_Gap + "   " + Mathf.Abs(wrist_L_Gap));
                if (_timeL > 2f)
                {
                    wrist_L_Move = false;   //움직이지 않는다
                    totalGoolState = false;  //완벽한 운동 카운트(시간시작) 끝
                    _guideL.text = "왼손을 돌려주세요.";
                    binState = dir_Up;
                }
                wrist_L_Before = wrist_L_Curr;
            }
            else if (Mathf.Abs(wrist_L_Gap) >= 10)
            {
                //빈 방향과 현재 방향이 같을 때(움직이질 않았음)
                if (binState.Equals(dir_Up))
                {
                    if (_timeL > 2f)
                    {
                        wrist_L_Move = false;   //움직이지 않는다
                        totalGoolState = false;  //완벽한 운동 카운트(시간시작) 끝
                        _guideL.text = "왼손을 돌려주세요.";
                        binState = dir_Up;
                    }
                }
                else
                {
                    mainUIManager.time_L = 0;
                    wrist_L_Move = true;   //움직인다.
                    binState = dir_Up;
                    _guideL.text = "";
                }
                wrist_L_Before = wrist_L_Curr;
            }
        }

        if (_index.Equals(7) && _checkstart.Equals(true) && (PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("") ||
            PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_4")))   //순서에 맞게 변경 1_1
        {
            if (_way > 0)
                dir_Up = true;  //양수 방향 위
            else
                dir_Up = false; //음수 방향 아래

            wrist_R_Curr = (int)_angle;
            wrist_R_Gap = wrist_R_Before - wrist_R_Curr;

            if ((Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 10))
            {
                //Debug.Log("타임 : " + wrist_R_Gap + "   " + Mathf.Abs(wrist_R_Gap));
                if (_timeR > 2f)
                {
                    wrist_R_Move = false;   //움직이지 않는다
                    totalGoolState = false;  //완벽한 운동 카운트(시간시작) 끝
                    _guideR.text = "오른손을 돌려주세요.";
                    binState = dir_Up;
                }
                wrist_R_Before = wrist_R_Curr;
            }
            else if (Mathf.Abs(wrist_R_Gap) >= 10)
            {
                //빈 방향과 현재 방향이 같을 때(움직이질 않았음)
                if (binState.Equals(dir_Up))
                {
                    if (_timeR > 2f)
                    {
                        wrist_R_Move = false;   //움직이지 않는다
                        totalGoolState = false;  //완벽한 운동 카운트(시간시작) 끝
                        _guideR.text = "오른손을 돌려주세요.";
                        binState = dir_Up;
                    }
                }
                else
                {
                    mainUIManager.time_R = 0;
                    wrist_R_Move = true;   //움직인다.
                    binState = dir_Up;
                    _guideR.text = "";
                }
                wrist_R_Before = wrist_R_Curr;
            }
        }
    }


    //율동 운동 - 5. 어깨 돌리기 함수
    public void TurningShoulders(int _index, float _angle, float _way, Text _guideL, Text _guideR, bool _checkstart, float _timeL, float _timeR)
    {
        Debug.Log("어깨돌리기");
        //왼쪽
        if (_index.Equals(4) && _checkstart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_5"))
        {
            shoulder_L_Curr = (int)_angle;
            shoulder_L_Gap = shoulder_L_Before - shoulder_L_Curr;

            if (Mathf.Abs(shoulder_L_Gap).Equals(shoulder_L_Curr) || Mathf.Abs(shoulder_L_Gap) < 17)
            {
                if (_timeL >= 2f)
                {
                    shoulder_L_Move = false;
                    totalGoolState = false;  //완벽한 운동 카운트 끝
                    _guideL.text = "왼쪽 어깨를 돌려주세요.";
                }
                shoulder_L_Before = shoulder_L_Curr;
            }
            else if (Mathf.Abs(shoulder_L_Gap) >= 17)
            {
                mainUIManager.time_L = 0;
                shoulder_L_Move = true;
                shoulder_L_Before = shoulder_L_Curr;
                _guideL.text = "";
            }
        }

        //오른쪽
        if (_index.Equals(4) && _checkstart.Equals(true) && PlayerPrefs.GetString("FKR_WorkOutEvnet").Equals("1_5"))
        {
            shoulder_R_Curr = (int)_angle;
            shoulder_R_Gap = shoulder_R_Before - shoulder_R_Curr;

            if (Mathf.Abs(shoulder_R_Gap).Equals(shoulder_R_Curr) || Mathf.Abs(shoulder_R_Gap) < 17)
            {
                if (_timeR >= 2f)
                {
                    shoulder_R_Move = false;
                    totalGoolState = false;  //완벽한 운동 카운트 끝
                    _guideR.text = "오른쪽 어깨를 돌려주세요.";
                }
                shoulder_R_Before = shoulder_R_Curr;
            }
            else if (Mathf.Abs(shoulder_R_Gap) >= 17)
            {
                mainUIManager.time_R = 0;
                shoulder_R_Move = true;
                shoulder_R_Before = shoulder_R_Curr;
                _guideR.text = "";
            }
        }
    }


    //율동 운동 - 6. 한팔 올리고 내리기
    public void OneArm_UpDown()
    {
        btn_Obj[0].SetActive(true); //버튼 하나 활성화
        btn_Obj[1].SetActive(false);    //버튼 하나 비활성화

        btn_Obj[0].transform.position = new Vector3(exercise_1_6_Pos.position.x, exercise_1_6_Pos.position.y,
            exercise_1_6_Pos.position.z);
    }



}
