using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_EnemyCamera : MonoBehaviour
{
    GameObject enemy;
    JH_PlayerMove pm;

    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<JH_PlayerMove>();
        enemy = GameObject.Find("Enemy");
        transform.position = enemy.transform.position + Vector3.up;
    }
    void Update()
    {
        Vector3 dir = (enemy.transform.position + Vector3.up) - transform.position;
        if (dir.magnitude > 0.1f && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back || 
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard) && pm.State != JH_PlayerMove.PlayerState.KnockBack)
            transform.position += dir * 3.0f * Time.deltaTime;

        if (dir.magnitude > 10f)
            transform.position += dir.normalized * 3.0f * Time.deltaTime;
    }
}
