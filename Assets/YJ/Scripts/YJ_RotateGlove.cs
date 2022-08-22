using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_RotateGlove : MonoBehaviour
{
    public bool isLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(-30f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeft)
            transform.Rotate(Vector3.up, Time.deltaTime * -70);
        else
            transform.Rotate(Vector3.up, Time.deltaTime * 70);
    }
}
