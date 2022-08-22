using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Effect : MonoBehaviour
{
    JH_PlayerMove pm;
    JH_PlayerCharge ch;
    JH_EnemyCharge ech;
    YJ_Hand_left lf;
    YJ_LeftFight_enemy elf;

    GameObject dash;
    GameObject guard;
    GameObject hit;
    GameObject killer;

    GameObject left;
    GameObject right;

    bool trail = false;
    bool isEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<JH_PlayerMove>();
        TryGetComponent<JH_PlayerCharge>(out ch);
        isEnemy = TryGetComponent<JH_EnemyCharge>(out ech);
        transform.Find("Left").TryGetComponent<YJ_Hand_left>(out lf);
        transform.Find("Left").TryGetComponent<YJ_LeftFight_enemy>(out elf);
        dash = transform.Find("DashEffect").gameObject;
        guard = transform.Find("GuardEffect").gameObject;
        hit = transform.Find("HitEffect").gameObject;
        killer = transform.Find("KillerEffect").gameObject;

        left = transform.Find("Left").gameObject;
        right = transform.Find("Right").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        DashTrail();
        ChargeEffect();
        GuardEffect();
        KillerEffect();
    }

    public void HittedEffect(bool play)
    {
        if (play)
            hit.GetComponent<ParticleSystem>().Play();
        else
            hit.GetComponent<ParticleSystem>().Stop();
    }

    public void DashEffect(bool play)
    {
        if (play)
            dash.transform.Find("Particle").GetComponent<ParticleSystem>().Play();
        else
            dash.transform.Find("Particle").GetComponent<ParticleSystem>().Stop();
    }

    public void DashTrail()
    {
        if (pm.IsDash && !trail)
        {
            dash.transform.Find("Trail").GetComponent<ParticleSystem>().Play();
            trail = true;
        }
        else if (!pm.IsDash)
        {
            dash.transform.Find("Trail").GetComponent<ParticleSystem>().Stop();
            trail = false;
        }
    }

    void ChargeEffect()
    {
        if (isEnemy)
        {
            if (ech.IsCharging)
            {
                left.transform.Find("ChargeEffect").gameObject.SetActive(true);
                right.transform.Find("ChargeEffect").gameObject.SetActive(true);
            }
            else
            {
                left.transform.Find("ChargeEffect").gameObject.SetActive(false);
                right.transform.Find("ChargeEffect").gameObject.SetActive(false);
            }
        }
        else
        {
            if (ch.IsCharging)
            {
                left.transform.Find("ChargeEffect").gameObject.SetActive(true);
                right.transform.Find("ChargeEffect").gameObject.SetActive(true);
            }
            else
            {
                left.transform.Find("ChargeEffect").gameObject.SetActive(false);
                right.transform.Find("ChargeEffect").gameObject.SetActive(false);
            }
        }
    }

    void GuardEffect()
    {
        if (isEnemy)
        {
            if (ech.IsGuard)
            {
                guard.SetActive(true);
            }
            else
            {
                guard.SetActive(false);
            }
        }
        else
        {
            if (ch.IsGuard)
            {
                guard.SetActive(true);
            }
            else
            {
                guard.SetActive(false);
            }
        }
    }

    void KillerEffect()
    {
        if (isEnemy)
        {
            if (elf.yj_KillerGage_enemy.killerModeOn_enemy)
            {
                killer.SetActive(true);
            }
            else
            {
                killer.SetActive(false);
            }
        }
        else
        {
            //if (lf.yj_KillerGage.killerModeOn)
            //{
            //    killer.SetActive(true);
            //}
            //else
            //{
            //    killer.SetActive(false);
            //}
        }
    }
}
