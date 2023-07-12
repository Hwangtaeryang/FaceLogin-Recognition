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
    

    public Image myImage;   //���̹���
    public Text nameText;   //�̸��ؽ�Ʈ
    public Text startText;  //��������ؽ�Ʈ
    public Text endText;    //��������ؽ�Ʈ
    public Text progressText;   //�����ؽ�Ʈ


    string allWeekStr;  //�� ��ü � ��� ���� ����
    string[] allWeekEventArr;   //�� �ֺ� ��� ¥�� �迭 ����(���� - ex:3�ֵ���)
    string[][] weekArr; //�ֺ� ��� ���� ex:weekarr[3][0] A,B,A ����Ǿ�����


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

    //�ش� ���� ������ ���´�.
    void UserDataInit()
    {
        data = CSVReader.Read("Student Data");

        for(int i = 0; i < data.Count; i++)
        {
            if(data[i]["�̸�"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
                PlayerPrefs.SetString("EP_UserNAME", data[i]["�̸�"].ToString());   //�����̸�
                PlayerPrefs.SetString("EP_UserSex", data[i]["����"].ToString());    //��������
                PlayerPrefs.SetString("EP_UserBrithDay", data[i]["�������"].ToString());   //�����������
                PlayerPrefs.SetString("EP_UserHeight", data[i]["Ű"].ToString()); //���� Ű
                PlayerPrefs.SetString("EP_UserWight", data[i]["������"].ToString());  //���� ������
                PlayerPrefs.SetString("EP_UserDisease_1", data[i]["������"].ToString());  //������
                PlayerPrefs.SetString("EP_UserDisease_2", data[i]["������"].ToString());  //������
                PlayerPrefs.SetString("EP_UserDisease_3", data[i]["�索"].ToString());  //�索
                PlayerPrefs.SetString("EP_UserDisease_4", data[i]["ġ��"].ToString());  //ġ��
                PlayerPrefs.SetString("EP_UserDisease_5", data[i]["������"].ToString());  //������
                PlayerPrefs.SetString("EP_UserDisease_6", data[i]["������"].ToString());  //������
                PlayerPrefs.SetString("EP_UserDisease_7", data[i]["������"].ToString());  //������
                PlayerPrefs.SetString("EP_OthersDisease", data[i]["��Ÿ"].ToString());  //������Ÿ
                PlayerPrefs.SetString("EP_StartDay", data[i]["������"].ToString());   //�������
                PlayerPrefs.SetString("EP_EndDay", data[i]["��������"].ToString());   //���������
                PlayerPrefs.SetString("EP_Progress", data[i]["����"].ToString());   //�����
                PlayerPrefs.SetString("EP_AllClass", data[i]["��ü���"].ToString());   //��ü���
                PlayerPrefs.SetString("EP_DanceRoutineClass", data[i]["�������"].ToString());   //�������
                PlayerPrefs.SetString("EP_MuscularUpClass", data[i]["�ٷ»�ü���"].ToString());   //�ٷ»�ü���
                PlayerPrefs.SetString("EP_MuscularDownClass", data[i]["�ٷ���ü���"].ToString());   //�ٷ���ü���
                PlayerPrefs.SetString("EP_OneWeek", data[i]["1��"].ToString());   //1��
                PlayerPrefs.SetString("EP_TwoWeek", data[i]["2��"].ToString());   //2��
                PlayerPrefs.SetString("EP_ThreeWeek", data[i]["3��"].ToString());   //3��
                PlayerPrefs.SetString("EP_FourWeek", data[i]["4��"].ToString());   //4��
            }
        }

        myImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));
        nameText.text = PlayerPrefs.GetString("EP_UserNAME");
        startText.text = PlayerPrefs.GetString("EP_StartDay");
        endText.text = PlayerPrefs.GetString("EP_EndDay");
        progressText.text = PlayerPrefs.GetString("EP_Progress");
    }

    

    //��������� ����
    public void UserProfileCorrectClickOn()
    {
        SceneManager.LoadScene("6.UserProfileCorrect");
    }

    //�����ƮȮ�� ��ư Ŭ��
    public void ExerciseReportClickOn()
    {
        SceneManager.LoadScene("7.ExerciseReport");
    }

    //����۹�ư Ŭ��
    public void ExerciseStartClickOn()
    {
        SceneManager.LoadScene("8.ExerciseScene");
    }



    //�������� ���� �ʱ�ȭ ����...
    public void MyProgressInit()
    {
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        string weekStr = "";
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            weekStr = "������Ϸ�";
            progressText.text = PlayerPrefs.GetInt("EP_AllWeekNumber") + "����-" + weekStr;
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            weekStr = "�ٷ¿��ü�Ϸ�";
            progressText.text = PlayerPrefs.GetInt("EP_AllWeekNumber") + "����-" + weekStr;
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            weekStr = "�ٷ¿��ü�Ϸ�";
            progressText.text = (PlayerPrefs.GetInt("EP_AllWeekNumber") + 1) + "����-" + weekStr;
        }
    }


    //�� ��ü ����� �ֺ��� ����
    public void AllClassDivide()
    {
        //�� ��ü ����� �ϳ��� ��ġ�� �۾�
        if (PlayerPrefs.GetString("EP_FourWeek") != "No")
        {
            Debug.Log("4��°");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek") + "/" + PlayerPrefs.GetString("EP_FourWeek");
        }
        else if (PlayerPrefs.GetString("EP_ThreeWeek") != "No")
        {
            Debug.Log("3��°");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek") + "/" +
                PlayerPrefs.GetString("EP_ThreeWeek");
        }
        else if (PlayerPrefs.GetString("EP_TwoWeek") != "No")
        {
            Debug.Log("2��°");
            allWeekStr = PlayerPrefs.GetString("EP_OneWeek") + "/" + PlayerPrefs.GetString("EP_TwoWeek");
        }
        else if (PlayerPrefs.GetString("EP_OneWeek") != "No")
        {
            Debug.Log("1��°");
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
            allWeekEventArr = classData.Split(ch); //A-A-B(1��), C-B-B(2��), A-C-B(3��) ���� ����


            weekArr = new string[allWeekEventArr.Length][];
            //Debug.Log(" allWeekEventArr " + allWeekEventArr.Length);
            PlayerPrefs.SetInt("EP_AllWeekNumber", allWeekEventArr.Length); //�� ȸ�� ����
            Debug.Log(" allWeekEventArr " + PlayerPrefs.GetInt("EP_AllWeekNumber"));

            for (int i = 0; i < allWeekEventArr.Length; i++)
            {
                string weekData = allWeekEventArr[i];   //A-A-B
                weekArr[i] = weekData.Split(weekCh);    //A,A,B ���� ����
            }


            int week = 0;
            if (allWeekEventArr.Length % 3 != 0)
                week = (allWeekEventArr.Length / 3) + 1;
            else if (allWeekEventArr.Length % 3 == 0)
                week = allWeekEventArr.Length / 3;

            //Debug.Log(" EP_LastWeek " + week);
            PlayerPrefs.SetString("EP_LastWeek", week.ToString());  //������ ��� �� ����
                                                                    //Debug.Log(" EP_LastWeek " + PlayerPrefs.GetInt("EP_LastWeek"));


            int remainder = 0;
            //�������� ���ؼ� ��� ����� ���ƴ��� Ȯ��(� 3���� ����� �������� 0�̸� 3���� ��� �� �޴ٴ� ��)
            if (allWeekEventArr.Length % 3 == 0)
                remainder = 3;  //� 3������ ������
            else if (allWeekEventArr.Length % 3 == 1)
                remainder = 1;
            else if (allWeekEventArr.Length % 3 == 2)
                remainder = 2;

            int count = 0;

            //�ֿ� �ش��ϴ� ���۹�ȣ���� �����ؾ��ϱ� ������ ��ü ���̿��� �������� ���� ���� �� ���������� ����..
            for (int i = allWeekEventArr.Length - remainder; i < allWeekEventArr.Length; i++)
            {
                for (int j = 0; j < weekArr[i].Length; j++)
                {
                    count += 1;
                }
            }
            PlayerPrefs.SetInt("EP_LastExerciseNumber", count % 3); //������ ��� ���� ���ڷ� ����(0:�ٷ���, 1:����, 2:�ٷ»�)
            Debug.Log(" EP_LastExerciseNumber " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        }
        else
        {
            PlayerPrefs.SetInt("EP_AllWeekNumber", 0); //�� ȸ�� ����
            PlayerPrefs.SetString("EP_LastWeek", "0");
            PlayerPrefs.SetInt("EP_LastExerciseNumber", 0);
        }
    }
}
