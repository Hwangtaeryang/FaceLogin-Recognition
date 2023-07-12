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

    public Image myImage;   //얼굴이미지
    public InputField nameField;   //이름텍스트
    public InputField brithdayField;   //생일텍스트
    public ToggleGroup sexToggleGroup; //성별토글그룹
    public Toggle[] sexToggle;    //성별텍스트
    public InputField heightField;  //키텍스트
    public InputField weightField;  //몸무게텍스트
    public Toggle[] diseaseToggle;  //병력토글
    public InputField othersField; //기타텍스트

    public GameObject virtualCanvas;    //가상키보드캔버스
    public VirtualTextInputBox TextInputBox = null; //가상키보드 인풋필드
    public InputField virtualField; //가상키보드 인풋필드
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

    //해당 유저 정보를 들고온다.
    void UserDataInit()
    {
        data = CSVReader.Read("Student Data");

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["이름"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
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
            }
        }

        if (PlayerPrefs.GetString("EP_MyFaceChange").Equals("No"))
            myImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));
        else
        {
            string path = "Assets/Resources/Snapshots/" + PlayerPrefs.GetString("EP_UserNAME") + ".png";
            AssetDatabase.DeleteAsset(path);    //기존의 사진 삭제
            AssetDatabase.Refresh();    //F5
            string path2 = "Assets/Resources/Snapshots/SnapShot_User.png";// AssetDatabase.GetAssetPath(Selection.activeObject);
            TextureImporter tImporter = AssetImporter.GetAtPath(path2) as TextureImporter;
            tImporter.textureType = TextureImporterType.Sprite; //타입변경
            tImporter.isReadable = true;    //읽기버전으로 변경
            AssetDatabase.ImportAsset(path2);
            AssetDatabase.Refresh();
            AssetDatabase.RenameAsset("Assets/Resources/Snapshots/SnapShot_User.png", PlayerPrefs.GetString("EP_UserNAME"));
            AssetDatabase.Refresh();

            myImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));

            PlayerPrefs.SetString("EP_MyFaceChange", "No"); //사진찍고 왔기때문에 다시 No로 변경
        }

        nameField.text = PlayerPrefs.GetString("EP_UserNAME");
        brithdayField.text = PlayerPrefs.GetString("EP_UserBrithDay");
        heightField.text = PlayerPrefs.GetString("EP_UserHeight");
        weightField.text = PlayerPrefs.GetString("EP_UserWight");

        if (PlayerPrefs.GetString("EP_UserSex").Equals("남자"))
            sexToggle[0].isOn = true;
        else if (PlayerPrefs.GetString("EP_UserSex").Equals("여자"))
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

    //사진변경버튼 클릭 시 이벤트
    public void MyFaceImageChangeClickOn()
    {
        PlayerPrefs.SetString("EP_MyFaceChange", "Yes"); //사진찍기
        SceneManager.LoadScene("3.FaceShoot");
    }


    /// 성별 선택
    public void SexAllToggleClick()
    {
        if (sexCurrentSeletion.name.Equals("ManToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "남자");
        }
        else if (sexCurrentSeletion.name.Equals("WomanToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "여자");
        }
    }

    ///각각의 인풋필드 선택 시 클릭 이벤트 함수
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

    //가상키보드를 사용해 입력한 텍스트를 정보란에 입력하는 함수
    public void UserInfoTextShow()
    {
        if (btnName.Equals("NameButton"))
        {
            nameField.text = virtualField.text;
            PlayerPrefs.SetString("EP_UserNAME", nameField.text);   //유저이름
        }
        else if (btnName.Equals("BrithDayButton"))
        {
            brithdayField.text = virtualField.text;
            PlayerPrefs.SetString("EP_UserBrithDay", brithdayField.text);   //유저생년월일
        }
        else if (btnName.Equals("HeightButton"))
        {
            heightField.text = virtualField.text;
            PlayerPrefs.SetString("EP_UserHeight", heightField.text); //유저 키
        }
        else if (btnName.Equals("WeightButton"))
        {
            weightField.text = virtualField.text;
            PlayerPrefs.SetString("EP_UserWight", weightField.text);  //유저 몸무게
        }
        else if (btnName.Equals("OthersButton"))
        {
            othersField.text = virtualField.text;
            PlayerPrefs.SetString("EP_OthersDisease", othersField.text);  //기타
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
        //저혈압 체크
        if (diseaseToggle[1].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_2", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_2", "No");
    }
    public void DiseaseCheck3()
    {
        //당뇨 체크
        if (diseaseToggle[2].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_3", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_3", "No");
    }
    public void DiseaseCheck4()
    {
        //치매 체크
        if (diseaseToggle[3].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_4", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_4", "No");
    }
    public void DiseaseCheck5()
    {
        //관절염 체크
        if (diseaseToggle[4].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_5", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_5", "No");
    }
    public void DiseaseCheck6()
    {
        //협심증 체크
        if (diseaseToggle[5].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_6", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_6", "No");
    }
    public void DiseaseCheck7()
    {
        //뇌졸증 체크
        if (diseaseToggle[6].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_7", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_7", "No");
    }
    public void DiseaseCheck8()
    {
        //기타 체크 - 예스면 기타 질병 가상텍스트 입력에서 저장
        if (diseaseToggle[7].isOn.Equals(true))
            PlayerPrefs.SetString("EP_OthersDisease", "Yes");
        else
            PlayerPrefs.SetString("EP_OthersDisease", "No");
    }



    //수정하기 버튼 클릭 이벤트
    public void UserProfileCorrectSave()
    {
        data = CSVReader.Read("Student Data");

        Debug.Log("안들어오니 " + data.Count);
        string filePath = getPath();

        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter(filePath);
        Debug.Log("뭐닝");
        //This is writing the line of the type, name, damage... etc... (I set these)
        writer.WriteLine("이름,생년월일,성별,키,몸무게,고혈압,저혈압,당뇨,치매,관절염,협심증,뇌졸증,기타,시작일,마지막일,진도," +
            "전체등급,율동등급,근력상체등급,근력하체등급,1주,2주,3주,4주");
        //This loops through everything in the inventory and sets the file to these.
        Debug.Log("data.Count: " + data.Count);

        for (int i = 0; i < data.Count; ++i)
        {
            if(data[i]["이름"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
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
                writer.WriteLine(data[i]["이름"] +
                "," + data[i]["생년월일"] +
                "," + data[i]["성별"] +
                "," + data[i]["키"] +
                "," + data[i]["몸무게"] +
                "," + data[i]["고혈압"] +
                "," + data[i]["저혈압"] +
                "," + data[i]["당뇨"] +
                "," + data[i]["치매"] +
                "," + data[i]["관절염"] +
                "," + data[i]["협심증"] +
                "," + data[i]["뇌졸증"] +
                "," + data[i]["기타"] +
                "," + data[i]["시작일"] +
                "," + data[i]["마지막일"] +
                "," + data[i]["진도"] +
                "," + data[i]["전체등급"] +
                "," + data[i]["율동등급"] +
                "," + data[i]["근력상체등급"] +
                "," + data[i]["근력하체등급"] +
                "," + data[i]["1주"] +
                "," + data[i]["2주"] +
                "," + data[i]["3주"] +
                "," + data[i]["4주"]);
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
