using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExerciseReportManager : MonoBehaviour
{
    public static ExerciseReportManager instance { get; private set; }

    List<Dictionary<string, object>> data;

    public Text reportText; //내정보보기에 있는 내 리포트텍스트

    int danceRoutione1 = 0, danceRoutione2 = 0, danceRoutione3 = 0;
    int muscularUp1 = 0, muscularUp2 = 0, muscularUp3 = 0;
    int muscularDown1 = 0, muscularDown2 = 0, muscularDown3 = 0;
    public GameObject[] exerciseDataImg;




    string classStr;    //운동 등급
    string allWeekStr;  //주 전체 운동 등급 저장 변수
    string[] allWeekEventArr;   //총 주별 등급 짜른 배열 변수(몇주 - ex:3주동안)
    string[][] weekArr; //주별 등급 저장 ex:weekarr[3][0] A,B,A 저장되어있음
    int exerciseClassSum;   //운동별 등급 점수 합계
    int exerciseNumber; //운동 횟수
    int allExerciseSum; //운동총점수
    int allExerciseNumber;  //운동총 횟수
    string[] saveClassStr;  //오픈될 스템프 등급 저장



    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    void Start()
    {
        AllClassDivide();
        saveClassStr = new string[9];
    }

    //백버튼 클릭 이벤트
    public void BackButton()
    {
        SceneManager.LoadScene("5.UserExercise");
    }


    void Update()
    {

    }


    //총 전체 등급을 주별로 나눔
    public void AllClassDivide()
    {
        //주 전체 등급을 하나로 합치는 작업
        if(PlayerPrefs.GetString("EP_FourWeek") != "No")
        {
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek") + "/" + PlayerPrefs.GetString("EP_FourWeek");
        }
        else if(PlayerPrefs.GetString("EP_ThreeWeek") != "No")
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
            string classData = allWeekStr;  //ex: A-A-B/C-B-B/A-C-B

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
        //Debug.Log("i : " + weekArr[0].Length);
        //Debug.Log(">>>> : " + weekArr[0][0]);
    }

    //총 운동 평균 등급
    public string AllExerciseAverage()
    {
        float average = 0;
        if (allExerciseSum != 0)
        {
            average = allExerciseSum / allExerciseNumber;
        }
        

        return ExerciseClassAverage(average);
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
                if(weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if(_index % 3 == 1)    //근력운동상체
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if(_index % 3 == 2)    //근력운동하체
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
        }
        Debug.Log("exerciseClassSum " + exerciseClassSum + "  " + exerciseNumber);

        int average = 0;
        //각 운동의 평균값
        if (exerciseClassSum != 0)
        {
            average = exerciseClassSum / exerciseNumber;
            Debug.Log("average " + exerciseClassSum / exerciseNumber + "   " + average);
        }
        

        //각 운동평균값에 따른 등급 반환
        return ExerciseClassAverage(average);
    }

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
        else if(weekArr[_i][_index].Equals("A-"))
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
        Debug.Log("allExerciseSum " + allExerciseSum + "    "  + allExerciseNumber);
    }

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

    //주 스템프 화면 보여주기
    public void WeekStampShow(Text _weekText, Button[] _stampBtn, Sprite[] _btnSprite)
    {
        int week = 0;
        if (allWeekEventArr.Length % 3 != 0)
            week = (allWeekEventArr.Length / 3) + 1;
        else if (allWeekEventArr.Length % 3 == 0)
            week = allWeekEventArr.Length / 3;

        PlayerPrefs.SetString("EP_LastWeek", week.ToString());  //마지막 운동한 주 저장
        _weekText.text = week + "주차";

        //주에 해당하는 리포트 보여주기
        UserWeekReportInit(week);

        Debug.Log("allWeekEventArr.Length " + allWeekEventArr.Length);

        int remainder = 0;
        //나머지를 구해서 어느 운동까지 마쳤는지 확인(운동 3개를 나누어서 나머지가 0이면 3가지 운동을 다 햇다는 듯)
        if (allWeekEventArr.Length % 3 == 0)
            remainder = 3;  //운동 3가지를 다했음
        else if (allWeekEventArr.Length % 3 == 1)
            remainder = 1;
        else if (allWeekEventArr.Length % 3 == 2)
            remainder = 2;

        int count = 0;

        //마지막 운동한 주의 시작 횟수
        PlayerPrefs.SetInt("EP_LastWeekStartPoint", allWeekEventArr.Length - remainder);
        //주에 해당하는 시작번호부터 시작해야하기 때문에 전체 길이에서 나머지를 구한 값을 뺀 값에서부터 시작..
        for (int i = allWeekEventArr.Length - remainder; i < allWeekEventArr.Length; i++)
        {
            for(int j = 0; j < weekArr[i].Length; j++)
            {
                Debug.Log(j + "  " + weekArr[i][j]);
                saveClassStr[count] = weekArr[i][j];    //오픈될 스템프의 등급 저장
                count += 1;
            }
        }
        PlayerPrefs.SetInt("EP_LastExerciseNumber", count % 3); //마지막 운동한 종류 숫자로 저장(0:근력하, 1:율동, 2:근력상)
        Debug.Log("count " + count);

        //오픈될 스템프(버튼) 활성화 시켜주기
        for(int i = 0; i < count; i++)
        {
            _stampBtn[i].gameObject.SetActive(true);
            //_stampBtn[i].transform.GetChild(0).GetComponent<Text>().text = saveClassStr[i];

            if (saveClassStr[i].Equals("A"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[0];
            else if (saveClassStr[i].Equals("A-"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[1];
            else if (saveClassStr[i].Equals("B+"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[2];
            else if (saveClassStr[i].Equals("B"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[3];
            else if (saveClassStr[i].Equals("B-"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[4];
            else if (saveClassStr[i].Equals("C+"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[5];
            else if (saveClassStr[i].Equals("C"))
                _stampBtn[i].gameObject.GetComponent<Image>().sprite = _btnSprite[6];
        }
    }

    //시작 시 버튼 활성화 비활성화
    public void WeekLeftRightButtonState(Button _leftBtn, Button _rightBtn)
    {
        if(PlayerPrefs.GetString("EP_LastWeek").Equals("1"))
        {
            _leftBtn.interactable = false;  //노 클릭
            _rightBtn.interactable = false; //노 클릭
        }
        else
        {
            _leftBtn.interactable = true;   //클릭
            _rightBtn.interactable = false; //노 클릭
        }
    }

    //주에 해당하는 리포트 들고오기
    void UserWeekReportInit(int _week)
    {
        PlayerPrefs.SetString("EP_WeekReport", ""); //주에 해당하는 리포트 저장
        data = CSVReader.Read("ExerciseReportClass");

        string reportStr = "";

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["이름"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
                if(data[i]["주차"].ToString().Equals(_week.ToString()))
                {
                    if (reportStr.Equals(""))
                    {
                        reportStr = data[i]["횟수"].ToString() + "회차 :: " + data[i]["리포트내용"].ToString();
                        //PlayerPrefs.SetString("EP_WeekReport",
                        //    data[i]["횟수"].ToString() +"회차 :: " + data[i]["리포트내용"].ToString());
                    }
                    else
                    {
                        reportStr = reportStr + "\n" + data[i]["횟수"].ToString() + "회차 :: " + data[i]["리포트내용"].ToString();
                        //PlayerPrefs.SetString("EP_WeekReport", PlayerPrefs.GetString("EP_WeekReport") + "\n" +
                        //    data[i]["횟수"].ToString() + "회차 :: " + data[i]["리포트내용"].ToString());
                    }
                }
            }
        }

        reportText.text = reportStr;
    }

    //왼쪽버튼
    public void WeekLeftButtonClick(Text _weekText, int _clickCount, Button _leftBtn, Button _rightBtn, Button[] _stampBtn)
    {
        int week = int.Parse(PlayerPrefs.GetString("EP_LastWeek")) - _clickCount;
        _weekText.text = week + "주차";

        //주에 해당하는 리포트 생성
        UserWeekReportInit(week);


        if (_weekText.text.Equals("1주차"))
        {
            //1주차때는 더 왼쪽으로 갈게 없으니 왼쪽버튼 비활성화
            _leftBtn.interactable = false;
            _rightBtn.interactable = true;
        }
        else
        {
            _leftBtn.interactable = true;
            _rightBtn.interactable = true;
        }

        //시작 범위 계산 : 3주차 7회이면 , EP_LastWeekSTartPoint가 6으로 저장이 되어있을것이다. 배열이라서 -1해줌
        //왼쪽버튼을 클릭 했기때문에 2주차 스템프가 보여줘야함. 2주차 4회~6회. 즉 3이 나와야하므로
        //6 -(왼쪽버튼 클릭횟수(1) * 3) = 3이 나옴.
        //1주차꺼는 6 - (왼쪾버튼 클릭횟수(2) *3) = 0 (1주차 1회~3회.즉0~2)
        int scope = PlayerPrefs.GetInt("EP_LastWeekStartPoint") - (_clickCount * 3);
        int count = 0;
        for (int i = 0; i < 9; i++)
            saveClassStr[i] = "";

        for (int i = scope; i < scope + 3; i++)
        {
            for (int j = 0; j < weekArr[i].Length; j++)
            {
                Debug.Log(j + "  " + weekArr[i][j]);
                saveClassStr[count] = weekArr[i][j];    //오픈될 스템프의 등급 저장
                count += 1;
            }
        }

        for(int i = 0; i < 9; i++)
            _stampBtn[i].gameObject.SetActive(false);

        //오픈될 스템프(버튼) 활성화 시켜주기
        for (int i = 0; i < count; i++)
        {
            _stampBtn[i].gameObject.SetActive(true);
            _stampBtn[i].transform.GetChild(0).GetComponent<Text>().text = saveClassStr[i];
        }
    }

    //오른쪽 버튼 
    public void WeekRightButtonClick(Text _weekText, int _clickCount, Button _leftBtn, Button _rightBtn, Button[] _stampBtn)
    {
        int week = int.Parse(PlayerPrefs.GetString("EP_LastWeek")) - _clickCount;
        _weekText.text = week + "주차";

        //주에 해당하는 리포트 생성
        UserWeekReportInit(week);

        if (_weekText.text.Equals(PlayerPrefs.GetString("EP_LastWeek") +"주차"))
        {
            //마지막 주차때는 더 오른쪽으로 갈게 없으니 오른쪽버튼 비활성화
            _rightBtn.interactable = false;
            _leftBtn.interactable = true;
        }
        else
        {
            _rightBtn.interactable = true;
            _leftBtn.interactable = true;
        }

        //시작 범위 계산 : 3주차 7회이면 , EP_LastWeekSTartPoint가 6으로 저장이 되어있을것이다. 배열이라서 -1해줌
        //왼쪽버튼을 클릭 했기때문에 2주차 스템프가 보여줘야함. 2주차 4회~6회. 즉 3이 나와야하므로
        //6 -(왼쪽버튼 클릭횟수(1) * 3) = 3이 나옴.
        //1주차꺼는 6 - (왼쪾버튼 클릭횟수(2) *3) = 0 (1주차 1회~3회.즉0~2)
        int scope = PlayerPrefs.GetInt("EP_LastWeekStartPoint") - (_clickCount * 3);
        int count = 0;
        int lastScope;  //for문 중간에 들어가는 범위

        for (int i = 0; i < 9; i++)
            saveClassStr[i] = "";
        
        //마지막 주차일 경우 for문 중간 범위는 총 횟수의 길이를 넣어주고
        if (_weekText.text.Equals(PlayerPrefs.GetString("EP_LastWeek") + "주차"))
            lastScope = allWeekEventArr.Length;
        else
            lastScope = scope + 3;  //아닐경우 범위에서 3더해준다.

        for (int i = scope; i < lastScope; i++)
        {
            for (int j = 0; j < weekArr[i].Length; j++)
            {
                Debug.Log(j + "  " + weekArr[i][j]);
                saveClassStr[count] = weekArr[i][j];    //오픈될 스템프의 등급 저장
                count += 1;
            }
        }

        for (int i = 0; i < 9; i++)
            _stampBtn[i].gameObject.SetActive(false);

        //오픈될 스템프(버튼) 활성화 시켜주기
        for (int i = 0; i < count; i++)
        {
            _stampBtn[i].gameObject.SetActive(true);
            _stampBtn[i].transform.GetChild(0).GetComponent<Text>().text = saveClassStr[i];
        }
    }



    
    //스템프 클릭했을 때 날짜 나오게 하는 함수들
    public void DanceRoutione1()
    {
        if (danceRoutione1.Equals(0))
            danceRoutione1 = 1;
        else if (danceRoutione1.Equals(1))
            danceRoutione1 = 0;

        if (danceRoutione1.Equals(1))
            exerciseDataImg[0].SetActive(true);
        else if (danceRoutione1.Equals(0))
            exerciseDataImg[0].SetActive(false);
    }
    public void DanceRoutione2()
    {
        if (danceRoutione2.Equals(0))
            danceRoutione2 = 1;
        else if (danceRoutione2.Equals(1))
            danceRoutione2 = 0;

        if (danceRoutione2.Equals(1))
            exerciseDataImg[3].SetActive(true);
        else if (danceRoutione2.Equals(0))
            exerciseDataImg[3].SetActive(false);
    }
    public void DanceRoutione3()
    {
        if (danceRoutione3.Equals(0))
            danceRoutione3 = 1;
        else if (danceRoutione3.Equals(1))
            danceRoutione3 = 0;

        if (danceRoutione3.Equals(1))
            exerciseDataImg[6].SetActive(true);
        else if (danceRoutione3.Equals(0))
            exerciseDataImg[6].SetActive(false);
    }
    public void MuscularUp1()
    {
        if (muscularUp1.Equals(0))
            muscularUp1 = 1;
        else if (muscularUp1.Equals(1))
            muscularUp1 = 0;

        if (muscularUp1.Equals(1))
            exerciseDataImg[1].SetActive(true);
        else if (muscularUp1.Equals(0))
            exerciseDataImg[1].SetActive(false);
    }
    public void MuscularUp2()
    {
        if (muscularUp2.Equals(0))
            muscularUp2 = 1;
        else if (muscularUp2.Equals(1))
            muscularUp2 = 0;

        if (muscularUp2.Equals(1))
            exerciseDataImg[4].SetActive(true);
        else if (muscularUp2.Equals(0))
            exerciseDataImg[4].SetActive(false);
    }
    public void MuscularUp3()
    {
        if (muscularUp3.Equals(0))
            muscularUp3 = 1;
        else if (muscularUp3.Equals(1))
            muscularUp3 = 0;

        if (muscularUp3.Equals(1))
            exerciseDataImg[7].SetActive(true);
        else if (muscularUp3.Equals(0))
            exerciseDataImg[7].SetActive(false);
    }
    public void MuscularDown1()
    {
        if (muscularDown1.Equals(0))
            muscularDown1 = 1;
        else if (muscularDown1.Equals(1))
            muscularDown1 = 0;

        if (muscularDown1.Equals(1))
            exerciseDataImg[2].SetActive(true);
        else if (muscularDown1.Equals(0))
            exerciseDataImg[2].SetActive(false);
    }
    public void MuscularDown2()
    {
        if (muscularDown2.Equals(0))
            muscularDown2 = 1;
        else if (muscularDown2.Equals(1))
            muscularDown2 = 0;

        if (muscularDown2.Equals(1))
            exerciseDataImg[5].SetActive(true);
        else if (muscularDown2.Equals(0))
            exerciseDataImg[5].SetActive(false);
    }
    public void MuscularDown3()
    {
        if (muscularDown3.Equals(0))
            muscularDown3 = 1;
        else if (muscularDown3.Equals(1))
            muscularDown3 = 0;

        if (muscularDown3.Equals(1))
            exerciseDataImg[8].SetActive(true);
        else if (muscularDown3.Equals(0))
            exerciseDataImg[8].SetActive(false);
    }
}
