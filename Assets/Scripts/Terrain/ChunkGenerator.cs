using UnityEngine;

public class ChunkGenerator: MonoBehaviour
{
    private HeightGenerator generator;
    private TerrainChunk chunkData;

    Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;

    void SelectGenerator()
    {
        switch(chunkData.heightGeneratorType)
        {
            case HeightGeneratorType.HeightMap:
                generator = new HeightMapGenerator();
                break;
            case HeightGeneratorType.ValueNoise:
                generator = new ValueNoiseGenerator();
                break;
            case HeightGeneratorType.PerlinNoise:
                generator = new PerlinNoiseGenerator();
                break;
        }
    }

    public void GenerateMesh(TerrainChunk data)
    {
        chunkData = data;

        SelectGenerator();
        GenerateMesh();
    }

    void GenerateMesh()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        
        int width = chunkData.options.sizeOptions.width;
        int height = chunkData.options.sizeOptions.height;
        float cellSize = chunkData.options.sizeOptions.cellSize;

        mesh.vertices = CreateVertices(width, height, cellSize);
        mesh.triangles = CreateTriangles(width, height);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        vertices = mesh.vertices;
        triangles = mesh.triangles;
    }

    Vector3[] CreateVertices(int width, int height, float size)
    {
        int w = width + 1;
        int h = height + 1;
        float halfCellSize = size * 0.5f;
        Vector3[] points = new Vector3[w * h];

        for (int i = 0; i < (w * h); i++)
        {
            float x = i % w;
            float z = i / w;

            points[i] = new Vector3(
                ((x * size) - halfCellSize) - width / 2, 
                generator.GenerateHeightValue(x, z, chunkData.options),
                ((z * size) - halfCellSize) - height / 2) + chunkData.options.sizeOptions.offset;
        }

        return points;
    }



    int[] CreateTriangles(int width, int height)
    {
        int[] triangles = new int[6 * width * height];
        int i = -1;

        for(int j = 0; j < (width * height) + width; j++)
        {
            bool isFirstVertex = j == 0;
            bool isLastColumnVertex = j % (width + 1) == width;

            if (isFirstVertex || !isLastColumnVertex)
            {
                triangles[++i] = j;
                triangles[++i] = j + width + 1;
                triangles[++i] = j + 1;

                triangles[++i] = j + 1;
                triangles[++i] = j + width + 1;
                triangles[++i] = j + width + 2;
            }
        }

        return triangles;
    }
}
