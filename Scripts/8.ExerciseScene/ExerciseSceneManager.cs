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

    //시작 시 판넬에 있는 텍스트 정보 초기화
    public void StartPanelTextInit(Text _upTilteText, Text _titleText, Text _week)
    {
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_LastExerciseNumber"));
        Debug.Log(" ++   " + PlayerPrefs.GetInt("EP_LastWeek"));
        if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(1))
        {
            _upTilteText.text = "근력 운동 상체";
            _titleText.text = "근력 운동 상체";
            _week.text = PlayerPrefs.GetString("EP_LastWeek") + "주차";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(2))
        {
            _upTilteText.text = "근력 운동 하체";
            _titleText.text = "근력 운동 하체";
            _week.text = PlayerPrefs.GetString("EP_LastWeek") + "주차";
        }
        else if (PlayerPrefs.GetInt("EP_LastExerciseNumber").Equals(0))
        {
            _upTilteText.text = "율동 운동";
            _titleText.text = "율동 운동";
            _week.text = PlayerPrefs.GetString("EP_LastWeek")  +"주차"; 
        }
    }
}
