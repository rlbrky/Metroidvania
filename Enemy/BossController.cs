using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using static Unity.VisualScripting.Member;

public class BossController : MonoBehaviour
{
    #region Public Values
    public bool isGrounded;
    public bool isColliding;
    public bool shouldFollow;
    #endregion

    #region Private Values
    private ShiftColorForDMG _getDamagedEffect;
    private Animator _animator;
    private bossHitHandler _hitHandler;
    private CinemachineImpulseSource _impulseSource;

    private float ySpeed;
    private float gravity = -9.81f;
    #endregion

    [Header("General")]
    [SerializeField] private float followCheckOffset = 2f;
    [SerializeField] private float shakeForce = 2f;
    [SerializeField] private EnemyHealthManager EnemyHealthManager;

    [Header("Gravity")]
    [SerializeField] float ySpeedCAP = -40f;
    [SerializeField] float gravityMultiplier = 6f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem slamParticles;
    [SerializeField] private VisualEffect slamEffect;
    [SerializeField] private ParticleSystem tornadoParticles;
    [SerializeField] private VisualEffect shoulderEffect;

    void Start()
    {
        _animator = GetComponent<Animator>();
        //_getDamagedEffect = GetComponent<ShiftColorForDMG>();
        _hitHandler = GetComponentInChildren<bossHitHandler>();
        _hitHandler.gameObject.SetActive(false);
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        CheckForGround();

        if (EnemyHealthManager.health.Health <= 0)
        {
            StartCoroutine(OnDeath());
        }

        if (!isGrounded)
        {
            gravity = Physics.gravity.y * gravityMultiplier;
            ySpeed += gravity * Time.deltaTime;
            if (ySpeed < ySpeedCAP)
                ySpeed = ySpeedCAP;
        }
        else
        {
            ySpeed = 0;
        }

        transform.position += new Vector3(0, ySpeed, 0) * Time.deltaTime;
    }

    void CheckForGround()
    {
        if (Physics.Raycast(transform.position + (transform.forward * followCheckOffset) + new Vector3(0, 0.3f, 0), Vector3.down, 1f))
        {
            shouldFollow = true;
        }
        else
            shouldFollow = false;

        //Debug.Log(shouldFollow);
    }

    void FixGetHit()
    {
        _animator.SetBool("getHit", false);
    }

    public void ActivateAttackHitbox()
    {
        _hitHandler.gameObject.SetActive(true);
    }

    public void DeactivateAttackHitbox()
    {
        _hitHandler.gameObject.SetActive(false);
    }

    IEnumerator OnDeath()
    {
        //Play death animation.
        _animator.Play("Death");
        yield return new WaitForSeconds(2);

        //isDead = true;
        //_spawner.isMobDead = true;
    }

    public void GroundSlamVFX()
    {
        slamEffect.Play();
        slamParticles.Play();
        _impulseSource.GenerateImpulse(shakeForce);
    }

    public void TornadoVFX()
    {
        tornadoParticles.Play();
    }

    public void ShoulderVFX()
    {
        shoulderEffect.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && other.tag == "Ground")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && other.tag == "Ground")
        {
            isColliding = false;
        }
    }
}
