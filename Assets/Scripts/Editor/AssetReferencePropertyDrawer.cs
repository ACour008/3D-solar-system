using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AssetReference<>))]
public class AssetReferencePropertyDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var assetReference = property.GetValue<AssetReference>();
        var assetPathProperty = property.FindPropertyRelative("assetPath");

        var asset = AssetDatabase.LoadAssetAtPath(assetPathProperty.stringValue, typeof(Object));
        var newAsset = EditorGUI.ObjectField(position, "MeshObject", asset, assetReference.GetRequiredType(), property.serializedObject.targetObject);

        if (newAsset != asset)
        {
            if (newAsset)
            {
                assetPathProperty.stringValue = AssetDatabase.GetAssetPath(newAsset);
            }
            else
            {
                assetPathProperty.stringValue = null;
            }
        }
    }

}