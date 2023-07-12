using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseScene_Manager : MonoBehaviour
{
    public static ExerciseScene_Manager instance { get; private set; }

    public bool startMotionState;   //동작 시작상태인지
    public MotionCustomAlarm motioncustom;  //동작 체크 함수들을 모아놓은 스크립트

    public float missTime_L;    //잘못 동작하고 있는 시간 왼쪽
    public float missTime_R;    //잘못 동작하고 있는 시간 오른쪽
    public float successTime_L;
    public float successTime_R;

    public GameObject narraListPref;    //리스트에 생길 내래이션 목차
    public GameObject parentObj;    //내래이션목차가 생길 위치
    GameObject copyNarra; //내래이션 복사본

    public Text videoTimeText;  //비디오 플레이 시간

    public Sprite[] classSprite;    //등급 이미지 A,B,c
    string motionAvageClass;    //동작 하나 끝났을 때 결과화면에 보여지는 등급

    string[] lastAllMotionClassArr; //모든 동작이 끝나고 각 동작 등급 저장
    string[] lastOnlyAllMotionClassArr; //모든 동작이 끝나고 각 동작 등급 저장
    string[] lastWeekCountArr;  //마지막에 엑셀에 저장하기위함


    float allAverage;   //총 동작의 평균

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    void Start()
    {
        //이 씬을 처음 시작하면 초기화 시킨다.
        //운동 동작 전체 등급을 저장하기 위한 프리팹.
        PlayerPrefs.SetString("EP_MotionAllClass", "");
    }

    
    void Update()
    {
        //운동 체크가 시작되었다면
        if (startMotionState.Equals(true))
        {
            missTime_L += Time.deltaTime;
            missTime_R += Time.deltaTime;
            successTime_L += Time.deltaTime;
            successTime_R += Time.deltaTime;
        }
    }

    //시작 판넬에 텍스트 초기화정보 입력
    public void StartPanelTextInit(Text _upExercisName, Text _exerciseName, Text _week, Text _number)
    {
        Debug.Log("EP_LastExerciseNumber  " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        //EP_LastExerciseNumber : 0 이면 마지막으로 한 운동이 근력하체. 즉, 시작하는 운동은 율동운동
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            _exerciseName.text = "율동 운동";
            _upExercisName.text = "율동 운동";
        }
        //EP_LastExerciseNumber : 1 이면 마지막으로 한 운동이 율동운동. 즉, 시작하는 운동은 근력상체
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            _exerciseName.text = "근력 운동 상체";
            _upExercisName.text = "근력 운동 상체";
        }
        //EP_LastExerciseNumber : 2 이면 마지막으로 한 운동이 근력상체. 즉, 시작하는 운동은 근력하체
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            _exerciseName.text = "근력 운동 하체";
            _upExercisName.text = "근력 운동 하체";
        }

        //EP_AllWeekNumber : 운동한 횟수를 의미한다.
        if ((PlayerPrefs.GetInt("EP_AllWeekNumber") % 3).Equals(0))
        {
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            {
                //EP_AllWeekNumber : 6일 경우, 6 / 3 = 2 ... 0 => 2주차 3회 운동을 했다.라는 의미
                //하지만 마지막 운동이 하체운동이였으면, 3회차 마지막하체 운동까지 한거기 떄문에
                //주차를 1 더해줘서 다음 주차로 넘어가야한다.
                _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1).ToString() + "주차";
                PlayerPrefs.SetInt("EP_PlayingWeek", (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1));    //운동한 주차
                _number.text = "1";
                PlayerPrefs.SetInt("EP_PlayingNumber", 1);  //운동한 횟수
            }
            else
            {
                //EP_AllWeekNumber : 6일 경우, 6 / 3 = 2 ... 0 => 2주차 3회 운동을 했다.라는 의미
                _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3).ToString() + "주차";
                PlayerPrefs.SetInt("EP_PlayingWeek", PlayerPrefs.GetInt("EP_AllWeekNumber") / 3);    //운동한 주차
                _number.text = "3";
                PlayerPrefs.SetInt("EP_PlayingNumber", 3);  //운동한 횟수
            }

        }
        else if ((PlayerPrefs.GetInt("EP_AllWeekNumber") % 3).Equals(1))
        {
            //EP_AllWeekNumber : 2일 경우, 2 / 3 = 0 ... 1 => 1주차 1회 운동을 했다.라는 의미 +1을 해줘서 주차를 해준다.
            _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1).ToString() + "주차";
            PlayerPrefs.SetInt("EP_PlayingWeek", PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1);    //운동한 주차

            Debug.Log("마지막: " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            {
                _number.text = "2";
                PlayerPrefs.SetInt("EP_PlayingNumber", 2);  //운동한 횟수
            }
            else
            {
                _number.text = "1";
                PlayerPrefs.SetInt("EP_PlayingNumber", 1);  //운동한 횟수
            }
        }
        else if ((PlayerPrefs.GetInt("EP_AllWeekNumber") % 3).Equals(2))
        {
            //EP_AllWeekNumber : 5일 경우, 5 / 3 = 1 ... 2 => 2주차 2회 운동을 했다.라는 의미 +1를 해줘서 주차를 해준다.
            _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1).ToString() + "주차";
            PlayerPrefs.SetInt("EP_PlayingWeek", PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1);    //운동한 주차

            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            {
                _number.text = "3";
                PlayerPrefs.SetInt("EP_PlayingNumber", 3);  //운동한 횟수
            }
            else
            {
                _number.text = "2";
                PlayerPrefs.SetInt("EP_PlayingNumber", 2);  //운동한 횟수
            }
        }
        string str = Regex.Replace(_number.text, "주차", string.Empty);
        PlayerPrefs.SetString("EP_WeekandCount", str + "=" + PlayerPrefs.GetInt("EP_PlayingNumber").ToString());   //주차 및 횟수 저장 (엑셀에 저장하기위함)
    }

    //결과화면에 있는 정보를 업데이트
    public void ResultPanelDataUpdata(Text _motionName, Image _avergeImage)
    {
        NarrationListUpdata();
        _motionName.text = PlayerPrefs.GetString("EP_PlayingExerciseName");
        //_avergeText.text = motionAvageClass;

        if (motionAvageClass.Equals("A"))
            _avergeImage.sprite = classSprite[0];
        else if (motionAvageClass.Equals("B"))
            _avergeImage.sprite = classSprite[1];
        else if (motionAvageClass.Equals("C"))
            _avergeImage.sprite = classSprite[2];

        if (PlayerPrefs.GetString("EP_MotionAllClass").Equals(""))
        {
            //휴식시간 포함 / 휴식시간 미포함 나눠서 저장 :: 엑셀에 저장하기 위해 미포함 따로 만들었음
            PlayerPrefs.SetString("EP_MotionAllClass", motionAvageClass);
            PlayerPrefs.SetString("EP_OnlyMotionAllClass", motionAvageClass);
        }
        else
        {
           PlayerPrefs.SetString("EP_MotionAllClass", PlayerPrefs.GetString("EP_MotionAllClass") + "=" + motionAvageClass);
            PlayerPrefs.SetString("EP_OnlyMotionAllClass", PlayerPrefs.GetString("EP_OnlyMotionAllClass") + "=" + motionAvageClass);
        }
    }

    //최종 마지막 동작 끝나고 모든 동작에 관한 결과화면에 정보를 업데이트
    public float LastResultPanelDataUpdata(Text _aText, Text _bText, Text _cText)
    {
        int one = 0; int two = 0; int three = 0;
        if(PlayerPrefs.GetString("EP_OnlyMotionAllClass") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_OnlyMotionAllClass");

            char ch = '=';
            lastAllMotionClassArr = lastClassData.Split(ch);
        }

        for(int i = 0; i < lastAllMotionClassArr.Length; i++)
        {
            if (lastAllMotionClassArr[i].Equals("A"))
                one += 1;
            else if (lastAllMotionClassArr[i].Equals("B"))
                two += 1;
            else if (lastAllMotionClassArr[i].Equals("C"))
            {
                three += 1;
            }
        }

        allAverage = (((one * 17) + (two * 11) + (three * 5)) / (one + two + three));
        Debug.Log("1차 : " + allAverage + " 합 " + ((one - ExerciseContentData.instacne.restCount) + two + three));

        _aText.text = one.ToString();
        _bText.text = two.ToString();
        _cText.text = three.ToString();

        return allAverage;
    }

    //최종 마지막 동작 끝나고 모든 동작에 관한 결과 리포트 정보를 업데이트
    public void LastResultPanelDataReportUpdata(Text _reportText)
    {
        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
            exerciseType = "근력운동상체";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
            exerciseType = "근력운동하체";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            exerciseType = "율동운동";

        if (PlayerPrefs.GetString("EP_MotionAllClass") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_MotionAllClass");

            char ch = '=';
            lastAllMotionClassArr = lastClassData.Split(ch);
        }

        for (int i = 0; i < lastAllMotionClassArr.Length; i++)
        {
            if (lastAllMotionClassArr[i].Equals("C"))
            {
                if(PlayerPrefs.GetString("EP_CErrorClassGroup").Equals(""))
                {
                    PlayerPrefs.SetString("EP_CErrorClassGroup",
                        ExerciseContentData.instacne.ErrorClassReport(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType, i));
                }
                else
                {
                    PlayerPrefs.SetString("EP_CErrorClassGroup", PlayerPrefs.GetString("EP_CErrorClassGroup") + "// " +
                        ExerciseContentData.instacne.ErrorClassReport(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType, i));
                }
            }
        }

        if (PlayerPrefs.GetString("EP_CErrorClassGroup") != "")
        {
            _reportText.text = "[" + exerciseType  + "] < " + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                " > 동작이 약합니다. 해당 동작을 신경써서 해주시면 더 나은 효과를 볼 수 있습니다.";
            PlayerPrefs.SetString("EP_CErrorClassGroup", _reportText.text);
        }
        else
        {
            for (int i = 0; i < lastAllMotionClassArr.Length; i++)
            {
                if (lastAllMotionClassArr[i].Equals("B"))
                {
                    if (PlayerPrefs.GetString("EP_CErrorClassGroup").Equals(""))
                    {
                        PlayerPrefs.SetString("EP_CErrorClassGroup",
                            ExerciseContentData.instacne.ErrorClassReport(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType, i));
                    }
                    else
                    {
                        PlayerPrefs.SetString("EP_CErrorClassGroup", PlayerPrefs.GetString("EP_CErrorClassGroup") + "// " +
                            ExerciseContentData.instacne.ErrorClassReport(PlayerPrefs.GetInt("EP_AllWeekNumber"), exerciseType, i));
                    }
                }
            }

            if(PlayerPrefs.GetString("EP_CErrorClassGroup") != "")
            {
                _reportText.text = "[" + exerciseType + "]" +" 대체로 약한 동작은 없습니다. 다만 < " + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                " > 동작을 유의해 주시면 좀 더 좋은 효과를 볼 수 있습니다. ";
                PlayerPrefs.SetString("EP_CErrorClassGroup", _reportText.text);
            }
            else
            {
                _reportText.text = "[" + exerciseType + "]" + " 전체적으로 동작이 좋습니다. 이대로 계속 운동을 하시면 됩니다.";
                PlayerPrefs.SetString("EP_CErrorClassGroup", _reportText.text);
            }
        }
    }


    List<Dictionary<string, object>> data;
    //최종 마지막 동작 끝나고 모든 동작에 관한 결과 엑셀에 저장.
    //ExerciseReportClass.csv 파일에 저장 // 운동 동작에 대한 등급 저장
    public void LastDataSaveUpdata()
    {
        //운동 이름 
        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
            exerciseType = "근력운동상체";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
            exerciseType = "근력운동하체";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            exerciseType = "율동운동";

        //평균등급 
        string averageStr = "";
        if (allAverage >= 16f)
            averageStr = "A";    //a
        else if (allAverage < 16f && allAverage >= 14f)
            averageStr = "A-";    //a-
        else if (allAverage < 14f && allAverage >= 12f)
            averageStr = "B+";    // b+
        else if (allAverage < 12f && allAverage >= 10f)
            averageStr = "B";    //b
        else if (allAverage < 10 && allAverage >= 8f)
            averageStr = "B-";    //b-
        else if (allAverage < 8f && allAverage >= 6f)
            averageStr = "C+";   //c+
        else if (allAverage < 6f && allAverage >= 5f)
            averageStr = "C"; //c
        else
            averageStr = "-";

        //주차 및 횟수
        if (PlayerPrefs.GetString("EP_WeekandCount") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_WeekandCount");

            char ch = '=';
            lastWeekCountArr = lastClassData.Split(ch); //주차, 횟수
        }

        
        //내용등급
        if (PlayerPrefs.GetString("EP_OnlyMotionAllClass") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_MotionAllClass");

            char ch = '=';
            lastOnlyAllMotionClassArr = lastClassData.Split(ch);
        }
        Debug.Log(lastWeekCountArr[0] + " :::: " + lastWeekCountArr[1] + " ---- " + lastOnlyAllMotionClassArr.Length);
        Debug.Log("lastOnlyAllMotionClassArr:::: " + lastOnlyAllMotionClassArr[0]);

        data = CSVReader.Read("ExerciseReportClass");
        string filePath = getPath2();
        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter(filePath);

        //This is writing the line of the type, name, damage... etc... (I set these)
        writer.WriteLine("이름,주차,횟수,구분,평균,리포트내용,내용1,내용2,내용3,내용4,내용5,내용6,내용7," +
            "내용8,내용9,내용10,내용11,내용12,내용13,내용14,내용15,내용16,내용17,내용18,내용19,내용20,내용21,내용22,내용23,내용24");


        Debug.Log("data.Count " + data.Count);
        for (int i = 0; i < data.Count + 1; ++i)
        {
            if(data.Count.Equals(0))
            {
                Debug.Log("--------?????");
                if (exerciseType.Equals("율동운동"))
                {
                    if (int.Parse(lastWeekCountArr[0]) <= 4)
                    {
                        writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                            "," + lastWeekCountArr[0] +
                            "," + lastWeekCountArr[1] +
                            "," + exerciseType +
                            "," + averageStr +
                            "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                            "," + lastOnlyAllMotionClassArr[0] + 
                            "," + lastOnlyAllMotionClassArr[1] +
                            "," + "" + //lastOnlyAllMotionClassArr[2] + 
                            "," + "" + //lastOnlyAllMotionClassArr[3] +
                            "," + ""+//lastOnlyAllMotionClassArr[4] + 
                            "," + "" +//lastOnlyAllMotionClassArr[5] +
                            "," + "" +//lastOnlyAllMotionClassArr[6] + 
                            "," + "" +//lastOnlyAllMotionClassArr[7] +
                            "," + "" +//lastOnlyAllMotionClassArr[8] + 
                            "," + "" +//lastOnlyAllMotionClassArr[9] +
                            "," + "" +//lastOnlyAllMotionClassArr[10] + 
                            "," + "" +//lastOnlyAllMotionClassArr[11] +
                            "," + "" +//lastOnlyAllMotionClassArr[12] + 
                            "," + "" +//lastOnlyAllMotionClassArr[13] +
                            "," + "" +//lastOnlyAllMotionClassArr[14] + 
                            "," + "" + 
                            "," + "" + 
                            "," + "" + 
                            "," + "" + 
                            "," + "" + 
                            "," + "" + 
                            "," + "" + 
                            "," + "" + 
                            "," + "");
                    }
                    else if (int.Parse(lastWeekCountArr[0]) > 4 && int.Parse(lastWeekCountArr[0]) <= 8)
                    {
                        writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                            "," + lastWeekCountArr[0] +
                            "," + lastWeekCountArr[1] +
                            "," + exerciseType +
                            "," + averageStr +
                            "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                            "," + lastOnlyAllMotionClassArr[0] + "," + lastOnlyAllMotionClassArr[1] +
                            "," + lastOnlyAllMotionClassArr[2] + "," + lastOnlyAllMotionClassArr[3] +
                            "," + lastOnlyAllMotionClassArr[4] + "," + lastOnlyAllMotionClassArr[5] +
                            "," + lastOnlyAllMotionClassArr[6] + "," + lastOnlyAllMotionClassArr[7] +
                            "," + lastOnlyAllMotionClassArr[8] + "," + lastOnlyAllMotionClassArr[9] +
                            "," + lastOnlyAllMotionClassArr[10] + "," + lastOnlyAllMotionClassArr[11] +
                            "," + lastOnlyAllMotionClassArr[12] + "," + lastOnlyAllMotionClassArr[13] +
                            "," + lastOnlyAllMotionClassArr[14] + "," + lastOnlyAllMotionClassArr[15] +
                            "," + lastOnlyAllMotionClassArr[16] + "," + lastOnlyAllMotionClassArr[17] +
                            "," + lastOnlyAllMotionClassArr[18] + "," + lastOnlyAllMotionClassArr[19]);
                    }
                    else if (int.Parse(lastWeekCountArr[0]) > 8)
                    {
                        writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                            "," + lastWeekCountArr[0] +
                            "," + lastWeekCountArr[1] +
                            "," + exerciseType +
                            "," + averageStr +
                            "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                            "," + lastOnlyAllMotionClassArr[0] + "," + lastOnlyAllMotionClassArr[1] +
                            "," + lastOnlyAllMotionClassArr[2] + "," + lastOnlyAllMotionClassArr[3] +
                            "," + lastOnlyAllMotionClassArr[4] + "," + lastOnlyAllMotionClassArr[5] +
                            "," + lastOnlyAllMotionClassArr[6] + "," + lastOnlyAllMotionClassArr[7] +
                            "," + lastOnlyAllMotionClassArr[8] + "," + lastOnlyAllMotionClassArr[9] +
                            "," + lastOnlyAllMotionClassArr[10] + "," + lastOnlyAllMotionClassArr[11] +
                            "," + lastOnlyAllMotionClassArr[12] + "," + lastOnlyAllMotionClassArr[13] +
                            "," + lastOnlyAllMotionClassArr[14] + "," + lastOnlyAllMotionClassArr[15] +
                            "," + lastOnlyAllMotionClassArr[16] + "," + lastOnlyAllMotionClassArr[17] +
                            "," + lastOnlyAllMotionClassArr[18] + "," + lastOnlyAllMotionClassArr[19] +
                            "," + lastOnlyAllMotionClassArr[20] + "," + lastOnlyAllMotionClassArr[21] +
                            "," + lastOnlyAllMotionClassArr[22]);
                    }
                }
                else if (exerciseType.Equals("근력운동상체"))
                {
                    writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                    "," + lastWeekCountArr[0] +
                    "," + lastWeekCountArr[1] +
                    "," + exerciseType +
                    "," + averageStr +
                    "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                    "," + lastOnlyAllMotionClassArr[0] +
                    "," + lastOnlyAllMotionClassArr[1] +
                    "," + lastOnlyAllMotionClassArr[2]);
                }
                else if (exerciseType.Equals("근력운동하체"))
                {
                    writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                    "," + lastWeekCountArr[0] +
                    "," + lastWeekCountArr[1] +
                    "," + exerciseType +
                    "," + averageStr +
                    "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                    "," + lastOnlyAllMotionClassArr[0] +
                    "," + lastOnlyAllMotionClassArr[1] +
                    "," + lastOnlyAllMotionClassArr[2] +
                    "," + lastOnlyAllMotionClassArr[3] +
                    "," + lastOnlyAllMotionClassArr[4]);
                }
            }
            else
            {
                if (i <= data.Count - 1)
                {
                    Debug.Log("--------");
                    writer.WriteLine(data[i]["이름"] +
                    "," + data[i]["주차"] +
                    "," + data[i]["횟수"] +
                    "," + data[i]["구분"] +
                    "," + data[i]["평균"] +
                    "," + data[i]["리포트내용"] +
                    "," + data[i]["내용1"] +
                    "," + data[i]["내용2"] +
                    "," + data[i]["내용3"] +
                    "," + data[i]["내용4"] +
                    "," + data[i]["내용5"] +
                    "," + data[i]["내용6"] +
                    "," + data[i]["내용7"] +
                    "," + data[i]["내용8"] +
                    "," + data[i]["내용9"] +
                    "," + data[i]["내용10"] +
                    "," + data[i]["내용11"] +
                    "," + data[i]["내용12"] +
                    "," + data[i]["내용13"] +
                    "," + data[i]["내용14"] +
                    "," + data[i]["내용15"] +
                    "," + data[i]["내용16"] +
                    "," + data[i]["내용17"] +
                    "," + data[i]["내용18"] + "," + data[i]["내용19"] +
                    "," + data[i]["내용20"] + "," + data[i]["내용21"] +
                    "," + data[i]["내용22"] + "," + data[i]["내용23"] +
                    "," + data[i]["내용24"]);
                }
                else if (i.Equals(data.Count))
                {
                    if (exerciseType.Equals("율동운동"))
                    {
                        if (int.Parse(lastWeekCountArr[0]) <= 4)
                        {
                            writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                            "," + lastWeekCountArr[0] +
                            "," + lastWeekCountArr[1] +
                            "," + exerciseType +
                            "," + averageStr +
                            "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                            "," + lastOnlyAllMotionClassArr[0] +
                            "," + lastOnlyAllMotionClassArr[1] +
                            "," + lastOnlyAllMotionClassArr[2] + 
                            "," + lastOnlyAllMotionClassArr[3] +
                            "," + lastOnlyAllMotionClassArr[4] + 
                            "," + lastOnlyAllMotionClassArr[5] +
                            "," + lastOnlyAllMotionClassArr[6] + 
                            "," + lastOnlyAllMotionClassArr[7] +
                            "," + "" +//lastOnlyAllMotionClassArr[8] + 
                            "," + "" +//lastOnlyAllMotionClassArr[9] +
                            "," + "" +//lastOnlyAllMotionClassArr[10] + 
                            "," + "" +//lastOnlyAllMotionClassArr[11] +
                            "," + "" +//lastOnlyAllMotionClassArr[12] + 
                            "," + "" +//lastOnlyAllMotionClassArr[13] +
                            "," + "" +//lastOnlyAllMotionClassArr[14] + 
                            "," + "" +
                            "," + "" +
                            "," + "" +
                            "," + "" +
                            "," + "" +
                            "," + "" +
                            "," + "" +
                            "," + "" +
                            "," + "");
                        }
                        else if (int.Parse(lastWeekCountArr[0]) > 4 && int.Parse(lastWeekCountArr[0]) <= 8)
                        {
                            writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                                "," + lastWeekCountArr[0] +
                                "," + lastWeekCountArr[1] +
                                "," + exerciseType +
                                "," + averageStr +
                                "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                                "," + lastOnlyAllMotionClassArr[0] + "," + lastOnlyAllMotionClassArr[1] +
                                "," + lastOnlyAllMotionClassArr[2] + "," + lastOnlyAllMotionClassArr[3] +
                                "," + lastOnlyAllMotionClassArr[4] + "," + lastOnlyAllMotionClassArr[5] +
                                "," + lastOnlyAllMotionClassArr[6] + "," + lastOnlyAllMotionClassArr[7] +
                                "," + lastOnlyAllMotionClassArr[8] + "," + lastOnlyAllMotionClassArr[9] +
                                "," + lastOnlyAllMotionClassArr[10] + "," + lastOnlyAllMotionClassArr[11] +
                                "," + lastOnlyAllMotionClassArr[12] + "," + lastOnlyAllMotionClassArr[13] +
                                "," + lastOnlyAllMotionClassArr[14] + "," + lastOnlyAllMotionClassArr[15] +
                                "," + lastOnlyAllMotionClassArr[16] + "," + lastOnlyAllMotionClassArr[17] +
                                "," + lastOnlyAllMotionClassArr[18] + "," + lastOnlyAllMotionClassArr[19] +
                                "," + "" + "," + "" + "," + "" + "," + "");
                        }
                        else if (int.Parse(lastWeekCountArr[0]) > 8)
                        {
                            writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                                "," + lastWeekCountArr[0] +
                                "," + lastWeekCountArr[1] +
                                "," + exerciseType +
                                "," + averageStr +
                                "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                                "," + lastOnlyAllMotionClassArr[0] + "," + lastOnlyAllMotionClassArr[1] +
                                "," + lastOnlyAllMotionClassArr[2] + "," + lastOnlyAllMotionClassArr[3] +
                                "," + lastOnlyAllMotionClassArr[4] + "," + lastOnlyAllMotionClassArr[5] +
                                "," + lastOnlyAllMotionClassArr[6] + "," + lastOnlyAllMotionClassArr[7] +
                                "," + lastOnlyAllMotionClassArr[8] + "," + lastOnlyAllMotionClassArr[9] +
                                "," + lastOnlyAllMotionClassArr[10] + "," + lastOnlyAllMotionClassArr[11] +
                                "," + lastOnlyAllMotionClassArr[12] + "," + lastOnlyAllMotionClassArr[13] +
                                "," + lastOnlyAllMotionClassArr[14] + "," + lastOnlyAllMotionClassArr[15] +
                                "," + lastOnlyAllMotionClassArr[16] + "," + lastOnlyAllMotionClassArr[17] +
                                "," + lastOnlyAllMotionClassArr[18] + "," + lastOnlyAllMotionClassArr[19] +
                                "," + lastOnlyAllMotionClassArr[20] + "," + lastOnlyAllMotionClassArr[21] +
                                "," + lastOnlyAllMotionClassArr[22] + "," + "");
                        }
                    }
                    else if (exerciseType.Equals("근력운동상체"))
                    {
                        writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                        "," + lastWeekCountArr[0] +
                        "," + lastWeekCountArr[1] +
                        "," + exerciseType +
                        "," + averageStr +
                        "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                        "," + lastOnlyAllMotionClassArr[0] +
                        "," + lastOnlyAllMotionClassArr[1] +
                        "," + lastOnlyAllMotionClassArr[2]);
                    }
                    else if (exerciseType.Equals("근력운동하체"))
                    {
                        writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                        "," + lastWeekCountArr[0] +
                        "," + lastWeekCountArr[1] +
                        "," + exerciseType +
                        "," + averageStr +
                        "," + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                        "," + lastOnlyAllMotionClassArr[0] +
                        "," + lastOnlyAllMotionClassArr[1] +
                        "," + lastOnlyAllMotionClassArr[2] +
                        "," + lastOnlyAllMotionClassArr[3] +
                        "," + lastOnlyAllMotionClassArr[4]);
                    }
                }
            }
        }
        writer.Flush();
        //This closes the file
        writer.Close();

        AssetDatabase.Refresh();
        data = CSVReader.Read("ExerciseReportClass");
    }



    string allWeekStr;  //주 전체 운동 등급 저장 변수
    string[] allWeekEventArr;   //총 주별 등급 짜른 배열 변수(몇주 - ex:3주동안)
    string[][] weekArr; //주별 등급 저장 ex:weekarr[3][0] A,B,A 저장되어있음
    //총 전체 등급을 주별로 나눔
    public void AllClassDivide()
    {
        //주 전체 등급을 하나로 합치는 작업
        if (PlayerPrefs.GetString("EP_FourWeek") != "No")
        {
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek") + "/" + PlayerPrefs.GetString("EP_FourWeek");
        }
        else if (PlayerPrefs.GetString("EP_ThreeWeek") != "No")
        {
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek");
        }
        else if (PlayerPrefs.GetString("EP_TwoWeek") != "No")
        {
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek");
        }
        else if (PlayerPrefs.GetString("EP_OneWeek") != "No")
        {
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek");
        }
        //else if(PlayerPrefs.GetString("EP_OneWeek").Equals("No"))
        //{

        //}

        //Debug.Log(" allWeekStr " + allWeekStr);
        char weekCh = '=';

        if (allWeekStr != "")
        {
            string classData = allWeekStr;  //ex: A=A=B/C=B=B/A=C=B

            char ch = '/';
            allWeekEventArr = classData.Split(ch); //A-A-B(1주), C-B-B(2주), A-C-B(3주) 따로 저장
        }

        weekArr = new string[allWeekEventArr.Length][];
        //Debug.Log(" allWeekEventArr " + allWeekEventArr.Length);
        PlayerPrefs.SetInt("EP_AllWeekNumber", allWeekEventArr.Length); //총 회차 저장

        for (int i = 0; i < allWeekEventArr.Length; i++)
        {
            string weekData = allWeekEventArr[i];   //A-A-B
            weekArr[i] = weekData.Split(weekCh);    //A,A,B 따로 저장
        }

        
    }
    //각 운동 등급
    public string ExerciseClass(int _index)
    {
        exerciseClassSum = 0;
        exerciseNumber = 0;
        //횟수로 총 몇번했는지 
        for (int i = 0; i < allWeekEventArr.Length; i++)
        {
            //율동운동이다.
            if (_index % 3 == 0)
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if (_index % 3 == 1)    //근력운동상체
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if (_index % 3 == 2)    //근력운동하체
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
        }
        Debug.Log("exerciseClassSum " + exerciseClassSum + "  " + exerciseNumber);

        int average = 0;
        if (exerciseClassSum != 0)
        {
            //각 운동의 평균값
            average = exerciseClassSum / exerciseNumber;
            Debug.Log("average " + exerciseClassSum / exerciseNumber + "   " + average);
        }
        

        //각 운동평균값에 따른 등급 반환
        return ExerciseClassAverage(average);
    }
    string classStr;
    //각 운동 평균값에 따른 등급
    string ExerciseClassAverage(float _average)
    {
        if (_average >= 16f) //A
        {
            classStr = "A";
        }
        else if (_average < 16f && _average >= 14f)  //A-
        {
            classStr = "A-";
            Debug.Log("여기아닌가? ");
        }
        else if (_average < 14f && _average >= 12f)   //B+
        {
            classStr = "B+";
        }
        else if (_average < 12f && _average >= 10f)    //B
        {
            classStr = "B";
        }
        else if (_average < 10 && _average >= 8f)   //B-
        {
            classStr = "B-";
        }
        else if (_average < 8f && _average >= 6f) //C+
        {
            classStr = "C+";
        }
        else if (_average < 6f && _average >= 5f) //C
        {
            classStr = "C";
        }
        else
        {
            classStr = "-";
        }
        return classStr;
    }
    int exerciseNumber; //운동 횟수
    int allExerciseSum; //운동총점수
    int allExerciseNumber;  //운동총 횟수
    int exerciseClassSum;   //운동별 등급 점수 합계
    //등급에 따른 합계
    void ExerciseClassScoreSum(int _i, int _index)
    {
        if (weekArr[_i][_index].Equals("A"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 17;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 17;   //운동 총 합계
        }
        else if (weekArr[_i][_index].Equals("A-"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 15;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 15;   //운동 총 합계
        }
        else if (weekArr[_i][_index].Equals("B+"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 13;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 13;   //운동 총 합계
        }
        else if (weekArr[_i][_index].Equals("B"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 11;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 11;   //운동 총 합계
        }
        else if (weekArr[_i][_index].Equals("B-"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 9;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 9;   //운동 총 합계
        }
        else if (weekArr[_i][_index].Equals("C+"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 7;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 7;   //운동 총 합계
        }
        else if (weekArr[_i][_index].Equals("C"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 5;

            allExerciseNumber += 1; //운동 총 횟수
            allExerciseSum += 5;    //운동 총 합계
        }
        Debug.Log("allExerciseSum " + allExerciseSum + "    " + allExerciseNumber);
    }
    //총 운동 평균 등급
    public string AllExerciseAverage()
    {
        float average = 0;
        if (allExerciseSum !=  0)
        {
            average = allExerciseSum / allExerciseNumber;
        }
        

        return ExerciseClassAverage(average);
    }

    //주차에 데이터 더해서 저장
    void WeekDataSave(string _exerciseType, string _week, string _averageStr)
    {
        if (_exerciseType.Equals("율동운동"))
        {
            Debug.Log("---------율동 등급 " + _week+ "  " + PlayerPrefs.GetString(_week));
            if (PlayerPrefs.GetString(_week).Equals("") || PlayerPrefs.GetString(_week).Equals("No"))
                PlayerPrefs.SetString(_week, _averageStr);
            else
                PlayerPrefs.SetString(_week, "/" + _averageStr);
        }
        else
        {
            PlayerPrefs.SetString(_week, "=" + _averageStr);
        }
            
    }
    List<Dictionary<string, object>> data2;
    //최종 마지막 동작 끝나고 모든 동작 각각의 운동에 대한 평균, 주차에 대한 저장 엑셀에
    //Student Data.csv 파일에 저장 // 이용자에 대한 기본정보와 등급에 대한 저장
    public void UserInfoLastSaveUpdata()
    {
        //방금 운동을 한 것에 대한 평균등급 
        string averageStr = "";
        if (allAverage >= 16f)
            averageStr = "A";    //a
        else if (allAverage < 16f && allAverage >= 14)
            averageStr = "A-";    //a-
        else if (allAverage < 14f && allAverage >= 12f)
            averageStr = "B+";    // b+
        else if (allAverage < 12f && allAverage >= 10f)
            averageStr = "B";    //b
        else if (allAverage < 10 && allAverage >= 8f)
            averageStr = "B-";    //b-
        else if (allAverage < 8f && allAverage >= 6f)
            averageStr = "C+";   //c+
        else if (allAverage < 6f && allAverage >= 5f)
            averageStr = "C"; //c

        //운동 이름 
        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            exerciseType = "근력운동상체";
            //PlayerPrefs.SetString("EP_MuscularUpClass", PlayerPrefs.GetString("EP_MuscularUpClass") + averageStr);

        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            exerciseType = "근력운동하체";
            //PlayerPrefs.SetString("EP_MuscularDownClass", PlayerPrefs.GetString("EP_MuscularDownClass") + averageStr);
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            exerciseType = "율동운동";
            //PlayerPrefs.SetString("EP_DanceRoutineClass", PlayerPrefs.GetString("EP_DanceRoutineClass") + averageStr);
        }
            

        //주차 및 횟수
        if (PlayerPrefs.GetString("EP_WeekandCount") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_WeekandCount");

            char ch = '=';
            lastWeekCountArr = lastClassData.Split(ch); //주차, 횟수
        }

        int weekNum = 12 - (12 - int.Parse(lastWeekCountArr[0]));
       
        //주차에 데이터 더해서 저장
        if(weekNum.Equals(1))
            WeekDataSave(exerciseType, "EP_OneWeek", averageStr);
        else if(weekNum.Equals(2))
            WeekDataSave(exerciseType, "EP_TwoWeek", averageStr);
        else if(weekNum.Equals(3))
            WeekDataSave(exerciseType, "EP_ThreeWeek", averageStr);
        else if(weekNum.Equals(4))
            WeekDataSave(exerciseType, "EP_FourWeek", averageStr);


        //전체 등급 주별로 나눠서 저장
        AllClassDivide();   





        data2 = CSVReader.Read("Student Data");


        string filePath = getPath();
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("이름,생년월일,성별,키,몸무게,고혈압,저혈압,당뇨,치매,관절염,협심증,뇌졸증,기타,시작일,마지막일,진도," +
            "전체등급,율동등급,근력상체등급,근력하체등급,1주,2주,3주,4주");


        PlayerPrefs.SetString("EP_EndDay", DateTime.Now.ToString("yyyy-MM-dd"));
        PlayerPrefs.SetString("EP_Progress", exerciseType + "완료");
        
        PlayerPrefs.SetString("EP_DanceRoutineClass", ExerciseClass(0));
        PlayerPrefs.SetString("EP_MuscularUpClass", ExerciseClass(1));
        PlayerPrefs.SetString("EP_MuscularDownClass", ExerciseClass(2));
        PlayerPrefs.SetString("EP_AllClass", AllExerciseAverage());

        Debug.Log("EP_EndDay : " + PlayerPrefs.GetString("EP_EndDay") + " EP_Progress " + PlayerPrefs.GetString("EP_Progress"));
        Debug.Log("EP_AllClass : " + PlayerPrefs.GetString("EP_AllClass") + " EP_DanceRoutineClass " + PlayerPrefs.GetString("EP_DanceRoutineClass"));
        Debug.Log("data.Count : " + data2.Count);
        for (int i = 0; i < data2.Count; ++i)
        {
            if (data2[i]["이름"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
                Debug.Log("들어왔음");
                writer.WriteLine(PlayerPrefs.GetString("EP_UserNAME") +
                "," + PlayerPrefs.GetString("EP_UserBrithDay") +
                "," + PlayerPrefs.GetString("EP_UserSex") +
                "," + PlayerPrefs.GetString("EP_UserHeight") +
                "," + PlayerPrefs.GetString("EP_UserWight") +
                "," + PlayerPrefs.GetString("EP_UserDisease_1") +
                "," + PlayerPrefs.GetString("EP_UserDisease_2") +
                "," + PlayerPrefs.GetString("EP_UserDisease_3") +
                "," + PlayerPrefs.GetString("EP_UserDisease_4") +
                "," + PlayerPrefs.GetString("EP_UserDisease_5") +
                "," + PlayerPrefs.GetString("EP_UserDisease_6") +
                "," + PlayerPrefs.GetString("EP_UserDisease_7") +
                "," + PlayerPrefs.GetString("EP_OthersDisease") +
                "," + PlayerPrefs.GetString("EP_StartDay") +
                "," + PlayerPrefs.GetString("EP_EndDay") +
                "," + PlayerPrefs.GetString("EP_Progress") +
                "," + PlayerPrefs.GetString("EP_AllClass") +
                "," + PlayerPrefs.GetString("EP_DanceRoutineClass") +
                "," + PlayerPrefs.GetString("EP_MuscularUpClass") +
                "," + PlayerPrefs.GetString("EP_MuscularDownClass") +
                "," + PlayerPrefs.GetString("EP_OneWeek") +
                "," + PlayerPrefs.GetString("EP_TwoWeek") +
                "," + PlayerPrefs.GetString("EP_ThreeWeek") +
                "," + PlayerPrefs.GetString("EP_FourWeek"));
            }
            else
            {
                writer.WriteLine(data2[i]["이름"] +
                "," + data2[i]["생년월일"] +
                "," + data2[i]["성별"] +
                "," + data2[i]["키"] +
                "," + data2[i]["몸무게"] +
                "," + data2[i]["고혈압"] +
                "," + data2[i]["저혈압"] +
                "," + data2[i]["당뇨"] +
                "," + data2[i]["치매"] +
                "," + data2[i]["관절염"] +
                "," + data2[i]["협심증"] +
                "," + data2[i]["뇌졸증"] +
                "," + data2[i]["기타"] +
                "," + data2[i]["시작일"] +
                "," + data2[i]["마지막일"] +
                "," + data2[i]["진도"] +
                "," + data2[i]["전체등급"] +
                "," + data2[i]["율동등급"] +
                "," + data2[i]["근력상체등급"] +
                "," + data2[i]["근력하체등급"] +
                "," + data2[i]["1주"] +
                "," + data2[i]["2주"] +
                "," + data2[i]["3주"] +
                "," + data2[i]["4주"]);
            }
        }
        writer.Flush();
        //This closes the file
        writer.Close();

        AssetDatabase.Refresh();
        data2 = CSVReader.Read("Student Data");
    }

    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + "Student Data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Student Data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Student Data.csv";
#else
        return Application.dataPath +"/"+"Student Data.csv";
#endif
    }

    private string getPath2()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + "ExerciseReportClass.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"ExerciseReportClass.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"ExerciseReportClass.csv";
#else
        return Application.dataPath +"/"+"ExerciseReportClass.csv";
#endif
    }




    //결과화면에 있는 내래이션 리스트 업데이트
    public void NarrationListUpdata()
    {
        if(PlayerPrefs.GetString("EP_MotionCheckBothGood") != "")
        {
            copyNarra = Instantiate(narraListPref, parentObj.transform);
            copyNarra.transform.parent = parentObj.transform;
            copyNarra.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("EP_MotionCheckBothGood");
            copyNarra.transform.GetChild(1).GetComponent<Text>().text =
                PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount").ToString();
            copyNarra.name = "BothGood";
        }

        if (PlayerPrefs.GetString("EP_MotionCheckBothBad") != "")
        {
            copyNarra = Instantiate(narraListPref, parentObj.transform);
            copyNarra.transform.parent = parentObj.transform;
            copyNarra.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("EP_MotionCheckBothBad");
            copyNarra.transform.GetChild(1).GetComponent<Text>().text =
                PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount").ToString();
            copyNarra.name = "BothBad";
        }

        if (PlayerPrefs.GetString("EP_MotionCheckLeft_Err1") != "")
        {
            copyNarra = Instantiate(narraListPref, parentObj.transform);
            copyNarra.transform.parent = parentObj.transform;
            copyNarra.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("EP_MotionCheckLeft_Err1");
            copyNarra.transform.GetChild(1).GetComponent<Text>().text =
                PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1").ToString();
            copyNarra.name = "LeftErr1";
        }

        if (PlayerPrefs.GetString("EP_MotionCheckRight_Err1") != "")
        {
            copyNarra = Instantiate(narraListPref, parentObj.transform);
            copyNarra.transform.parent = parentObj.transform;
            copyNarra.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("EP_MotionCheckRight_Err1");
            copyNarra.transform.GetChild(1).GetComponent<Text>().text =
                PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1").ToString();
            copyNarra.name = "RightErr1";
        }

        if (PlayerPrefs.GetString("EP_MotionCheckLeft_Err2") != "")
        {
            copyNarra = Instantiate(narraListPref, parentObj.transform);
            copyNarra.transform.parent = parentObj.transform;
            copyNarra.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("EP_MotionCheckLeft_Err2");
            copyNarra.transform.GetChild(1).GetComponent<Text>().text =
                PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2").ToString();
            copyNarra.name = "LeftErr2";
        }

        if (PlayerPrefs.GetString("EP_MotionCheckRight_Err2") != "")
        {
            copyNarra = Instantiate(narraListPref, parentObj.transform);
            copyNarra.transform.parent = parentObj.transform;
            copyNarra.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("EP_MotionCheckRight_Err2");
            copyNarra.transform.GetChild(1).GetComponent<Text>().text =
                PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2").ToString();
            copyNarra.name = "RightErr2";
        }

        //동작 평균을 내기 위해서 잘못한 동작 총 갯수를 구한다.
        //int allmotionCount = PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") +
        //    PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") +
        //    PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2");

        //if (allmotionCount >= 0 && allmotionCount <= 5)
        //    motionAvageClass = "A";
        //else if (allmotionCount > 5 && allmotionCount <= 10)
        //    motionAvageClass = "B";
        //else if(allmotionCount > 10)
        //    motionAvageClass = "C";

        //동작 평균을 내기 위해서 잘한 동작 갯수를 구해서 평가한다.
        if (PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") >= 6)
            motionAvageClass = "A";
        else if (PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") < 6 && PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") >= 3)
            motionAvageClass = "B";
        else if (PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") < 3)
            motionAvageClass = "C";
    }

    //리스트 목록에 있는 자식들 삭제하는 함수
    public void NarrationListDelete()
    {
        //한번하고 리스트는 사라져야하기 때문에 자식들을 삭제해준다.
        Transform[] child = parentObj.GetComponentsInChildren<Transform>();

        for (int i = 1; i < child.Length; i++)
            Destroy(child[i].gameObject);
    }



    //운동 체크하는 함수
    public void ExerciseMotionCheck(float _angle, int _index, float _way)
    {
        string motionName = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            motionName = "율동운동";
        //EP_LastExerciseNumber : 1 이면 마지막으로 한 운동이 율동운동. 즉, 시작하는 운동은 근력상체
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
            motionName = "근력운동상체";
        //EP_LastExerciseNumber : 2 이면 마지막으로 한 운동이 근력상체. 즉, 시작하는 운동은 근력하체
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
            motionName = "근력운동하체";

        //운동 시작 상태가 되었다. ExercisTimer.cs에서 true/false값이 됨
        if (startMotionState.Equals(true))
        {
            if(motionName.Equals("율동운동"))
            {
                Debug.Log(" +++++++++ " + PlayerPrefs.GetString("EP_PlayingExerciseName"));
                if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("손목 털기"))
                {
                    //Debug.Log("체크시작");
                    motioncustom.ShakeWristMove(_index, _angle);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("팔 앞으로 뻗기"))
                {
                    motioncustom.StretchArmsForward(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("손목 꺾기"))
                {
                    motioncustom.WristBreak(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("손목 돌리기"))
                {
                    motioncustom.TurningWristMove(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("어깨 돌리기"))
                {
                    motioncustom.TurningShoulders(_index, _angle);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("팔을 위로 올리기/내리기"))
                {
                    motioncustom.ArmUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("팔을 앞으로 올리기/내리기"))
                {
                    motioncustom.ArmFrontUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("팔을 옆으로 올리기/내리기"))
                {
                    motioncustom.ArmSideUpDown(_index, _angle, _way, videoTimeText);
                }
                else if(PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("양팔을 위로 올리기/내리기"))
                {
                    motioncustom.BothArmUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("양팔을 앞으로 올리기/내리기"))
                {
                    motioncustom.BothArmFrontUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("양팔을 옆으로 올리기/내리기"))
                {
                    motioncustom.BothArmSideUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("오른쪽 팔 돌리기"))
                {
                    motioncustom.RightArmSpin(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("왼쪽 팔 돌리기"))
                {
                    motioncustom.LeftArmSpin(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("양팔 위로 올리기"))
                {
                    motioncustom.BothArmUp(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("양팔 옆으로 벌리기"))
                {
                    motioncustom.BothArmSideUp(_index, _angle, _way);
                }
            }
            else if(motionName.Equals("근력운동상체"))
            {
                if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("양팔 올리기/내리기"))
                {
                    //motioncustom.BothArmsUpDown(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("한쪽 팔 교대로 옆으로 올리기/내리기"))
                {
                    //motioncustom.OneArmTakeTurnsUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("한쪽 팔 교대로 앞으로 올리기/내리기"))
                {
                    //motioncustom.OneArmTakeFrontBack(_index, _angle, _way);
                }
            }
            else if(motionName.Equals("근력운동하체"))
            {
                if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("무릎 구부리기/펴기"))
                {
                    //motioncustom.KneeBendStretch(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("다리 구부리기/펴기"))
                {
                    //motioncustom.LegGendStretch(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("다리 벌리기/오므리기"))
                {
                    //motioncustom.LegOpenClose(_index, _angle, _way);
                }
            }
        }
    }

}
