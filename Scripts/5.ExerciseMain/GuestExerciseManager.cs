using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuestExerciseManager : MonoBehaviour
{
    public Text announcementText;
    public ToggleGroup sportChoiceToggleGroup;

    string choiceName;



    public Toggle sportChoiceCurrentSelection
    {
        get { return sportChoiceToggleGroup.ActiveToggles().FirstOrDefault(); }
    }


    void Start()
    {
        StartCoroutine(AnnouncementTextShow());
    }

    
    void Update()
    {
        
    }

    IEnumerator AnnouncementTextShow()
    {
        announcementText.text = "오늘도 힘차게 운동해볼까요?";
        yield return new WaitForSeconds(1f);
        announcementText.text = "운동따위 뭐하러";
        yield return new WaitForSeconds(1f);
        announcementText.text = "오늘하루 기분이 아주 -_-";
        yield return new WaitForSeconds(1f);
        announcementText.text = "런하자! 그게 답이다!";
        yield return new WaitForSeconds(1f);
        announcementText.text = "귀닫고눈닫고입닫고";
        yield return new WaitForSeconds(1f);
        announcementText.text = "집으로 고고고고";
        yield return new WaitForSeconds(1f);
        announcementText.text = "최악이지..ㅉㅉ";
        yield return new WaitForSeconds(1f);

        StartCoroutine(AnnouncementTextShow());
    }


    public void SportChoiceClick()
    {
        if(sportChoiceToggleGroup.ActiveToggles().Any())
        {
            if(sportChoiceCurrentSelection.name.Equals("DanceRoutineToggle"))
            {
                choiceName = "DanceRoutineToggle";
            }
            else if(sportChoiceCurrentSelection.name.Equals("MuscularUpToggle"))
            {
                choiceName = "MuscularUpToggle";
            }
            else if(sportChoiceCurrentSelection.name.Equals("MuscularDownToggle"))
            {
                choiceName = "MuscularDownToggle";
            }
        }
    }

    public void SportStartButtonClick()
    {
        if(choiceName.Equals("DanceRoutineToggle"))
        {
            SceneManager.LoadScene("8.ExerciseScene");
        }
        else if (choiceName.Equals("MuscularUpToggle"))
        {
            SceneManager.LoadScene("8.ExerciseScene");
        }
        else if (choiceName.Equals("MuscularDownToggle"))
        {
            SceneManager.LoadScene("8.ExerciseScene");
        }
    }

}
