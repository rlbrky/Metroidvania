using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    private bool canDetect;

    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(canDetect && collision.collider.tag == "Ground")
            controller.ledgeDetected = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (canDetect && collision.collider.tag == "Ground")
            controller.ledgeDetected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
            canDetect = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
            canDetect = true;
    }
}
