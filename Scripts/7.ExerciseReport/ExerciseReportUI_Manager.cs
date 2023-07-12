using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseReportUI_Manager : MonoBehaviour
{
    //public Text allClassText;
    public Image allClassImg;
    public Sprite[] allClassSprite;
    public Text danceRoutineText;
    public Text muscularUpText;
    public Text muscularDownText;
    public Text weekText;
    public Button[] stampBtn;
    public Sprite[] stampBtnSprtie;
    public Button leftBtn;
    public Button rightBtn;

    int clickCount;

    void Start()
    {

        danceRoutineText.text = ExerciseReportManager.instance.ExerciseClass(0);
        muscularUpText.text = ExerciseReportManager.instance.ExerciseClass(1);
        muscularDownText.text = ExerciseReportManager.instance.ExerciseClass(2);
        //allClassText.text = ExerciseReportManager.instance.AllExerciseAverage();

        if (ExerciseReportManager.instance.AllExerciseAverage().Equals("A"))
            allClassImg.sprite = allClassSprite[0];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("A-"))
            allClassImg.sprite = allClassSprite[1];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("B+"))
            allClassImg.sprite = allClassSprite[2];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("B"))
            allClassImg.sprite = allClassSprite[3];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("B-"))
            allClassImg.sprite = allClassSprite[4];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("C+"))
            allClassImg.sprite = allClassSprite[5];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("C"))
            allClassImg.sprite = allClassSprite[6];
        else if (ExerciseReportManager.instance.AllExerciseAverage().Equals("-"))
            allClassImg.sprite = allClassSprite[7];


        ExerciseReportManager.instance.WeekStampShow(weekText, stampBtn, stampBtnSprtie);   //스템프 버튼 보여주기
        ExerciseReportManager.instance.WeekLeftRightButtonState(leftBtn, rightBtn); //처음 버튼 활성화/비활성화
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //주차 다음 버튼 클릭 이벤트
    public void WeekLeftButtonClick()
    {
        clickCount += 1;
        Debug.Log("왼쪽 : " + clickCount);
        ExerciseReportManager.instance.WeekLeftButtonClick(weekText, clickCount, leftBtn, rightBtn, stampBtn);
    }

    //주차 이전 버튼 클릭 이벤트
    public void WeekRightButtonClick()
    {
        clickCount -= 1;
        Debug.Log("오른쪽 : " + clickCount);
        ExerciseReportManager.instance.WeekRightButtonClick(weekText, clickCount, leftBtn, rightBtn, stampBtn);
    }
}
