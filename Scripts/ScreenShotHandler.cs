using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenShotHandler : MonoBehaviour
{
    static ScreenShotHandler instance;

    public GameObject profilePopup;
    public Image profileImg;
    public InputField userNameText;

    Camera myCamera;
    bool takeScreenShotOnNextFrame;
    RenderTexture renderTexture;

    public RawImage display;
    WebCamTexture camTexture;
    int currentIndex = 0;


    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    void Start()
    {
        WebCamShow();
    }

    void WebCamShow()
    {
        if (camTexture != null)
        {
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }

        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name);
        display.texture = camTexture;
        camTexture.Play();
    }
    bool click;
    private void Update()
    {
        if(click.Equals(true))
        {
            camTexture.Stop();
            Debug.Log("왜");
        }
    }

    private void OnPostRender()
    {
        if(takeScreenShotOnNextFrame)
        {
            takeScreenShotOnNextFrame = false;
            renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height,
                TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            
            byte[] byteArray = renderResult.EncodeToPNG();
            string fileName = SnapshotName();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/Snapshots/SnapShot_" + SnapshotName(), byteArray); //Application.dataPath + "/Test.png"
            Debug.Log("Saved Camera");
            click = true;
            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }


    string SnapshotName()
    {
        //return string.Format("{0}/Snapshots/snap_{1}x{2}_{3}.png",
        //    Application.dataPath,
        //    renderTexture.width,
        //    renderTexture.height,
        //    System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));

        string today = "User";//System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        return today + ".png";
    }

    void TakeScreenShot(int _width, int _height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(_width, _height, 16);
        takeScreenShotOnNextFrame = true;
    }
    public static void TakeScreenShot_Static(int _width, int _height)
    {
        instance.TakeScreenShot(_width, _height);
//#if UNITY_EIDTOR
        AssetDatabase.Refresh();
//#endif
    }

    //이름 변경
    public void PictureNameChange()
    {
        
        StartCoroutine(_PictureNameChange());

        SceneManager.LoadScene("1.Login");
    }

    IEnumerator _PictureNameChange()
    {
        //AssetDatabase.RenameAsset("Assets/Resources/Snapshots/SnapShot_User.png", "SnapShot_User");
        AssetDatabase.Refresh();
        yield return new WaitForSeconds(1);
        
        profilePopup.SetActive(true);
        AssetDatabase.Refresh();
        OnPreprocessTexture();
        //OnPreprocessTexture();
        //AssetDatabase.RenameAsset("Assets/Snapshots/SnapShot_User.png", "TEST");
        //AssetDatabase.Refresh();
    }

    //팝업에 이미지 올리기
    void OnPreprocessTexture()
    {
        string path = "Assets/Resources/Snapshots/SnapShot_User.png";// AssetDatabase.GetAssetPath(Selection.activeObject);
        TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        tImporter.textureType = TextureImporterType.Sprite; //타입변경
        tImporter.isReadable = true;    //읽기버전으로 변경
        AssetDatabase.ImportAsset(path);

        profileImg.sprite = Resources.Load<Sprite>("Snapshots/SnapShot_User");
    }

    //사용자 정보 저장 버튼 클릭 시 이벤트
    public void UserInfoDataSvae()
    {
        AssetDatabase.RenameAsset("Assets/Resources/Snapshots/SnapShot_User.png", userNameText.text);
        AssetDatabase.Refresh();
        profilePopup.SetActive(false);

    }
}
