using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ColEnable : MonoBehaviour
{
    Collider col;
    Collider myCol;
    [SerializeField]
    bool isEnemy = false;
    [SerializeField]
    bool isLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        myCol = GetComponent<Collider>();
        if (isEnemy)
        {
            if (isLeft)
            {
                col = GameObject.Find("Enemy").transform.Find("Left").GetComponent<Collider>();
            }
            else if (!isLeft)
            {
                col = GameObject.Find("Enemy").transform.Find("Right").GetComponent<Collider>();
            }
        }
        else if (!isEnemy)
        {
            if (isLeft)
            {
                col = GameObject.Find("Player").transform.Find("Left").GetComponent<Collider>();
            }
            else if (!isLeft)
            {
                col = GameObject.Find("Player").transform.Find("Right").GetComponent<Collider>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (col.enabled)
            myCol.enabled = true;
        else if (col.enabled == false)
            myCol.enabled = false;
    }
}
