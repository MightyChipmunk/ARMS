using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Line : MonoBehaviour
{
    GameObject des;
    // Start is called before the first frame update
    void Start()
    {
        des = GameObject.Find("Revolver_1");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(des.transform.position, transform.position) < 0.5f)
        {
            Destroy(gameObject);
        }    
    }

}
