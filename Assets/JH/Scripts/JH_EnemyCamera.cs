using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_EnemyCamera : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    JH_PlayerMove pm;

    [SerializeField]
    float speed = 5.0f;

    void Start()
    {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        pm = player.GetComponent<JH_PlayerMove>();
        transform.position = enemy.transform.position;
    }
    void Update()
    {
        Vector3 dir = (enemy.transform.position) - transform.position;
        if (dir.magnitude > 0.1f && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back || 
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard) && pm.State != JH_PlayerMove.PlayerState.KnockBack
            || pm.State == JH_PlayerMove.PlayerState.Attack)
            transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Angle(enemy.transform.forward, -player.transform.forward) > 45f)
            transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
