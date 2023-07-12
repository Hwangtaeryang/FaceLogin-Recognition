using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FaceShotHandler : MonoBehaviour
{
    //public static FaceShotHandler instance { get; private set; }

    Camera myCamera;
    WebCamTexture camTexture;
    public RawImage display;    
    int currentIndex = 0;

    bool takeScreenShotOnNextFrame;
    RenderTexture renderTexture;


    private void Awake()
    {
        //if (instance != null)
        //    Destroy(this);
        //else instance = this;

        myCamera = gameObject.GetComponent<Camera>();
    }

    void Start()
    {
        WebCamShow();
    }

    //ȭ�鿡 �� ������ �ϴ� �Լ�
    void WebCamShow()
    {
        if (camTexture != null)
        {
            Debug.Log("����");
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }

        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name);
        display.texture = camTexture;
        Debug.Log("����????");
        camTexture.Play();
    }

    private void OnPostRender()
    {
        if (takeScreenShotOnNextFrame)
        {
            takeScreenShotOnNextFrame = false;
            renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height,
                TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);


            byte[] byteArray = renderResult.EncodeToPNG();
            string fileName = SnapshotName();
            //����Ƽ �� ����
            //System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/Snapshots/SnapShot_" + SnapshotName(), byteArray); //Application.dataPath + "/Test.png"
            
            //�ܺ�����
            System.IO.File.WriteAllBytes(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "'\'Zoom" + SnapshotName(), byteArray);
            Debug.Log("Saved Camera");

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

    //���� ��� �Լ�
    public void TakeScreenShot_Static(int _width, int _height)
    {
        TakeScreenShot(_width, _height);
//#if UNITY_EIDTOR
        AssetDatabase.Refresh();    //���ΰ�ħ(f5����)
//#endif
    }

    private void OnDestroy()
    {
        camTexture?.Stop();
    }
}
