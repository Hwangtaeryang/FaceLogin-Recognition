using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI_Manager : MonoBehaviour
{
    public Image userFaceImg;   //�� �̹���
    public Toggle othersToggle; //��Ÿ���
    public GameObject othersObj;    //��Ÿ�ʵ������Ʈ



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
