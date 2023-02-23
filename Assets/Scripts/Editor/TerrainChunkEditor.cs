using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainChunk))]
public class TerrainChunkEditor : Editor
{
    TerrainChunk _terrainChunk;
    SerializedProperty meshObject;
    SerializedProperty terrainOptions;
    SerializedProperty generatorType;


    private void OnEnable()
    {
        _terrainChunk = (TerrainChunk)serializedObject.targetObject;
        meshObject = serializedObject.FindProperty("meshObject");
        generatorType = serializedObject.FindProperty("heightGeneratorType");
        terrainOptions = serializedObject.FindProperty("options");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(meshObject);
        EditorGUILayout.PropertyField(generatorType);
        EditorGUILayout.PropertyField(terrainOptions.FindPropertyRelative("sizeOptions"));

        if (_terrainChunk.heightGeneratorType == HeightGeneratorType.HeightMap)
        {
            EditorGUILayout.PropertyField(terrainOptions.FindPropertyRelative("heightMapOptions"));
        }

        if(_terrainChunk.heightGeneratorType == HeightGeneratorType.PerlinNoise)
        {
            EditorGUILayout.PropertyField(terrainOptions.FindPropertyRelative("noiseOptions"));
        }

        if (_terrainChunk.heightGeneratorType == HeightGeneratorType.SimplexNoise)
        {
            EditorGUILayout.LabelField("This has not yet been implemented");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
