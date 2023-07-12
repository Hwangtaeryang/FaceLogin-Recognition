using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionCustomAlarm : MonoBehaviour
{
    public static MotionCustomAlarm instance { get; private set; }

    public ExerciseScene_Manager exerSceneManager;

    int wrist_L_Before, wrist_R_Before; //�ո� ���� ������ ����
    int wrist_L_Curr, wrist_R_Curr; //�ո� ���� ������ ����
    int wrist_L_Gap, wrist_R_Gap;   //�ո� ������ ��

    int shoulder_L_Before, shoulder_R_Before;   //��� ���� ������ ����
    int shoulder_L_Curr, shoulder_R_Curr;   //��� ���� ������ ����
    int shoulder_L_Gap, shoulder_R_Gap; //��� ������ ��

    bool l_Motion_Good, r_Motion_Good;  //���� ������ ����� ���ϰ� �ִ� ���¿���
    bool allAotion_Good;   //���� ���Ѵ�! ����
    bool l_MotionDown_Bad, l_MotionUp_Bad;  //���� ��� �Ʒ�, �� ������
    bool r_MotionDown_Bad, r_MotionUp_Bad;  //�����ʸ�� �Ʒ�, �� ������
    bool allSpin_Bad;   //���پȵ���

    bool dir_State;  //���� �� ������(�� ����)
    bool dir_Up; //���� �� ����(���� ����)

    float voiceTimer;   //��Ҹ� ������ �ð�(�ѹ� ��Ʈ�� �� 3~4���� ���� �ֱ�����)


    //�׽�Ʈ ��ǥ Ȯ�ο� �ؽ�Ʈ
    public Text leftText;
    public Text rightText;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }


    void Start()
    {

    }


    void Update()
    {

    }

    string GoodNarrationSay()
    {
        string goolName = "";
        //1. �������߾��  2.�ߵ������ �־��  3. ����������   4. �������ϰ��־��,���ݸ���������
        //5. ��ϴ� ����̰ǰ��غ�����  6. ����� ��� ���߰� �����ּ���  7.��� ��ġ�� �ʰ� �������ּ���.
        int number = Random.Range(1, 8);

        if (number.Equals(1)) goolName = "140 ���� ���߾��!";
        else if (number.Equals(2)) goolName = "141 �� ������� �־��!";
        else if (number.Equals(3)) goolName = "142 ���� ������!";
        else if (number.Equals(4)) goolName = "144 ���� ���ϰ� �־��, ���ݸ� �� ��������!";
        else if (number.Equals(5)) goolName = "145 ��ϴ� ����� �ǰ��غ�����!";
        else if (number.Equals(6)) goolName = "148 ����� ��� ���߰� �����ּ���.";
        else if (number.Equals(7)) goolName = "152 ��� ��ġ�� �ʰ� �������ּ���.";

        return goolName;
    }

    //float errVoiceTimer;
    //���� � - 1. �ո� �б� Ȯ�� �Լ�
    public void ShakeWristMove(int _index, float _angle)
    {
        //< ���� �ո� >
        if (_index.Equals(6))
        {
            //Debug.Log("���� üũ����");
            wrist_L_Curr = (int)_angle;
            wrist_L_Gap = wrist_L_Before - wrist_L_Curr;

            //�߸��ϰ� �ִ�. 
            if ((Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 15))
            {
                //�߸��� ������ 2.5�� �̻��� �Ǹ� �����϶�� �����̼��� ���´�.
                if (exerSceneManager.missTime_L >= 4f)
                {
                    l_Motion_Good = false;
                    allAotion_Good = false; // ���  Ʋ�ȴ�.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
            //���ϰ� �ִ�.
            else if (Mathf.Abs(wrist_L_Gap) >= 15)
            {
                if(exerSceneManager.successTime_L >= 3f)
                {
                    l_Motion_Good = true;
                    exerSceneManager.missTime_L = 0;    //���ϰ� ������ ��� 0�ð��̶�� �ʱ�ȭ
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
        }
        //< �����U �ո� >
        if (_index.Equals(7))
        {
            //Debug.Log("������ üũ����");
            wrist_R_Curr = (int)_angle;
            wrist_R_Gap = wrist_R_Before - wrist_R_Curr;

            //�߸��ϰ� �ִ�. 
            if ((Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 15))
            {
                //�߸��� ������ 2.5�� �̻��� �Ǹ� �����϶�� �����̼��� ���´�.
                if (exerSceneManager.missTime_R >= 4f)
                {
                    r_Motion_Good = false;
                    allAotion_Good = false; // ���  Ʋ�ȴ�.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
            //���ϰ� �ִ�.
            else if (Mathf.Abs(wrist_R_Gap) >= 15)
            {
                if(exerSceneManager.successTime_R >= 3f)
                {
                    r_Motion_Good = true;
                    exerSceneManager.missTime_R = 0;    //���ϰ� ������ ��� 0�ð��̶�� �ʱ�ȭ
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
        }
        WristNarrationSoundStart();
    }
    void WristNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else
        {
            allAotion_Good = false;
        }

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if(allAotion_Good.Equals(false))
        {
            //������ �߸������� �ո��б�
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "85 ������ ������ �����ϼ���");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "������ ������ �����ϼ���.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            //�������� �߸�������
            else if (r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "audio_1_������_�ո���_�о��ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "������ �ո��� �о��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
            //������ �߸�������
            else if (l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "audio_0_����_�ո���_�о��ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "���� �ո��� �о��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 2. �� ������ ���� �Լ�
    //_way - ���� : ���/�� , ����/�Ʒ� (��)
    //_way - ���� : ���/�Ʒ�, ����/�� (��)
    public void StretchArmsForward(int _index, float _angle, float _way)
    {
        //<����>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            leftText.text = "���ʾ�� " + (int)_angle + "  " + _way;
            if (_way < -230)    //���� : �Ʒ�
            {
                if (exerSceneManager.missTime_L >= 3f)
                {
                    //������ �÷��ּ���.
                    l_Motion_Good = false;
                    l_MotionDown_Bad = true;   //���� ��������
                    //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                    exerSceneManager.missTime_L = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                }
            }
            //��� : ��
            else
            {
                if (shoulder_L_Curr >= 120f)// && shoulder_L_Curr <= 155f)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                        //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 120f)
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //���� �� �÷��ּ���
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;   //���� ��������
                        //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
                //else if (shoulder_L_Curr > 155f)
                //{
                //    if (exerSceneManager.missTime_L >= 3f)
                //    {
                //        //���� �� �����ּ���
                //        l_Motion_Good = false;
                //        l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                //        l_MotionUp_Bad = true; //���� �ö���.
                //        exerSceneManager.missTime_L = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                //        exerSceneManager.successTime_L = 0;
                //    }
                //}
            }
        }

        //<������>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            rightText.text = "�����ʾ�� " + (int)_angle + "  " + _way;
            //if (_way > 0)    //��� : �Ʒ�
            //{
            //    if (exerSceneManager.missTime_R >= 3f)
            //    {
            //        �������� �÷��ּ���.
            //        r_Motion_Good = false;
            //        r_MotionDown_Bad = true;   //������ ��������
            //        r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
            //        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
            //    }
            //}
            //��� : ��
            //else
            {
                if (shoulder_R_Curr >= 125f)// && shoulder_R_Curr <= 150f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 125f)
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //������ �� �÷��ּ���
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;   //������ ��������
                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                //else if (shoulder_R_Curr > 150f)
                //{
                //    if (exerSceneManager.missTime_R >= 3f)
                //    {
                //        //������ �� �����ּ���
                //        r_Motion_Good = false;
                //        r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                //        r_MotionUp_Bad = true; //������ �ö���.
                //        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                //        exerSceneManager.successTime_R =0;
                //    }
                //}
            }
        }
        StretchArmsNarrationSoundStart();
    }
    void StretchArmsNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if ((l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15))
            {
                //���� �� �߸��ϰ� ����.
                SoundMaixerManager.instance.PlayNarration("�����", "86 ���� ������� �÷��ּ���.");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� ������� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
            {
                //������ �� �÷�����
                SoundMaixerManager.instance.PlayNarration("�����", "87 �������� ������̱��� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "�������� ������̱��� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //�������� �� �÷�����.
                SoundMaixerManager.instance.PlayNarration("�����", "89 �������� ������̱��� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� ������̱��� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }

    string[] timeArr;
    //���� � - 3. �ո� ����
    //_way - ���� : �ո� ���� ����(���), �ո� �Ʒ��� ����(����) (��)
    public void WristBreak(int _index, float _angle, float _way, Text _videoTimeText)
    {

        //if(_index.Equals(6))
        //{
        //    rightText.text = "�����ʼո� " + _angle + "  " + _way;
        //}

        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //������ �ո� ���� �ð� ����
        if ((videoTimer >= 25 && videoTimer <= 30) || (videoTimer >= 38 && videoTimer <= 42) ||
            (videoTimer >= 49 && videoTimer <= 53))
        {
            exerSceneManager.missTime_L = 0;
            //������ �ո� ����
            if (_index.Equals(7))
            {
                rightText.text = "�����ʼո� " + _angle + "  " + _way;
                wrist_R_Curr = (int)_angle;
                if (_way > 0)//��� �ո� ���� ����
                {
                    if (wrist_R_Curr >= 120f)    //�ո� �� ������
                    {
                        if(exerSceneManager.successTime_L >= 3f)
                        {
                            r_Motion_Good = true;
                            l_Motion_Good = true;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                    else
                    {
                        //�ո��� ��������.
                        if (exerSceneManager.missTime_R > 3f)
                        {
                            r_Motion_Good = false;
                            l_Motion_Good = true;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
                else
                {
                    //�ո��� ��������.
                    if (exerSceneManager.missTime_R > 3f)
                    {
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
            }
        }
        else if ((videoTimer >= 32 && videoTimer <= 37) || (videoTimer >= 44 && videoTimer <= 48) ||
            (videoTimer >= 54 && videoTimer <= 58))
        {
            exerSceneManager.missTime_R = 0;
            //���� �ո� ����
            if (_index.Equals(6))
            {
                leftText.text = "���ʼո� " + _angle + "   " + _way;

                wrist_L_Curr = (int)_angle;

                if (_way > -40)  //���� �ո� ���� ����
                {
                    if (wrist_L_Curr >= 120f)
                    {
                        if(exerSceneManager.successTime_R >= 3f)
                        {
                            l_Motion_Good = true;
                            r_Motion_Good = true;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                    else
                    {
                        //�ո��� ��������.
                        if (exerSceneManager.missTime_L > 3f)
                        {
                            l_Motion_Good = false;
                            r_Motion_Good = true;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
                else
                {
                    //�ո��� ��������.
                    if (exerSceneManager.missTime_L > 3f)
                    {
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_R = 0;
                    }
                }
            }
        }
        WristBreakNarrationSoundStart();
    }
    void WristBreakNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 10)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(true) && voiceTimer >= 10)
            {
                //�޼��� �����ּ���
                SoundMaixerManager.instance.PlayNarration("�����", "���� �ո��� �����ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "���� �ո��� �����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && l_Motion_Good.Equals(true) && voiceTimer >= 10)
            {
                //���������� �����ּ���
                SoundMaixerManager.instance.PlayNarration("�����", "������ �ո��� �����ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "������ �ո��� �����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 4. �ո� ������
    //_way - ���� : ���/�� , ����/�Ʒ� (��)
    //_way - ���� : ���/�Ʒ�, ����/�� (��)
    public void TurningWristMove(int _index, float _angle, float _way)
    {
        //<����>
        if (_index.Equals(6))
        {
            if (_way > 0)
                dir_Up = true;  //��� ���� ��
            else
                dir_Up = false; //���� ���� �Ʒ�

            wrist_L_Curr = (int)_angle;
            wrist_L_Gap = wrist_L_Before - wrist_L_Curr;
            leftText.text = "�޼ո� " + (int)_angle + "  " + _way + " �� " + Mathf.Abs(wrist_L_Gap);

            if (Mathf.Abs(wrist_L_Gap).Equals(wrist_L_Curr) || Mathf.Abs(wrist_L_Gap) < 10)
            {
                if (exerSceneManager.missTime_L > 3f)
                {
                    //���� ��� �����ּ���
                    l_Motion_Good = false;  //�������� �ʴ´�.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
            else if (Mathf.Abs(wrist_L_Gap) >= 15)
            {
                //if(exerSceneManager.successTime_L >= 3f)
                {
                    //����
                    l_Motion_Good = true;  //�����̴�.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                wrist_L_Before = wrist_L_Curr;
            }
        }

        //<������>
        if (_index.Equals(7))
        {
            if (_way > 0)
                dir_Up = true;  //��� ���� ��
            else
                dir_Up = false; //���� ���� �Ʒ�

            wrist_R_Curr = (int)_angle;
            wrist_R_Gap = wrist_R_Before - wrist_R_Curr;
            rightText.text = "���ո� " + (int)_angle + "  " + _way + " �� " + Mathf.Abs(wrist_R_Gap);

            if (Mathf.Abs(wrist_R_Gap).Equals(wrist_R_Curr) || Mathf.Abs(wrist_R_Gap) < 10)
            {
                if (exerSceneManager.missTime_R > 3f)
                {
                    //������ ��� �����ּ���
                    r_Motion_Good = false;  //�������� �ʴ´�.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
            else if (Mathf.Abs(wrist_R_Gap) >= 15)
            {
                //if(exerSceneManager.successTime_R >= 3f)
                {
                    //����
                    r_Motion_Good = true;  //�����̴�.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                wrist_R_Before = wrist_R_Curr;
            }
        }
        TurningWristNarrationSoundStart();
    }
    void TurningWristNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                //���ʴ� �����ּ���.
                SoundMaixerManager.instance.PlayNarration("�����", "85 ������ ������ �����ϼ���");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "������ ������ �����ϼ���.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //�޼��� �����ּ���
                SoundMaixerManager.instance.PlayNarration("�����", "audio_2_����_�ո���_�����ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "���� �ո��� �����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && l_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //���������� �����ּ���
                SoundMaixerManager.instance.PlayNarration("�����", "audio_3_������_�ո���_�����ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "������ �ո��� �����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 5. ��������� �Լ�
    public void TurningShoulders(int _index, float _angle)
    {
        //���� ���
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            shoulder_L_Gap = shoulder_L_Before - shoulder_L_Curr;
            leftText.text = "���ո� " + (int)_angle + " �� " + Mathf.Abs(shoulder_L_Gap);

            if (Mathf.Abs(shoulder_L_Gap).Equals(shoulder_L_Curr) || Mathf.Abs(shoulder_L_Gap) < 5)
            {
                if (exerSceneManager.missTime_L > 3f)
                {
                    //���� ��� �����ּ���
                    l_Motion_Good = false;  //�������� �ʴ´�.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                shoulder_L_Before = shoulder_L_Curr;
            }
            else if (Mathf.Abs(shoulder_L_Gap) >= 10)
            {
                //if(exerSceneManager.successTime_L >= 3f)
                {
                    //����
                    l_Motion_Good = true;  //�����̴�.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
                shoulder_L_Before = shoulder_L_Curr;
            }
        }

        //������ ���
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            shoulder_R_Gap = shoulder_R_Before - shoulder_R_Curr;
            rightText.text = "���ո� " + (int)_angle + " �� " + Mathf.Abs(shoulder_R_Gap);

            if (Mathf.Abs(shoulder_R_Gap).Equals(shoulder_R_Curr) || Mathf.Abs(shoulder_R_Gap) < 5)
            {
                if (exerSceneManager.missTime_R > 3f)
                {
                    //������ ��� �����ּ���
                    r_Motion_Good = false;  //�������� �ʴ´�.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                shoulder_R_Before = shoulder_R_Curr;
            }
            else if (Mathf.Abs(shoulder_R_Gap) >= 10)
            {
                //if(exerSceneManager.successTime_R >= 3f)
                {
                    //����
                    r_Motion_Good = true;  //�����̴�.
                    exerSceneManager.missTime_R = 0;
                    exerSceneManager.successTime_R = 0;
                }
                shoulder_R_Before = shoulder_R_Curr;
            }
        }
        TurningShouldersNarrationSoundStart();
    }
    void TurningShouldersNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "85 ������ ������ �����ϼ���");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "������ ������ �����ϼ���.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(false) && r_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //���ʾ�� ������
                SoundMaixerManager.instance.PlayNarration("�����", "audio_4_����_�����_�����ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "����_�����_�����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && l_Motion_Good.Equals(true) && voiceTimer >= 15)
            {
                //�����ʾ�� ������
                SoundMaixerManager.instance.PlayNarration("�����", "audio_5_������_�����_�����ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "������_�����_�����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 6.���� ���� �ø���/������
    public void ArmUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //������ �� ���� �ð� ����
        if ((videoTimer >= 16 && videoTimer <= 19) || (videoTimer >= 25 && videoTimer <= 27) ||
            (videoTimer >= 33 && videoTimer <= 36) || (videoTimer >= 40 && videoTimer <= 43) ||
            (videoTimer >= 49 && videoTimer <= 51))
        {
            //<������>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + _angle + "  " + _way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //�������� �÷��ּ���.
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125f)
                    {
                        r_Motion_Good = true;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                    }
                }
            }
        }
        else if ((videoTimer >= 20 && videoTimer <= 23) || (videoTimer >= 29 && videoTimer <= 31) ||
            (videoTimer >= 37 && videoTimer <= 39) || (videoTimer >= 44 && videoTimer <= 46) ||
            (videoTimer >= 52 && videoTimer <= 54))
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + _angle + "  " + _way;
                if (_way < 0) //���� : �Ʒ�
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //������ �÷����.
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        l_Motion_Good = true;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
            }
        }
        ArmUpDownNarrationSoundStart();
    }
    void ArmUpDownNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(true) && l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "������ ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "�������� ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 7. ���� ������ �ø���/������
    public void ArmFrontUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //������ �� ���� �ð� ����
        if ((videoTimer >= 15 && videoTimer <= 18) || (videoTimer >= 23 && videoTimer <= 26) ||
            (videoTimer >= 32 && videoTimer <= 35) || (videoTimer >= 39 && videoTimer <= 42) ||
            (videoTimer >= 46 && videoTimer <= 48))
        {
            //<������>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + _angle + "  " + _way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //�������� �÷��ּ���.
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125f)
                    {
                        r_Motion_Good = true;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                    }
                }
            }
        }
        else if ((videoTimer >= 20 && videoTimer <= 22) || (videoTimer >= 28 && videoTimer <= 30) ||
            (videoTimer >= 35 && videoTimer <= 37) || (videoTimer >= 42 && videoTimer <= 44) ||
            (videoTimer >= 49 && videoTimer <= 51))
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + _angle + "  " + _way;
                if (_way < 0) //���� : �Ʒ�
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //������ �÷����.
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        l_Motion_Good = true;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
            }
        }

        ArmFrontUpDownSoundStart();
    }
    void ArmFrontUpDownSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(true) && l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "������ ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "�������� ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 8. ���� ������ �ø���/������
    public void ArmSideUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //������ �� ���� �ð� ����
        if ((videoTimer >= 15 && videoTimer <= 22) || (videoTimer >= 28 && videoTimer <= 32) ||
            (videoTimer >= 38 && videoTimer <= 41) || (videoTimer >= 48 && videoTimer <= 52))
        {
            //<������>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + _angle + "  " + _way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //�������� �÷��ּ���.
                        r_Motion_Good = false;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 160f)
                    {
                        r_Motion_Good = true;
                        l_Motion_Good = true;
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                    }
                }
            }
        }
        else if ((videoTimer >= 23 && videoTimer <= 27) || (videoTimer >= 34 && videoTimer <= 37) ||
            (videoTimer >= 43 && videoTimer <= 47) || (videoTimer >= 53 && videoTimer <= 57))
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + _angle + "  " + _way;
                if (_way < 0) //���� : �Ʒ�
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //������ �÷����.
                        l_Motion_Good = false;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr >= 140f)
                    {
                        l_Motion_Good = true;
                        r_Motion_Good = true;
                        exerSceneManager.missTime_L = 0;
                    }
                }
            }
        }

        ArmSideUpDownSoundStart();
    }
    void ArmSideUpDownSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(true) && l_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "������ ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "�������� ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 9. ������ ���� �ø���/������
    public void BothArmUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);



        //������ ���� �ö� �־���ϴ� ����
        if ((videoTimer >= 15 && videoTimer <= 20) || (videoTimer >= 24 && videoTimer <= 29) ||
            (videoTimer >= 33 && videoTimer <= 37) || (videoTimer >= 41 && videoTimer <= 45) ||
            (videoTimer >= 50 && videoTimer <= 54))
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way < -230) //230���� ������ ����//���� : �Ʒ�
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //������ �÷����.
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;    // ������ ��������.
                        //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        if(exerSceneManager.successTime_L >= 3f)
                        {
                            l_Motion_Good = true;
                            l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                                                        //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //������
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //�������� �÷��ּ���.
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;    // �������� ��������.
                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125)
                    {
                        if(exerSceneManager.successTime_R >= 3f)
                        {
                            r_Motion_Good = true;
                            r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                                                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
            }
        }
        else
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way < -230f) //230���� ������ ����//���� : �Ʒ�
                {
                    if (exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionUp_Bad = false;   //���� ������������. �ڼ�����
                                                  //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    //if (shoulder_L_Curr > 120f)
                    {
                        if (exerSceneManager.missTime_L >= 3f)
                        {
                            //������ �÷����.
                            l_Motion_Good = false;
                            l_MotionUp_Bad = true;    // ������ ��������.
                                                      //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //������
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionUp_Bad = false;   //������ �������� �ڼ�����
                                                  //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    //if (shoulder_R_Curr >= 120f)
                    {
                        if (exerSceneManager.missTime_R >= 3f)
                        {
                            //�������� �÷��ּ���.
                            r_Motion_Good = false;
                            r_MotionUp_Bad = true;    // �������� ������������ ������
                                                      //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0�� �־ �ð� 0���� �ʱ�ȭ
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
            }
        }
        BothArmUpDownNarrationSoundStart(videoTimer);
    }
    void BothArmUpDownNarrationSoundStart(float _videoTimer)
    {
        voiceTimer += Time.deltaTime;
        //errVoiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if ((_videoTimer >= 15 && _videoTimer <= 20) || (_videoTimer >= 24 && _videoTimer <= 29) ||
            (_videoTimer >= 33 && _videoTimer <= 37) || (_videoTimer >= 41 && _videoTimer <= 45) ||
            (_videoTimer >= 50 && _videoTimer <= 54))
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                r_Motion_Good = false; l_Motion_Good = false; allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //������ �������ִ�. �Ѵ� �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "106 ���� ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //������ �������ִ�. ���� �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ ���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //�������� �����Դ�. ������ �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� ���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
        else 
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                //SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                //PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
                //PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                r_Motion_Good = false; l_Motion_Good = false; allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                Debug.Log("1���� �ȵ����� ???????");
                if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //������ �ö��ִ�. �Ѵ� ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "���� �Ʒ��� �����ּ���-");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //������ �ö��ִ�. ���� ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "98 ������ �Ʒ��� �����ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(false) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //�������� �ö��ִ�. ������ ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "99 �������� �Ʒ��� �����ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }

    }


    //���� � - 10. ������ ������ �ø���/������
    public void BothArmFrontUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //������ ������ �ö� �־���ϴ� ����
        if ((videoTimer >= 14 && videoTimer <= 17) || (videoTimer >= 22 && videoTimer <= 25) ||
            (videoTimer >= 27 && videoTimer <= 31) || (videoTimer >= 35 && videoTimer <= 38) ||
            (videoTimer >= 41 && videoTimer <= 45) || (videoTimer >= 49 && videoTimer <= 52) ||
            (videoTimer >= 53 && videoTimer <= 56))
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way < -230) //230���� ������ ����//���� : �Ʒ�
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //������ �÷����.
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;    // ������ ��������.
                        //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    if (shoulder_L_Curr > 125f)
                    {
                        if(exerSceneManager.successTime_L >= 3f)
                        {
                            l_Motion_Good = true;
                            l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                                                        //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //������
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //�������� �÷��ּ���.
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;    // �������� ��������.
                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    if (shoulder_R_Curr >= 125)
                    {
                        if(exerSceneManager.successTime_R >= 3f)
                        {
                            r_Motion_Good = true;
                            r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                                                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }

            }
        }
        else
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way < -230f) //230���� ������ ����//���� : �Ʒ�
                {
                    if (exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionUp_Bad = false;   //���� ������������. �ڼ�����
                                                  //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else
                {
                    //if (shoulder_L_Curr > 120f)
                    {
                        if (exerSceneManager.missTime_L >= 3f)
                        {
                            //������ �÷����.
                            l_Motion_Good = false;
                            l_MotionUp_Bad = true;    // ������ ��������.
                                                      //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                    }
                }
            }
            //������
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
                if (_way > 0)    //��� : �Ʒ�
                {
                    if (exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionUp_Bad = false;   //������ �������� �ڼ�����
                                                  //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else
                {
                    //if (shoulder_R_Curr >= 120f)
                    {
                        if (exerSceneManager.missTime_R >= 3f)
                        {
                            //�������� �÷��ּ���.
                            r_Motion_Good = false;
                            r_MotionUp_Bad = true;    // �������� ������������ ������
                                                      //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                            exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0�� �־ �ð� 0���� �ʱ�ȭ
                            exerSceneManager.successTime_R = 0;
                        }
                    }
                }
            }
        }

        BothArmFrontUpDownNarrationSoundStart(videoTimer);
    }
    void BothArmFrontUpDownNarrationSoundStart(float _videoTimer)
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if ((_videoTimer >= 14 && _videoTimer <= 17) || (_videoTimer >= 22 && _videoTimer <= 25) ||
            (_videoTimer >= 27 && _videoTimer <= 31) || (_videoTimer >= 35 && _videoTimer <= 38) ||
            (_videoTimer >= 41 && _videoTimer <= 45) || (_videoTimer >= 49 && _videoTimer <= 52) ||
            (_videoTimer >= 53 && _videoTimer <= 56))
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //������ �������ִ�. �Ѵ� �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "106 ���� ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //������ �������ִ�. ���� �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ ���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //�������� �����Դ�. ������ �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� ���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
        else
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                //SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                //PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
                //PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                Debug.Log("2���� �ȵ����� ???????");
                if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //������ �ö��ִ�. �Ѵ� ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "���� �Ʒ��� �����ּ���-");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //������ �ö��ִ�. ���� ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "98 ������ �Ʒ��� �����ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(false) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //�������� �ö��ִ�. ������ ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "99 �������� �Ʒ��� �����ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
    }


    //���� � - 11.������ ������ �ø���/������
    public void BothArmSideUpDown(int _index, float _angle, float _way, Text _videoTimeText)
    {
        if (_videoTimeText.text != "")
        {
            string timeData = _videoTimeText.text;
            char ch = ':';
            timeArr = timeData.Split(ch);
        }
        float videoTimer = float.Parse(timeArr[1]);

        //������ ������ �ö� �־���ϴ� ����
        if ((videoTimer >= 13 && videoTimer <= 16) || (videoTimer >= 20 && videoTimer <= 23) ||
            (videoTimer >= 26 && videoTimer <= 29) || (videoTimer >= 34 && videoTimer <= 37) ||
            (videoTimer >= 40 && videoTimer <= 43) || (videoTimer >= 47 && videoTimer <= 50) ||
            (videoTimer >= 53 && videoTimer <= 57))
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
                if (shoulder_L_Curr >= 160f)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                                                    //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 160f)
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //���� �� �÷��ּ���
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;   //���� ��������
                                                   // l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
            }
            //<������>
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
                if (shoulder_R_Curr >= 160f)//&& shoulder_R_Curr <= 145f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                                                    //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 160f)
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //������ �� �÷��ּ���
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;   //������ ��������
                                                   //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
            }
        }
        else
        {
            //<����>
            if (_index.Equals(4))
            {
                shoulder_L_Curr = (int)_angle;
                leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;

                if (shoulder_L_Curr >= 160f)//����
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        l_Motion_Good = false;
                        l_MotionUp_Bad = true;   //���ʿö�����
                        exerSceneManager.missTime_L = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 160f)//����
                {
                    if (exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionUp_Bad = false;   //���� ��������
                        exerSceneManager.missTime_L = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
            }
            //������
            if (_index.Equals(5))
            {
                shoulder_R_Curr = (int)_angle;
                rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;

                if (shoulder_R_Curr >= 160f)//&& shoulder_R_Curr <= 145f) //����
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        r_Motion_Good = false;
                        r_MotionUp_Bad = true;   //������ ������������. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 160f)//����
                {
                    if (exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionUp_Bad = false;   //������ ��������
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
            }
        }

        BothArmSideUpDownNarrationSoundStart(videoTimer);
    }
    void BothArmSideUpDownNarrationSoundStart(float _videoTimer)
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if ((_videoTimer >= 13 && _videoTimer <= 16) || (_videoTimer >= 20 && _videoTimer <= 23) ||
            (_videoTimer >= 26 && _videoTimer <= 29) || (_videoTimer >= 34 && _videoTimer <= 37) ||
            (_videoTimer >= 40 && _videoTimer <= 43) || (_videoTimer >= 47 && _videoTimer <= 50) ||
            (_videoTimer >= 53 && _videoTimer <= 57))
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //������ �������ִ�. �Ѵ� �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "106 ���� ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //������ �������ִ�. ���� �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ ���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //�������� �����Դ�. ������ �÷���!
                    SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� ���� �÷��ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
        else 
        {
            if (allAotion_Good.Equals(true) && voiceTimer >= 15)
            {
                //SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
                //PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
                //PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
                allAotion_Good = false;
                voiceTimer = 0;
            }
            else if (allAotion_Good.Equals(false))
            {
                Debug.Log("3���� �ȵ����� ???????");
                if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //������ �ö��ִ�. �Ѵ� ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "���� �Ʒ��� �����ּ���-");
                    PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(true) && r_MotionUp_Bad.Equals(false) && voiceTimer >= 15)
                {
                    //������ �ö��ִ�. ���� ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "98 ������ �Ʒ��� �����ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                    voiceTimer = 0;
                }
                else if (l_MotionUp_Bad.Equals(false) && r_MotionUp_Bad.Equals(true) && voiceTimer >= 15)
                {
                    //�������� �ö��ִ�. ������ ������!
                    SoundMaixerManager.instance.PlayNarration("�����", "99 �������� �Ʒ��� �����ּ���");
                    PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� �Ʒ��� �����ּ���.");
                    PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                    voiceTimer = 0;
                }
            }
        }
    }


    //���� � - 12. ������ �� ������
    public void RightArmSpin(int _index, float _angle, float _way)
    {
        //<������>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            shoulder_R_Gap = shoulder_R_Before - shoulder_R_Curr;
            rightText.text = "�����ʾ�� " + _angle + "  " + _way + "�� " + Mathf.Abs(shoulder_R_Gap);
            //if (_way > 0)    //��� : �Ʒ�
            //{
            //    if (exerSceneManager.missTime_R >= 3f)
            //    {
            //        //�������� �÷��ּ���.
            //        r_Motion_Good = false;
            //        allSpin_Bad = false;
            //        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
            //    }
            //    shoulder_R_Before = shoulder_R_Curr;
            //}
            //else
            {
                if (shoulder_R_Curr >= 160f)
                {
                    if (Mathf.Abs(shoulder_R_Gap).Equals(shoulder_R_Curr) || Mathf.Abs(shoulder_R_Gap) < 5)
                    {
                        if (exerSceneManager.missTime_R > 3f)
                        {
                            //������ ��� �����ּ���
                            r_Motion_Good = false;  //�������� �ʴ´�.
                            allSpin_Bad = true;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                        shoulder_R_Before = shoulder_R_Curr;
                    }
                    else if (Mathf.Abs(shoulder_R_Gap) >= 8)
                    {
                        //if(exerSceneManager.successTime_R >= 3f)
                        {
                            //����
                            r_Motion_Good = true;  //�����̴�.
                            allSpin_Bad = false;
                            exerSceneManager.missTime_R = 0;
                            exerSceneManager.successTime_R = 0;
                        }
                        shoulder_R_Before = shoulder_R_Curr;
                    }
                }
                else
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //�������� �÷��ּ���.
                        r_Motion_Good = false;
                        allSpin_Bad = false;
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                    shoulder_R_Before = shoulder_R_Curr;
                }
            }
        }
        RightArmSpinNarrationSoundStart();
    }
    void RightArmSpinNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (r_Motion_Good.Equals(true) && allSpin_Bad.Equals(false))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (r_Motion_Good.Equals(false) && allSpin_Bad.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "�������� ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (r_Motion_Good.Equals(false) && allSpin_Bad.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "audio_5_������_�����_�����ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� ����� �����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 13. ���� �� ������
    public void LeftArmSpin(int _index, float _angle, float _way)
    {
        //<����>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            shoulder_L_Gap = shoulder_L_Before - shoulder_L_Curr;
            leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way + "�� " + Mathf.Abs(shoulder_L_Gap);
            //if (_way < 0) //���� : �Ʒ�
            //{
            //    if (exerSceneManager.missTime_L >= 3f)
            //    {
            //        //������ �÷����.
            //        l_Motion_Good = false;
            //        r_Motion_Good = true;
            //        exerSceneManager.missTime_L = 0;
            //    }
            //}
            //else
            {
                if (shoulder_L_Curr >= 160)
                {
                    if (Mathf.Abs(shoulder_L_Gap).Equals(shoulder_L_Curr) || Mathf.Abs(shoulder_L_Gap) < 5)
                    {
                        if (exerSceneManager.missTime_L > 3f)
                        {
                            //���� ��� �����ּ���
                            l_Motion_Good = false;  //�������� �ʴ´�.
                            allSpin_Bad = true;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                        shoulder_L_Before = shoulder_L_Curr;
                    }
                    else if (Mathf.Abs(shoulder_L_Gap) >= 10)
                    {
                        //if(exerSceneManager.successTime_L >= 3f)
                        {
                            //����
                            l_Motion_Good = true;  //�����̴�.
                            allSpin_Bad = false;
                            exerSceneManager.missTime_L = 0;
                            exerSceneManager.successTime_L = 0;
                        }
                        shoulder_L_Before = shoulder_L_Curr;
                    }
                }
                else
                {
                    if (exerSceneManager.missTime_L > 3f)
                    {
                        //���� ��� �����ּ���
                        l_Motion_Good = false;  //�������� �ʴ´�.
                        allSpin_Bad = false;
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                    shoulder_L_Before = shoulder_L_Curr;
                }
            }
        }
        LeftArmSpinNarrationSoundStart();
    }
    void LeftArmSpinNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && allSpin_Bad.Equals(false))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_Motion_Good.Equals(false) && allSpin_Bad.Equals(false) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "������ ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_Motion_Good.Equals(false) && allSpin_Bad.Equals(true) && voiceTimer >= 15)
            {
                SoundMaixerManager.instance.PlayNarration("�����", "audio_4_����_�����_�����ּ���_");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "������ ����� �����ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 14. ���� ���� �ø���
    public void BothArmUp(int _index, float _angle, float _way)
    {
        //<����>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
            if (_way > 0) //���� : �Ʒ�
            {
                if (exerSceneManager.missTime_L >= 4f)
                {
                    //������ �÷����.
                    l_Motion_Good = false;
                    l_MotionDown_Bad = true;    // ������ ��������.
                    exerSceneManager.missTime_L = 0;
                    exerSceneManager.successTime_L = 0;
                }
            }
            else
            {
                //if (shoulder_L_Curr > 125)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        //��
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                        exerSceneManager.missTime_L = 0;
                        exerSceneManager.successTime_L = 0;
                    }
                }
                //else
                //{
                //    l_Motion_Good = false;
                //    l_MotionDown_Bad = true;   //���� ������������. �ڼ�����
                //    exerSceneManager.missTime_L = 0;
                //    exerSceneManager.successTime_L = 0;
                //}
            }
        }
        //<������>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
            if (_way < 0)    //���� -40���� ũ��: �Ʒ�
            {
                if (exerSceneManager.missTime_R >= 3f)
                {
                    //�������� �÷��ּ���.
                    r_Motion_Good = false;
                    r_MotionDown_Bad = true;    // �������� ��������.
                    exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                    exerSceneManager.successTime_R = 0;
                }
            }
            else
            {
                //if (shoulder_R_Curr >= 125f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                //else
                //{
                //    //�������� �÷��ּ���.
                //    r_Motion_Good = false;
                //    r_MotionDown_Bad = true;    // �������� ��������.
                //    exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                //    exerSceneManager.successTime_R = 0;
                //}
            }
        }
        BothArmUpNarrationSoundStart();
    }
    void BothArmUpNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;
        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //���� ��������
                SoundMaixerManager.instance.PlayNarration("�����", "106 ���� ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
            {
                //���� ��������
                SoundMaixerManager.instance.PlayNarration("�����", "96 ������ ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err1", "������ ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount1") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //������ ��������
                SoundMaixerManager.instance.PlayNarration("�����", "97 �������� ���� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err1", "�������� ���� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount1", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount1") + 1);
                voiceTimer = 0;
            }
        }
    }


    //���� � - 15. ���� ������ ������
    public void BothArmSideUp(int _index, float _angle, float _way)
    {
        //<����>
        if (_index.Equals(4))
        {
            shoulder_L_Curr = (int)_angle;
            leftText.text = "���ʾ�� " + (int)_angle + "  " + (int)_way;
            //if (_way < 0)    //���� : �Ʒ�
            //{
            //    if (exerSceneManager.missTime_L >= 3f)
            //    {
            //        //������ �÷��ּ���.
            //        l_Motion_Good = false;
            //        l_MotionDown_Bad = true;   //���� ��������
            //        l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
            //        exerSceneManager.missTime_L = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
            //    }
            //}
            ////��� : ��
            //else
            {
                if (shoulder_L_Curr >= 150f)
                {
                    if(exerSceneManager.successTime_L >= 3f)
                    {
                        l_Motion_Good = true;
                        l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                                                    //l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_L = 0;
                    }
                }
                else if (shoulder_L_Curr < 150f)
                {
                    if (exerSceneManager.missTime_L >= 3f)
                    {
                        //���� �� �÷��ּ���
                        l_Motion_Good = false;
                        l_MotionDown_Bad = true;   //���� ��������
                                                   // l_MotionUp_Bad = false; //���� �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_L = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_L = 0;
                    }
                }
                //else if (shoulder_L_Curr > 145f)
                //{
                //    if (exerSceneManager.missTime_L >= 3f)
                //    {
                //        //���� �� �����ּ���
                //        l_Motion_Good = false;
                //        l_MotionDown_Bad = false;   //���� ������������. �ڼ�����
                //        //l_MotionUp_Bad = true; //���� �ö���.
                //        exerSceneManager.missTime_L = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                //    }
                //}
            }
        }
        //<������>
        if (_index.Equals(5))
        {
            shoulder_R_Curr = (int)_angle;
            rightText.text = "�����ʾ�� " + (int)_angle + "  " + (int)_way;
            //if (_way > 0)    //��� : �Ʒ�
            //{
            //    if (exerSceneManager.missTime_R >= 3f)
            //    {
            //        //�������� �÷��ּ���.
            //        r_Motion_Good = false;
            //        r_MotionDown_Bad = true;   //������ ��������
            //        r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
            //        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
            //    }
            //}
            ////��� : ��
            //else
            {
                if (shoulder_R_Curr >= 150f)//&& shoulder_R_Curr <= 145f)
                {
                    if(exerSceneManager.successTime_R >= 3f)
                    {
                        r_Motion_Good = true;
                        r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                                                    //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //���ϰ� �־ �ð� 0���� �ʱ�ȭ
                        exerSceneManager.successTime_R = 0;
                    }
                }
                else if (shoulder_R_Curr < 150f)
                {
                    if (exerSceneManager.missTime_R >= 3f)
                    {
                        //������ �� �÷��ּ���
                        r_Motion_Good = false;
                        r_MotionDown_Bad = true;   //������ ��������
                        //r_MotionUp_Bad = false; //������ �ö��� ����. �ڼ�����
                        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                        exerSceneManager.successTime_R = 0;
                    }
                }
                //else if (shoulder_R_Curr > 145f)
                //{
                //    if (exerSceneManager.missTime_R >= 3f)
                //    {
                //        //������ �� �����ּ���
                //        r_Motion_Good = false;
                //        r_MotionDown_Bad = false;   //������ ������������. �ڼ�����
                //        r_MotionUp_Bad = true; //������ �ö���.
                //        exerSceneManager.missTime_R = 0;    //üũ�� �����ϱ� �ʱ�ȭ 0
                //    }
                //}
            }
        }

        BothArmSideUpNarrationSoundStart();
    }
    void BothArmSideUpNarrationSoundStart()
    {
        voiceTimer += Time.deltaTime;

        //���� ������ ����� �Ѵ� ���ϰ� �ִ�.
        if (l_Motion_Good.Equals(true) && r_Motion_Good.Equals(true))
        {
            allAotion_Good = true; // ��� �ʹ� ���ϰ� �ִ�.
        }
        else allAotion_Good = false;

        if (allAotion_Good.Equals(true) && voiceTimer >= 15)
        {
            SoundMaixerManager.instance.PlayNarration("Good", GoodNarrationSay());
            PlayerPrefs.SetString("EP_MotionCheckBothGood", "���ϰ��־��.");
            PlayerPrefs.SetInt("EP_MotionCheckBothBothGoodCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothGoodCount") + 1);
            voiceTimer = 0;
        }
        else if (allAotion_Good.Equals(false))
        {
            if ((l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15))
            {
                //���� �� �߸��ϰ� ����.
                SoundMaixerManager.instance.PlayNarration("�����", "86 ���� ������� �÷��ּ���.");
                PlayerPrefs.SetString("EP_MotionCheckBothBad", "���� ������� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckBothBothBadCount", PlayerPrefs.GetInt("EP_MotionCheckBothBothBadCount") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(true) && r_MotionDown_Bad.Equals(false) && voiceTimer >= 15)
            {
                //������ �� �÷�����
                SoundMaixerManager.instance.PlayNarration("�����", "87 �������� ������̱��� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckLeft_Err2", "�������� ������̱��� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckLeft_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckLeft_ErrCount2") + 1);
                voiceTimer = 0;
            }
            else if (l_MotionDown_Bad.Equals(false) && r_MotionDown_Bad.Equals(true) && voiceTimer >= 15)
            {
                //�������� �� �÷�����.
                SoundMaixerManager.instance.PlayNarration("�����", "89 �������� ������̱��� �÷��ּ���");
                PlayerPrefs.SetString("EP_MotionCheckRight_Err2", "�������� ������̱��� �÷��ּ���.");
                PlayerPrefs.SetInt("EP_MotionCheckRight_ErrCount2", PlayerPrefs.GetInt("EP_MotionCheckRight_ErrCount2") + 1);
                voiceTimer = 0;
            }
        }
    }
}
