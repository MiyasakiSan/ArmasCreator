using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Cinemachine;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;

public class PlayerRpgMovement : NetworkBehaviour
{
    //private weapon weaponHeld      Inprogress**
    public MovementAnim animController;

    [Header("Walk State Setting")]
    [Range(0f, 10f)]
    public float movementSpeed;
    [Range(1f, 1000f)]
    public float speedMultiplier;

    private float defaultSpeedMultiplier;

    [Header("Run State Setting")]
    [Range(0f, 10f)]
    public float movementSpeed_Run;

    [Header("Dodge State Setting")]
    [Range(0, 100f)]
    public float dodgestaminaUse;
    [SerializeField] AnimationCurve dodgeCurve;
    public bool isDodging;
    float dodgeTimer;
    [Range(100f, 1000f)]
    public float dodgeForce;

    [Header("Camera Setting")]
    [Range(0f, 1f)]
    public float turnSmoothTime;
    float turnSmoothVelocity;
    [SerializeField]
    private GameObject deadCam;

    [Header("Battle setting")]
    public bool isBattle;
    private CombatRpgManager combatManager;

    [SerializeField]
    private CinemachineFreeLook Vcam;

    [SerializeField]
    private Transform mainCam;

    public bool canMove;
    public bool canWalk;
    public bool canRun;

    private Rigidbody rb;

    private bool isMovingForward;
    private Coroutine MoveForwardCoroutine;

    [Header("Float Collider")]
    [SerializeField]
    private LayerMask groundLayer;
    public float distanceToGround;

    public bool CanRotate;

    public enum movementState
    {
        idle,walk,run,roll
    }

    public movementState currentMovementState;

    private GameModeController gameModeController;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
    }

    void Start()
    {
        defaultSpeedMultiplier = speedMultiplier;

        if (isSinglePlayer) 
        {
            rb = this.GetComponent<Rigidbody>();
            combatManager = this.GetComponent<CombatRpgManager>();
            animController = this.GetComponent<MovementAnim>();
            animController.Init(this);

            currentMovementState = movementState.idle;

            Keyframe dodge_LastFrame = dodgeCurve[dodgeCurve.length - 1];
            dodgeTimer = dodge_LastFrame.time;

            return;
        }

        if (IsLocalPlayer && gameModeController.IsMultiPlayerMode) 
        {
            rb = this.GetComponent<Rigidbody>();
            animController = this.GetComponent<MovementAnim>();

            mainCam = Camera.main.transform;
            currentMovementState = movementState.idle;
            Vcam = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineFreeLook>();

            Vcam.Follow = this.gameObject.transform;
            Vcam.LookAt = this.gameObject.transform;

            Keyframe dodge_LastFrame = dodgeCurve[dodgeCurve.length - 1];
            dodgeTimer = dodge_LastFrame.time;
        }
        else
        {
            this.enabled = false;
        }
    }
    void Update()
    {
        if (IsLocalPlayer || isSinglePlayer)
        {
            floatCollider();

            if (!canMove) return;

            Movement();

            if(mainCam == null)
            {
                mainCam = Camera.main.transform;
                Vcam = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineFreeLook>();

                Vcam.Follow = this.gameObject.transform;
                Vcam.LookAt = this.gameObject.transform;
            }
        }
    }

    public void SetSpeedMultiplierOnUsingItem()
    {
        speedMultiplier *= 0.5f;
        animController.playerAnim.SetFloat("speedMultiplier", 0.5f);
    }

    public void ResetSpeedMultiplierOnUsingItem()
    {
        speedMultiplier = defaultSpeedMultiplier;
        animController.playerAnim.SetFloat("speedMultiplier", 1f);
    }

    public void SetCanRotate()
    {
        CanRotate = false;
    }

    public void ResetRotate()
    {
        CanRotate = true;
    }

    public void MoveForward(float speed)
    {
        isMovingForward = true;

        if(MoveForwardCoroutine != null)
        {
            StopMoveForward();
        }

        MoveForwardCoroutine = StartCoroutine(MovingForwardCoroutine(speed));
    }

    IEnumerator MovingForwardCoroutine(float speed)
    {
        while (isMovingForward )
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            float inputMultiplier = 1;
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude <= 0)
            {
                inputMultiplier = 0;
            }

            rb.AddForce(transform.forward * speed * inputMultiplier * speedMultiplier * Time.deltaTime);
            yield return null;
        }
    }

    public void StopMoveForward()
    {
        if(MoveForwardCoroutine != null)
        {
            StopCoroutine(MoveForwardCoroutine);

            MoveForwardCoroutine = null;
        }

        rb.velocity = Vector3.zero;
    }

    public void StopMoveForwardNotResetVelo()
    {
        if (MoveForwardCoroutine != null)
        {
            StopCoroutine(MoveForwardCoroutine);

            MoveForwardCoroutine = null;
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        IsStopRunning();
        currentMovementState = CheckMovementState(direction);
        if (isDodging) { return; }
        MoveDependOnState(direction);
    }
    private void MoveDependOnState(Vector3 direction)
    {
        switch (currentMovementState)
        {
            case movementState.walk:
                walkToDirection(direction);
                break;

            case movementState.run:
                runToDirection(direction);
                break;

            case movementState.roll:
                rollToDirection(direction);
                break;

            case movementState.idle:
                
                if (isSinglePlayer)
                {
                    animController.AnimationState(currentMovementState.ToString());
                }
                else
                {
                    animController.AnimationStateServerRpc(currentMovementState.ToString());
                }

                break;
        }
    }
    private movementState CheckMovementState(Vector3 direction)
    {
        bool canDodge = !animController.currentAnimatorStateBaseIsName("Dodge") &&
                        !animController.currentAnimatorCombatStateInfoIsName("Dodge");

        if (direction.magnitude < 0.1f) { return movementState.idle; }

        if (Input.GetKeyUp(KeyCode.Space) && haveStamina(dodgestaminaUse) /*&& canDodge && !isDodging*/)
        {
            return movementState.roll;
        }

        if (Input.GetKey(KeyCode.LeftShift)) { return movementState.run; }
        else { return movementState.walk; }
    }
    private bool haveStamina(float dodgeStaminaUse)
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        return playerStat.CurrentStamina - dodgeStaminaUse >= 0;
    }
    IEnumerator Dodge()
    {
        if (isDodging)
        {
            yield break;
        }

        if (isSinglePlayer)
        {
            animController.Dodge();
        }
        else
        {
            animController.DodgeServerRpc();
        }

        isDodging = true;

        float timer = 0;
        reduceStaminaOnDodge(dodgestaminaUse);

        if (combatManager.isUsingItem)
        {
            combatManager.CancelUseItem();
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        while (timer < dodgeTimer)
        {
            StopMoveForwardNotResetVelo();

            float speed = dodgeCurve.Evaluate(timer);

            Vector3 direction = new Vector3(horizontal, 0f, vertical);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 dir = transform.forward * speed;
            rb.AddForce(dir *dodgeForce* Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
    }
    private void reduceStaminaOnDodge(float amount)
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        playerStat.reduceStamina(amount);
    }
    private void walkToDirection(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (CanRotate)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (canWalk)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * movementSpeed * speedMultiplier * Time.deltaTime);

            if (isSinglePlayer)
            {
                animController.AnimationState(currentMovementState.ToString());
            }
            else
            {
                animController.AnimationStateServerRpc(currentMovementState.ToString());
            }
        }
    }

    private void rollToDirection(Vector3 direction)
    {
        combatManager.ResetAnimBoolean();

        StartCoroutine(Dodge());
    }

    private void runToDirection(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (CanRotate)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (canRun)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * movementSpeed_Run * speedMultiplier * Time.deltaTime);
            ReduceStaminaOnRun();

            if (isSinglePlayer)
            {
                animController.AnimationState(currentMovementState.ToString());
            }
            else
            {
                animController.AnimationStateServerRpc(currentMovementState.ToString());
            }
        }
        else
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * movementSpeed * speedMultiplier * Time.deltaTime);

            animController.AnimationState(movementState.walk.ToString());
        }
    }
    private void IsStopRunning()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerStat playerStat = GetComponent<PlayerStat>();
            playerStat.stopReduceStamina();
        }
    }
    private void ReduceStaminaOnRun()
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        playerStat.reduceStaminaAmountOverTime(1f);
    }

    private void floatCollider()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundLayer))
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y + distanceToGround;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.2f);
            rb.useGravity = false;
        }
    }
    public void PlayerDie()
    {
        if (animController.currentAnimatorStateBaseIsName("Die")) { return; }
        canMove = false;
        StartCoroutine(dieThenRespawn());
    }

    private IEnumerator dieThenRespawn()
    {
        this.gameObject.GetComponent<CombatRpgManager>().dieState();
        deadCam.SetActive(true);

        if (isSinglePlayer)
        {
            animController.dieAnimaiton();
        }
        else
        {
            animController.dieAnimaitonServerRpc();
        }

        yield return new WaitForSeconds(6f);
        Respawn();
    }
    public void Respawn()
    {
        deadCam.SetActive(false);
        GameObject respawnPos = GameObject.Find("SpawnPosition");
        transform.position = respawnPos.transform.position;
        canMove = true;
        this.gameObject.GetComponent<CombatRpgManager>().respawnState();
        this.gameObject.GetComponent<PlayerStat>().respawnResetHealth();
    }
}
