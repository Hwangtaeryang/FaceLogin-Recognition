using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI_Manager : MonoBehaviour
{
    public Image userFaceImg;   //얼굴 이미지
    public Toggle othersToggle; //기타토글
    public GameObject othersObj;    //기타필드오브젝트



    void Start()
    {
        userFaceImg.sprite = Resources.Load<Sprite>("Snapshots/SnapShot_User");
    }

    // Update is called once per frame
    void Update()
    {
        if(othersToggle.isOn.Equals(true))
        {
            othersObj.SetActive(true);
        }
        else
        {
            othersObj.SetActive(false);
        }
    }
}
