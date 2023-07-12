using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserProfileCorrectManager : MonoBehaviour
{
    List<Dictionary<string, object>> data;

    public Image myImage;   //���̹���
    public InputField nameField;   //�̸��ؽ�Ʈ
    public InputField brithdayField;   //�����ؽ�Ʈ
    public ToggleGroup sexToggleGroup; //������۱׷�
    public Toggle[] sexToggle;    //�����ؽ�Ʈ
    public InputField heightField;  //Ű�ؽ�Ʈ
    public InputField weightField;  //�������ؽ�Ʈ
    public Toggle[] diseaseToggle;  //�������
    public InputField othersField; //��Ÿ�ؽ�Ʈ

    public GameObject virtualCanvas;    //����Ű����ĵ����
    public VirtualTextInputBox TextInputBox = null; //����Ű���� ��ǲ�ʵ�
    public InputField virtualField; //����Ű���� ��ǲ�ʵ�
    string btnName;



    public Toggle sexCurrentSeletion
    {
        get { return sexToggleGroup.ActiveToggles().FirstOrDefault(); }
    }

    void Start()
    {
        UserDataInit();
    }

    public void BackButtonClick()
    {
        SceneManager.LoadScene("5.UserExercise");
    }

    //�ش� ���� ������ ���´�.
    void UserDataInit()
    {
        data = CSVReader.Read("Student Data");

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["�̸�"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
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
            }
        }

        if (PlayerPrefs.GetString("EP_MyFaceChange").Equals("No"))
            myImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));
        else
        {
            string path = "Assets/Resources/Snapshots/" + PlayerPrefs.GetString("EP_UserNAME") + ".png";
            AssetDatabase.DeleteAsset(path);    //������ ���� ����
            AssetDatabase.Refresh();    //F5
            string path2 = "Assets/Resources/Snapshots/SnapShot_User.png";// AssetDatabase.GetAssetPath(Selection.activeObject);
            TextureImporter tImporter = AssetImporter.GetAtPath(path2) as TextureImporter;
            tImporter.textureType = TextureImporterType.Sprite; //Ÿ�Ժ���
            tImporter.isReadable = true;    //�б�������� ����
            AssetDatabase.ImportAsset(path2);
            AssetDatabase.Refresh();
            AssetDatabase.RenameAsset("Assets/Resources/Snapshots/SnapShot_User.png", PlayerPrefs.GetString("EP_UserNAME"));
            AssetDatabase.Refresh();

            myImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));

            PlayerPrefs.SetString("EP_MyFaceChange", "No"); //������� �Ա⶧���� �ٽ� No�� ����
        }

        nameField.text = PlayerPrefs.GetString("EP_UserNAME");
        brithdayField.text = PlayerPrefs.GetString("EP_UserBrithDay");
        heightField.text = PlayerPrefs.GetString("EP_UserHeight");
        weightField.text = PlayerPrefs.GetString("EP_UserWight");

        if (PlayerPrefs.GetString("EP_UserSex").Equals("����"))
            sexToggle[0].isOn = true;
        else if (PlayerPrefs.GetString("EP_UserSex").Equals("����"))
            sexToggle[1].isOn = true;

        if (PlayerPrefs.GetString("EP_UserDisease_1").Equals("Yes"))
            diseaseToggle[0].isOn = true;
        if (PlayerPrefs.GetString("EP_UserDisease_2").Equals("Yes"))
            diseaseToggle[1].isOn = true;
        if (PlayerPrefs.GetString("EP_UserDisease_3").Equals("Yes"))
            diseaseToggle[2].isOn = true;
        if (PlayerPrefs.GetString("EP_UserDisease_4").Equals("Yes"))
            diseaseToggle[3].isOn = true;
        if (PlayerPrefs.GetString("EP_UserDisease_5").Equals("Yes"))
            diseaseToggle[4].isOn = true;
        if (PlayerPrefs.GetString("EP_UserDisease_6").Equals("Yes"))
            diseaseToggle[5].isOn = true;
        if (PlayerPrefs.GetString("EP_UserDisease_7").Equals("Yes"))
            diseaseToggle[6].isOn = true;
        if (PlayerPrefs.GetString("EP_OthersDisease") != "No")
        {
            diseaseToggle[7].isOn = true;
            othersField.text = PlayerPrefs.GetString("EP_OthersDisease");
        }
            
    }

    //���������ư Ŭ�� �� �̺�Ʈ
    public void MyFaceImageChangeClickOn()
    {
        PlayerPrefs.SetString("EP_MyFaceChange", "Yes"); //�������
        SceneManager.LoadScene("3.FaceShoot");
    }


    /// ���� ����
    public void SexAllToggleClick()
    {
        if (sexCurrentSeletion.name.Equals("ManToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "����");
        }
        else if (sexCurrentSeletion.name.Equals("WomanToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "����");
        }
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
        if (btnName.Equals("NameButton"))
        {
            nameField.text = virtualField.text;
            PlayerPrefs.SetString("EP_UserNAME", nameField.text);   //�����̸�
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
        else if (btnName.Equals("OthersButton"))
        {
            othersField.text = virtualField.text;
            PlayerPrefs.SetString("EP_OthersDisease", othersField.text);  //��Ÿ
        }

        TextInputBox.Clear();
        virtualCanvas.SetActive(false);
    }


    public void DiseaseCheck1()
    {
        if (diseaseToggle[0].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_1", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_1", "No");
    }
    public void DiseaseCheck2()
    {
        //������ üũ
        if (diseaseToggle[1].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_2", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_2", "No");
    }
    public void DiseaseCheck3()
    {
        //�索 üũ
        if (diseaseToggle[2].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_3", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_3", "No");
    }
    public void DiseaseCheck4()
    {
        //ġ�� üũ
        if (diseaseToggle[3].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_4", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_4", "No");
    }
    public void DiseaseCheck5()
    {
        //������ üũ
        if (diseaseToggle[4].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_5", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_5", "No");
    }
    public void DiseaseCheck6()
    {
        //������ üũ
        if (diseaseToggle[5].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_6", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_6", "No");
    }
    public void DiseaseCheck7()
    {
        //������ üũ
        if (diseaseToggle[6].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_7", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_7", "No");
    }
    public void DiseaseCheck8()
    {
        //��Ÿ üũ - ������ ��Ÿ ���� �����ؽ�Ʈ �Է¿��� ����
        if (diseaseToggle[7].isOn.Equals(true))
            PlayerPrefs.SetString("EP_OthersDisease", "Yes");
        else
            PlayerPrefs.SetString("EP_OthersDisease", "No");
    }



    //�����ϱ� ��ư Ŭ�� �̺�Ʈ
    public void UserProfileCorrectSave()
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

        for (int i = 0; i < data.Count; ++i)
        {
            if(data[i]["�̸�"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
            {
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
        }
        writer.Flush();
        //This closes the file
        writer.Close();

        AssetDatabase.Refresh();
        data = CSVReader.Read("Student Data");

        SceneManager.LoadScene("5.UserExercise");
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

}
