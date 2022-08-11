using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    public static InputManager Instance { get; private set; }

    #region 플레이어 입력
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

    bool fire1;
    public bool Fire1 { get { return fire1; } }

    bool fire2;
    public bool Fire2 { get { return fire2; } }

    bool grap;
    public bool Grap { get { return grap; } }
    #endregion
    #region 에너미 입력
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
    #endregion

    bool changeAct = true;
    int ran = 0;
    int actRan = 0;

    //int moveRan = 0;
    //int atkRan = 0;
    //int defRan = 0;

    void Awake()
    {
        Instance = this;
        player = GameObject.Find("Player").gameObject;
        enemy = GameObject.Find("Enemy").gameObject;
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

        if (!player.GetComponent<SY_PlayerHp>().IsKnock && !player.GetComponent<JH_PlayerMove>().hittedp)
        {
            fire1 = Input.GetMouseButtonDown(0);
            fire2 = Input.GetMouseButtonDown(1);
            grap = Input.GetMouseButtonDown(2);
        }

        if (!player.GetComponent<SY_PlayerHp>().IsKnock && !player.GetComponent<JH_PlayerMove>().hittedp
            && !player.GetComponent<JH_PlayerMove>().IsFire())
        {
            guard = Input.GetKey(KeyCode.F);
            guardUp = Input.GetKeyUp(KeyCode.F);
        }

        if (changeAct)
        {
            ran = UnityEngine.Random.Range(1, 20);
            actRan = UnityEngine.Random.Range(1, 10);
            StartCoroutine("RandomAct");
        }

        enemyFront = ran <= 2 ? true : false;
        enemyBack = (ran >= 3 && ran <= 5) ? true : false;
        enemyRight = (ran >= 6 && ran <= 8) ? true : false;
        enemyLeft = (ran >= 9 && ran <= 11) ? true : false;
        enemyDash = actRan <= 3 ? true : false;
        enemyJump = actRan >= 8 ? true : false;

        if (!enemy.GetComponent<SY_EnemyHp>().IsKnock && !enemy.GetComponent<JH_PlayerMove>().hittedp)
        {
            enemyFire1 = (ran >= 12 && ran <= 13) ? true : false;   
            enemyFire2 = (ran >= 14 && ran <= 15) ? true : false;
            //enemyGrap = (ran >= 16 && ran <= 17) ? true : false;
        }
        else
        {
            enemyFire1 = false;
            enemyFire2 = false;
            //enemyGrap = false;
        }

        if (!enemy.GetComponent<SY_EnemyHp>().IsKnock && !enemy.GetComponent<JH_PlayerMove>().hittedp
            && !enemy.GetComponent<JH_PlayerMove>().IsFire(true))
        {
            enemyGuard = (ran >= 17) ? true : false;
            enemyGuardUp = (ran < 17) ? true : false;
        }
        else
        {
            enemyGuard = false ;
            enemyGuardUp = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.7f);
        changeAct = true;
    }
}
