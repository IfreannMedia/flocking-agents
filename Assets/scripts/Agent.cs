using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    public Vector3 x, v, a;

    public World world;
    void Start()
    {
        world = FindObjectOfType<World>();
    }

    void Update()
    {
        
    }

    public Vector3 cohesion()
    {
        return Vector3.zero;
    }

    public Vector3 seperation()
    {
        return Vector3.zero;
    }

    public Vector3 allignment()
    {
        return Vector3.zero;
    }

    public Vector3 combine()
    {
        return Vector3.zero;
    }
}
