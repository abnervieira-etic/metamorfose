using UnityEngine;
using UnityEngine.UI;

public class EnemyA : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float followDelay = 0.2f;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackDuration = 0.25f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Color attackColor = Color.blue;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Vector3 healthBarOffset = new Vector3(0f, 1.2f, 0f);
    [SerializeField] private Camera healthBarCamera;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private RectTransform healthBarRect;
    private Canvas healthBarCanvas;
    private Color originalColor;
    private Vector2 delayedPlayerPosition;
    private Vector2 delayedPlayerVelocity;
    private int currentHealth;
    private float attackTimer;
    private float attackCooldownTimer;
    private float playerSearchTimer;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning($"{nameof(EnemyA)} on {name} has no SpriteRenderer; attack color cannot be displayed.", this);
        }

        currentHealth = Mathf.Max(1, maxHealth);
        if (healthBar != null)
        {
            healthBarRect = healthBar.GetComponent<RectTransform>();
            healthBarCanvas = healthBar.GetComponentInParent<Canvas>();
            healthBar.minValue = 0;
            healthBar.maxValue = currentHealth;
            healthBar.wholeNumbers = true;
            healthBar.value = currentHealth;
        }
        else
        {
            Debug.LogWarning($"{nameof(EnemyA)} on {name} has no health bar assigned.", this);
        }
    }

    

    private void Start()
    {
        FindPlayer();
        if (player != null)
        {
            delayedPlayerPosition = player.position;
        }
        else
        {
            playerSearchTimer = 1f;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            playerSearchTimer -= Time.deltaTime;
            if (playerSearchTimer > 0)
            {
                return;
            }

            playerSearchTimer = 1f;
            FindPlayer();
            if (player == null)
            {
                return;
            }

            delayedPlayerPosition = player.position;
        }

        attackCooldownTimer = Mathf.Max(0, attackCooldownTimer - Time.deltaTime);
        attackTimer = Mathf.Max(0, attackTimer - Time.deltaTime);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = attackTimer > 0 ? attackColor : originalColor;
        }

        if (Vector2.Distance(transform.position, player.position) <= attackDistance)
        {
            rb.linearVelocity = Vector2.zero;
            if (attackCooldownTimer <= 0)
            {
                Attack();
            }

            return;
        }

        delayedPlayerPosition = Vector2.SmoothDamp(
            delayedPlayerPosition,
            player.position,
            ref delayedPlayerVelocity,
            Mathf.Max(0.01f, followDelay));
    }

    private void FixedUpdate()
    {
        if (player == null || isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= attackDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (delayedPlayerPosition - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public void GetHit(int damage)
    {
        if(isDead) return;

        if (damage <= 0)
        {
            Debug.LogWarning($"{nameof(GetHit)} on {name} received non-positive damage ({damage}).", this);
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        attackTimer = Mathf.Max(0, attackDuration);
        attackCooldownTimer = Mathf.Max(0, attackCooldown);
    }

    private void FindPlayer()
    {

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnEnemyDeath(gameObject);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}