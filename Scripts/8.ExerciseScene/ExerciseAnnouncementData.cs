using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseAnnouncementData : MonoBehaviour
{
    public static ExerciseAnnouncementData instacne { get; private set; }

    List<Dictionary<string, object>> data;

    int exerciseAllCount; //해당 운동 타입의 운동 갯수
    int exerciseListIndex;  //해당 운동 타입의 운동 마지막 인덱스

    public Text test;

    public void Awake()
    {
        if (instacne != null)
            Destroy(this);
        else instacne = this;
    }


    void Start()
    {
        //ExerciseName(1, "근력운동상체", 2, test);
    }


    //운동 이름 뽑아오는 함수
    //_ExerciseType -> 율동운동/ 근력운동상체 / 근력운동하체
    //율동운동 :  손목 털기 / 근력상 : 양팔 올리기/내리기 / 근력하 : 무릎 구부리기/펴기
    public void ExerciseName(int _week, string _ExerciseType, int _playIndex, Text _videoName)
    {
        exerciseAllCount = 0;
        data = CSVReader.Read("ExerciseData");

        //해당 주차가 어디에 속하는지 설정해주는 부분
        int week = 0;
        if (_week <= 4)
            week = 4;
        else if (_week > 4 && _week <= 8)
            week = 8;
        else if (_week > 8 && _week <= 12)
            week = 12;


        Debug.Log("data.Count " + data.Count);
        for(int i = 0; i < data.Count; i++)
        {
            if(int.Parse(data[i]["주차"].ToString()).Equals(week))
            {
                if(data[i]["운동종류"].ToString().Equals(_ExerciseType))
                {
                    exerciseAllCount += 1;  //동작 총 갯수
                    Debug.Log("i : " + i);
                    exerciseListIndex = i;  //마지막 인덱스 값 저장 (시작점을 알기 위해서)
                }
            }
        }
        //PlayerPrefs.SetInt("EP_ExerciseTypeAllNumber", exerciseAllCount); //해당 운동의 동작 총 갯수저장
        Debug.Log(_ExerciseType + " _playIndex "  + _playIndex);
        Debug.Log("운동 갯수 : " + exerciseAllCount + "   " + _ExerciseType);

        //해당 운동의 동작이 시작되는 인덱스값 설정
        //_playIndex는 동작의 번호, 율동운동 중에서 몇번째 하고 있는지
        int dataIndex = (exerciseListIndex - exerciseAllCount) + _playIndex; 

        //운동할 인덱스값이 마지막 인덱스값보다 하나가 작을떄까지 운동한다.
        if(dataIndex != exerciseListIndex - 1)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (int.Parse(data[i]["주차"].ToString()).Equals(week))
                {
                    if (data[i]["운동종류"].ToString().Equals(_ExerciseType))
                    {
                        Debug.Log("들어오나요?" + dataIndex + " _ExerciseType " + _ExerciseType);
                        Debug.Log(data[dataIndex]["내용"].ToString());
                        _videoName.text = data[dataIndex]["내용"].ToString();
                        VideoHandler.instance.LoadVideo(_ExerciseType, _week.ToString(), _playIndex);
                    }
                }
            }
        }
        else if(dataIndex.Equals(exerciseListIndex - 1))
        {
            //이게 마지막 운동이다! 라고 신호를 보내줘야함.
            ExerciseSceneUI_Manager.instance.lastExerciseState = true;

            for (int i = 0; i < data.Count; i++)
            {
                if (int.Parse(data[i]["주차"].ToString()).Equals(week))
                {
                    if (data[i]["운동종류"].ToString().Equals(_ExerciseType))
                    {
                        Debug.Log("들어오나요?" + dataIndex + " _ExerciseType " + _ExerciseType);
                        Debug.Log(data[dataIndex]["내용"].ToString());
                        _videoName.text = data[dataIndex]["내용"].ToString();
                        VideoHandler.instance.LoadVideo(_ExerciseType, _week.ToString(), _playIndex);
                    }
                }
            }
        }
    }
}
