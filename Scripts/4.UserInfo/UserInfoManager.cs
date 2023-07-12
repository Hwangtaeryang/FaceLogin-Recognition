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


    public GameObject virtualCanvas;    //가상키보드캔버스
    public VirtualTextInputBox TextInputBox = null; //가상키보드 인풋필드
    public InputField virtualField; //가상키보드 인풋필드
    public InputField nameInputField;   //사용자이름필드
    public InputField brithdayField;    //사용자생년월드필드
    public InputField heightField;  //사용자키필드
    public InputField weightField;  //사용자몸무게필드
    public ToggleGroup sexToggleGroup; //성별토글그룹

    public InputField othersField;  //기타필드
    public Toggle[] diseaseToggle;  //질병토글
    

    string btnName;
    bool nextBtn;   //[다음으로]버튼 클릭 여부

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
        DiseaseCheck(); //질병체크
    }

    void DiseaseCheck()
    {
        //고혈압 체크
        if (diseaseToggle[0].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_1", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_1", "No");

        //저혈압 체크
        if (diseaseToggle[1].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_2", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_2", "No");

        //당뇨 체크
        if (diseaseToggle[2].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_3", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_3", "No");

        //치매 체크
        if (diseaseToggle[3].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_4", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_4", "No");

        //관절염 체크
        if (diseaseToggle[4].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_5", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_5", "No");

        //협심증 체크
        if (diseaseToggle[5].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_6", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_6", "No");

        //뇌졸증 체크
        if (diseaseToggle[6].isOn.Equals(true))
            PlayerPrefs.SetString("EP_UserDisease_7", "Yes");
        else
            PlayerPrefs.SetString("EP_UserDisease_7", "No");

        //기타 체크 - 예스면 기타 질병 가상텍스트 입력에서 저장
        if (diseaseToggle[7].isOn.Equals(false))
            PlayerPrefs.SetString("EP_OthersDisease", "No");
    }

    public void SexAllToggleClick()
    {
        if(sexCurrentSeletion.name.Equals("ManToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "남자");
        }
        else if(sexCurrentSeletion.name.Equals("WomanToggle"))
        {
            PlayerPrefs.SetString("EP_UserSex", "여자");
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

        for (int i = 0; i < data.Count + 1; ++i)
        {
            Debug.Log("-----" + (data.Count - 1));
            if (i <= data.Count - 1)
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
        if(nextBtn.Equals(false))
        {
            if (btnName.Equals("NameButton"))
            {
                nameInputField.text = virtualField.text;
                PlayerPrefs.SetString("EP_UserNAME", nameInputField.text);   //유저이름
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
        }
        else
        {
            if (btnName.Equals("OthersButton"))
            {
                othersField.text = virtualField.text;
                PlayerPrefs.SetString("EP_OthersDisease", othersField.text);  //기타
            }
        }

        TextInputBox.Clear();
        virtualCanvas.SetActive(false);
    }

}
