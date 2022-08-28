using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_RightFox_lazer : MonoBehaviour
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
