using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginResultManager : MonoBehaviour
{
    List<Dictionary<string, object>> data;

    public GameObject noUserPanel;
    public GameObject yesUserPanel;

    public Text nameText;   //이름텍스트
    public Text brithdayText;   //생년월일텍스트
    public Image userFaceImage; //회원이미지


    void Start()
    {
        data = CSVReader.Read("Student Data");

        //회원정보가 있을 경우
        if(PlayerPrefs.GetString("EP_UserNAME") != "")
        {
            yesUserPanel.SetActive(true);
            for (var i = 0; i < data.Count; i++)
            {
                //Debug.Log("index " + (i).ToString() + " : " + data[i]["이름"] + " " + data[i]["생년월일"] + " " + data[i]["성별"]);
                //string str = data[i]["이름"].ToString();
                //Debug.Log("str " + str);
                //동일한 이름이 있다면
                if (data[i]["이름"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
                {
                    PlayerPrefs.SetString("EP_UserBrithDay", data[i]["생년월일"].ToString());
                    PlayerPrefs.SetString("EP_UserSex", data[i]["성별"].ToString());

                    userFaceImage.sprite = Resources.Load<Sprite>("Snapshots/" + PlayerPrefs.GetString("EP_UserNAME"));
                    nameText.text = PlayerPrefs.GetString("EP_UserNAME");
                    brithdayText.text = PlayerPrefs.GetString("EP_UserBrithDay");
                }
            }
        }
        else
        {
            noUserPanel.SetActive(true);
        }

        Debug.Log("str??? " + PlayerPrefs.GetString("EP_UserNAME"));
    }

    
    void Update()
    {
        
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
