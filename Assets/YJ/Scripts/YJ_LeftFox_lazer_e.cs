using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_LeftFox_lazer_e : MonoBehaviour
{
    public bool triggerOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            triggerOn = true;
        }
    }

    
    void Start()
    {
        
    }
}
