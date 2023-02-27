using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public bool debug;
    public TerrainChunk chunkData;
    Dictionary<string, ChunkGenerator> chunks;

    ChunkGenerator currentChunk;

    Camera mainCamera;

    void Awake()
    {
        chunks = new Dictionary<string, ChunkGenerator>();
    }

    void Start()
    {
        var chunkGameObject = GameObject.Instantiate<GameObject>(AssetBundles.LoadAsset<GameObject>(chunkData.meshObject));
        var chunkGenerator = chunkGameObject.GetComponent<ChunkGenerator>();
        currentChunk = chunkGenerator;

        currentChunk.GenerateMesh(chunkData);
        string key = currentChunk.transform.position.x + "." + currentChunk.transform.position.z;
        chunks.Add(key, currentChunk);
    }

    void Update()
    {
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && debug)
        {
            foreach(KeyValuePair<string, ChunkGenerator> kv in chunks)
            {
                foreach(Vector3 vertex in kv.Value.vertices)    
                {
                    Gizmos.DrawSphere(vertex, 0.1f);
                }
            }
        }
    }
}
