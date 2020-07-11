using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTCL_IgnoreTimescale : MonoBehaviour
{

    public ParticleSystem ps;


    void Update()
    {
        ps.Simulate(Time.unscaledDeltaTime, true, false);
    }
}
