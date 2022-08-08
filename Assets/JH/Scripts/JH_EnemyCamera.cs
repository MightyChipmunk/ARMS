using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_EnemyCamera : MonoBehaviour
{
    GameObject enemy;

    void Start()
    {
        enemy = GameObject.Find("Enemy");
        transform.position = enemy.transform.position + Vector3.up;
    }
    void Update()
    {
        Vector3 dir = (enemy.transform.position + Vector3.up) - transform.position;
        if (dir.magnitude > 0.1f && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                                  || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)))
            transform.position += dir * 3.0f * Time.deltaTime;

        if (dir.magnitude > 10f)
            transform.position += dir.normalized * 3.0f * Time.deltaTime;
    }
}
