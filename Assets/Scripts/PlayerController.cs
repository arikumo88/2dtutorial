using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastMoveX;
    private float lastMoveY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        bool isMoving = movement.magnitude > 0;

        Debug.Log("isMoving: " + isMoving);
        
        if(isMoving)
        {
            lastMoveX = movement.x;
            lastMoveY = movement.y;
        }

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", isMoving);

        if(!isMoving)
        {
            animator.SetFloat("LastMoveX", lastMoveX);
            animator.SetFloat("LastMoveY", lastMoveY);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
