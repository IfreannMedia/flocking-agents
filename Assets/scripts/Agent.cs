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
        v = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }

    /**
     * the update method moves the agent according to some equations, 
     * calculating the position, velocity, and acceleration
     * **/
    void Update()
    {
        float t = Time.deltaTime;

        a = combine();
        a = Vector3.ClampMagnitude(a, conf.maxA);

        v = v + a * t;
        v = Vector3.ClampMagnitude(v, conf.maxV);

        x = x + v * t;


        wrapAround(ref x, -world.bounds, world.bounds);
        transform.position = x;

        if(v.magnitude > 0)
        transform.LookAt(x + v); // look at our position + velocity, a point just in front of us
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
        r = Vector3.Normalize(r);
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
        // return direciton without magnitude
        return seperation.normalized;
    }

    public Vector3 allignment()
    {
        Vector3 allignment = new Vector3();

        var neighbours = world.getNeigh(this, conf.Ra);

        foreach (var agent in neighbours)
        {
            allignment += agent.v;
        }

        // return direction without magnitude
        return allignment.normalized;
    }

    public Vector3 combine()
    {
        Vector3 combine = new Vector3();
        Vector3 a = allignment();
        Vector3 c = cohesion();
        Vector3 s = seperation();

        combine = conf.Kc * c + conf.Ks * s + conf.Ka * a;
        return combine;
    }


    public void wrapAround(ref Vector3 v, float min, float max)
    {
        v.x = wrapAroundFloat(v.x, min, max);
        v.y = wrapAroundFloat(v.y, min, max);
        v.z = wrapAroundFloat(v.z, min, max);
    }

    private float wrapAroundFloat(float value, float min, float max)
    {
        return value > max ? min : value < min ? max : value;
    }
}
