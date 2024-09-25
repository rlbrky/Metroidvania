using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private float shakeForce = 1f;

    private CinemachineImpulseSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(Vector3 direction)
    {
        source.GenerateImpulse(new Vector3(-direction.z, 0, 0) * shakeForce);
    }
}
