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
        announcementText.text = "���õ� ������ ��غ����?";
        yield return new WaitForSeconds(1f);
        announcementText.text = "����� ���Ϸ�";
        yield return new WaitForSeconds(1f);
        announcementText.text = "�����Ϸ� ����� ���� -_-";
        yield return new WaitForSeconds(1f);
        announcementText.text = "������! �װ� ���̴�!";
        yield return new WaitForSeconds(1f);
        announcementText.text = "�ʹݰ��ݰ��Դݰ�";
        yield return new WaitForSeconds(1f);
        announcementText.text = "������ �����";
        yield return new WaitForSeconds(1f);
        announcementText.text = "�־�����..����";
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
