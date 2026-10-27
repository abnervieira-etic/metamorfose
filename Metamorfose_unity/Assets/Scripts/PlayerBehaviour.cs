using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float dragValue = 3f; // Adjust for fluidity

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.linearDamping = dragValue; // Apply drag for smooth deceleration
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        movementInput = new Vector2(h, v).normalized;

        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
}
