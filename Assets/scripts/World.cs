using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Transform agentPrefab;

    public int nAgents;
    public List<Agent> agents;
    void Start()
    {
        this.spawn(agentPrefab, nAgents);
        agents.AddRange(FindObjectsOfType<Agent>());
    }

    void Update()
    {
        
    }

    public void spawn(Transform prefab, int n)
    {
        for (int i = 0; i < n; i++)
        {
            var obj = Instantiate(prefab,
                new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                Quaternion.identity);
        }
    }

    /**
     * get the agents within the interest zone/radius of param agent
     */
    public List<Agent> getNeigh(Agent agent, float radius)
    {
        List<Agent> neighbours = new List<Agent>();

        foreach (var possibleNeihbour in agents)
        {
            if (possibleNeihbour == agent)
                continue;
            if(Vector3.Distance(agent.x, possibleNeihbour.x) > radius)
            {
                neighbours.Add(possibleNeihbour);
            }
        }

        return neighbours;
    }

}
