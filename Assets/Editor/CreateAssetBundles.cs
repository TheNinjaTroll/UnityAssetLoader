using UnityEngine;
using UnityEditor;
using System.IO;

/* Create folder called "Editor" inside Assets folder then place this script inside the Editor Folder. 
 * https://learn.unity.com/tutorial/introduction-to-asset-bundles# */

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        // BuildSettings should be changed if assetbundle is used for different platform than current BuildTarget.
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
