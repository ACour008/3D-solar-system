using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AssetReference
{
    public abstract Type GetRequiredType();
}

[System.Serializable]
public class AssetReference<T> : AssetReference
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
    where T : UnityEngine.Object
{
    public string assetPath;
    public string assetBundle;

    public override Type GetRequiredType() => typeof(T);

#if UNITY_EDITOR
    public T editorAsset => AssetDatabase.LoadAssetAtPath<T>(assetPath);

    public void OnBeforeSerialize()
    {
        if(string.IsNullOrEmpty(assetPath))
        {
            assetBundle = null;
            assetPath = null;
        }
        else
        {
            var importer = AssetImporter.GetAtPath(assetPath);
            if (!importer)
                Debug.LogError($"Couldn't find importer for {assetPath}");
            else
               assetBundle = importer.assetBundleName;
        }
    }

    public void OnAfterDeserialize()
    {
    }
#endif
}
