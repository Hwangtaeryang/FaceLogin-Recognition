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
    public Image timeRotateImg; //Ÿ�� ���ư��� �̹���

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
        //soundCtrl.ShootSound(); //������� �Ҹ�
        shotHandler.TakeScreenShot_Static(1024, 1024);
        yield return new WaitForSeconds(1);
        timeText.text = "1";
        yield return new WaitForSeconds(1);

        PictureInspectorChange();
        //SceneManager.LoadScene("2.LoginResult");
    }

    //���� �Ӽ� ����
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
        tImporter.textureType = TextureImporterType.Sprite; //Ÿ�Ժ���
        tImporter.isReadable = true;    //�б�������� ����
        AssetDatabase.ImportAsset(path);

        AssetDatabase.Refresh();

        yield return new WaitForSeconds(1f);

        //���� ó�� ��� - ȸ������
        if(PlayerPrefs.GetString("EP_MyFaceChange").Equals("No"))
            SceneManager.LoadScene("4.UserInfo");
        //�����ʼ������� ���� �������
        else
            SceneManager.LoadScene("6.UserProfileCorrect");
    }
}
