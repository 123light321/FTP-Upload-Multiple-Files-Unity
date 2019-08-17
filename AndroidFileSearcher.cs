using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidFileSearcher: MonoBehaviour
{
    public List<string> files;
    public Queue queueDir;
    public Uploader uploader;
    public float timeToWaitBeforeUploading;
    private string fileTypeToFind; // example .png .txt


    void Start()
    {
        //Assign the variable "uploader" here or in unity 
        queueDir = new Queue();

        //By Default
        fileTypeToFind = ".png";
        timeToWaitBeforeUploading = 15.0f;
		
		//Call this method where you want
        uplaodListOfFiles();
    }

    public void GetFiles()
    {
        string path = "";
        try
        {
            //For getting root location of sd memory of phone
            AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
            path = jc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");

            //Searches files in the root folder
            string[] tempRoot = Directory.GetFiles(Application.dataPath);
            for (int j = 0; j < tempRoot.Length; j++)
            {
                if (Path.GetExtension(tempRoot[j]) == fileTypeToFind)
                {
                    files.Add(tempRoot[j]);
                }
            }

            //Searches files in the sub folders
            do
            {
                string[] s = Directory.GetDirectories(path);
                for (int i = 0; i < s.Length; i++)
                {
                    queueDir.Enqueue(s[i]);
                }
                path = (string)queueDir.Dequeue();

                string[] subRootPaths = Directory.GetFiles(path);
                for (int j = 0; j < subRootPaths.Length; j++)
                {
                    if (Path.GetExtension(subRootPaths[j]) == fileTypeToFind)
                    {
                        files.Add(subRootPaths[j]);
                    }
                }
            } while (queueDir.Count > 0);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void uplaodListOfFiles()
    {
        StartCoroutine(WaitToFinishSearching());
    }

    IEnumerator WaitToFinishSearching()
    {
        GetFiles();
        yield return new WaitForSeconds(timeToWaitBeforeUploading);
        uploader.FTPUpload(files);
    }

}

