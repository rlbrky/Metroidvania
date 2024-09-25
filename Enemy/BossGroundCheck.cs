using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundCheck : MonoBehaviour
{
    [SerializeField] BossController enemyController;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Wall") && other.tag == "Ground")
            enemyController.isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Wall") && other.tag == "Ground")
            enemyController.isGrounded = false;
    }
}
