using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FindPlayerForCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void OnSpawnPlayer()
    {
        _virtualCamera.Follow = GameManager.instance.Player.transform;
        _virtualCamera.LookAt = GameManager.instance.Player.transform;
    }
}
