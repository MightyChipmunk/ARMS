using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Destroy : MonoBehaviour
{
    [SerializeField]
    float time = 2;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }

}
