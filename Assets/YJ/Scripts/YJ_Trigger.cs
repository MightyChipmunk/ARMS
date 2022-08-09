using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Trigger : MonoBehaviour
{
    Vector3 localpos;
    GameObject enemy;
    GameObject player;
    JH_PlayerMove jh_PlayerMove;
    CharacterController cc;
    public bool enemyCome;
    public bool enemyGo;
    float backspeed = 20f;
    float currentTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        jh_PlayerMove = enemy.GetComponent<JH_PlayerMove>();
        cc = enemy.GetComponent<CharacterController>();
        localpos = transform.localPosition;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCome)
        {
            enemy.transform.position = transform.position - new Vector3(0,0.5f,0);

            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2.8f)
            {
                enemyCome = false;
                enemyGo = true;
            }
        }
        if(enemyGo)
        {
            Vector3 dir = Camera.main.transform.forward + (Vector3.up * 0.2f);

            if(cc.collisionFlags == CollisionFlags.Sides)
            {
                dir.y -= 0.5f;
            }
            cc.Move(dir * backspeed * Time.deltaTime);

            if( !jh_PlayerMove.enabled && enemy.transform.position.y < 0.7f )
            {
                backspeed = 0;

                // 1초동안 못움직이게하기
                currentTime += Time.deltaTime;
                if( currentTime > 1f )
                {
                    //jh_PlayerMove.enabled = true;
                    enemyGo = false;
                    backspeed = 20f;
                    gameObject.SetActive(false);
                    currentTime = 0;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Enemy"))
        {
            //jh_PlayerMove.enabled = false;
            enemyCome = true;
        }
    }

}
