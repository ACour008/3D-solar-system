
[System.Serializable]
public abstract class HeightGenerator: UnityEngine.Object
{
    public abstract float GenerateHeightValue(float x, float y, TerrainOptions options);
}
