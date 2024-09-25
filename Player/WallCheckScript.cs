using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class WallCheckScript : MonoBehaviour
{
    PlayerController parentScript;
    Collider _collider;
    [SerializeField] LayerMask mask;
    [SerializeField] VisualEffect slideSmoke;

    private void Start()
    {
        parentScript = transform.parent.GetComponent<PlayerController>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!parentScript.isWallSliding)
            slideSmoke.Stop();

        if (parentScript.isWallJumping)
            StartCoroutine(DisableCollider());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 0 && collision.collider.tag == "Ground")
        {
            parentScript.isWallSliding = true;
            parentScript.isWallJumping = false;
            parentScript.wallNormal = collision.contacts[0].normal;
            slideSmoke.transform.position = collision.contacts[0].point;
            slideSmoke.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != 0 && collision.collider.tag == "Ground")
        {
            parentScript.isWallSliding = false;
        }
    }

    IEnumerator DisableCollider()
    {
        _collider.enabled = false;
        parentScript.isWallSliding = false;
        yield return new WaitForSeconds(0.2f);
        _collider.enabled = true;
    }
}