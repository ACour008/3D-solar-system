using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueNoiseGenerator: HeightGenerator
{
    private Dictionary<string, float> values;

    public ValueNoiseGenerator()
    {
        values = new Dictionary<string, float>();
    }

    private float GetRandomValue(float x, float y)
    {
        string key = x + "." + y;
        if (!values.ContainsKey(key))
        {
            values[key] = Random.value;
        }

        return values[key];
    }

    public override float GenerateHeightValue(float x, float y, TerrainOptions options)
    {
        float width = options.sizeOptions.width;
        float p11 = GetRandomValue(x, y);
        float p21 = GetRandomValue(x, y);
        float p12 = GetRandomValue(x, y);
        float p22 = GetRandomValue(x, y);

        return Mathf.Lerp(
            Mathf.Lerp(p11, p21, x / width),
            Mathf.Lerp(p12, p22, y / width),
            options.sizeOptions.height
        );

    }
}