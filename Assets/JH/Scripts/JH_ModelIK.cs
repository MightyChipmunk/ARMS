using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ModelIK : MonoBehaviour
{
    
    GameObject target;
    Animator anim;

    private void Start()
    {
        target = GameObject.Find("Podium").transform.Find("PodiumCam").gameObject;
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(1);
        anim.SetLookAtPosition(target.transform.position);
    }
}
