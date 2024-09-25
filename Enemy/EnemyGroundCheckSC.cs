using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheckSC : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    Rigidbody rb;
    List<Collider> groundObjs;

    private void Awake()
    {
        groundObjs = new List<Collider>();
    }

    private void Start()
    {
        rb = enemyController.gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (groundObjs.Count > 0)
        {
            enemyController._isGrounded = true;
            //rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        else
        {
            enemyController._isGrounded = false;
            //rb.useGravity = true;
        }
    }

    public void ClearList()
    {
        groundObjs.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Wall") && other.tag == "Ground")
            groundObjs.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Wall") && other.tag == "Ground")
        {
            groundObjs.Remove(other);
        }
    }
}
