using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    public Vector3 
        x, // position 
        v, // velocity
        a; // accelaration

    public World world;
    public AgentConfig conf;
    void Start()
    {
        world = FindObjectOfType<World>();
        x = transform.position;

         conf = FindObjectOfType<AgentConfig>();


    }

    /**
     * the update method moves the agent according to some equations, 
     * calculating the position, velocity, and acceleration
     * **/
    void Update()
    {
        float t = Time.deltaTime;

        //a = combine();
        a = seperation();
        a = Vector3.ClampMagnitude(a, conf.maxA);

        v = v + a * t;
        v = Vector3.ClampMagnitude(v, conf.maxV);

        x = x + v * t;
         
        transform.position = x;
    }

    public Vector3 cohesion()
    {
        Vector3 r = new Vector3();

        List<Agent> neighbours = world.getNeigh(this, conf.Rc);
        if (neighbours.Count == 0)
            return r;

        foreach (var agent in neighbours)
        {
            r += agent.x;
        }

        r /= neighbours.Count;

        r -= this.x;
        Vector3.Normalize(r);
        return r;
    }

    public Vector3 seperation()
    {
        Vector3 seperation = new Vector3();
        var agents = world.getNeigh(this, conf.Rs);

        if (agents.Count == 0)
            return seperation;

        foreach (var agent in agents)
        {
            // compute arrow/vector from neightbour to ourselves:
            Vector3 towardsMe = this.x - agent.x;

            if(towardsMe.magnitude > 0)
            {
                seperation += towardsMe.normalized / towardsMe.magnitude;
            }

        }
        return seperation.normalized;
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
