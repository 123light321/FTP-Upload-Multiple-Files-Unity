using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;


public class Uploader : MonoBehaviour
{
    private string FTPHost;
    private string FTPUserName;
    private string FTPPassword;
    private string InnerPath;

    void Start()
    {
        FTPHost = "ftp://ftpupload.net"; //example "ftp://website.com" or IP address
        FTPUserName = "epiz_24201306"; //example "username"
        FTPPassword = "pJSaxY2UCOEU"; //example "password"
        InnerPath = "/htdocs/"; //example "/folder/"
    }

    public void FTPUpload(List<string> files)
    {

        string path = "";
        for (int i = 0; i < files.Count; i++)
        {
            try
            {
                path = files[i];

                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(FTPHost + InnerPath + new FileInfo(path).Name);
                ftp.Credentials = new NetworkCredential(FTPUserName, FTPPassword);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(path);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    void OnFileUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
    {
        Debug.Log("Uploading Progreess: " + e.ProgressPercentage);
    }

    void OnFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
    {
        Debug.Log("File Uploaded");
    }

}