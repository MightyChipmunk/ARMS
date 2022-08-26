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

    bool killer;
    public bool Killer { get { return killer; } }
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

    bool enemyKiller;
    public bool EnemyKiller { get { return enemyKiller; } }
    #endregion

    bool changeAct = true;
    int ran = 0;
    int actRan = 0;

    bool canFire1 = true;
    bool canFire2 = true;
    bool canGrap = true;
    bool canKill = true;
    bool canJump = true;

    //int moveRan = 0;
    //int atkRan = 0;
    //int defRan = 0;

    void Awake()
    {
        Instance = this;
        player = GameObject.Find("Player").gameObject;
        enemy = GameObject.Find("Enemy").gameObject;
    }

    public void Update()
    {
        if (JH_Count.Instance.IsStart)
        {
            front = Input.GetKey(KeyCode.W);
            back = Input.GetKey(KeyCode.S);
            right = Input.GetKey(KeyCode.D);
            left = Input.GetKey(KeyCode.A);
            dash = Input.GetKeyDown(KeyCode.LeftShift);
            jump = Input.GetKeyDown(KeyCode.Space);

            if (!player.GetComponent<SY_PlayerHp>().IsKnock && !player.GetComponent<JH_PlayerMove>().hittedp
                && !player.GetComponent<JH_PlayerMove>().IsGrapped() && !player.GetComponent<JH_PlayerCharge>().IsGuard
                && player.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Grap
                && player.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Die
                && player.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Win)
            {
                fire1 = Input.GetMouseButtonDown(0);
                fire2 = Input.GetMouseButtonDown(1);
                grap = Input.GetMouseButtonDown(2);
                killer = Input.GetKeyDown(KeyCode.Q);
            }

            if (!player.GetComponent<SY_PlayerHp>().IsKnock && !player.GetComponent<JH_PlayerMove>().hittedp
                && !player.GetComponent<JH_PlayerMove>().IsGrapped() && !player.GetComponent<JH_PlayerMove>().IsFire()
                && player.GetComponent<CharacterController>().isGrounded
                && player.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Grap
                && player.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Die
                && player.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Win)
            {
                guard = Input.GetKey(KeyCode.F);
                guardUp = Input.GetKeyUp(KeyCode.F);
            }
            else if (player.GetComponent<JH_PlayerMove>().IsGrapped())
                guard = false;
            
            if (changeAct)
            {
                ran = Random.Range(1, 22);
                actRan = Random.Range(1, 10);
                StartCoroutine("RandomAct");
            }

            enemyFront = ran <= 2 ? true : false;
            enemyBack = (ran >= 3 && ran <= 5) ? true : false;
            enemyRight = (ran >= 6 && ran <= 8) ? true : false;
            enemyLeft = (ran >= 9 && ran <= 11) ? true : false;
            enemyDash = actRan <= 3 ? true : false;

            // GetKeyDown 구현
            if (actRan >= 9 && canJump)
            {
                enemyJump = true;
                canJump = false;
            }
            else if (actRan < 9)
                canJump = true;
            else if (canJump == false)
                enemyJump = false;

            if (!enemy.GetComponent<SY_EnemyHp>().IsKnock && !enemy.GetComponent<JH_PlayerMove>().hittedp
                && !enemy.GetComponent<JH_PlayerMove>().IsGrapped(true) && !enemy.GetComponent<JH_EnemyCharge>().IsGuard
                && enemy.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Die
                && enemy.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Win)
                {
                // GetKeyDown 구현
                if (ran >= 12 && ran <= 13 && canFire1)
                {
                    enemyFire1 = true;
                    canFire1 = false;
                }
                else if (!(ran >= 12 && ran <= 13))
                    canFire1 = true;
                else if (canFire1 == false)
                    enemyFire1 = false;
                // GetKeyDown 구현
                if (ran >= 14 && ran <= 15 && canFire2)
                {
                    enemyFire2 = true;
                    canFire2 = false;
                }
                else if (!(ran >= 14 && ran <= 15))
                    canFire2 = true;
                else if (canFire2 == false)
                    enemyFire2 = false;
                // GetKeyDown 구현
                if (ran >= 16 && ran <= 17 && canKill)
                {
                    enemyKiller = true;
                    canKill = false;
                }
                else if (!(ran >= 16 && ran <= 17))
                    canKill = true;
                else if (canKill == false)
                    enemyKiller = false;
                // GetKeyDown 구현
                if (ran >= 18 && ran <= 19 && canGrap)
                {
                    enemyGrap = true;
                    canGrap = false;
                }
                else if (!(ran >= 18 && ran <= 19))
                    canGrap = true;
                else if (canGrap == false)
                    enemyGrap = false;
            }
            else
            {
                enemyFire1 = false;
                enemyFire2 = false;
                enemyGrap = false;
            }

            if (!enemy.GetComponent<SY_EnemyHp>().IsKnock && !enemy.GetComponent<JH_PlayerMove>().hittedp
                && !enemy.GetComponent<JH_PlayerMove>().IsGrapped(true) && !enemy.GetComponent<JH_PlayerMove>().IsFire(true)
                && enemy.GetComponent<CharacterController>().isGrounded
                && enemy.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Die
                && enemy.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Win
                && enemy.GetComponent<JH_PlayerMove>().State != JH_PlayerMove.PlayerState.Grap)
            {
                enemyGuard = (ran >= 20) ? true : false;
                enemyGuardUp = (ran < 20) ? true : false;
            }
            else
            {
                enemyGuard = false;
                enemyGuardUp = false;
            }
        }
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.7f);
        changeAct = true;
    }
}
