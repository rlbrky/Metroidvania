using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    #region GeneralVariables
    //THE KEY ITEM LIST
    public List<KeyItem> keyItems;
    public bool canGrabKeyItem;
    public KeyItem currentItem;

    SkillIconLoader _iconLoader;
    AnimationEvents _animEvents;
    GameObject main;

    Vector3 playerVelocity;
    Vector3 movementDirection;
    Vector3 slashEffectRotationG1;
    Vector3 slashEffectRotationG2;

    float ySpeed;
    float gravity;
    float startGravity;
    float originalStepOffset;

    float? lastGroundedTime; //The "?" is for null statements further down in the code.
    float? jumpButtonPressedTime;

    bool canJump;
    bool isJumping;
    bool isGrounded;
    bool hasDoubleJump;

    [Header("General Values")]
    [SerializeField] stepOffsetScript stepOffsetSc;

    [SerializeField][Tooltip("How fast character turns around.")] float rotationSpeed;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float ySpeedCAP = -40f;
    [SerializeField] float gravityMultiplier;
    [SerializeField] float jumpHorizontalSpeed;
    [SerializeField] float jumpButtonGracePeriod;

    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public RestingPlaceScript _altarSc;
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool ledgeDetected;
    #endregion

    #region Attack Variables
    [HideInInspector] public bool skillsChanged;
    //Change this into a skill tree & the character will carry 3 skills. Only one will be active at a time.
    public List<SkillBase> _skills;

    float damage;
    float attackPressedTime;
    int comboCount;
    int activeSkillIndex = 0;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isMovingByInput;

    [SerializeField] private float comboGracePeriod;
    [SerializeField] public float attackSpeed = 2f;
    #endregion

    #region Wall Slide

    [HideInInspector]
    public bool isWallSliding;
    [SerializeField] 
    float wallSlidingSpeed = 1f;

    #endregion

    #region Wall Jump Variables

    [HideInInspector] public Vector3 wallNormal;
    public bool isWallJumping;

    Quaternion toRotationWJ = Quaternion.identity;

    float wallJumpingCounter;
    float wallJumpAnimControl;
    float wjHorizontalSpeed; //This value is taken from normal horizontal speed.
    bool rotationCancelled;

    [Header("WALL JUMP")]
    [SerializeField] float wallJumpingTime = 0.2f;
    [SerializeField] float wallJumpingDuration = 1.0f;
    [SerializeField] float wjGravityMultiplier = 3f;
    [SerializeField] [Tooltip("This is how much the speed will get cut. (Its division.)")] float wjHorizontalSpeedCut = 1.006f;
    #endregion

    #region DashVariables
    private bool canDash = true;
    private bool isDashing;
    [Header("Dash")]
    [SerializeField] float dashingTime;
    [SerializeField] float dashPower;
    [SerializeField] float dashCooldown;
    [SerializeField] Vector3 dashOffset;

    [SerializeField] public VisualEffect dashVFX;
    #endregion

    #region LedgeClimb Variables
    //[Header("Ledge Info")]
    //[SerializeField] [Tooltip("Offset for position before climb")] private Vector3 offset1;
    //[SerializeField][Tooltip("Offset for position after climb")] private Vector3 offset2;

    //private Vector3 climbBegunPos;
    //private Vector3 climbDonePos;

    private bool canGrabLedge = true;
    private bool canClimb; //THIS IS FOR ANIMATION.

    #endregion

    #region Force Variables

    Vector3 _forceDirection;
    float _force = 30f;
    float _forceTimer = 2f;
    bool _isForceActive;

    #endregion

    #region Cannonball Variables
    
    [HideInInspector] public bool canEnter;
    [HideInInspector] public teleportObjSC _tpSC;

    private float _cannonForceDecreaseRate;
    private float _cannonForce;
    private float _currentForce;

    private bool _isThrownByCannon;
    private float _dropCountdown;
    #endregion

    #region Slide Variables

    [Header("Slide")]
    [SerializeField] private float maxSlideTime = 0.5f;
    [SerializeField] private float _slideCooldown = 1f;
    [SerializeField] private float slideForce = 30f;

    private float _slideTimer;
    private bool _isSliding;
    private bool _willSlide;
    private bool _canSlide = true;

    [HideInInspector] public float _hitboxHeight;
    [HideInInspector] public Vector3 _hitboxCenter;
    #endregion
    //IGNORE
    [HideInInspector]
    public Vector3 maara;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _iconLoader = GameObject.Find("SkillIcon").GetComponent<SkillIconLoader>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        _animEvents = GetComponent<AnimationEvents>();
        originalStepOffset = characterController.stepOffset;
        main = gameObject;
        keyItems = new List<KeyItem>();
        animator.SetFloat("attackSpeed", attackSpeed);

        slashEffectRotationG1 = _animEvents.rotationGround1;
        slashEffectRotationG2 = _animEvents.rotationGround2;

        _hitboxCenter = characterController.center;
        _hitboxHeight = characterController.height;
    }

    // Update is called once per frame
    void Update()
    {
        #region IGNORE
        transform.position = new Vector3(maara.x, transform.position.y, transform.position.z);
        #endregion

        CheckForLedge();
        if (canClimb)
        {
            Invoke(nameof(LedgeClimbOver), 1f); //To guarantee the return gets canceled after some time.
            return;
        }

        if (isDashing)
        {
            playerVelocity = Vector3.zero;

            if (transform.rotation.eulerAngles.y <= 60.0f)
                playerVelocity.z = dashPower;
            else
                playerVelocity.z = -dashPower;
        }
        else
        {
        
            #region Attack
        if (Input.GetMouseButtonDown(1) && characterController.isGrounded) //Gets Right-Click on mouse
        {
            OnClick();
        }

        if (Input.GetMouseButtonDown(1) && !characterController.isGrounded)
        {
            OnAerialClick();
        }
        #region Skills
        //This is the best way i could think of.
        if (_skills.Count > 0 && _skills[activeSkillIndex].icon != null)
        {
            _iconLoader.LoadIcon(_skills[activeSkillIndex].icon);
            _iconLoader.gameObject.SetActive(true);
        }
        else //Revisit this.
            _iconLoader.gameObject.SetActive(false);

        if (skillsChanged)
        {
            _skills.Clear();
            _skills.AddRange(GetComponents<SkillBase>());
        }

        if (_skills.Count > 0 && Input.GetKeyDown(KeyCode.R))
        {
            activeSkillIndex++;
            if(activeSkillIndex >= _skills.Count)
                activeSkillIndex = 0;
        }
        if (_skills.Count > 0 && Input.GetKeyDown(KeyCode.F))
        {
            attackPressedTime = Time.time;
            _skills[activeSkillIndex].UseSkill();
        }
        #endregion
        //Combo Grace
        if (Time.time - attackPressedTime > comboGracePeriod)
        {
            comboCount = 0;
            isAttacking = false;
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
            animator.SetBool("jumpAttack1", false);
            animator.SetBool("jumpAttack2", false);
        }
        #endregion

            Movement();

            #region Grab

             if(Input.GetKeyDown(KeyCode.E))
             {
                 if (_altarSc != null)
                 {
                     if (!UIManager.instance.keyPressed)
                     {
                         UIManager.instance.OnKeyPressed();
                     }
                     else
                     {
                         UIManager.instance.OnExitUIWithKeyAltar();
                     }
                 }

                 if(canGrabKeyItem && currentItem != null)
                 {
                     Debug.Log("ITEM ADDED TO THE LIST" +  currentItem.itemName);
                     keyItems.Add(currentItem);
                     UIManager.instance.ShowKeyItems(GameManager.instance.areaName);
                 }
             }

        #endregion

            if (animator.GetBool("jumpAttack1") || animator.GetBool("jumpAttack2"))
            {
                playerVelocity.y = 0;
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)
                    playerVelocity.y = ySpeed;
            }
            else
                playerVelocity.y = ySpeed;
        
        }//End of else for dashing

        #region Throw Player
        if (_isForceActive)
        {
            isDashing = false;

            if (_forceTimer > 0)
            {
                _forceTimer -= Time.deltaTime;
                playerVelocity = _forceDirection * _force;
                ySpeed = (_forceDirection * _force).y;
                //transform.Translate(_forceDirection * _force * Time.deltaTime);
            }
            else
                _isForceActive = false;
        }
        #endregion

        #region Cannonball
        if (_isThrownByCannon)
        {
            isDashing = false;

            if (_currentForce >= 0)
            {
                _currentForce = (_currentForce - Mathf.Pow(_cannonForceDecreaseRate, 2) * Time.deltaTime);

                playerVelocity = _forceDirection * _currentForce;
                ySpeed = (_forceDirection * _currentForce).y;
            }
            else
                _isThrownByCannon = false;
        }
        #endregion

        if (_tpSC == null || !_tpSC.isPlayerInside)
            characterController.Move(playerVelocity * Time.deltaTime); //All the movement gets calculated and added in the end with one line instead of everything triggering its own Move.
    }

    void Movement()
    {
        #region Movement
        float input = Input.GetAxis("Horizontal");

        movementDirection = new Vector3(0f, 0f, input);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
        movementDirection.Normalize();

        //movement on Z
        if(!isAttacking)
            playerVelocity.z = (movementDirection * inputMagnitude * playerSpeed).z;

        gravity = Physics.gravity.y * gravityMultiplier;

        if (isJumping && ySpeed > 0f && Input.GetKey(KeyCode.Space) == false && !isWallJumping) //This lets us control how high we can jump if we just tap the spacebar.
        {
            gravity *= 3;
        }

        //Early walljump end
        if (isWallJumping && ySpeed > 0f && Input.GetKey(KeyCode.Space) == false)
        {
            gravity *= wjGravityMultiplier;
        }

        ySpeed += gravity * Time.deltaTime;
        if (ySpeed < ySpeedCAP)
            ySpeed = ySpeedCAP;

        if (characterController.isGrounded && stepOffsetSc.groundExists)
        {
            lastGroundedTime = Time.time;

            isWallJumping = false;
            isWallSliding = false;

            if(!_isSliding)
                canJump = true;

            hasDoubleJump = true;
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            jumpButtonPressedTime = Time.time;
        }

        Jump(jumpButtonPressedTime);

        if (movementDirection != Vector3.zero) //Rotate player.
        {
            //If animation bugs out add animator.SetBool("isWallSliding", false); in here.
            animator.SetBool("isMoving", true);
            isMovingByInput = true;

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            if(transform.rotation.eulerAngles.y > 60f)
            {
                _animEvents.rotationGround1 = -slashEffectRotationG1;
                _animEvents.rotationGround2 = -slashEffectRotationG2;
            }
            else
            {
                _animEvents.rotationGround1 = slashEffectRotationG1;
                _animEvents.rotationGround2 = slashEffectRotationG2;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            isMovingByInput = false;
        }

        if (!isGrounded && !isWallJumping) //Keep the momentum for jumping while moving.
        {
            animator.SetBool("skipTransition", true);
            playerVelocity.z = (movementDirection * inputMagnitude * jumpHorizontalSpeed).z;
        }
        #endregion

        #region WallJump
        WallJump(); //See below for function's usage

        if (isWallJumping)
        {
            if (ySpeed > 0f && Input.GetKey(KeyCode.Space) == false)
                wjHorizontalSpeed = wjHorizontalSpeed / wjHorizontalSpeedCut;
            
            playerVelocity.z = (wallNormal * wjHorizontalSpeed).z;
        }
        else
            wjHorizontalSpeed = jumpHorizontalSpeed;
        #endregion

        #region Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && movementDirection != Vector3.zero)
        {
            StartCoroutine(Dash());
        }
        #endregion

        #region Slide

        if (Input.GetKeyDown(KeyCode.LeftControl) && movementDirection != Vector3.zero && _canSlide)
        {
            if (!isGrounded)
            {
                _willSlide = true;
            }
            else if(!_willSlide && isGrounded)
                StartSlide();
        }

        if (_willSlide && isGrounded)
            StartSlide();

        if(_isSliding && isGrounded)
            SlideMovement();
        #endregion

        #region Teleport

        if (_tpSC != null && canEnter && Input.GetKeyDown(KeyCode.V))
        {
            _tpSC.isPlayerInside = true;
            _dropCountdown = _tpSC.canStayCountdown;
            transform.position = new Vector3(transform.position.x, _tpSC.gameObject.transform.position.y, _tpSC.gameObject.transform.position.z);
        }

        if (_tpSC != null && _dropCountdown > 0)
            _dropCountdown -= Time.deltaTime;
        else if(_tpSC != null)
            _tpSC.CancelCannon();

        #endregion
    }

    //All jump related code is here (including double jump)
    void Jump(float? jumpPressedTime)
    {
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            if(!_isSliding)
                ySpeed = -0.5f;
            animator.SetBool("isWallSliding", false);
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);

            if (Time.time - jumpPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                animator.SetBool("isJumping", true);
                animator.SetBool("isWallSliding", false);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
            else
                animator.SetBool("skipTransition", false);
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("isGrounded", false);
            isGrounded = false;

            StopSlide();

            if (Input.GetKeyDown(KeyCode.Space) && isJumping && hasDoubleJump && !isWallSliding)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                hasDoubleJump = false;
            }

            if (isWallSliding)
            {//Previous loc of hasDoubleJump = true.
                ySpeed = -wallSlidingSpeed;
                animator.SetBool("isWallSliding", true);
            }

            if ((isJumping && ySpeed < 0 && !isWallSliding) || (ySpeed < -2 && !stepOffsetSc.groundExists))
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isWallSliding", false);
                animator.SetBool("isWallJumping", false);
            }
        }
    }

    void OnAnimatorMove()
    {
        if(Time.time - attackPressedTime <= comboGracePeriod)
            isAttacking = true;
        if (isAttacking)
            playerVelocity.z = animator.deltaPosition.z / Time.deltaTime;

        if(canClimb)
        {
            characterController.Move(animator.deltaPosition * 1.1f);
        }

        //if (isGrounded) //Only move using the character controller if we are grounded.
        //{
        //    playerVelocity.z = animator.deltaPosition.z * playerSpeed;
        //}
        //if (isWallJumping)
        //{
        //    transform.rotation = animator.rootRotation;
        //}
    }

    void WallJump() //This function checks if we can wall jump.
    {
        if (isWallSliding)
        {
            hasDoubleJump = true;
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
            animator.SetBool("isFalling", false);
            animator.SetBool("isWallJumping", false);

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
        {
            isWallJumping = true;
            wallJumpingCounter = 0f;

            ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);

            animator.SetBool("isFalling", false);
            animator.SetBool("isWallSliding", false);
            animator.SetBool("isWallJumping", true);
            
            toRotationWJ = Quaternion.LookRotation(-transform.forward, Vector3.up);
            rotationCancelled = false;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        if (isWallJumping) //Controlling blend tree animation for wall jump.
        {
            wallJumpAnimControl += Time.deltaTime;
            animator.SetFloat("wallJumpTime", Mathf.Clamp01(wallJumpAnimControl), 0.05f, Time.deltaTime);
        }
        else
            wallJumpAnimControl = 0f;

        //If there is no input rotate player.
        if (movementDirection == Vector3.zero && !rotationCancelled)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotationWJ, rotationSpeed * Time.deltaTime);
        else if (movementDirection != Vector3.zero)
            rotationCancelled = true;
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        //dashVFX.transform.position = transform.position;
        dashVFX.Play();
        animator.Play("Dash");
        GameManager.instance._playerHealth._isImmune = true;

        yield return new WaitForSeconds(dashingTime);

        //Dash Complete
        isDashing = false;
        GameManager.instance._playerHealth._isImmune = false;
        //dashVFX.transform.position = transform.position + new Vector3(0, -1000);

        yield return new WaitForSeconds(dashCooldown);
        //Cooldown Over
        canDash = true;
    }

    void OnClick()
    {
        attackPressedTime = Time.time;
        comboCount++;

        if (comboCount == 1) //|| comboCount > 2)
        {
            animator.SetBool("attack1", true);
            //animator.Play("Attack1");
            //comboCount = 1;
        }

        if(comboCount == 2)
        {
            animator.SetBool("attack2", true);
            //animator.Play("Attack2");
        }

        //comboCount = Mathf.Clamp(comboCount, 0, 2);
        if (comboCount > 2)
            comboCount = 0;
    }

    void OnAerialClick() //AERIAL ATTACKS
    {
        attackPressedTime = Time.time;
        comboCount++;

        if (comboCount == 1)
        {
            animator.SetBool("jumpAttack1", true);
            animator.Play("JumpAttack1");
        }

        if (animator.GetBool("jumpAttack1") && comboCount == 2)
        {
            animator.SetBool("jumpAttack2", true);
        }

        if(comboCount > 2)
            comboCount = 0;
    }

    #region Ledge Climb Functions
    void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            //Vector3 ledgePos = GetComponentInChildren<LedgeDetection>().transform.position;

            //climbBegunPos = ledgePos + offset1;
            //climbDonePos = ledgePos + offset2;

            //climbBegunPos = ledgePos;

            canClimb = true; //SET ANIMATOR HERE
            animator.SetBool("isClimbing", canClimb);
        }

        //if (canClimb)
        //    transform.position = climbBegunPos;

    }

    void LedgeClimbOver() //Use it in the animation events.
    {
        canClimb = false; //ANIMATOR
        animator.SetBool("isClimbing", canClimb);
        canGrabLedge = true;
        //Invoke("AllowLedgeGrab", .1f);
    }

    //This is to fix a bug: If you use ledgeclimbover at the exact second when the player is still attached to the ledge so it can trigger twice.
    //But if we invoke it a tiny bit later it won't happen.
    void AllowLedgeGrab() => canGrabLedge = true;
    #endregion}

    public void ThrowPlayer(Vector3 direction, float force, float forceTimer)
    {
        _isForceActive = true;
        _forceTimer = forceTimer;
        _forceDirection = direction;
        _force = force;
    }

    public void Cannonball(Vector3 direction, float force, float cannonForceDecreaseRate)
    {
        _isThrownByCannon = true;
        _forceDirection = direction;
        _currentForce = force;
        _cannonForceDecreaseRate = cannonForceDecreaseRate;
    }

    #region Slide

    void StartSlide()
    {
        canJump = false;

        _willSlide = false;
        _canSlide = false;
        _isSliding = true;

        characterController.center = new Vector3(0, 1.8f, 0);
        characterController.height = 2f;

        _slideTimer = maxSlideTime;

        //animator.SetBool("isSliding", true);
        animator.Play("Sliding");
    }

    void SlideMovement()
    {
        playerVelocity = movementDirection * slideForce;
        _slideTimer -= Time.deltaTime;

        if(_slideTimer <= 0) 
            StopSlide();
    }

    void StopSlide()
    {
        _isSliding = false;
        canJump = true;

        //Hitbox fix got moved to the animation events.

        StartCoroutine(SlideCD());
    }

    IEnumerator SlideCD()
    {
        yield return new WaitForSeconds(_slideCooldown);
        _canSlide = true;
    }
    #endregion

    public Vector3 GetPlayerCoords()
    {
        return transform.position;
    }
}