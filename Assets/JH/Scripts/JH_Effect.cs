using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Effect : MonoBehaviour
{
    JH_PlayerMove pm;
    SY_LeftCharge lc;
    SY_EnemyLeftCharge elc;

    GameObject dash;
    GameObject guard;

    bool trail = false;
    bool isEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<JH_PlayerMove>();
        transform.Find("Left").TryGetComponent<SY_LeftCharge>(out lc);
        isEnemy = transform.Find("Left").TryGetComponent<SY_EnemyLeftCharge>(out elc);
        dash = transform.Find("DashEffect").gameObject;
        guard = transform.Find("GuardEffect").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        DashTrail();
        ChargeEffect();
        GuardEffect();
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
            if (elc.IsCharging)
            {
                transform.Find("Left").transform.Find("ChargeEffect").gameObject.SetActive(true);
                transform.Find("Right").transform.Find("ChargeEffect").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("Left").transform.Find("ChargeEffect").gameObject.SetActive(false);
                transform.Find("Right").transform.Find("ChargeEffect").gameObject.SetActive(false);
            }
        }
        else
        {
            if (lc.IsCharging)
            {
                transform.Find("Left").transform.Find("ChargeEffect").gameObject.SetActive(true);
                transform.Find("Right").transform.Find("ChargeEffect").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("Left").transform.Find("ChargeEffect").gameObject.SetActive(false);
                transform.Find("Right").transform.Find("ChargeEffect").gameObject.SetActive(false);
            }
        }
    }

    void GuardEffect()
    {
        if (isEnemy)
        {
            if (elc.IsGuard)
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
            if (lc.IsGuard)
            {
                guard.SetActive(true);
            }
            else
            {
                guard.SetActive(false);
            }
        }
    }
}
