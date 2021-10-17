using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig : MonoBehaviour
{

    public float
        Rc, Rs, Ra, Ravoid,
        Kc, Ks, Ka, Kw, Kavoid;

    public float maxA, maxV;
    public float maxViewAngle = 180;
    public float WanderJitter;
    public float WanderRadius;
    public float WanderDistance;
}
