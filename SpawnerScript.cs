using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] FindPlayerForCamera _camSc;
    private void Awake()
    {
        _camSc = FindObjectOfType<FindPlayerForCamera>();
    }
    private void Start()
    {
        GameManager.instance.SpawnPlayer();
        _camSc.OnSpawnPlayer();
    }
}
