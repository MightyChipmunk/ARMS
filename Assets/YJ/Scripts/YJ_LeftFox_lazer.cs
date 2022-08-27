using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_LeftFox_lazer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            yj_leftfox.distance = 0;
            yj_leftfox.lazerOn = false;
            yj_leftfox.scaleDown = true;

        }
    }

    YJ_LeftFox yj_leftfox;
    
    void Start()
    {
        yj_leftfox = GameObject.Find("Left").GetComponent<YJ_LeftFox>();
    }
}
