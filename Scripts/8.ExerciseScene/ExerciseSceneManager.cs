using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseSceneManager : MonoBehaviour
{
    public static ExerciseSceneManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_AllWeekNumber"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���� �� �ǳڿ� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
    public void StartPanelTextInit(Text _upTilteText, Text _titleText, Text _week)
    {
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_LastWeek"));
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            _upTilteText.text = "�ٷ� � ��ü";
            _titleText.text = "�ٷ� � ��ü";
            _week.text = PlayerPrefs.GetString("EP_LastWeek") + "����";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            _upTilteText.text = "�ٷ� � ��ü";
            _titleText.text = "�ٷ� � ��ü";
            _week.text = PlayerPrefs.GetString("EP_LastWeek") + "����";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            _upTilteText.text = "���� �";
            _titleText.text = "���� �";
            _week.text = PlayerPrefs.GetString("EP_LastWeek")  +"����"; 
        }
    }
}
