using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float attackCooldown = 0.3f;
    public float sprintCooldown = 0.3f;
    public GameObject weaponHitbox;
    
    public float dashSpeedMultiplier = 4f;
    public float dashDuration = 0.05f;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D capsuleCol;
    
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;

    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    private float lastSprintTime = -999f;
    
    public Transform visualRoot;
    private Vector2 lastMoveDir = Vector2.right;
    
    private bool isDashing = false;
    private Vector2 dashDirection = Vector2.right;
    
    private string normalLayerName = "Player";
    private string dashingLayerName = "PlayerInvincible";

    private int normalLayer;
    private int dashingLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        
        inputActions = new InputSystem_Actions();
        
        normalLayer  = LayerMask.NameToLayer(normalLayerName);
        dashingLayer = LayerMask.NameToLayer(dashingLayerName);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        DisableWeaponHitbox();
        
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;

        inputActions.Player.Attack.performed += OnAttackPerformed;
        inputActions.Player.Sprint.performed += OnSprintPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Sprint.performed -= OnSprintPerformed;
        DisableWeaponHitbox();

        inputActions.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            StartAttack();
        }
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
    {
        if (isDashing) return;
        if (Time.time < lastSprintTime + sprintCooldown) return;
        if (isAttacking) return;

        lastSprintTime = Time.time;
        StartCoroutine(Dash());
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            float dashSpeed = moveSpeed * dashSpeedMultiplier;
            rb.linearVelocity = dashDirection * dashSpeed;
        }
        else if (!isAttacking)
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
    }
    
    IEnumerator Dash()
    {
        isDashing = true;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDash();
        }
        
        Vector2 inputDir = moveInput;
        if (inputDir.sqrMagnitude < 0.01f)
        {
            inputDir = lastMoveDir.sqrMagnitude > 0.01f ? lastMoveDir : Vector2.right;
        }
        dashDirection = inputDir.normalized;
        
        int previousLayer = gameObject.layer;
        gameObject.layer = dashingLayer;

        yield return new WaitForSeconds(dashDuration);

        gameObject.layer = previousLayer;

        isDashing = false;
        
        rb.linearVelocity = Vector2.zero;
    }

    void StartAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerAttack();
        }
        
        lastAttackTime = Time.time;
        
        anim.SetTrigger("AttackTrigger");
    }

    public void EndAttack()
    {
        isAttacking = false;
        
        DisableWeaponHitbox();
    }

    void UpdateAnimations()
    {
        float speed = rb.linearVelocity.magnitude;
        anim.SetFloat("Speed", speed);
        
        if (moveInput.x != 0f && visualRoot != null)
        {
            lastMoveDir = new Vector2(moveInput.x, 0f);

            Vector3 scale = visualRoot.localScale;
            scale.x = Mathf.Sign(moveInput.x) * Mathf.Abs(scale.x);
            visualRoot.localScale = scale;
        }
    }
    
    public void EnableWeaponHitbox()
    {
        if (weaponHitbox != null)
            weaponHitbox.SetActive(true);
    }
    public void DisableWeaponHitbox()
    {
        if (weaponHitbox != null)
            weaponHitbox.SetActive(false);
    }

}

