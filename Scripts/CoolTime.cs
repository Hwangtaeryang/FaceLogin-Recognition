using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    public static CoolTime instance { get; private set; }




    public Image coolImg;   //쿨타임 이미지
    public float currCoolTime = 0;
    public float maxCoolTime = 30f;

    public bool coolTimeStart;    //쿨타임 종료 상태


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
        //운동 시작되었으면
        if (MainUI_Manager.instance.checkStart.Equals(true))
        {
            currCoolTime += Time.deltaTime; //초 카운트
            coolImg.fillAmount =  currCoolTime / maxCoolTime;

            if (currCoolTime < maxCoolTime)
                coolTimeStart = true;
            else if (currCoolTime >= maxCoolTime && coolTimeStart.Equals(true))
            {
                coolTimeStart = false;
                currCoolTime = maxCoolTime + 1;

                //결과 화면 보여주기
                MainUI_Manager.instance.ResultPanelShow(); 

                //다음 운동 이름 저장 및 다음 운동 소개 멘트 시작
                GetWorkOutEventName();
                
            }   
        }
    }


    //다음 운동 이름 저장
    public void GetWorkOutEventName()
    {
        StartCoroutine(_GetWorkOutEventName());
    }

    IEnumerator _GetWorkOutEventName()
    {
        yield return new WaitForSeconds(5.1f);

        string name = PlayerPrefs.GetString("FKR_WorkOutEvnet");
        string[] nameTokens = name.Split('_');
        string num = "";

        Debug.Log(PlayerPrefs.GetString("FKR_WorkOutEvnet"));
        Debug.Log(nameTokens[0] + "   " + nameTokens[1]);


        if (nameTokens[0].Equals("1"))
        {
            if (nameTokens[1].Equals("15"))
                num = "2_1";
            else
                num = nameTokens[0] + "_" + (int.Parse(nameTokens[1]) + 1).ToString();
        }
        else if (nameTokens[0].Equals("2"))
        {
            if (nameTokens[1].Equals("3"))
                num = "3_1";
            else
                num = nameTokens[0] + "_" + (int.Parse(nameTokens[1]) + 1).ToString();
        }
        else if (nameTokens[1].Equals("3"))
        {
            if (nameTokens[1].Equals("5"))
                num = "4_1";
        }
        PlayerPrefs.SetString("FKR_WorkOutEvnet", num);

        MainUI_Manager.instance.WorkOutEvnetReport();   //다음 운동 소개
    }

}
