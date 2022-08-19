using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Siccity.GLTFUtility;

public class ModelLoader : MonoBehaviour
{
    GameObject wrapper;
    string filePath;

    private void Start()
    {
        //Store files to a folder in appdata/locallow/organization/project/ called "Files"
        filePath = $"{Application.persistentDataPath}/Files/";
        wrapper = new GameObject
        {
            name = "Model"
        };
    }

    public void DownloadFile(string url) //Called with a url.
    {
        string path = GetFilePath(url);
        if (File.Exists(path))  //Check if we have the file locally already
        {
            Debug.Log("Found file locally, loading...");
            LoadModel(path);
            return;
        }

        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
        {
            if (req.result != UnityWebRequest.Result.ConnectionError && req.result != UnityWebRequest.Result.ProtocolError)
            {
                LoadModel(path);
            }
            else
            {
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
            }
        }));
    }

    // Use the URL Ending as file name
    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length -1];

        return $"{filePath}{filename}";
    }

    // Get the model data and animations from the GLTF importer
    void LoadModel(string path)
    {
        AnimationClip[] clips;
        var i = new ImportSettings();
        i.animationSettings.useLegacyClips = true;
        ResetWrapper();
        GameObject model = Importer.LoadFromFile(path,i , out clips);
        model.transform.SetParent(wrapper.transform);
        if (clips != null) //check if model data has animations and then play them
        {
            if (clips.Length > 0)
            {
                Animation animation = model.AddComponent<Animation>();
                animation.AddClip(clips[0], clips[0].name);
                animation.clip = animation.GetClip(clips[0].name);
                animation.Play();
                animation.wrapMode = WrapMode.Loop;
            }
        }
    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using UnityWebRequest req = UnityWebRequest.Get(url);
        {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));
            yield return req.SendWebRequest();
            callback(req);
        }
    }

    // clear existing wrapper
    void ResetWrapper()
    {
        if (wrapper != null)
        {
            foreach (Transform trans in wrapper.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }
}