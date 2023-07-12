using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FaceShootManager : MonoBehaviour
{
    public SceneSoundCtrl soundCtrl;
    public Text timeText;
    public FaceShotHandler shotHandler;
    public Image timeRotateImg; //타임 돌아가는 이미지

    bool isRotate;

    void Start()
    {
        StartCoroutine(TimeCount());
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotate.Equals(true))
            timeRotateImg.transform.Rotate(new Vector3(0, 0, -0.5f));
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
        //FaceShotHandler.instance.TakeScreenShot_Static(600, 480);
        //soundCtrl.ShootSound(); //사진찍는 소리
        shotHandler.TakeScreenShot_Static(1024, 1024);
        yield return new WaitForSeconds(1);
        timeText.text = "1";
        yield return new WaitForSeconds(1);

        PictureInspectorChange();
        //SceneManager.LoadScene("2.LoginResult");
    }

    //사진 속성 변경
    void PictureInspectorChange()
    {
        StartCoroutine(_PictureInspectorChange());
    }

    IEnumerator _PictureInspectorChange()
    {
        AssetDatabase.Refresh();
        yield return new WaitForSeconds(1);

        string path = "Assets/Resources/Snapshots/SnapShot_User.png";// AssetDatabase.GetAssetPath(Selection.activeObject);
        TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        tImporter.textureType = TextureImporterType.Sprite; //타입변경
        tImporter.isReadable = true;    //읽기버전으로 변경
        AssetDatabase.ImportAsset(path);

        AssetDatabase.Refresh();

        yield return new WaitForSeconds(1f);

        //사진 처음 찍기 - 회원가입
        if(PlayerPrefs.GetString("EP_MyFaceChange").Equals("No"))
            SceneManager.LoadScene("4.UserInfo");
        //프로필수정에서 사진 새로찍기
        else
            SceneManager.LoadScene("6.UserProfileCorrect");
    }
}
