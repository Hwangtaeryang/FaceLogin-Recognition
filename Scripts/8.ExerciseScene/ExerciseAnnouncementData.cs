using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseAnnouncementData : MonoBehaviour
{
    public static ExerciseAnnouncementData instacne { get; private set; }

    List<Dictionary<string, object>> data;

    int exerciseAllCount; //�ش� � Ÿ���� � ����
    int exerciseListIndex;  //�ش� � Ÿ���� � ������ �ε���

    public Text test;

    public void Awake()
    {
        if (instacne != null)
            Destroy(this);
        else instacne = this;
    }


    void Start()
    {
        //ExerciseName(1, "�ٷ¿��ü", 2, test);
    }


    //� �̸� �̾ƿ��� �Լ�
    //_ExerciseType -> �����/ �ٷ¿��ü / �ٷ¿��ü
    //����� :  �ո� �б� / �ٷ»� : ���� �ø���/������ / �ٷ��� : ���� ���θ���/���
    public void ExerciseName(int _week, string _ExerciseType, int _playIndex, Text _videoName)
    {
        exerciseAllCount = 0;
        data = CSVReader.Read("ExerciseData");

        //�ش� ������ ��� ���ϴ��� �������ִ� �κ�
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
            if(int.Parse(data[i]["����"].ToString()).Equals(week))
            {
                if(data[i]["�����"].ToString().Equals(_ExerciseType))
                {
                    exerciseAllCount += 1;  //���� �� ����
                    Debug.Log("i : " + i);
                    exerciseListIndex = i;  //������ �ε��� �� ���� (�������� �˱� ���ؼ�)
                }
            }
        }
        //PlayerPrefs.SetInt("EP_ExerciseTypeAllNumber", exerciseAllCount); //�ش� ��� ���� �� ��������
        Debug.Log(_ExerciseType + " _playIndex "  + _playIndex);
        Debug.Log("� ���� : " + exerciseAllCount + "   " + _ExerciseType);

        //�ش� ��� ������ ���۵Ǵ� �ε����� ����
        //_playIndex�� ������ ��ȣ, ����� �߿��� ���° �ϰ� �ִ���
        int dataIndex = (exerciseListIndex - exerciseAllCount) + _playIndex; 

        //��� �ε������� ������ �ε��������� �ϳ��� ���������� ��Ѵ�.
        if(dataIndex != exerciseListIndex - 1)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (int.Parse(data[i]["����"].ToString()).Equals(week))
                {
                    if (data[i]["�����"].ToString().Equals(_ExerciseType))
                    {
                        Debug.Log("��������?" + dataIndex + " _ExerciseType " + _ExerciseType);
                        Debug.Log(data[dataIndex]["����"].ToString());
                        _videoName.text = data[dataIndex]["����"].ToString();
                        VideoHandler.instance.LoadVideo(_ExerciseType, _week.ToString(), _playIndex);
                    }
                }
            }
        }
        else if(dataIndex.Equals(exerciseListIndex - 1))
        {
            //�̰� ������ ��̴�! ��� ��ȣ�� ���������.
            ExerciseSceneUI_Manager.instance.lastExerciseState = true;

            for (int i = 0; i < data.Count; i++)
            {
                if (int.Parse(data[i]["����"].ToString()).Equals(week))
                {
                    if (data[i]["�����"].ToString().Equals(_ExerciseType))
                    {
                        Debug.Log("��������?" + dataIndex + " _ExerciseType " + _ExerciseType);
                        Debug.Log(data[dataIndex]["����"].ToString());
                        _videoName.text = data[dataIndex]["����"].ToString();
                        VideoHandler.instance.LoadVideo(_ExerciseType, _week.ToString(), _playIndex);
                    }
                }
            }
        }
    }
}
