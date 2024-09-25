using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class MobSpawner : MonoBehaviour
{
    [Header("Patrol Points")] [Tooltip("The mob that spawns from this spawner will patrol between these points.")]
    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;

    [Header("General")]
    [SerializeField] private float despawnDistance;
    [SerializeField] private MobManager mobManager;

    [HideInInspector] public bool isMobDead = false; //This is controlled in EnemyController.

    private GameObject _enemy;
    private EnemyController _enemyController;
    private List<Transform> _waypoints;
    private bool _isSpawned;
    private IPatrolMob _mob;

    private void Start()
    {
        _waypoints = new List<Transform>();
        _waypoints.Add(waypoint1);
        _waypoints.Add(waypoint2);
    }

    void Update()
    {
        CheckDistance();

        if(isMobDead)
            gameObject.SetActive(false);
    }

    private void CheckDistance()
    {
        if(!isMobDead && (Vector2.Distance(new Vector2(transform.position.z, transform.position.y), new Vector2(PlayerController.instance.GetPlayerCoords().z, PlayerController.instance.GetPlayerCoords().y)) <= despawnDistance))
        {
            if (!_isSpawned && _mob == null)
            {
                _enemy = mobManager.UseObj_Pool();
                _enemyController = _enemy.GetComponent<EnemyController>();

                if (_enemy.TryGetComponent<IPatrolMob>(out IPatrolMob mob))
                {
                    _mob = mob;
                }

                MobCheck();

                _isSpawned = true;

                _enemy.transform.position = transform.position;

                _enemyController.isDead = false;
                _enemyController._spawner = this;

                _enemy.SetActive(true);

                if (Mathf.Abs(waypoint1.position.z - (transform.forward + transform.position).z) > Mathf.Abs(waypoint1.position.z - transform.position.z))
                    _enemy.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                else
                    _enemy.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
        }
        else if(((!isMobDead && (Vector2.Distance(new Vector2(transform.position.z, transform.position.y), new Vector2(PlayerController.instance.GetPlayerCoords().z, PlayerController.instance.GetPlayerCoords().y)) > despawnDistance))
            || (isMobDead && _isSpawned)) && _mob != null)
        {
            _mob.ResetTarget();

            mobManager.ReturnObj_Pool(_enemy);
            _mob = null;
            _isSpawned = false;
        }
    }

    private void MobCheck()
    {
        if(_mob != null)
        {
            _mob.Waypoints = _waypoints;
            _mob.EnemyController = _enemyController;
            _mob.EnemyController._isGrounded = false;
            _mob.EnemyController.ClearGroundForSpawn();
            if (_mob.EnemyController.healthManager.health != null)
                _mob.EnemyController.healthManager.RefreshHealth();
        }
    }
}
