using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    private ShiftColorForDMG _getDamagedEffect;

    public UnitHealth health;
    public bool canBleed = false;

    #region Knockback
    [Header("Knockback")]
    [SerializeField] private float _knockbackTime = 0.25f;
    [SerializeField] private float _knockbackForce = 50f;

    [HideInInspector]
    public bool _isKnockingBack;

    private Rigidbody _rigidbody;

    private float _timer;
    Collider _collider;
    #endregion

    [Header("General")]
    [SerializeField] private float MaxHealth = 100;
    void Start()
    {
        health = new UnitHealth(MaxHealth, MaxHealth);

        _getDamagedEffect = GetComponent<ShiftColorForDMG>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (_isKnockingBack)
        {
            _timer += Time.deltaTime;
            if (_timer > _knockbackTime)
            {
                _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
                //_rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, _rigidbody.velocity.z / 20);
                _rigidbody.angularVelocity = Vector3.zero;
                _isKnockingBack = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (health.Health > 0)
            health.DamageUnit(damage);

        _getDamagedEffect.PlayEffect();

        Debug.Log(this.name + " has " + health.Health + " left.");
    }

    public void StartKnockback(Vector3 direction)
    {
        _isKnockingBack = true;
        _timer = 0f;
        _rigidbody.AddForce(direction * _knockbackForce, ForceMode.Impulse);
    }

    public void RefreshHealth()
    {
        health.Health = health.MaxHealth;
    }
}
