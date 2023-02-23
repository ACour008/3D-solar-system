using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlacer : MonoBehaviour
{
    public GameObject prefab;
    public int numObjects;
    public Vector3 maxPosition;
    public float maxScale;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numObjects; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-maxPosition.x, maxPosition.x), Random.Range(-maxPosition.y, maxPosition.y), Random.Range(-maxPosition.z, maxPosition.z));
            GameObject asteroid = GameObject.Instantiate<GameObject>(prefab, randomPosition, Random.rotation);
            asteroid.transform.localScale = new Vector3(Random.Range(1, maxScale), Random.Range(1, maxScale), Random.Range(1, maxScale));
            asteroid.transform.SetParent(transform);
        }    
    }
}
