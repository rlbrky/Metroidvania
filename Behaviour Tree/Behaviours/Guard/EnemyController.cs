using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class EnemyController : MonoBehaviour
{
    #region Public Values
    public bool isDead; // Spawner will make it false.
    public bool _isGrounded;
    public bool isColliding;
    public bool routeChanged = false;
    public bool shouldFollow;
    
    [Header("Dissolve Values")]
    public float _dissolveTime = 2f;
    public float _dissolveMultiplier = 2f;
    public float _dissolveVFXSENS = 0.75f;
    #endregion

    #region Private Values
    private Animator _animator;
    private hitHandler _hitHandler;
    private Vector3 dashTO;
    private Rigidbody _rigidbody;

    private float ySpeed;
    private float gravity = -9.81f;

    private bool _dashRight;
    private int playerPos_id;
    #endregion

    [Header("General")]
    [SerializeField] private float followCheckOffset = 2f;
    [SerializeField] private EnemyGroundCheckSC groundCheckSC;
    [SerializeField] public EnemyHealthManager healthManager;

    [Header("Dash")]
    [SerializeField] private float _maxDashRange = 8.0f;
    [SerializeField] private float _dashSpeed = 2.0f;

    [Header("Gravity")]
    [SerializeField] float ySpeedCAP = -40f;
    [SerializeField] float gravityMultiplier = 6f;

    [Header("Dissolve VFX")]
    [SerializeField] SkinnedMeshRenderer rend;
    [SerializeField] VisualEffect vfx;
    [SerializeField] Transform playerTrans;

    
    [HideInInspector] public bool _dashActive;
    [HideInInspector] public MobSpawner _spawner;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _hitHandler = GetComponentInChildren<hitHandler>();
        _hitHandler.gameObject.SetActive(false);
        playerPos_id = Shader.PropertyToID("Player_Pos");
        playerTrans = PlayerController.instance.gameObject.transform;
    }

    private void Update()
    {
        CheckForGround();

        if (healthManager.health.Health <= 0 && !isDead)
        {
            StartCoroutine(OnDeath());
            isDead = true;
        }

        if(!healthManager._isKnockingBack)
        {
            if (_dashActive && _dashRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, dashTO, _dashSpeed * Time.deltaTime);
            }
            else if (_dashActive && !_dashRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, dashTO, _dashSpeed * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isGrounded)
        {
            //gravity = Physics.gravity.y * gravityMultiplier;
            _rigidbody.AddForce(Physics.gravity * gravityMultiplier * _rigidbody.mass);
            //ySpeed += gravity * Time.deltaTime;
            //if (ySpeed < ySpeedCAP)
            //    ySpeed = ySpeedCAP;
        }
        //else
        //{
        //    ySpeed = 0;
        //}
    }

    void Dodge()
    {
        int result = Random.Range(0, 2);
        if (result == 0)
        {
            if (transform.forward != Vector3.forward)
                _animator.Play("DodgeBack", 3);
            else
                _animator.Play("DodgeFront", 3);
            _dashRight = true;
            dashTO = transform.position + new Vector3(0.0f, 0.0f, _maxDashRange);
            StartCoroutine(nameof(DeactivateDash));
        }
        else
        {
            if (transform.forward != Vector3.forward)
                _animator.Play("DodgeFront", 3);
            else
                _animator.Play("DodgeBack", 3);
            _dashRight = false;
            dashTO = transform.position - new Vector3(0.0f, 0.0f, _maxDashRange);
            StartCoroutine(nameof(DeactivateDash));
        }
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

    public void ClearGroundForSpawn()
    {
        groundCheckSC.ClearList();
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
        StartCoroutine(Dissolve());
        yield return new WaitForSeconds(_dissolveTime);

        isDead = true;
        _spawner.isMobDead = true;
    }

    IEnumerator DeactivateDash()
    {
        _dashActive = true;
        yield return new WaitForSeconds(1f);
        _dashActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && other.tag == "Ground")
        {
            healthManager._isKnockingBack = false;

            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
            _rigidbody.angularVelocity = Vector3.zero;

            _dashActive = false;
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && other.tag == "Ground")
        {
            isColliding = false;
            routeChanged = false;
        }
    }

    IEnumerator Dissolve()
    {
        float currTime = _dissolveTime;
        float dissolveSpeed;

        float dissolveRatio = 0;
        vfx.Play();

        while (currTime > 0f)
        {

            vfx.SetVector3(playerPos_id, playerTrans.position);
            dissolveSpeed = (Time.deltaTime / _dissolveTime) * _dissolveMultiplier;

            currTime -= Time.deltaTime;
            dissolveRatio += dissolveSpeed;

            foreach (var mat in rend.materials)
            {
                mat.SetFloat("_Dissolve_Ratio", dissolveRatio);
            }

            if (dissolveRatio > _dissolveVFXSENS && vfx.isActiveAndEnabled)
            {
                vfx.Stop();
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}