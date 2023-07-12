using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExerciseContentData : MonoBehaviour
{
    public static ExerciseContentData instacne { get; private set; }

    List<Dictionary<string, object>> data;


    int exerciseAllCount; //�ش� � Ÿ���� � ����
    int exerciseListIndex;  //�ش� � Ÿ���� � ������ �ε���
    public int restCount;   //���½ð� ī��Ʈ


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

        //�ش� ������ ��� ���ϴ��� �������ִ� �κ�
        int week = 0;
        if (_allWeek <= 4)
            week = 4;
        else if (_allWeek > 4 && _allWeek <= 8)
            week = 8;
        else if (_allWeek > 8 && _allWeek <= 12)
            week = 12;

        //�ش��� ���� �� ������ ������ �ε����� ���²�� �����ϱ� ����
        for (int i = 0; i < data.Count; i++)
        {
            if (int.Parse(data[i]["����"].ToString()).Equals(week))
            {
                if (data[i]["�����"].ToString().Equals(_ExerciseType))
                {
                    exerciseAllCount += 1;  //���� �� ����
                    //Debug.Log("i : " + i);
                    exerciseListIndex = i;  //������ �ε��� �� ���� (�������� �˱� ���ؼ�)
                }
            }
        }
        Debug.Log("�Ѱ��� : " + exerciseAllCount);
        //�ش� ��� ������ ���۵Ǵ� �ε����� ����
        //_playIndex�� ������ ��ȣ, ����� �߿��� ���° �ϰ� �ִ���
        int dataIndex = (exerciseListIndex - exerciseAllCount) + _playIndex;

        //�ش� ������ ������ �������� �˷��ش�.
        if (dataIndex.Equals(exerciseListIndex))
            ExerciseScene_UIManager.instance.lastMotionState = true;

        for(int i = 0; i < data.Count; i++)
        {
            if(int.Parse(data[i]["����"].ToString()).Equals(week))
            {
                if (data[i]["�����"].ToString().Equals(_ExerciseType))
                {
                    if(i.Equals(dataIndex))
                    {
                        _videoNameText.text = data[dataIndex]["����"].ToString();
                        PlayerPrefs.SetString("EP_PlayingExerciseName", data[dataIndex]["����"].ToString());   //��� ������ �̸�
                        PlayerPrefs.SetInt("EP_NarratoinTime", int.Parse(data[dataIndex]["�����̼ǽð�"].ToString()));  //�����̼ǳ����� �ð�
                        PlayerPrefs.SetString("EP_IsExerciseName", data[i]["�����"].ToString()); //���� �ϰ� �ִ� ������ �����

                        if (data[i]["����"].ToString() != "�޽Ľð�")
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
                            restCount += 1; //���½ð� ī��Ʈ
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

        //�ش� ������ ��� ���ϴ��� �������ִ� �κ�
        int week = 0;
        if (_allWeek <= 4)
            week = 4;
        else if (_allWeek > 4 && _allWeek <= 8)
            week = 8;
        else if (_allWeek > 8 && _allWeek <= 12)
            week = 12;

        //�ش��� ���� �� ������ ������ �ε����� ���²�� �����ϱ� ����
        for (int i = 0; i < data.Count; i++)
        {
            if (int.Parse(data[i]["����"].ToString()).Equals(week))
            {
                if (data[i]["�����"].ToString().Equals(_ExerciseType))
                {
                    exerciseAllCount += 1;  //���� �� ����
                    //Debug.Log("i : " + i);
                    exerciseListIndex = i;  //������ �ε��� �� ���� (�������� �˱� ���ؼ�)
                }
            }
        }

        //�ش� ��� ������ ���۵Ǵ� �ε����� ����
        //int dataIndex = (exerciseListIndex - exerciseAllCount) + 1;
        string str = "";
        for (int i = 0; i < data.Count; i++)
        {
            if (int.Parse(data[i]["����"].ToString()).Equals(week))
            {
                if (data[i]["�����"].ToString().Equals(_ExerciseType))
                {
                    if (i.Equals(_index))
                    {
                        str = data[_index]["����"].ToString();
                    }
                }
            }
        }

        return str;
    }
}
