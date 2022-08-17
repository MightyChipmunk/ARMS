using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    bool guard;
    public bool Guard { get { return guard; } }

    bool guardUp;
    public bool GuardUp { get { return guardUp; } }

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

    bool enemyFire1;
    public bool EnemyFire1 { get { return enemyFire1; } }
    bool enemyFire2;
    public bool EnemyFire2 { get { return enemyFire2; } }
    bool enemyGrap;
    public bool EnemyGrap { get { return enemyGrap; } }

    bool enemyGuard;
    public bool EnemyGuard { get { return enemyGuard; } }

    bool enemyGuardUp;
    public bool EnemyGuardUp { get { return enemyGuardUp; } }

    bool changeAct = true;

    int ran = 0;

    //int moveRan = 0;
    //int atkRan = 0;
    //int defRan = 0;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        front = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        right = Input.GetKey(KeyCode.D);
        left = Input.GetKey(KeyCode.A);
        dash = Input.GetKeyDown(KeyCode.LeftShift);
        jump = Input.GetKeyDown(KeyCode.Space);
        guard = Input.GetKey(KeyCode.F);
        guardUp = Input.GetKeyUp(KeyCode.F);

        if (changeAct)
        {
            ran = UnityEngine.Random.Range(1, 20);
            StartCoroutine("RandomAct");
            //print(ran);
        }

        enemyFront = ran <= 2 ? true : false;
        enemyBack = (ran >= 3 && ran <= 4) ? true : false;
        enemyRight = (ran >= 4 && ran <= 6) ? true : false;
        enemyLeft = (ran >= 6 && ran <= 8) ? true : false;
        enemyDash = ran <= 3 ? true : false;
        enemyJump = (ran >= 9 && ran <= 10) ? true : false;

        enemyFire1 = (ran >= 11 && ran <= 12) ? true : false;   
        enemyFire2 = (ran >= 13 && ran <= 14) ? true : false;
        enemyGrap = (ran >= 17 && ran <= 17) ? true : false;

        enemyGuard = (ran >= 18) ? true : false;
        enemyGuardUp = (ran < 18) ? true : false;

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene("Main");
        //}
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(1f);
        changeAct = true;
    }
}
