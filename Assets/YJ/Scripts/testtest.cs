using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtest : MonoBehaviour
{

    Vector3 leftOriginLocalPos;
    float backspeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        leftOriginLocalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            transform.position -= Vector3.forward * 1f * Time.deltaTime;
        }    

        if(Input.GetMouseButton(1))
        {
          transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);

        }
    }
}
