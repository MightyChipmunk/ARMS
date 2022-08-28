using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class JH_Effect : MonoBehaviour
{
    PostProcessProfile profile;
    JH_PlayerMove pm;
    JH_PlayerCharge ch;
    JH_EnemyCharge ech;
    YJ_Hand_left lf;
    YJ_Hand_left elf;

    GameObject dash;
    GameObject guard;
    GameObject hit;
    GameObject killer;
    public GameObject killerStart;

    GameObject left;
    GameObject right;

    bool trail = false;
    bool isEnemy = false;
    Light _light;
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<JH_PlayerMove>();
        TryGetComponent<JH_PlayerCharge>(out ch);
        isEnemy = TryGetComponent<JH_EnemyCharge>(out ech);
        transform.Find("Left").TryGetComponent<YJ_Hand_left>(out lf);
        transform.Find("Left").TryGetComponent<YJ_Hand_left>(out elf);
        dash = transform.Find("DashEffect").gameObject;
        guard = transform.Find("GuardEffect").gameObject;
        hit = transform.Find("HitEffect").gameObject;
        killer = transform.Find("KillerEffect").gameObject;

        left = transform.Find("Left").gameObject;
        right = transform.Find("Right").gameObject;
        _light = GameObject.Find("Directional Light").GetComponent<Light>();

        if (!isEnemy)
            profile = transform.Find("Main Camera").transform.Find("Post-process Volume").GetComponent<PostProcessVolume>().profile;
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
            if (lf.yj_KillerGage.killerModeOn)
            {
                if (killer.activeSelf == false)
                {
                    StartCoroutine("KillerSpawn");
                    StartCoroutine("KillerTime");
                    _light.cullingMask = 0;
                    _light.cullingMask = 1 << LayerMask.NameToLayer("Player");
                    _light.cullingMask |= 1 << LayerMask.NameToLayer("Enemy");
                    _light.cullingMask |= 1 << LayerMask.NameToLayer("PlayerHand");
                    _light.cullingMask |= 1 << LayerMask.NameToLayer("EnemyHand");

                    profile.GetSetting<AmbientOcclusion>().intensity.Override(2f);
                    profile.GetSetting<Bloom>().intensity.Override(1f);
                }
                killer.SetActive(true);
            }
            else
            {
                if (killer.activeSelf == true)
                {
                    _light.cullingMask = -1;
                    profile.GetSetting<Bloom>().intensity.Override(2f);
                    profile.GetSetting<AmbientOcclusion>().intensity.Override(0f);
                }
                killer.SetActive(false);
            }
        }
    }

    IEnumerator KillerTime()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
    }

    IEnumerator KillerSpawn()
    {
        GameObject start = Instantiate(killerStart);
        start.transform.position = transform.position;
        yield return new WaitForSecondsRealtime(0.2f);
        GameObject start2 = Instantiate(killerStart);
        start2.transform.position = transform.position;
        yield return new WaitForSecondsRealtime(0.2f);
        GameObject start3 = Instantiate(killerStart);
        start3.transform.position = transform.position;
        yield return new WaitForSecondsRealtime(0.2f);
        GameObject start4 = Instantiate(killerStart);
        start4.transform.position = transform.position;
    }
}
