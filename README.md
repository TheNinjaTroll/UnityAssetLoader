# UnityAssetLoader
Loading assets to Unity during runtime from the web in the form of 3d models or AssetBundles.

For Loading GLTF and GLB 3D-models it is required to download and install GLTF Utility to your Unity project from
https://github.com/Siccity/GLTFUtility

For creation of AssetBundles CreateAssetBundles.cs is included in the Editor folder. For more info how to create AssetBundles can be found from
https://learn.unity.com/tutorial/introduction-to-asset-bundles#

How to Use:

1: Add files to existing Unity project. (JustForeStructure.txt files can be removed)

2: Create a empty gameobject in the scene and drag the Loader scripts to that objects inspector. AssetBundleLoader.cs is needed for asset bundles and ModelLoader.cs is needed for .gltf / .gnb models.

3: Both scripts have a public method(GetBundle, DownloadFile) that requires a string that is the url of the file you wish to download. The AssetBundleLoader also requires that you have already build the StreamingAssets.manifest file.

4: Currently AssetBundleLoader.cs automatically plays the 1st scene found in the bundle, if no scene files are found it instantiates all gameobjects instead.

5: Downloaded AssetBundles are cached uncompressed in to a folder called "DLC" inside your unity project folder. Downloaded 3d files are stored to AppData/LocalLow/*Unity Company name*/*project name*/Files .
