using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum HeightGeneratorType
{
    HeightMap,
    ValueNoise,
    PerlinNoise,
    SimplexNoise
}

[Serializable]
public class TerrainSizeOptions
{
    public int width;
    public int height;
    public float cellSize;
    public Vector3 offset;
}

[Serializable]
public class HeightMapOptions
{
    public Texture2D heightMap;
}

[Serializable]
public class PerlinNoiseOptions
{
    public float scale;
    public float octaves;
    public float persistence;
    public float lacunarity;
    public float height;
}

[Serializable]
public class TerrainOptions
{
    public TerrainSizeOptions sizeOptions;
    public HeightMapOptions heightMapOptions;
    public PerlinNoiseOptions noiseOptions;
}

[CreateAssetMenu(fileName = "TerrainChunk", menuName = "Terrain/Terrain Chunk")]
public class TerrainChunk : ScriptableObject
{
    public AssetReference<GameObject> meshObject;
    public HeightGeneratorType heightGeneratorType;
    public TerrainOptions options;

}
