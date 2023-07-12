using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserExerciseManager : MonoBehaviour
{
    public static UserExerciseManager instance { get; private set; }

    List<Dictionary<string, object>> data;
    

    public Image myImage;   //얼굴이미지
    public Text nameText;   //이름텍스트
    public Text startText;  //운동시작일텍스트
    public Text endText;    //마지막운동텍스트
    public Text progressText;   //진도텍스트


    string allWeekStr;  //주 전체 운동 등급 저장 변수
    string[] allWeekEventArr;   //총 주별 등급 짜른 배열 변수(몇주 - ex:3주동안)
    string[][] weekArr; //주별 등급 저장 ex:weekarr[3][0] A,B,A 저장되어있음


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        UserDataInit();
        AllClassDivide();
    }

    //해당 유저 정보를 들고온다.
    void UserDataInit()
    {
        data = CSVReader.Read("Student Data");

        for(int i = 0; i < data.Count; i++)
        {
            if(data[i]["이름"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
                PlayerPrefs.SetString("EP_UserNAME", data[i]["이름"].ToString());   //유저이름
                PlayerPrefs.SetString("EP_UserSex", data[i]["성별"].ToString());    //유저성별
                PlayerPrefs.SetString("EP_UserBrithDay", data[i]["생년월일"].ToString());   //유저생년월일
                PlayerPrefs.SetString("EP_UserHeight", data[i]["키"].ToString()); //유저 키
                PlayerPrefs.SetString("EP_UserWight", data[i]["몸무게"].ToString());  //유저 몸무게
                PlayerPrefs.SetString("EP_UserDisease_1", data[i]["고혈압"].ToString());  //고혈압
                PlayerPrefs.SetString("EP_UserDisease_2", data[i]["저혈압"].ToString());  //저혈압
                PlayerPrefs.SetString("EP_UserDisease_3", data[i]["당뇨"].ToString());  //당뇨
                PlayerPrefs.SetString("EP_UserDisease_4", data[i]["치매"].ToString());  //치매
                PlayerPrefs.SetString("EP_UserDisease_5", data[i]["관절염"].ToString());  //관절염
                PlayerPrefs.SetString("EP_UserDisease_6", data[i]["협심증"].ToString());  //협심증
                PlayerPrefs.SetString("EP_UserDisease_7", data[i]["뇌졸증"].ToString());  //뇌졸증
                PlayerPrefs.SetString("EP_OthersDisease", data[i]["기타"].ToString());  //질병기타
                PlayerPrefs.SetString("EP_StartDay", data[i]["시작일"].ToString());   //운동시작일
                PlayerPrefs.SetString("EP_EndDay", data[i]["마지막일"].ToString());   //운동마지막일
                PlayerPrefs.SetString("EP_Progress", data[i]["진도"].ToString());   //운동진도
                PlayerPrefs.SetString("EP_AllClass", data[i]["전체등급"].ToString());   //전체등급
                PlayerPrefs.SetString("EP_DanceRoutineClass", data[i]["율동등급"].ToString());   //율동등급
                PlayerPrefs.SetString("EP_MuscularUpClass", data[i]["근력상체등급"].ToString());   //근력상체등급
                PlayerPrefs.SetString("EP_MuscularDownClass", data[i]["근력하체등급"].ToString());   //근력하체등급
                PlayerPrefs.SetString("EP_OneWeek", data[i]["1주"].ToString());   //1주
                PlayerPrefs.SetString("EP_TwoWeek", data[i]["2주"].ToString());   //2주
                PlayerPrefs.SetString("EP_ThreeWeek", data[i]["3주"].ToString());   //3주
                PlayerPrefs.SetString("EP_FourWeek", data[i]["4주"].ToString());   //4주
            }
        }

        myImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));
        nameText.text = PlayerPrefs.GetString("EP_UserNAME");
        startText.text = PlayerPrefs.GetString("EP_StartDay");
        endText.text = PlayerPrefs.GetString("EP_EndDay");
        progressText.text = PlayerPrefs.GetString("EP_Progress");
    }

    

    //사용자정보 수정
    public void UserProfileCorrectClickOn()
    {
        SceneManager.LoadScene("6.UserProfileCorrect");
    }

    //운동리포트확인 버튼 클릭
    public void ExerciseReportClickOn()
    {
        SceneManager.LoadScene("7.ExerciseReport");
    }

    //운동시작버튼 클릭
    public void ExerciseStartClickOn()
    {
        SceneManager.LoadScene("8.ExerciseScene");
    }



    //나의진도 정보 초기화 보류...
    public void MyProgressInit()
    {
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        string weekStr = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            weekStr = "율동운동완료";
            progressText.text = PlayerPrefs.GetInt("EP_AllWeekNumber") + "주차-" + weekStr;
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            weekStr = "근력운동상체완료";
            progressText.text = PlayerPrefs.GetInt("EP_AllWeekNumber") + "주차-" + weekStr;
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            weekStr = "근력운동하체완료";
            progressText.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") + 1) + "주차-" + weekStr;
        }
    }


    //총 전체 등급을 주별로 나눔
    public void AllClassDivide()
    {
        //주 전체 등급을 하나로 합치는 작업
        if (PlayerPrefs.GetString("EP_FourWeek") != "No")
        {
            Debug.Log("4번째");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek") + "/" + PlayerPrefs.GetString("EP_FourWeek");
        }
        else if (PlayerPrefs.GetString("EP_ThreeWeek") != "No")
        {
            Debug.Log("3번째");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek");
        }
        else if (PlayerPrefs.GetString("EP_TwoWeek") != "No")
        {
            Debug.Log("2번째");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek");
        }
        else if (PlayerPrefs.GetString("EP_OneWeek") != "No")
        {
            Debug.Log("1번째");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek");
        }
        else
        {
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek");
        }
        //else if(PlayerPrefs.GetString("EP_OneWeek").Equals("No"))
        //{

        //}

        Debug.Log(" allWeekStr " + allWeekStr);
        char weekCh = '=';

        if (allWeekStr != "" && allWeekStr != "No")
        {
            string classData = allWeekStr;  //ex: A-A-B/C-B-B/A-C-B

            char ch = '/';
            allWeekEventArr = classData.Split(ch); //A-A-B(1주), C-B-B(2주), A-C-B(3주) 따로 저장


            weekArr = new string[allWeekEventArr.Length][];
            //Debug.Log(" allWeekEventArr " + allWeekEventArr.Length);
            PlayerPrefs.SetInt("EP_AllWeekNumber", allWeekEventArr.Length); //총 회차 저장
            Debug.Log(" allWeekEventArr " + PlayerPrefs.GetInt("EP_AllWeekNumber"));

            for (int i = 0; i < allWeekEventArr.Length; i++)
            {
                string weekData = allWeekEventArr[i];   //A-A-B
                weekArr[i] = weekData.Split(weekCh);    //A,A,B 따로 저장
            }


            int week = 0;
            if (allWeekEventArr.Length % 3 != 0)
                week = (allWeekEventArr.Length / 3) + 1;
            else if (allWeekEventArr.Length % 3 == 0)
                week = allWeekEventArr.Length / 3;

            //Debug.Log(" EP_LastWeek " + week);
            PlayerPrefs.SetString("EP_LastWeek", week.ToString());  //마지막 운동한 주 저장
                                                                    //Debug.Log(" EP_LastWeek " + PlayerPrefs.GetInt("EP_LastWeek"));


            int remainder = 0;
            //나머지를 구해서 어느 운동까지 마쳤는지 확인(운동 3개를 나누어서 나머지가 0이면 3가지 운동을 다 햇다는 듯)
            if (allWeekEventArr.Length % 3 == 0)
                remainder = 3;  //운동 3가지를 다했음
            else if (allWeekEventArr.Length % 3 == 1)
                remainder = 1;
            else if (allWeekEventArr.Length % 3 == 2)
                remainder = 2;

            int count = 0;

            //주에 해당하는 시작번호부터 시작해야하기 때문에 전체 길이에서 나머지를 구한 값을 뺀 값에서부터 시작..
            for (int i = allWeekEventArr.Length - remainder; i < allWeekEventArr.Length; i++)
            {
                for (int j = 0; j < weekArr[i].Length; j++)
                {
                    count += 1;
                }
            }
            PlayerPrefs.SetInt("EP_LastExerciseNumber", count % 3); //마지막 운동한 종류 숫자로 저장(0:근력하, 1:율동, 2:근력상)
            Debug.Log(" EP_LastExerciseNumber " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        }
        else
        {
            PlayerPrefs.SetInt("EP_AllWeekNumber", 0); //총 회차 저장
            PlayerPrefs.SetString("EP_LastWeek", "0");
            PlayerPrefs.SetInt("EP_LastExerciseNumber", 0);
        }
    }
}
