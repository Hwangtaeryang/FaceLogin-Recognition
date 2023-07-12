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

    public Text nameText;   //�̸��ؽ�Ʈ
    public Text brithdayText;   //��������ؽ�Ʈ
    public Image userFaceImage; //ȸ���̹���


    void Start()
    {
        data = CSVReader.Read("Student Data");

        //ȸ�������� ���� ���
        if(PlayerPrefs.GetString("EP_UserNAME") != "")
        {
            yesUserPanel.SetActive(true);
            for (var i = 0; i < data.Count; i++)
            {
                //Debug.Log("index " + (i).ToString() + " : " + data[i]["�̸�"] + " " + data[i]["�������"] + " " + data[i]["����"]);
                //string str = data[i]["�̸�"].ToString();
                //Debug.Log("str " + str);
                //������ �̸��� �ִٸ�
                if (data[i]["�̸�"].ToString().Equals(PlayerPrefs.GetString("EP_UserNAME")))
                {
                    PlayerPrefs.SetString("EP_UserBrithDay", data[i]["�������"].ToString());
                    PlayerPrefs.SetString("EP_UserSex", data[i]["����"].ToString());

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
