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

    public bool startMotionState;   //���� ���ۻ�������
    public MotionCustomAlarm motioncustom;  //���� üũ �Լ����� ��Ƴ��� ��ũ��Ʈ

    public float missTime_L;    //�߸� �����ϰ� �ִ� �ð� ����
    public float missTime_R;    //�߸� �����ϰ� �ִ� �ð� ������
    public float successTime_L;
    public float successTime_R;

    public GameObject narraListPref;    //����Ʈ�� ���� �����̼� ����
    public GameObject parentObj;    //�����̼Ǹ����� ���� ��ġ
    GameObject copyNarra; //�����̼� ���纻

    public Text videoTimeText;  //���� �÷��� �ð�

    public Sprite[] classSprite;    //��� �̹��� A,B,c
    string motionAvageClass;    //���� �ϳ� ������ �� ���ȭ�鿡 �������� ���

    string[] lastAllMotionClassArr; //��� ������ ������ �� ���� ��� ����
    string[] lastOnlyAllMotionClassArr; //��� ������ ������ �� ���� ��� ����
    string[] lastWeekCountArr;  //�������� ������ �����ϱ�����


    float allAverage;   //�� ������ ���

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    void Start()
    {
        //�� ���� ó�� �����ϸ� �ʱ�ȭ ��Ų��.
        //� ���� ��ü ����� �����ϱ� ���� ������.
        PlayerPrefs.SetString("EP_MotionAllClass", "");
    }

    
    void Update()
    {
        //� üũ�� ���۵Ǿ��ٸ�
        if (startMotionState.Equals(true))
        {
            missTime_L += Time.deltaTime;
            missTime_R += Time.deltaTime;
            successTime_L += Time.deltaTime;
            successTime_R += Time.deltaTime;
        }
    }

    //���� �ǳڿ� �ؽ�Ʈ �ʱ�ȭ���� �Է�
    public void StartPanelTextInit(Text _upExercisName, Text _exerciseName, Text _week, Text _number)
    {
        Debug.Log("EP_LastExerciseNumber  " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        //EP_LastExerciseNumber : 0 �̸� ���������� �� ��� �ٷ���ü. ��, �����ϴ� ��� �����
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            _exerciseName.text = "���� �";
            _upExercisName.text = "���� �";
        }
        //EP_LastExerciseNumber : 1 �̸� ���������� �� ��� �����. ��, �����ϴ� ��� �ٷ»�ü
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            _exerciseName.text = "�ٷ� � ��ü";
            _upExercisName.text = "�ٷ� � ��ü";
        }
        //EP_LastExerciseNumber : 2 �̸� ���������� �� ��� �ٷ»�ü. ��, �����ϴ� ��� �ٷ���ü
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            _exerciseName.text = "�ٷ� � ��ü";
            _upExercisName.text = "�ٷ� � ��ü";
        }

        //EP_AllWeekNumber : ��� Ƚ���� �ǹ��Ѵ�.
        if ((PlayerPrefs.GetInt("EP_AllWeekNumber") % 3).Equals(0))
        {
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            {
                //EP_AllWeekNumber : 6�� ���, 6 / 3 = 2 ... 0 => 2���� 3ȸ ��� �ߴ�.��� �ǹ�
                //������ ������ ��� ��ü��̿�����, 3ȸ�� ��������ü ����� �Ѱű� ������
                //������ 1 �����༭ ���� ������ �Ѿ���Ѵ�.
                _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1).ToString() + "����";
                PlayerPrefs.SetInt("EP_PlayingWeek", (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1));    //��� ����
                _number.text = "1";
                PlayerPrefs.SetInt("EP_PlayingNumber", 1);  //��� Ƚ��
            }
            else
            {
                //EP_AllWeekNumber : 6�� ���, 6 / 3 = 2 ... 0 => 2���� 3ȸ ��� �ߴ�.��� �ǹ�
                _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3).ToString() + "����";
                PlayerPrefs.SetInt("EP_PlayingWeek", PlayerPrefs.GetInt("EP_AllWeekNumber") / 3);    //��� ����
                _number.text = "3";
                PlayerPrefs.SetInt("EP_PlayingNumber", 3);  //��� Ƚ��
            }

        }
        else if ((PlayerPrefs.GetInt("EP_AllWeekNumber") % 3).Equals(1))
        {
            //EP_AllWeekNumber : 2�� ���, 2 / 3 = 0 ... 1 => 1���� 1ȸ ��� �ߴ�.��� �ǹ� +1�� ���༭ ������ ���ش�.
            _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1).ToString() + "����";
            PlayerPrefs.SetInt("EP_PlayingWeek", PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1);    //��� ����

            Debug.Log("������: " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            {
                _number.text = "2";
                PlayerPrefs.SetInt("EP_PlayingNumber", 2);  //��� Ƚ��
            }
            else
            {
                _number.text = "1";
                PlayerPrefs.SetInt("EP_PlayingNumber", 1);  //��� Ƚ��
            }
        }
        else if ((PlayerPrefs.GetInt("EP_AllWeekNumber") % 3).Equals(2))
        {
            //EP_AllWeekNumber : 5�� ���, 5 / 3 = 1 ... 2 => 2���� 2ȸ ��� �ߴ�.��� �ǹ� +1�� ���༭ ������ ���ش�.
            _week.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1).ToString() + "����";
            PlayerPrefs.SetInt("EP_PlayingWeek", PlayerPrefs.GetInt("EP_AllWeekNumber") / 3 + 1);    //��� ����

            if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            {
                _number.text = "3";
                PlayerPrefs.SetInt("EP_PlayingNumber", 3);  //��� Ƚ��
            }
            else
            {
                _number.text = "2";
                PlayerPrefs.SetInt("EP_PlayingNumber", 2);  //��� Ƚ��
            }
        }
        string str = Regex.Replace(_number.text, "����", string.Empty);
        PlayerPrefs.SetString("EP_WeekandCount", str + "=" + PlayerPrefs.GetInt("EP_PlayingNumber").ToString());   //���� �� Ƚ�� ���� (������ �����ϱ�����)
    }

    //���ȭ�鿡 �ִ� ������ ������Ʈ
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
            //�޽Ľð� ���� / �޽Ľð� ������ ������ ���� :: ������ �����ϱ� ���� ������ ���� �������
            PlayerPrefs.SetString("EP_MotionAllClass", motionAvageClass);
            PlayerPrefs.SetString("EP_OnlyMotionAllClass", motionAvageClass);
        }
        else
        {
           PlayerPrefs.SetString("EP_MotionAllClass", PlayerPrefs.GetString("EP_MotionAllClass") + "=" + motionAvageClass);
            PlayerPrefs.SetString("EP_OnlyMotionAllClass", PlayerPrefs.GetString("EP_OnlyMotionAllClass") + "=" + motionAvageClass);
        }
    }

    //���� ������ ���� ������ ��� ���ۿ� ���� ���ȭ�鿡 ������ ������Ʈ
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
        Debug.Log("1�� : " + allAverage + " �� " + ((one - ExerciseContentData.instacne.restCount) + two + three));

        _aText.text = one.ToString();
        _bText.text = two.ToString();
        _cText.text = three.ToString();

        return allAverage;
    }

    //���� ������ ���� ������ ��� ���ۿ� ���� ��� ����Ʈ ������ ������Ʈ
    public void LastResultPanelDataReportUpdata(Text _reportText)
    {
        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
            exerciseType = "�ٷ¿��ü";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
            exerciseType = "�ٷ¿��ü";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            exerciseType = "�����";

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
                " > ������ ���մϴ�. �ش� ������ �Ű�Ἥ ���ֽø� �� ���� ȿ���� �� �� �ֽ��ϴ�.";
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
                _reportText.text = "[" + exerciseType + "]" +" ��ü�� ���� ������ �����ϴ�. �ٸ� < " + PlayerPrefs.GetString("EP_CErrorClassGroup") +
                " > ������ ������ �ֽø� �� �� ���� ȿ���� �� �� �ֽ��ϴ�. ";
                PlayerPrefs.SetString("EP_CErrorClassGroup", _reportText.text);
            }
            else
            {
                _reportText.text = "[" + exerciseType + "]" + " ��ü������ ������ �����ϴ�. �̴�� ��� ��� �Ͻø� �˴ϴ�.";
                PlayerPrefs.SetString("EP_CErrorClassGroup", _reportText.text);
            }
        }
    }


    List<Dictionary<string, object>> data;
    //���� ������ ���� ������ ��� ���ۿ� ���� ��� ������ ����.
    //ExerciseReportClass.csv ���Ͽ� ���� // � ���ۿ� ���� ��� ����
    public void LastDataSaveUpdata()
    {
        //� �̸� 
        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
            exerciseType = "�ٷ¿��ü";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
            exerciseType = "�ٷ¿��ü";
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            exerciseType = "�����";

        //��յ�� 
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

        //���� �� Ƚ��
        if (PlayerPrefs.GetString("EP_WeekandCount") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_WeekandCount");

            char ch = '=';
            lastWeekCountArr = lastClassData.Split(ch); //����, Ƚ��
        }

        
        //������
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
        writer.WriteLine("�̸�,����,Ƚ��,����,���,����Ʈ����,����1,����2,����3,����4,����5,����6,����7," +
            "����8,����9,����10,����11,����12,����13,����14,����15,����16,����17,����18,����19,����20,����21,����22,����23,����24");


        Debug.Log("data.Count " + data.Count);
        for (int i = 0; i < data.Count + 1; ++i)
        {
            if(data.Count.Equals(0))
            {
                Debug.Log("--------?????");
                if (exerciseType.Equals("�����"))
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
                else if (exerciseType.Equals("�ٷ¿��ü"))
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
                else if (exerciseType.Equals("�ٷ¿��ü"))
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
                    writer.WriteLine(data[i]["�̸�"] +
                    "," + data[i]["����"] +
                    "," + data[i]["Ƚ��"] +
                    "," + data[i]["����"] +
                    "," + data[i]["���"] +
                    "," + data[i]["����Ʈ����"] +
                    "," + data[i]["����1"] +
                    "," + data[i]["����2"] +
                    "," + data[i]["����3"] +
                    "," + data[i]["����4"] +
                    "," + data[i]["����5"] +
                    "," + data[i]["����6"] +
                    "," + data[i]["����7"] +
                    "," + data[i]["����8"] +
                    "," + data[i]["����9"] +
                    "," + data[i]["����10"] +
                    "," + data[i]["����11"] +
                    "," + data[i]["����12"] +
                    "," + data[i]["����13"] +
                    "," + data[i]["����14"] +
                    "," + data[i]["����15"] +
                    "," + data[i]["����16"] +
                    "," + data[i]["����17"] +
                    "," + data[i]["����18"] + "," + data[i]["����19"] +
                    "," + data[i]["����20"] + "," + data[i]["����21"] +
                    "," + data[i]["����22"] + "," + data[i]["����23"] +
                    "," + data[i]["����24"]);
                }
                else if (i.Equals(data.Count))
                {
                    if (exerciseType.Equals("�����"))
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
                    else if (exerciseType.Equals("�ٷ¿��ü"))
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
                    else if (exerciseType.Equals("�ٷ¿��ü"))
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



    string allWeekStr;  //�� ��ü � ��� ���� ����
    string[] allWeekEventArr;   //�� �ֺ� ��� ¥�� �迭 ����(���� - ex:3�ֵ���)
    string[][] weekArr; //�ֺ� ��� ���� ex:weekarr[3][0] A,B,A ����Ǿ�����
    //�� ��ü ����� �ֺ��� ����
    public void AllClassDivide()
    {
        //�� ��ü ����� �ϳ��� ��ġ�� �۾�
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
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if (_index % 3 == 1)    //�ٷ¿��ü
            {
                if (weekArr[i].Length >= _index + 1)
                {
                    ExerciseClassScoreSum(i, _index);
                }
            }
            else if (_index % 3 == 2)    //�ٷ¿��ü
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
            //�� ��� ��հ�
            average = exerciseClassSum / exerciseNumber;
            Debug.Log("average " + exerciseClassSum / exerciseNumber + "   " + average);
        }
        

        //�� ���հ��� ���� ��� ��ȯ
        return ExerciseClassAverage(average);
    }
    string classStr;
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
    int exerciseNumber; //� Ƚ��
    int allExerciseSum; //�������
    int allExerciseNumber;  //��� Ƚ��
    int exerciseClassSum;   //��� ��� ���� �հ�
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
        else if (weekArr[_i][_index].Equals("A-"))
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
        Debug.Log("allExerciseSum " + allExerciseSum + "    " + allExerciseNumber);
    }
    //�� � ��� ���
    public string AllExerciseAverage()
    {
        float average = 0;
        if (allExerciseSum !=  0)
        {
            average = allExerciseSum / allExerciseNumber;
        }
        

        return ExerciseClassAverage(average);
    }

    //������ ������ ���ؼ� ����
    void WeekDataSave(string _exerciseType, string _week, string _averageStr)
    {
        if (_exerciseType.Equals("�����"))
        {
            Debug.Log("---------���� ��� " + _week+ "  " + PlayerPrefs.GetString(_week));
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
    //���� ������ ���� ������ ��� ���� ������ ��� ���� ���, ������ ���� ���� ������
    //Student Data.csv ���Ͽ� ���� // �̿��ڿ� ���� �⺻������ ��޿� ���� ����
    public void UserInfoLastSaveUpdata()
    {
        //��� ��� �� �Ϳ� ���� ��յ�� 
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

        //� �̸� 
        string exerciseType = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            exerciseType = "�ٷ¿��ü";
            //PlayerPrefs.SetString("EP_MuscularUpClass", PlayerPrefs.GetString("EP_MuscularUpClass") + averageStr);

        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            exerciseType = "�ٷ¿��ü";
            //PlayerPrefs.SetString("EP_MuscularDownClass", PlayerPrefs.GetString("EP_MuscularDownClass") + averageStr);
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            exerciseType = "�����";
            //PlayerPrefs.SetString("EP_DanceRoutineClass", PlayerPrefs.GetString("EP_DanceRoutineClass") + averageStr);
        }
            

        //���� �� Ƚ��
        if (PlayerPrefs.GetString("EP_WeekandCount") != "")
        {
            string lastClassData = PlayerPrefs.GetString("EP_WeekandCount");

            char ch = '=';
            lastWeekCountArr = lastClassData.Split(ch); //����, Ƚ��
        }

        int weekNum = 12 - (12 - int.Parse(lastWeekCountArr[0]));
       
        //������ ������ ���ؼ� ����
        if(weekNum.Equals(1))
            WeekDataSave(exerciseType, "EP_OneWeek", averageStr);
        else if(weekNum.Equals(2))
            WeekDataSave(exerciseType, "EP_TwoWeek", averageStr);
        else if(weekNum.Equals(3))
            WeekDataSave(exerciseType, "EP_ThreeWeek", averageStr);
        else if(weekNum.Equals(4))
            WeekDataSave(exerciseType, "EP_FourWeek", averageStr);


        //��ü ��� �ֺ��� ������ ����
        AllClassDivide();   





        data2 = CSVReader.Read("Student Data");


        string filePath = getPath();
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("�̸�,�������,����,Ű,������,������,������,�索,ġ��,������,������,������,��Ÿ,������,��������,����," +
            "��ü���,�������,�ٷ»�ü���,�ٷ���ü���,1��,2��,3��,4��");


        PlayerPrefs.SetString("EP_EndDay", DateTime.Now.ToString("yyyy-MM-dd"));
        PlayerPrefs.SetString("EP_Progress", exerciseType + "�Ϸ�");
        
        PlayerPrefs.SetString("EP_DanceRoutineClass", ExerciseClass(0));
        PlayerPrefs.SetString("EP_MuscularUpClass", ExerciseClass(1));
        PlayerPrefs.SetString("EP_MuscularDownClass", ExerciseClass(2));
        PlayerPrefs.SetString("EP_AllClass", AllExerciseAverage());

        Debug.Log("EP_EndDay : " + PlayerPrefs.GetString("EP_EndDay") + " EP_Progress " + PlayerPrefs.GetString("EP_Progress"));
        Debug.Log("EP_AllClass : " + PlayerPrefs.GetString("EP_AllClass") + " EP_DanceRoutineClass " + PlayerPrefs.GetString("EP_DanceRoutineClass"));
        Debug.Log("data.Count : " + data2.Count);
        for (int i = 0; i < data2.Count; ++i)
        {
            if (data2[i]["�̸�"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
                Debug.Log("������");
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
                writer.WriteLine(data2[i]["�̸�"] +
                "," + data2[i]["�������"] +
                "," + data2[i]["����"] +
                "," + data2[i]["Ű"] +
                "," + data2[i]["������"] +
                "," + data2[i]["������"] +
                "," + data2[i]["������"] +
                "," + data2[i]["�索"] +
                "," + data2[i]["ġ��"] +
                "," + data2[i]["������"] +
                "," + data2[i]["������"] +
                "," + data2[i]["������"] +
                "," + data2[i]["��Ÿ"] +
                "," + data2[i]["������"] +
                "," + data2[i]["��������"] +
                "," + data2[i]["����"] +
                "," + data2[i]["��ü���"] +
                "," + data2[i]["�������"] +
                "," + data2[i]["�ٷ»�ü���"] +
                "," + data2[i]["�ٷ���ü���"] +
                "," + data2[i]["1��"] +
                "," + data2[i]["2��"] +
                "," + data2[i]["3��"] +
                "," + data2[i]["4��"]);
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




    //���ȭ�鿡 �ִ� �����̼� ����Ʈ ������Ʈ
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

        //���� ����� ���� ���ؼ� �߸��� ���� �� ������ ���Ѵ�.
        //int allmotionCount = PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") +
        //    PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") +
        //    PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2");

        //if (allmotionCount >= 0 && allmotionCount <= 5)
        //    motionAvageClass = "A";
        //else if (allmotionCount > 5 && allmotionCount <= 10)
        //    motionAvageClass = "B";
        //else if(allmotionCount > 10)
        //    motionAvageClass = "C";

        //���� ����� ���� ���ؼ� ���� ���� ������ ���ؼ� ���Ѵ�.
        if (PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") >= 6)
            motionAvageClass = "A";
        else if (PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") < 6 && PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") >= 3)
            motionAvageClass = "B";
        else if (PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") < 3)
            motionAvageClass = "C";
    }

    //����Ʈ ��Ͽ� �ִ� �ڽĵ� �����ϴ� �Լ�
    public void NarrationListDelete()
    {
        //�ѹ��ϰ� ����Ʈ�� ��������ϱ� ������ �ڽĵ��� �������ش�.
        Transform[] child = parentObj.GetComponentsInChildren<Transform>();

        for (int i = 1; i < child.Length; i++)
            Destroy(child[i].gameObject);
    }



    //� üũ�ϴ� �Լ�
    public void ExerciseMotionCheck(float _angle, int _index, float _way)
    {
        string motionName = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
            motionName = "�����";
        //EP_LastExerciseNumber : 1 �̸� ���������� �� ��� �����. ��, �����ϴ� ��� �ٷ»�ü
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
            motionName = "�ٷ¿��ü";
        //EP_LastExerciseNumber : 2 �̸� ���������� �� ��� �ٷ»�ü. ��, �����ϴ� ��� �ٷ���ü
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
            motionName = "�ٷ¿��ü";

        //� ���� ���°� �Ǿ���. ExercisTimer.cs���� true/false���� ��
        if (startMotionState.Equals(true))
        {
            if(motionName.Equals("�����"))
            {
                Debug.Log(" +++++++++ " + PlayerPrefs.GetString("EP_PlayingExerciseName"));
                if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("�ո� �б�"))
                {
                    //Debug.Log("üũ����");
                    motioncustom.ShakeWristMove(_index, _angle);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("�� ������ ����"))
                {
                    motioncustom.StretchArmsForward(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("�ո� ����"))
                {
                    motioncustom.WristBreak(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("�ո� ������"))
                {
                    motioncustom.TurningWristMove(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("��� ������"))
                {
                    motioncustom.TurningShoulders(_index, _angle);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� ���� �ø���/������"))
                {
                    motioncustom.ArmUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� ������ �ø���/������"))
                {
                    motioncustom.ArmFrontUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� ������ �ø���/������"))
                {
                    motioncustom.ArmSideUpDown(_index, _angle, _way, videoTimeText);
                }
                else if(PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("������ ���� �ø���/������"))
                {
                    motioncustom.BothArmUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("������ ������ �ø���/������"))
                {
                    motioncustom.BothArmFrontUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("������ ������ �ø���/������"))
                {
                    motioncustom.BothArmSideUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("������ �� ������"))
                {
                    motioncustom.RightArmSpin(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� �� ������"))
                {
                    motioncustom.LeftArmSpin(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� ���� �ø���"))
                {
                    motioncustom.BothArmUp(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� ������ ������"))
                {
                    motioncustom.BothArmSideUp(_index, _angle, _way);
                }
            }
            else if(motionName.Equals("�ٷ¿��ü"))
            {
                if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� �ø���/������"))
                {
                    //motioncustom.BothArmsUpDown(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� �� ����� ������ �ø���/������"))
                {
                    //motioncustom.OneArmTakeTurnsUpDown(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� �� ����� ������ �ø���/������"))
                {
                    //motioncustom.OneArmTakeFrontBack(_index, _angle, _way);
                }
            }
            else if(motionName.Equals("�ٷ¿��ü"))
            {
                if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("���� ���θ���/���"))
                {
                    //motioncustom.KneeBendStretch(_index, _angle, _way);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("�ٸ� ���θ���/���"))
                {
                    //motioncustom.LegGendStretch(_index, _angle, _way, videoTimeText);
                }
                else if (PlayerPrefs.GetString("EP_PlayingExerciseName").Equals("�ٸ� ������/���Ǹ���"))
                {
                    //motioncustom.LegOpenClose(_index, _angle, _way);
                }
            }
        }
    }

}
