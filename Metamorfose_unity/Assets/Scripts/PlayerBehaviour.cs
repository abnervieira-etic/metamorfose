using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    private float moveSpeed;
    private float moveX;
    private float moveZ;
    [SerializeField] private Rigidbody2D playerRigidbody;
    void Start()
    {
        moveSpeed = 5f;
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * moveSpeed;
        moveZ = Input.GetAxis("Vertical") * moveSpeed;

        playerRigidbody.linearVelocity = new Vector2(moveX, moveZ);
    }
}
