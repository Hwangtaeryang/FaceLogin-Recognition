using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExerciseReportManager : MonoBehaviour
{
    public static ExerciseReportManager instance { get; private set; }

    List<Dictionary<string, object>> data;

    public Text reportText; //���������⿡ �ִ� �� ����Ʈ�ؽ�Ʈ

    int danceRoutione1 = 0, danceRoutione2 = 0, danceRoutione3 = 0;
    int muscularUp1 = 0, muscularUp2 = 0, muscularUp3 = 0;
    int muscularDown1 = 0, muscularDown2 = 0, muscularDown3 = 0;
    public GameObject[] exerciseDataImg;




    string classStr;    //� ���
    string allWeekStr;  //�� ��ü � ��� ���� ����
    string[] allWeekEventArr;   //�� �ֺ� ��� ¥�� �迭 ����(���� - ex:3�ֵ���)
    string[][] weekArr; //�ֺ� ��� ���� ex:weekarr[3][0] A,B,A ����Ǿ�����
    int exerciseClassSum;   //��� ��� ���� �հ�
    int exerciseNumber; //� Ƚ��
    int allExerciseSum; //�������
    int allExerciseNumber;  //��� Ƚ��
    string[] saveClassStr;  //���µ� ������ ��� ����



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

    //���ư Ŭ�� �̺�Ʈ
    public void BackButton()
    {
        SceneManager.LoadScene("5.UserExercise");
    }


    void Update()
    {

    }


    //�� ��ü ����� �ֺ��� ����
    public void AllClassDivide()
    {
        //�� ��ü ����� �ϳ��� ��ġ�� �۾�
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
            allWeekEventArr = classData.Split(ch); //A-A-B(1��), C-B-B(2��), A-C-B(3��) ���� ����
        }

        weekArr = new string[allWeekEventArr.Length][];
        //Debug.Log(" allWeekEventArr " + allWeekEventArr.Length);
        PlayerPrefs.SetInt("EP_AllWeekNumber", allWeekEventArr.Length); //�� ȸ�� ����

        for (int i = 0; i < allWeekEventArr.Length; i++)
        {
            string weekData = allWeekEventArr[i];   //A-A-B
            weekArr[i] = weekData.Split(weekCh);    //A,A,B ���� ����
        }
        //Debug.Log("i : " + weekArr[0].Length);
        //Debug.Log(">>>> : " + weekArr[0][0]);
    }

    //�� � ��� ���
    public string AllExerciseAverage()
    {
        float average = 0;
        if (allExerciseSum != 0)
        {
            average = allExerciseSum / allExerciseNumber;
        }
        

        return ExerciseClassAverage(average);
    }

    //�� � ���
    public string ExerciseClass(int _index)
    {
        exerciseClassSum = 0;
        exerciseNumber = 0;
        //Ƚ���� �� ����ߴ��� 
        for (int i = 0; i < allWeekEventArr.Length; i++)
        {
            //������̴�.
            if (_index % 3 == 0)
            {
                if(weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if(_index % 3 == 1)    //�ٷ¿��ü
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if(_index % 3 == 2)    //�ٷ¿��ü
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
        }
        Debug.Log("exerciseClassSum " + exerciseClassSum + "  " + exerciseNumber);

        int average = 0;
        //�� ��� ��հ�
        if (exerciseClassSum != 0)
        {
            average = exerciseClassSum / exerciseNumber;
            Debug.Log("average " + exerciseClassSum / exerciseNumber + "   " + average);
        }
        

        //�� ���հ��� ���� ��� ��ȯ
        return ExerciseClassAverage(average);
    }

    //��޿� ���� �հ�
    void ExerciseClassScoreSum(int _i, int _index)
    {
        if (weekArr[_i][_index].Equals("A"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 17;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 17;   //� �� �հ�
        }
        else if(weekArr[_i][_index].Equals("A-"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 15;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 15;   //� �� �հ�
        }
        else if (weekArr[_i][_index].Equals("B+"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 13;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 13;   //� �� �հ�
        }
        else if (weekArr[_i][_index].Equals("B"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 11;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 11;   //� �� �հ�
        }
        else if (weekArr[_i][_index].Equals("B-"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 9;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 9;   //� �� �հ�
        }
        else if (weekArr[_i][_index].Equals("C+"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 7;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 7;   //� �� �հ�
        }
        else if (weekArr[_i][_index].Equals("C"))
        {
            exerciseNumber += 1;
            exerciseClassSum += 5;

            allExerciseNumber += 1; //� �� Ƚ��
            allExerciseSum += 5;    //� �� �հ�
        }
        Debug.Log("allExerciseSum " + allExerciseSum + "    "  + allExerciseNumber);
    }

    //�� � ��հ��� ���� ���
    string ExerciseClassAverage(float _average)
    {
        if (_average >= 16f) //A
        {
            classStr = "A";
        }
        else if (_average < 16f && _average >= 14f)  //A-
        {
            classStr = "A-";
            Debug.Log("����ƴѰ�? ");
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

    //�� ������ ȭ�� �����ֱ�
    public void WeekStampShow(Text _weekText, Button[] _stampBtn, Sprite[] _btnSprite)
    {
        int week = 0;
        if (allWeekEventArr.Length % 3 != 0)
            week = (allWeekEventArr.Length / 3) + 1;
        else if (allWeekEventArr.Length % 3 == 0)
            week = allWeekEventArr.Length / 3;

        PlayerPrefs.SetString("EP_LastWeek", week.ToString());  //������ ��� �� ����
        _weekText.text = week + "����";

        //�ֿ� �ش��ϴ� ����Ʈ �����ֱ�
        UserWeekReportInit(week);

        Debug.Log("allWeekEventArr.Length " + allWeekEventArr.Length);

        int remainder = 0;
        //�������� ���ؼ� ��� ����� ���ƴ��� Ȯ��(� 3���� ����� �������� 0�̸� 3���� ��� �� �޴ٴ� ��)
        if (allWeekEventArr.Length % 3 == 0)
            remainder = 3;  //� 3������ ������
        else if (allWeekEventArr.Length % 3 == 1)
            remainder = 1;
        else if (allWeekEventArr.Length % 3 == 2)
            remainder = 2;

        int count = 0;

        //������ ��� ���� ���� Ƚ��
        PlayerPrefs.SetInt("EP_LastWeekStartPoint", allWeekEventArr.Length - remainder);
        //�ֿ� �ش��ϴ� ���۹�ȣ���� �����ؾ��ϱ� ������ ��ü ���̿��� �������� ���� ���� �� ���������� ����..
        for (int i = allWeekEventArr.Length - remainder; i < allWeekEventArr.Length; i++)
        {
            for(int j = 0; j < weekArr[i].Length; j++)
            {
                Debug.Log(j + "  " + weekArr[i][j]);
                saveClassStr[count] = weekArr[i][j];    //���µ� �������� ��� ����
                count += 1;
            }
        }
        PlayerPrefs.SetInt("EP_LastExerciseNumber", count % 3); //������ ��� ���� ���ڷ� ����(0:�ٷ���, 1:����, 2:�ٷ»�)
        Debug.Log("count " + count);

        //���µ� ������(��ư) Ȱ��ȭ �����ֱ�
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

    //���� �� ��ư Ȱ��ȭ ��Ȱ��ȭ
    public void WeekLeftRightButtonState(Button _leftBtn, Button _rightBtn)
    {
        if(PlayerPrefs.GetString("EP_LastWeek").Equals("1"))
        {
            _leftBtn.interactable = false;  //�� Ŭ��
            _rightBtn.interactable = false; //�� Ŭ��
        }
        else
        {
            _leftBtn.interactable = true;   //Ŭ��
            _rightBtn.interactable = false; //�� Ŭ��
        }
    }

    //�ֿ� �ش��ϴ� ����Ʈ ������
    void UserWeekReportInit(int _week)
    {
        PlayerPrefs.SetString("EP_WeekReport", ""); //�ֿ� �ش��ϴ� ����Ʈ ����
        data = CSVReader.Read("ExerciseReportClass");

        string reportStr = "";

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["�̸�"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
                if(data[i]["����"].ToString().Equals(_week.ToString()))
                {
                    if (reportStr.Equals(""))
                    {
                        reportStr = data[i]["Ƚ��"].ToString() + "ȸ�� :: " + data[i]["����Ʈ����"].ToString();
                        //PlayerPrefs.SetString("EP_WeekReport",
                        //    data[i]["Ƚ��"].ToString() +"ȸ�� :: " + data[i]["����Ʈ����"].ToString());
                    }
                    else
                    {
                        reportStr = reportStr + "\n" + data[i]["Ƚ��"].ToString() + "ȸ�� :: " + data[i]["����Ʈ����"].ToString();
                        //PlayerPrefs.SetString("EP_WeekReport", PlayerPrefs.GetString("EP_WeekReport") + "\n" +
                        //    data[i]["Ƚ��"].ToString() + "ȸ�� :: " + data[i]["����Ʈ����"].ToString());
                    }
                }
            }
        }

        reportText.text = reportStr;
    }

    //���ʹ�ư
    public void WeekLeftButtonClick(Text _weekText, int _clickCount, Button _leftBtn, Button _rightBtn, Button[] _stampBtn)
    {
        int week = int.Parse(PlayerPrefs.GetString("EP_LastWeek")) - _clickCount;
        _weekText.text = week + "����";

        //�ֿ� �ش��ϴ� ����Ʈ ����
        UserWeekReportInit(week);


        if (_weekText.text.Equals("1����"))
        {
            //1�������� �� �������� ���� ������ ���ʹ�ư ��Ȱ��ȭ
            _leftBtn.interactable = false;
            _rightBtn.interactable = true;
        }
        else
        {
            _leftBtn.interactable = true;
            _rightBtn.interactable = true;
        }

        //���� ���� ��� : 3���� 7ȸ�̸� , EP_LastWeekSTartPoint�� 6���� ������ �Ǿ��������̴�. �迭�̶� -1����
        //���ʹ�ư�� Ŭ�� �߱⶧���� 2���� �������� ���������. 2���� 4ȸ~6ȸ. �� 3�� ���;��ϹǷ�
        //6 -(���ʹ�ư Ŭ��Ƚ��(1) * 3) = 3�� ����.
        //1�������� 6 - (�ަU��ư Ŭ��Ƚ��(2) *3) = 0 (1���� 1ȸ~3ȸ.��0~2)
        int scope = PlayerPrefs.GetInt("EP_LastWeekStartPoint") - (_clickCount * 3);
        int count = 0;
        for (int i = 0; i < 9; i++)
            saveClassStr[i] = "";

        for (int i = scope; i < scope + 3; i++)
        {
            for (int j = 0; j < weekArr[i].Length; j++)
            {
                Debug.Log(j + "  " + weekArr[i][j]);
                saveClassStr[count] = weekArr[i][j];    //���µ� �������� ��� ����
                count += 1;
            }
        }

        for(int i = 0; i < 9; i++)
            _stampBtn[i].gameObject.SetActive(false);

        //���µ� ������(��ư) Ȱ��ȭ �����ֱ�
        for (int i = 0; i < count; i++)
        {
            _stampBtn[i].gameObject.SetActive(true);
            _stampBtn[i].transform.GetChild(0).GetComponent<Text>().text = saveClassStr[i];
        }
    }

    //������ ��ư 
    public void WeekRightButtonClick(Text _weekText, int _clickCount, Button _leftBtn, Button _rightBtn, Button[] _stampBtn)
    {
        int week = int.Parse(PlayerPrefs.GetString("EP_LastWeek")) - _clickCount;
        _weekText.text = week + "����";

        //�ֿ� �ش��ϴ� ����Ʈ ����
        UserWeekReportInit(week);

        if (_weekText.text.Equals(PlayerPrefs.GetString("EP_LastWeek") +"����"))
        {
            //������ �������� �� ���������� ���� ������ �����ʹ�ư ��Ȱ��ȭ
            _rightBtn.interactable = false;
            _leftBtn.interactable = true;
        }
        else
        {
            _rightBtn.interactable = true;
            _leftBtn.interactable = true;
        }

        //���� ���� ��� : 3���� 7ȸ�̸� , EP_LastWeekSTartPoint�� 6���� ������ �Ǿ��������̴�. �迭�̶� -1����
        //���ʹ�ư�� Ŭ�� �߱⶧���� 2���� �������� ���������. 2���� 4ȸ~6ȸ. �� 3�� ���;��ϹǷ�
        //6 -(���ʹ�ư Ŭ��Ƚ��(1) * 3) = 3�� ����.
        //1�������� 6 - (�ަU��ư Ŭ��Ƚ��(2) *3) = 0 (1���� 1ȸ~3ȸ.��0~2)
        int scope = PlayerPrefs.GetInt("EP_LastWeekStartPoint") - (_clickCount * 3);
        int count = 0;
        int lastScope;  //for�� �߰��� ���� ����

        for (int i = 0; i < 9; i++)
            saveClassStr[i] = "";
        
        //������ ������ ��� for�� �߰� ������ �� Ƚ���� ���̸� �־��ְ�
        if (_weekText.text.Equals(PlayerPrefs.GetString("EP_LastWeek") + "����"))
            lastScope = allWeekEventArr.Length;
        else
            lastScope = scope + 3;  //�ƴҰ�� �������� 3�����ش�.

        for (int i = scope; i < lastScope; i++)
        {
            for (int j = 0; j < weekArr[i].Length; j++)
            {
                Debug.Log(j + "  " + weekArr[i][j]);
                saveClassStr[count] = weekArr[i][j];    //���µ� �������� ��� ����
                count += 1;
            }
        }

        for (int i = 0; i < 9; i++)
            _stampBtn[i].gameObject.SetActive(false);

        //���µ� ������(��ư) Ȱ��ȭ �����ֱ�
        for (int i = 0; i < count; i++)
        {
            _stampBtn[i].gameObject.SetActive(true);
            _stampBtn[i].transform.GetChild(0).GetComponent<Text>().text = saveClassStr[i];
        }
    }



    
    //������ Ŭ������ �� ��¥ ������ �ϴ� �Լ���
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
