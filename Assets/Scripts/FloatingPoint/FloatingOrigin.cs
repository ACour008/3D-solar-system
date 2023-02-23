// https://gist.github.com/brihernandez/9ebbaf35070181fa1ee56f9e702cc7a5

using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOrigin : MonoBehaviour
{
    public Transform player;
    public Vector3 distanceThreshold;

    public void FixedRefresh()
    {
        
        Vector3 playerPosition = player.position;
        bool isCrossedXThreshold = playerPosition.x > distanceThreshold.x || playerPosition.x < -distanceThreshold.x;
        bool isCrossedYThreshold = playerPosition.y > distanceThreshold.y || playerPosition.y < -distanceThreshold.y;
        bool isCrossedZThreshold = playerPosition.z > distanceThreshold.z || playerPosition.z < -distanceThreshold.z;

        if (isCrossedXThreshold || isCrossedYThreshold || isCrossedZThreshold)
        {
            for(int i = 0; i < SceneManager.sceneCount; i++)
            {
                foreach(GameObject rootObj in SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    if (rootObj.CompareTag("Player"))
                        rootObj.GetComponent<PlayerMovement>().OnPositionChange(playerPosition);
                    else if (rootObj.CompareTag("MainCamera"))
                        rootObj.GetComponent<SmoothFollow>()?.OnPositionChange(playerPosition);
                    else
                        rootObj.transform.position -= playerPosition;
                }
            }
        }
    }
}
