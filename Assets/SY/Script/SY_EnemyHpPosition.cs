using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_EnemyHpPosition : MonoBehaviour
{
    Quaternion rot;
    // Start is called before the first frame update
    void Start()
    {
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation * rot;
    }
}
