using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Linq;
using UnityEditor;

public class CSVFile : MonoBehaviour
{
    public static CSVFile instance { get; private set; }

    public List<Face> SearchableFaces = new List<Face>();
    List<Dictionary<string, object>> data;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        
    }

    public void GetFaceData(List<Face> _SearchableFaces)
    {
        data = CSVReader.Read("Student Data");

        for (var i = 0; i < data.Count; i++)
        {
            Debug.Log("index " + (i).ToString() + " : " + data[i]["�̸�"] + " " + data[i]["�������"] + " " + data[i]["����"]);
            //string str = data[i]["�̸�"].ToString();
            //Debug.Log("str " + str);

            _SearchableFaces.Add(new Face(data[i]["�̸�"].ToString(), Color.green, data[i]["�̸�"].ToString(),
                data[i]["�������"].ToString(), data[i]["����"].ToString()));
        }
    }

    public void SaveInventory()
    {
        Debug.Log("�ȵ�����");
        string filePath = getPath();

        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter(filePath);
        Debug.Log("����");
        //This is writing the line of the type, name, damage... etc... (I set these)
        writer.WriteLine("�̸�,�������,����");
        //This loops through everything in the inventory and sets the file to these.
        Debug.Log("data.Count: " + data.Count);

        for (int i = 0; i < data.Count+1; ++i)
        {
            Debug.Log("-----" + (data.Count - 1));
            if (i <= data.Count-1)
            {
                Debug.Log("��������� ???");
                writer.WriteLine(data[i]["�̸�"] +
                "," + data[i]["�������"] +
                "," + data[i]["����"]);
            }
            else if(i.Equals(data.Count))
            {
                writer.WriteLine("ȫ�浿" +
                "," + "20001212" +
                "," + "����");
            }
        }
        writer.Flush();
        //This closes the file
        writer.Close();

        
        AssetDatabase.Refresh();
        data = CSVReader.Read("Student Data");
    }

    public void UserDataSvae()
    {
        string filePath = getPath();

        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter(filePath);

        //����� ������ �Ѹ� ������
        if(data.Count.Equals(0))
        {
            //writer.WriteLine
        }
        else
        {

        }
    }

    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + "Student Data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Student Data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Student Data.csv";
#else
        return Application.dataPath +"/"+"Student Data.csv";
#endif
    }
}
