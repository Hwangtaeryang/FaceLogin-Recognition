using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




//https://xd.adobe.com/view/edd8f973-b9fe-4c49-9fd1-2802a33ed13f-b88b/
//https://docs.google.com/spreadsheets/d/1n5PIYpdMMxZ8Abj3co8LVhGDbyLH2Thn3XpLW57CZcg/edit?usp=sharing
public class LoginManager : MonoBehaviour
{
    public Text nameText;
    public Text timeText;
    public Image timeRotateImg;

    bool isRotate;  //Ÿ�̸� �̹��� ȸ�� ���� ����
    bool faceCountEnd;
    string userNameStr = "";

    void Start()
    {
        PlayerPrefs.SetString("EP_UserNAME", "");   //�����̸�
        PlayerPrefs.SetString("EP_UserSex", "");    //��������
        PlayerPrefs.SetString("EP_UserBrithDay", "");   //�����������
        PlayerPrefs.SetString("EP_UserHeight", ""); //���� Ű
        PlayerPrefs.SetString("EP_UserWight", "");  //���� ������
        PlayerPrefs.SetString("EP_UserDisease_1", "No");  //������
        PlayerPrefs.SetString("EP_UserDisease_2", "No");  //������
        PlayerPrefs.SetString("EP_UserDisease_3", "No");  //�索
        PlayerPrefs.SetString("EP_UserDisease_4", "No");  //ġ��
        PlayerPrefs.SetString("EP_UserDisease_5", "No");  //������
        PlayerPrefs.SetString("EP_UserDisease_6", "No");  //������
        PlayerPrefs.SetString("EP_UserDisease_7", "No");  //������
        PlayerPrefs.SetString("EP_OthersDisease", "No");  //������Ÿ
        PlayerPrefs.SetString("EP_StartDay", "");   //�������
        PlayerPrefs.SetString("EP_EndDay", "");   //���������
        PlayerPrefs.SetString("EP_Progress", "");   //�����
        PlayerPrefs.SetString("EP_AllClass", "No");   //��ü���
        PlayerPrefs.SetString("EP_DanceRoutineClass", "No");   //�������
        PlayerPrefs.SetString("EP_MuscularUpClass", "No");    //�ٷ»�ü���
        PlayerPrefs.SetString("EP_MuscularDownClass", "No");  //�ٷ���ü���
        PlayerPrefs.SetString("EP_OneWeek", "No");    //1�� A-A-C/B-C-C/A-C-B
        PlayerPrefs.SetString("EP_TwoWeek", "No");    //2��
        PlayerPrefs.SetString("EP_ThreeWeek", "No");  //3��
        PlayerPrefs.SetString("EP_FourWeek", "No");   //4��
        PlayerPrefs.SetString("EP_WeekReport", ""); //�ֿ� �ش��ϴ� ����Ʈ ����

        PlayerPrefs.SetString("EP_MyFaceChange", "No"); //�������
        PlayerPrefs.SetInt("EP_AllWeekNumber", 0);  //�� ��� ȸ��(�ְ� �ƴ�, Ƚ����)
        PlayerPrefs.SetInt("EP_LastExerciseNumber", 0); //������ ���ȣ(0:�ٷ���, 1:����, 2:�ٷ»�)
        PlayerPrefs.SetString("EP_LastWeek", "No"); //������ ��� ��
        PlayerPrefs.SetInt("EP_LastWeekStartPoint", 0);   //������ ��� ���� ���� Ƚ��

        PlayerPrefs.SetInt("EP_PlayCount", 0);  //� ���� �÷��̵� Ƚ��
        PlayerPrefs.SetString("EP_PlayingExerciseName", "");   //��� ������ �̸�
        PlayerPrefs.SetInt("EP_PlayingWeek", 1);    //��� ����
        PlayerPrefs.SetInt("EP_PlayingNumber", 1);  //��� Ƚ��
        PlayerPrefs.SetInt("EP_NarratoinTime", 1);  //�����̼ǳ����� �ð�
        PlayerPrefs.SetString("EP_IsExerciseName", ""); //���� �ϰ� �ִ� ������ �����

        //PlayerPrefs.SetInt("EP_ExerciseTypeAllNumber", 0); //�ش� ��� ���� �Ѱ���

        //� ������ �Ҷ� üũ�Ѱ� Ƚ���� ���� ���Դ��� üũ�� ���Ѱ͵�
        PlayerPrefs.SetString("EP_MotionCheckBothGood", "");
        PlayerPrefs.SetString("EP_MotionCheckBothBad", "");
        PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "");
        PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "");
        PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "");
        PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "");
        PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", 0);
        PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", 0);
        PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", 0);
        PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", 0);
        PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", 0);
        PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", 0);

        PlayerPrefs.SetString("EP_MotionAllClass", ""); //� ���� ��� ��� ����(�޽Ľð�����)
        PlayerPrefs.SetString("EP_OnlyMotionAllClass", ""); //����� ��� ��� ���� - �޽Ľð� ������
        PlayerPrefs.SetString("EP_CErrorClassGroup", "");   //C��� ���� ����
        PlayerPrefs.SetString("EP_WeekandCount", "");   //���� �� Ƚ�� ���� (������ �����ϱ�����)

        StartCoroutine(TimeCount());    //5��ī��Ʈ
    }

    // Update is called once per frame
    void Update()
    {
        if (userNameStr.Equals(""))
        {
            if (nameText.text != "")
                userNameStr = nameText.text;

            PlayerPrefs.SetString("EP_UserNAME", userNameStr);
        }

        if (isRotate.Equals(true))
            timeRotateImg.transform.Rotate(new Vector3(0, 0, -10f));
    }

    IEnumerator TimeCount()
    {
        isRotate = true;
        yield return new WaitForSeconds(1);

        timeText.text = "4";
        yield return new WaitForSeconds(1);
        timeText.text = "3";
        yield return new WaitForSeconds(1);
        timeText.text = "2";
        yield return new WaitForSeconds(1);
        timeText.text = "1";
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("2.LoginResult");
    }
}
