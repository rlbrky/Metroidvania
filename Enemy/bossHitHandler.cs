using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHitHandler : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public int attackDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance._playerHealth.DamageUnit(attackDamage);
        }
    }
}
