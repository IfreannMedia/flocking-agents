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
}
