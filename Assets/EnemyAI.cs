using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.3f;
    public float attackCooldown = 1.0f;

    public GameObject weaponHitbox;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;

    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    
    public LayerMask obstacleMask;
    public Transform[] stairWaypoints;

    private Transform currentWaypoint;
    
    public float chaseStartRange = 8f;
    public float chaseStopRange = 14f;
    private bool isChasing = false;


    
    public Transform visualRoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        if (!isChasing && distance <= chaseStartRange)
        {
            isChasing = true;
        }
        
        if (isChasing && distance > chaseStopRange)
        {
            isChasing = false;
            currentWaypoint = null;
        }


        if (!isAttacking)
        {
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                StartAttack();
            }
        }

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (player == null || isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (!isChasing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        if (distanceToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        Vector2 enemyPos = transform.position;
        Vector2 toPlayer = (Vector2)player.position - enemyPos;

        bool blocked = Physics2D.Raycast(enemyPos, toPlayer.normalized, distanceToPlayer, obstacleMask);

        Vector2 targetPosition;

        if (!blocked && currentWaypoint == null)
        {
            targetPosition = player.position;
        }
        else
        {
            if (currentWaypoint == null || Vector2.Distance(enemyPos, currentWaypoint.position) < 0.2f)
            {
                currentWaypoint = GetBestWaypoint();
            }

            if (currentWaypoint != null)
            {
                targetPosition = currentWaypoint.position;

                Vector2 toPlayerFromHere = (Vector2)player.position - enemyPos;
                bool nowClear = !Physics2D.Raycast(enemyPos, toPlayerFromHere.normalized, toPlayerFromHere.magnitude, obstacleMask);

                if (nowClear)
                {
                    currentWaypoint = null;
                    targetPosition = player.position;
                }
            }
            else
            {
                targetPosition = player.position;
            }
        }

        Vector2 dir = ((Vector2)targetPosition - enemyPos).normalized;
        rb.linearVelocity = dir * moveSpeed;

        if (visualRoot != null && Mathf.Abs(dir.x) > 0.01f)
        {
            Vector3 scale = visualRoot.localScale;
            scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
            visualRoot.localScale = scale;
        }
    }



    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsAttacking", true);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyAttack();
        }

        if (weaponHitbox != null)
            weaponHitbox.SetActive(true);

        Invoke(nameof(EndAttack), 0.25f);
    }

    void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", false);

        if (weaponHitbox != null)
            weaponHitbox.SetActive(false);
    }

    void UpdateAnimations()
    {
        float speed = rb.linearVelocity.magnitude;
        anim.SetFloat("Speed", speed);
    }
    
    private Transform GetBestWaypoint()
    {
        if (stairWaypoints == null || stairWaypoints.Length == 0)
            return null;

        Transform best = null;
        float bestScore = Mathf.Infinity;

        Vector2 enemyPos = transform.position;
        Vector2 playerPos = player.position;

        foreach (Transform wp in stairWaypoints)
        {
            if (wp == null) continue;
            
            float score = Vector2.Distance(enemyPos, wp.position) +
                          Vector2.Distance(wp.position, playerPos);

            if (score < bestScore)
            {
                bestScore = score;
                best = wp;
            }
        }

        return best;
    }
    
    public void InitializeStats(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }
}

