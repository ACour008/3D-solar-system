using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeightMapGenerator: HeightGenerator
{
    public override float GenerateHeightValue(float x, float y, TerrainOptions options)
    {
            Texture2D heightMap = options.heightMapOptions.heightMap;
            int row = Mathf.FloorToInt((float)x / options.sizeOptions.width * heightMap.width);
            int col = Mathf.FloorToInt((float)y / options.sizeOptions.height * heightMap.height);

            float remainderX = row - (float)x;
            float remainderY = col - (float)y;

            float x1 = heightMap.GetPixel(row, col).r;
            float x2 = heightMap.GetPixel(row + 1, col).r;
            float y1 = heightMap.GetPixel(row, col+1).r;
            float y2 = heightMap.GetPixel(row+1, col+1).r;

            return Mathf.Lerp(
                Mathf.Lerp(x1, x2, remainderX),
                Mathf.Lerp(y1, y2, remainderX),
                remainderY
            );
    }
}
