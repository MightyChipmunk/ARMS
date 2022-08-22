using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ArmDamage : MonoBehaviour
{
    [SerializeField]
    int damage;

    public int Damage
    {
        get { return damage; }
    }

    [SerializeField]
    int chargeDamage;

    public int ChargeDamage
    {
        get { return chargeDamage; }
    }
}
