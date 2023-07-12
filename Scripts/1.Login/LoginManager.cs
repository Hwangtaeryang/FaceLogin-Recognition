using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




//https://xd.adobe.com/view/edd8f973-b9fe-4c49-9fd1-2802a33ed13f-b88b/
//https://docs.google.com/spreadsheets/d/1n5PIYpdMMxZ8Abj3co8LVhGDbyLH2Thn3XpLW57CZcg/edit?usp=sharing
public class LoginManager : MonoBehaviour
{
    public Text nameText;
    public Text timeText;
    public Image timeRotateImg;

    bool isRotate;  //타이머 이미지 회전 시작 여부
    bool faceCountEnd;
    string userNameStr = "";

    void Start()
    {
        PlayerPrefs.SetString("EP_UserNAME", "");   //유저이름
        PlayerPrefs.SetString("EP_UserSex", "");    //유저성별
        PlayerPrefs.SetString("EP_UserBrithDay", "");   //유저생년월일
        PlayerPrefs.SetString("EP_UserHeight", ""); //유저 키
        PlayerPrefs.SetString("EP_UserWight", "");  //유저 몸무게
        PlayerPrefs.SetString("EP_UserDisease_1", "No");  //고혈압
        PlayerPrefs.SetString("EP_UserDisease_2", "No");  //저혈압
        PlayerPrefs.SetString("EP_UserDisease_3", "No");  //당뇨
        PlayerPrefs.SetString("EP_UserDisease_4", "No");  //치매
        PlayerPrefs.SetString("EP_UserDisease_5", "No");  //관절염
        PlayerPrefs.SetString("EP_UserDisease_6", "No");  //협심증
        PlayerPrefs.SetString("EP_UserDisease_7", "No");  //뇌졸증
        PlayerPrefs.SetString("EP_OthersDisease", "No");  //질병기타
        PlayerPrefs.SetString("EP_StartDay", "");   //운동시작일
        PlayerPrefs.SetString("EP_EndDay", "");   //운동마지막일
        PlayerPrefs.SetString("EP_Progress", "");   //운동진도
        PlayerPrefs.SetString("EP_AllClass", "No");   //전체등급
        PlayerPrefs.SetString("EP_DanceRoutineClass", "No");   //율동등급
        PlayerPrefs.SetString("EP_MuscularUpClass", "No");    //근력상체등급
        PlayerPrefs.SetString("EP_MuscularDownClass", "No");  //근력하체등급
        PlayerPrefs.SetString("EP_OneWeek", "No");    //1주 A-A-C/B-C-C/A-C-B
        PlayerPrefs.SetString("EP_TwoWeek", "No");    //2주
        PlayerPrefs.SetString("EP_ThreeWeek", "No");  //3주
        PlayerPrefs.SetString("EP_FourWeek", "No");   //4주
        PlayerPrefs.SetString("EP_WeekReport", ""); //주에 해당하는 리포트 저장

        PlayerPrefs.SetString("EP_MyFaceChange", "No"); //사진찍기
        PlayerPrefs.SetInt("EP_AllWeekNumber", 0);  //총 운동한 회차(주가 아님, 횟수임)
        PlayerPrefs.SetInt("EP_LastExerciseNumber", 0); //마지막 운동번호(0:근력하, 1:율동, 2:근력상)
        PlayerPrefs.SetString("EP_LastWeek", "No"); //마지막 운동한 주
        PlayerPrefs.SetInt("EP_LastWeekStartPoint", 0);   //마지막 운동한 주의 시작 횟수

        PlayerPrefs.SetInt("EP_PlayCount", 0);  //운동 동작 플레이된 횟수
        PlayerPrefs.SetString("EP_PlayingExerciseName", "");   //운동한 동작의 이름
        PlayerPrefs.SetInt("EP_PlayingWeek", 1);    //운동한 주차
        PlayerPrefs.SetInt("EP_PlayingNumber", 1);  //운동한 횟수
        PlayerPrefs.SetInt("EP_NarratoinTime", 1);  //내레이션나오는 시간
        PlayerPrefs.SetString("EP_IsExerciseName", ""); //현재 하고 있는 동작의 운동종류

        //PlayerPrefs.SetInt("EP_ExerciseTypeAllNumber", 0); //해당 운동의 동작 총갯수

        //운동 동작을 할때 체크한거 횟수랑 뭐가 나왔는지 체크를 위한것들
        PlayerPrefs.SetString("EP_MotionCheckBothGood", "");
        PlayerPrefs.SetString("EP_MotionCheckBothBad", "");
        PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "");
        PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "");
        PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "");
        PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "");
        PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", 0);
        PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", 0);
        PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", 0);
        PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", 0);
        PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", 0);
        PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", 0);

        PlayerPrefs.SetString("EP_MotionAllClass", ""); //운동 동작 등급 모두 저장(휴식시간포함)
        PlayerPrefs.SetString("EP_OnlyMotionAllClass", ""); //운동동작 등급 모두 저장 - 휴식시간 미포함
        PlayerPrefs.SetString("EP_CErrorClassGroup", "");   //C등급 동작 모음
        PlayerPrefs.SetString("EP_WeekandCount", "");   //주차 및 횟수 저장 (엑셀에 저장하기위함)

        StartCoroutine(TimeCount());    //5초카운트
    }

    // Update is called once per frame
    void Update()
    {
        if (userNameStr.Equals(""))
        {
            if (nameText.text != "")
                userNameStr = nameText.text;

            PlayerPrefs.SetString("EP_UserNAME", userNameStr);
        }

        if (isRotate.Equals(true))
            timeRotateImg.transform.Rotate(new Vector3(0, 0, -10f));
    }

    IEnumerator TimeCount()
    {
        isRotate = true;
        yield return new WaitForSeconds(1);

        timeText.text = "4";
        yield return new WaitForSeconds(1);
        timeText.text = "3";
        yield return new WaitForSeconds(1);
        timeText.text = "2";
        yield return new WaitForSeconds(1);
        timeText.text = "1";
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("2.LoginResult");
    }
}
