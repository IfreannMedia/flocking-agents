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
    //public GameObject debugWanderCube = new GameObject();
    void Start()
    {
        world = FindObjectOfType<World>();
        x = transform.position;

        conf = FindObjectOfType<AgentConfig>();
        v = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

        //debugWanderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
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

        if (v.magnitude > 0)
            transform.LookAt(x + v); // look at our position + velocity, a point just in front of us
    }

    public Vector3 cohesion()
    {
        Vector3 r = new Vector3();

        List<Agent> neighbours = world.getNeigh(this, conf.Rc);

        int neighboursCount = 0;
        foreach (var agent in neighbours)
        {
            if (isInFieldOfView(agent.transform.position))
            {
                r += agent.x;
                neighboursCount++;
            }
        }

        if (neighboursCount == 0)
            return r;
        r /= neighboursCount;

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
            if (isInFieldOfView(agent.transform.position))
            {

                // compute arrow/vector from neightbour to ourselves:
                Vector3 towardsMe = this.x - agent.x;

                if (towardsMe.magnitude > 0)
                {

                    seperation += towardsMe.normalized / towardsMe.magnitude;
                }
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
            if (isInFieldOfView(agent.transform.position))
                allignment += agent.v;
        }

        // return direction without magnitude
        return allignment.normalized;
    }

    virtual protected Vector3 combine()
    {
        return conf.Kc * cohesion() 
            + conf.Ks * seperation()
            + conf.Ka * allignment() 
            + conf.Kw * wander()
            + conf.Kavoid * avoidEnemeies();
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

    private bool isInFieldOfView(Vector3 obj)
    {
        return Vector3.Angle(this.v, obj - this.x) <= conf.maxViewAngle;
    }

    Vector3 wanderTarget; 

    protected Vector3 wander()
    {
        float jitter = conf.WanderJitter * Time.deltaTime;

        wanderTarget += new Vector3(RandomBinomial() * jitter, 0, RandomBinomial() * jitter);
        wanderTarget = wanderTarget.normalized;

        wanderTarget *= conf.WanderRadius;

        Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, 0, conf.WanderDistance);
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        //debugWanderCube.transform.position = targetInWorldSpace;

        targetInWorldSpace -= this.x;
        return targetInWorldSpace.normalized;

    }


    public float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    virtual protected Vector3 avoidEnemeies()
    {
        Vector3 avoid = this.transform.forward;

        var enemies = world.getPredators(this, conf.Ravoid);

        foreach (var enemy in enemies)
        {
            avoid += flee(enemy.x);
        }

        return avoid.normalized;
    }

    public Vector3 flee(Vector3 target)
    {
        Vector3 desiredVel = (x - target).normalized * conf.maxV;

        return desiredVel - v;
    }
}
