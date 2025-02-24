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
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        movement.x = inputX;
        movement.y = inputY;

        movement = movement.normalized;
        bool isMoving = movement.magnitude > 0;

        Debug.Log("isMoving: " + isMoving);
        
        if(isMoving)
        {
            if (Mathf.Abs(inputX) > Mathf.Abs(inputY))
            {
                lastMoveX = inputX;
                lastMoveY = 0;
            }
            else
            {
                lastMoveX = 0;
                lastMoveY = inputY;
            }
        }

        animator.SetFloat("MoveX", lastMoveX);
        animator.SetFloat("MoveY", lastMoveY);
        animator.SetBool("IsMoving", isMoving);

        if(!isMoving)
        {
            animator.SetFloat("LastMoveX", lastMoveX);
            animator.SetFloat("LastMoveY", lastMoveY);

            //debug
            Debug.Log($"Idle Transition: LastMoveX: {lastMoveX}, LastMoveY: {lastMoveY}");
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
