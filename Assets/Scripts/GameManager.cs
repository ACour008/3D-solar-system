using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FloatingOrigin floatingOrigin;
    public PlayerMovement playerMovement;
    public SmoothFollow smoothFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        playerMovement.LateRefresh();
        smoothFollow.LateRefresh();
    }

    void FixedUpdate()
    {
        floatingOrigin.FixedRefresh();
    }
}
