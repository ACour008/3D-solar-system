using UnityEngine;
using UnityEditor;
using System.IO;

public static class AssetBundles
{

    private static readonly string BUNDLE_PATH = "Assets/AssetBundles/";
#if UNITY_EDITOR
    [MenuItem("Build/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
#endif

    public static T LoadAsset<T>(AssetReference<T> assetReference) where T : UnityEngine.Object
    {
        var bundle = LoadBundle(assetReference.assetBundle);
        if (bundle == null)
        {
            Debug.Log("Failed to load bundle");
            return null;
        }

        return bundle.LoadAsset<T>(assetReference.assetPath);
    }

    private static AssetBundle LoadBundle(string bundleName)
    {
        return AssetBundle.LoadFromFile(Path.Combine(BUNDLE_PATH, bundleName));
    }
}
