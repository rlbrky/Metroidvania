using System.Collections;
using UnityEngine;

public class hitHandler : MonoBehaviour
{
    [SerializeField] Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        if (_animator.GetCurrentAnimatorStateInfo(2).IsName("Attack"))
            if(_animator.GetCurrentAnimatorStateInfo(2).normalizedTime < 1 && other.tag == "Player")
            {
                GameManager.instance._playerHealth.DamageUnit(GuardBT.attackDamage);
            }
    }
}
