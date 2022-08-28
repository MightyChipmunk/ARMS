using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ArmInit : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        GameObject left = Instantiate(JH_ArmSelect.Instance.leftHand);
        left.transform.parent = player.transform;
        left.name = "Left";
        GameObject right = Instantiate(JH_ArmSelect.Instance.rightHand);
        right.transform.parent = player.transform;
        right.name = "Right";

        enemy = GameObject.Find("Enemy");
        GameObject enemyLeft = Instantiate(JH_ArmSelect.Instance.enemyLeftHand);
        enemyLeft.transform.parent = enemy.transform;
        enemyLeft.name = "Left";
        GameObject enemyRight = Instantiate(JH_ArmSelect.Instance.enemyRightHand);
        enemyRight.transform.parent = enemy.transform;
        enemyRight.name = "Right";
    }

    private void Start()
    {
        
    }
}
