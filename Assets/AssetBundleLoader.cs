using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AssetBundleLoader : MonoBehaviour
{

    public void LoadBundle (string url) //called from a button with a url.
    {
        StartCoroutine(GetBundle(url));
    }

    IEnumerator GetBundle(string url)   // Should be made cleaner in future. Add asset bundle manifest for assetbunde management.
    {
        //Load the projects main assetbundle manifest
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "StreamingAssets"));
        AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //Create cache directory
        string directory = "DLC";
        Directory.CreateDirectory(directory);
        Cache newCache = Caching.AddCache(directory);

        //Set current cache for writing to the new cache if the cache is valid
        if (newCache.valid)
            Caching.currentCacheForWriting = newCache;

        // Download the bundle, with the hash parameter UnityWebRequest automatically checks if we have matching file cached locally.
        Hash128 hash = manifest.GetAssetBundleHash("testbundle");
        using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(url,hash,0))
        {
            yield return req.SendWebRequest();

            //check for http errors.
            if (req.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(req.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(req);

                // Get all cached versions. Should be expanded on if we want to check for a conditional on our cached bundles.
                List<Hash128> listOfCachedVersion = new List<Hash128>();
                Caching.GetCachedVersions(bundle.name, listOfCachedVersion);

                //Check if the AssetBundle contains any scenes, then loads one.
                if (bundle.isStreamedSceneAssetBundle) 
                {
                    string[] scenePaths = bundle.GetAllScenePaths();
                    string sceneName = Path.GetFileNameWithoutExtension(scenePaths[0]);
                    SceneManager.LoadScene(sceneName);
                }
                else // if not we instantiate all game objects instead.
                {
                    GameObject[] prefabs = bundle.LoadAllAssets<GameObject>(); //should be changed to async if handling large bundle.
                    foreach (GameObject prefab in prefabs)
                    {
                        Instantiate(prefab);
                    }
                }
                 
                bundle.Unload(false); //Unloads the assetbundle from memory while keeping instantiated objects on scene.
            }
        }
    }

}