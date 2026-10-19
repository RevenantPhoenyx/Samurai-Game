using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Cayote Time")]
    [SerializeField] private float coyoteTime; //how much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player run of the edge

    [Header("Dash / Roll")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header ("Jump Sound")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Vector3 originalScale;

    private float wallJumpCooldown;
    private float horizontalInput;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Dash input
        dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && dashCooldownTimer <= 0 && !isDashing)
            StartCoroutine(Dash());

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Adjustable Jump Height
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero;
        }
        else if (!isDashing)
        {
            body.gravityScale = 7;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }

        // Coyote time logic
        if (isGrounded())
        {
            coyoteCounter = coyoteTime;
            jumpCounter = extraJumps;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Reset coyote counter to avoid double jumps
        coyoteCounter = 0;
    }


    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return;
        //if coyote counter is 0 or less and not on the wall and doesn't have extra jumps don't do anything

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();

        else
        {
            if (isGrounded())
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            else
            {
                //if not on the ground and coyote counter is bigger than 0 do a normal jump
                if(coyoteCounter > 0)
                    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                else
                {
                    if(jumpCounter > 0) //if we have extra jumps then jump and decrease the jump counter
                    {
                        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0,Vector2.down,0.1f,groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,new Vector2(transform.localScale.x, 0),0.1f,wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    private IEnumerator Dash()
    {
       isDashing = true;
       dashCooldownTimer = dashCooldown;

       anim.SetTrigger("dash");

       float originalGravity = body.gravityScale;
       body.gravityScale = 0;

       body.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);

       yield return new WaitForSeconds(dashDuration);

       body.gravityScale = originalGravity;
       isDashing = false;
    }
  }

