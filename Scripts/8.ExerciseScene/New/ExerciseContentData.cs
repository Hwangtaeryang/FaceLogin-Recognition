using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExerciseContentData : MonoBehaviour
{
    public static ExerciseContentData instacne { get; private set; }

    List<Dictionary<string, object>> data;


    int exerciseAllCount; //해당 운동 타입의 운동 갯수
    int exerciseListIndex;  //해당 운동 타입의 운동 마지막 인덱스
    public int restCount;   //쉬는시간 카운트


    public void Awake()
    {
        if (instacne != null)
            Destroy(this);
        else instacne = this;
    }

    public void ExerciseVideoNameShow(int _allWeek, string _ExerciseType, int _playIndex, Text _videoNameText, 
        GameObject _restPanel, GameObject _videoPanel, GameObject _myImage)
    {
        exerciseAllCount = 0;
        data = CSVReader.Read("ExerciseData");

        //해당 주차가 어디에 속하는지 설정해주는 부분
        int week = 0;
        if (_allWeek <= 4)
            week = 4;
        else if (_allWeek > 4 && _allWeek <= 8)
            week = 8;
        else if (_allWeek > 8 && _allWeek <= 12)
            week = 12;

        //해당운동의 동작 총 갯수와 마지막 인덱스가 몇번짼지 저장하기 위함
        for (int i = 0; i < data.Count; i++)
        {
            if (int.Parse(data[i]["주차"].ToString()).Equals(week))
            {
                if (data[i]["운동종류"].ToString().Equals(_ExerciseType))
                {
                    exerciseAllCount += 1;  //동작 총 갯수
                    //Debug.Log("i : " + i);
                    exerciseListIndex = i;  //마지막 인덱스 값 저장 (시작점을 알기 위해서)
                }
            }
        }
        Debug.Log("총갯수 : " + exerciseAllCount);
        //해당 운동의 동작이 시작되는 인덱스값 설정
        //_playIndex는 동작의 번호, 율동운동 중에서 몇번째 하고 있는지
        int dataIndex = (exerciseListIndex - exerciseAllCount) + _playIndex;

        //해당 동작이 마지막 동작인지 알려준다.
        if (dataIndex.Equals(exerciseListIndex))
            ExerciseScene_UIManager.instance.lastMotionState = true;

        for(int i = 0; i < data.Count; i++)
        {
            if(int.Parse(data[i]["주차"].ToString()).Equals(week))
            {
                if (data[i]["운동종류"].ToString().Equals(_ExerciseType))
                {
                    if(i.Equals(dataIndex))
                    {
                        _videoNameText.text = data[dataIndex]["내용"].ToString();
                        PlayerPrefs.SetString("EP_PlayingExerciseName", data[dataIndex]["내용"].ToString());   //운동한 동작의 이름
                        PlayerPrefs.SetInt("EP_NarratoinTime", int.Parse(data[dataIndex]["내레이션시간"].ToString()));  //내레이션나오는 시간
                        PlayerPrefs.SetString("EP_IsExerciseName", data[i]["운동종류"].ToString()); //현재 하고 있는 동작의 운동종류

                        if (data[i]["내용"].ToString() != "휴식시간")
                        {
                            VideoHandler.instance.LoadVideo(_ExerciseType, week.ToString(), _playIndex);
                            VideoHandler.instance.PlayVideo();
                        }
                        else
                        {
                            VideoHandler.instance.LoadNullVideo();
                            _videoPanel.SetActive(false);
                            _myImage.SetActive(false);
                            _restPanel.SetActive(true);
                            restCount += 1; //쉬는시간 카운트
                            PlayerPrefs.SetString("EP_MotionAllClass", PlayerPrefs.GetString("EP_MotionAllClass") + "=A");
                        }
                    }
                }
            }
        }
    }


    public string ErrorClassReport(int _allWeek, string _ExerciseType, int _index)
    {
        data = CSVReader.Read("ExerciseData");

        //해당 주차가 어디에 속하는지 설정해주는 부분
        int week = 0;
        if (_allWeek <= 4)
            week = 4;
        else if (_allWeek > 4 && _allWeek <= 8)
            week = 8;
        else if (_allWeek > 8 && _allWeek <= 12)
            week = 12;

        //해당운동의 동작 총 갯수와 마지막 인덱스가 몇번짼지 저장하기 위함
        for (int i = 0; i < data.Count; i++)
        {
            if (int.Parse(data[i]["주차"].ToString()).Equals(week))
            {
                if (data[i]["운동종류"].ToString().Equals(_ExerciseType))
                {
                    exerciseAllCount += 1;  //동작 총 갯수
                    //Debug.Log("i : " + i);
                    exerciseListIndex = i;  //마지막 인덱스 값 저장 (시작점을 알기 위해서)
                }
            }
        }

        //해당 운동의 동작이 시작되는 인덱스값 설정
        //int dataIndex = (exerciseListIndex - exerciseAllCount) + 1;
        string str = "";
        for (int i = 0; i < data.Count; i++)
        {
            if (int.Parse(data[i]["주차"].ToString()).Equals(week))
            {
                if (data[i]["운동종류"].ToString().Equals(_ExerciseType))
                {
                    if (i.Equals(_index))
                    {
                        str = data[_index]["내용"].ToString();
                    }
                }
            }
        }

        return str;
    }
}
