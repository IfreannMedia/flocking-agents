using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Agent
{

    override protected Vector3 combine()
    {
        return conf.Kw * wander();
    }
}
