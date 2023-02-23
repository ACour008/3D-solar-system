using UnityEngine;

[System.Serializable]
public class PerlinNoiseGenerator: HeightGenerator
{
    public override float GenerateHeightValue(float x, float y, TerrainOptions options)
    {
        PerlinNoiseOptions opts = options.noiseOptions;

        float xSample = x / opts.scale;
        float ySample = y / opts.scale;
        float amplitude = 1.0f;
        float frequency = 1.0f;
        float normalization = 0;
        float noiseHeight = 0;

        for (int o = 0; o < opts.octaves; o++)
        {
            float noiseValue = Mathf.PerlinNoise(xSample * frequency, ySample * frequency) * 2f - 1f;
            noiseHeight += noiseValue * amplitude;
            normalization += amplitude;
            amplitude *= opts.persistence;
            frequency *= opts.lacunarity;
        }

        noiseHeight /= normalization;
        return noiseHeight * opts.height;
    }
}
