using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class UserInfoManager : MonoBehaviour
{
    List<Dictionary<string, object>> data;


    public GameObject virtualCanvas;    //����Ű����ĵ����
    public VirtualTextInputBox TextInputBox = null; //����Ű���� ��ǲ�ʵ�
    public InputField virtualField; //����Ű���� ��ǲ�ʵ�
    public InputField nameInputField;   //������̸��ʵ�
    public InputField brithdayField;    //����ڻ�������ʵ�
    public InputField heightField;  //�����Ű�ʵ�
    public InputField weightField;  //����ڸ������ʵ�
    public ToggleGroup sexToggleGroup; //������۱׷�

    public InputField othersField;  //��Ÿ�ʵ�
    public Toggle[] diseaseToggle;  //�������
    

    string btnName;
    bool nextBtn;   //[��������]��ư Ŭ�� ����

    public Toggle sexCurrentSeletion
    {
        get { return sexToggleGroup.ActiveToggles().FirstOrDefault(); }
    }


    void Start()
    {
        data = CSVReader.Read("Student Data");
    }

    
    void Update()
    {
        DiseaseCheck(); //����üũ
    }

    void DiseaseCheck()
    {
        //������ üũ
        if (diseaseToggle[0].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_1", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_1", "No");

        //������ üũ
        if (diseaseToggle[1].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_2", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_2", "No");

        //�索 üũ
        if (diseaseToggle[2].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_3", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_3", "No");

        //ġ�� üũ
        if (diseaseToggle[3].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_4", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_4", "No");

        //������ üũ
        if (diseaseToggle[4].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_5", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_5", "No");

        //������ üũ
        if (diseaseToggle[5].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_6", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_6", "No");

        //������ üũ
        if (diseaseToggle[6].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_7", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_7", "No");

        //��Ÿ üũ - ������ ��Ÿ ���� �����ؽ�Ʈ �Է¿��� ����
        if (diseaseToggle[7].isOn.Equals(false))
            PlayerPrefs.SetString("EP_OthersDisease", "No");
    }

    public void SexAllToggleClick()
    {
        if(sexCurrentSeletion.name.Equals("ManToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "����");
        }
        else if(sexCurrentSeletion.name.Equals("WomanToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "����");
        }
    }


    public void NextButtonOnClick()
    {
        nextBtn = true;
//#if UNITY_EIDTOR
        AssetDatabase.RenameAsset("Assets/Resources/Snapshots/SnapShot_User.png", PlayerPrefs.GetString("EP_UserNAME"));
       // AssetDatabase.RenameAsset("Assets/Resources/Snapshots", PlayerPrefs.GetString("EP_UserNAME"));
        AssetDatabase.Refresh();
//#endif
    }

    public void UserDataSaveOnClick()
    {
        //Debug.Log(PlayerPrefs.GetString("EP_UserNAME") + ", " +
        //    PlayerPrefs.GetString("EP_UserSex") + ", " +
        //    PlayerPrefs.GetString("EP_UserBrithDay") + ", " +
        //    PlayerPrefs.GetString("EP_UserWight") + ", " +
        //    PlayerPrefs.GetString("EP_UserDisease_1") + ", " +
        //    PlayerPrefs.GetString("EP_UserDisease_7") + ", " +
        //    PlayerPrefs.GetString("EP_OthersDisease") + ", ");

        SaveInventory();
        StartCoroutine(GoLoginScene());
    }

    IEnumerator GoLoginScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("1.Login");
    }

    public void SaveInventory()
    {
        data = CSVReader.Read("Student Data");

        Debug.Log("�ȵ����� " + data.Count);
        string filePath = getPath();

        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter(filePath);
        Debug.Log("����");
        //This is writing the line of the type, name, damage... etc... (I set these)
        writer.WriteLine("�̸�,�������,����,Ű,������,������,������,�索,ġ��,������,������,������,��Ÿ,������,��������,����," +
            "��ü���,�������,�ٷ»�ü���,�ٷ���ü���,1��,2��,3��,4��");
        //This loops through everything in the inventory and sets the file to these.
        Debug.Log("data.Count: " + data.Count);

        for (int i = 0; i < data.Count + 1; ++i)
        {
            Debug.Log("-----" + (data.Count - 1));
            if (i <= data.Count - 1)
            {
                writer.WriteLine(data[i]["�̸�"] +
                "," + data[i]["�������"] +
                "," + data[i]["����"] +
                "," + data[i]["Ű"] +
                "," + data[i]["������"] +
                "," + data[i]["������"] +
                "," + data[i]["������"] +
                "," + data[i]["�索"] +
                "," + data[i]["ġ��"] +
                "," + data[i]["������"] +
                "," + data[i]["������"] +
                "," + data[i]["������"] +
                "," + data[i]["��Ÿ"] +
                "," + data[i]["������"] +
                "," + data[i]["��������"] +
                "," + data[i]["����"] +
                "," + data[i]["��ü���"] +
                "," + data[i]["�������"] +
                "," + data[i]["�ٷ»�ü���"] +
                "," + data[i]["�ٷ���ü���"] +
                "," + data[i]["1��"] +
                "," + data[i]["2��"] +
                "," + data[i]["3��"] +
                "," + data[i]["4��"]);
            }
            else if (i.Equals(data.Count))
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                PlayerPrefs.SetString("EP_StartDay", today);
                PlayerPrefs.SetString("EP_EndDay", today);
                PlayerPrefs.SetString("EP_Progress", "No");

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
        }
        writer.Flush();
        //This closes the file
        writer.Close();

        AssetDatabase.Refresh();
        data = CSVReader.Read("Student Data");
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

    ///������ ��ǲ�ʵ� ���� �� Ŭ�� �̺�Ʈ �Լ�
    public void NameButtonOnClick()
    {
        TextInputBox.Clear();
        btnName = "NameButton";
    }

    public void BrithDayButtonOnClick()
    {
        TextInputBox.Clear();
        btnName = "BrithDayButton";
    }


    public void HeightButtonOnClick()
    {
        TextInputBox.Clear();
        btnName = "HeightButton";
    }

    public void WeightButtonOnClick()
    {
        TextInputBox.Clear();
        btnName = "WeightButton";
    }

    public void OthersButtonOnClick()
    {
        TextInputBox.Clear();
        btnName = "OthersButton";
    }


    //����Ű���带 ����� �Է��� �ؽ�Ʈ�� �������� �Է��ϴ� �Լ�
    public void UserInfoTextShow()
    {
        if(nextBtn.Equals(false))
        {
            if (btnName.Equals("NameButton"))
            {
                nameInputField.text = virtualField.text;
                PlayerPrefs.SetString("EP_UserNAME", nameInputField.text);   //�����̸�
            }
            else if (btnName.Equals("BrithDayButton"))
            {
                brithdayField.text = virtualField.text;
                PlayerPrefs.SetString("EP_UserBrithDay", brithdayField.text);   //�����������
            }
            else if (btnName.Equals("HeightButton"))
            {
                heightField.text = virtualField.text;
                PlayerPrefs.SetString("EP_UserHeight", heightField.text); //���� Ű
            }
            else if (btnName.Equals("WeightButton"))
            {
                weightField.text = virtualField.text;
                PlayerPrefs.SetString("EP_UserWight", weightField.text);  //���� ������
            }
        }
        else
        {
            if (btnName.Equals("OthersButton"))
            {
                othersField.text = virtualField.text;
                PlayerPrefs.SetString("EP_OthersDisease", othersField.text);  //��Ÿ
            }
        }

        TextInputBox.Clear();
        virtualCanvas.SetActive(false);
    }

}
