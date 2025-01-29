using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float movementInputDirection;
    private int amountOfJumpsLeft;
    private bool isFacingRigth = true;
    private bool isRunning;
    private bool isGrounded;
    private bool canJump;
    private Rigidbody2D rb;
    private Animator anim;
    public int amountOfDoublejumps = 1;
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfDoublejumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.angularVelocity <= 0)
        {
            amountOfJumpsLeft = amountOfDoublejumps;
        }

        if (amountOfJumpsLeft <= 0)
        { 
            canJump = false;
        }
        else
        {
            canJump = true;
        }

}

    private void CheckMovementDirection()
    {
        if(isFacingRigth && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRigth && movementInputDirection > 0)
        {
            Flip();
        }

        if (rb.linearVelocity.x != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    //FUNCTIONS

    private void Jump()
    {
        if (canJump)
        {
            amountOfJumpsLeft--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(movementSpeed * movementInputDirection, rb.linearVelocity.y);
    }

    private void Flip()
    {
        isFacingRigth = !isFacingRigth;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
