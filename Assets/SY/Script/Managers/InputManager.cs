using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    bool front;
    public bool Front { get { return front; } }

    bool back;
    public bool Back { get { return back; } }

    bool right;
    public bool Right { get { return right; } }

    bool left;
    public bool Left { get { return left; } }

    bool dash;
    public bool Dash { get { return dash; } }

    bool jump;
    public bool Jump { get { return jump; } }

    bool enemyFront;
    public bool EnemyFront { get { return enemyFront; } }

    bool enemyBack;
    public bool EnemyBack { get { return enemyBack; } }

    bool enemyRight;
    public bool EnemyRight { get { return enemyRight; } }

    bool enemyLeft;
    public bool EnemyLeft { get { return enemyLeft; } }

    bool enemyDash;
    public bool EnemyDash { get { return enemyDash; } }

    bool enemyJump;
    public bool EnemyJump { get { return enemyJump; } }


    bool changeAct = true;
    int ran = 0;

    void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        front = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        right = Input.GetKey(KeyCode.D);
        left = Input.GetKey(KeyCode.A);
        dash = Input.GetKeyDown(KeyCode.LeftShift);
        jump = Input.GetKeyDown(KeyCode.Space);

        if (changeAct)
        {
            ran = UnityEngine.Random.Range(1, 10);
            StartCoroutine("RandomAct");
        }

        enemyFront = ran <= 2 ? true : false;
        enemyBack = (ran >= 3 && ran <= 4) ? true : false;
        enemyRight = (ran >= 4 && ran <= 6) ? true : false;
        enemyLeft = (ran >= 6 && ran <= 8) ? true : false;
        enemyDash = ran <= 3 ? true : false;
        enemyJump = ran > 8 ? true : false;
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.5f);
        changeAct = true;
    }
}
