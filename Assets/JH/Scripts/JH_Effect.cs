using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Effect : MonoBehaviour
{
    JH_PlayerMove pm;
    SY_LeftCharge lc;

    GameObject dash;
    GameObject guard;

    bool trail = false;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<JH_PlayerMove>();
        //lc = transform.Find("Left").GetComponent<SY_LeftCharge>();
        dash = transform.Find("Dash").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        DashTrail();
    }

    public void DashEffect(bool play)
    {
        if (play)
            dash.transform.Find("Particle").GetComponent<ParticleSystem>().Play();
        else
            dash.transform.Find("Particle").GetComponent<ParticleSystem>().Stop();
    }

    public void DashTrail()
    {
        if (pm.IsDash && !trail)
        {
            dash.transform.Find("Trail").GetComponent<ParticleSystem>().Play();
            trail = true;
        }
        else if (!pm.IsDash)
        {
            dash.transform.Find("Trail").GetComponent<ParticleSystem>().Stop();
            trail = false;
        }
    }
}
