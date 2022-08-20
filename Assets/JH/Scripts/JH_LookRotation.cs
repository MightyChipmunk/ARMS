using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_LookRotation : MonoBehaviour
{
    public Transform lookRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lookRotation != null)
            transform.rotation = Quaternion.LookRotation(lookRotation.position - transform.position + Vector3.up * 1.1f + Vector3.right); 
    }
}
