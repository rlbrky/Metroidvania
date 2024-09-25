using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] VisualEffect dustEffect;

    PlayerController _playerController;
    SwordScript _swordScript;
    Animator _animator;
    Transform enemyLoc;

    //Slash Effect Variables
    [Header("General")]
    [SerializeField] float yOffsetForBloodVFX;
    [Header("Visual Effects")]
    [SerializeField] VisualEffect slashEffect;
    [SerializeField] VisualEffect bloodEffect;
    int slashEffectRotationID;
    [Header("Slash Effect Rotations")]
    [SerializeField] public Vector3 rotationGround1;
    [SerializeField] public Vector3 rotationGround2;
    [SerializeField] Vector3 rotationAir1;
    [SerializeField] Vector3 rotationAir2;

    public void Start()
    {
        _swordScript = GetComponentInChildren<SwordScript>();
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        slashEffectRotationID = Shader.PropertyToID("AngleXYZ");
    }

    void AttackRegister()
    {
        if (_swordScript.GetEnemy() != null)
        {
            //_swordScript.GetEnemy().GetComponent<Animator>().Play("DAMAGED");
            enemyLoc = _swordScript.GetEnemy().transform;
            _swordScript.GetEnemy().GetComponent<Animator>().Play("GetHit", 1);
            _swordScript.GetEnemy().GetComponent<ScreenShaker>().Shake(transform.forward);

            
            EnemyHealthManager _tempController = _swordScript.GetEnemy().GetComponent<EnemyHealthManager>();
            _tempController.StartKnockback(transform.forward);
            _tempController.TakeDamage(_swordScript.GetStrength());
            if (_tempController.canBleed)
                PlayBloodVFX();
            
            _swordScript.SetEnemy();
        }
    }

    void AttackRegisterForSkills()
    {
        if (_swordScript.GetEnemy() != null)
        {
            enemyLoc = _swordScript.GetEnemy().transform;
            //_swordScript.GetEnemy().GetComponent<Animator>().Play("DAMAGED");
            _swordScript.GetEnemy().GetComponent<Animator>().Play("GetHit", 1);
            _swordScript.GetEnemy().GetComponent<ScreenShaker>().Shake(transform.forward);

            EnemyHealthManager _tempController = _swordScript.GetEnemy().GetComponent<EnemyHealthManager>();
            _tempController.StartKnockback(transform.forward);
            _tempController.TakeDamage(_swordScript.GetStrength() * 1.5f);
            if (_tempController.canBleed)
                PlayBloodVFX();

            _swordScript.SetEnemy();
        }
    }

    void MakeAttack1False()
    {
        _animator.SetBool("attack1", false);
        PlayerController.instance.isAttacking = false;
    }
    void MakeAttack2False()
    {
        _animator.SetBool("attack2", false);
        PlayerController.instance.isAttacking = false;
    }

    void MakeJumpAttack1False()
    {
        _animator.SetBool("jumpAttack1", false);
    }
    void MakeJumpAttack2False()
    {
        _animator.SetBool("jumpAttack2", false);
    }

    void PlayDustEffect()
    {
        if(_playerController.isMovingByInput)
            dustEffect.Play();
    }

    void PlayBloodVFX()
    {
        bloodEffect.gameObject.transform.position = new Vector3(enemyLoc.position.x, enemyLoc.position.y + yOffsetForBloodVFX, enemyLoc.position.z);
        //Blood vfx y rotation fix needed here.
        bloodEffect.Play();
    }

    void PlaySE_Attack1()
    {
        slashEffect.SetVector3(slashEffectRotationID, rotationGround1);
        slashEffect.Play();
    }
    void PlaySE_Attack2()
    {
        slashEffect.SetVector3(slashEffectRotationID, rotationGround2);
        slashEffect.Play();
    }
    void PlaySE_AirAttack1()
    {
        slashEffect.SetVector3(slashEffectRotationID, rotationAir1);
        slashEffect.Play();
    }
    void PlaySE_AirAttack2()
    {
        slashEffect.SetVector3(slashEffectRotationID, rotationAir2);
        slashEffect.Play();
    }

    void FixPlayerHitbox()
    {
        _playerController.characterController.center = _playerController._hitboxCenter;
        _playerController.characterController.height = _playerController._hitboxHeight;
    }

    //void MakeAttack3False()
    //{
    //    animator.SetBool("attack3", false);
    //    comboCount = 0;
    //}
}
