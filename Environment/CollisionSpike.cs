using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpike : MonoBehaviour
{
    Animator m_Animator;

    [SerializeField] private float delay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine("ActivateTrap");
        }
    }

    IEnumerator ActivateTrap()
    {
        yield return new WaitForSeconds(delay);
        m_Animator.Play("TrapActive");
    }
}
