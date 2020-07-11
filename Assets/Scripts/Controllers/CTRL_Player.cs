using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_Player : MonoBehaviour
{

    [Header("GameObjects/Components")]
    public GameObject playerSpriteObject;
    public Animator animator;
    public Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private CapsuleCollider2D capsuleCollider;
    public Util_Trajectory trajectory;

    [Header("Movement Variables")]
    public float runSpeed;
    public float jumpForce;
    private Vector2 movementForce;
    public bool isGrounded;

    [Header("Art/Animation Variables")]
    private bool isFlipped = false;
    private bool isRunning = false;
    public bool isJumping = false;
    public float jumpTimer;
    public float jumpTimerRestart;
    private Vector3 oldPosition;
    private Vector3 playerVelocity;

    private Vector3 playerUnflippedScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 playerFlippedScale = new Vector3(-1.0f, 1.0f, 1.0f);

    [Header("Particle FX")]
    public ParticleSystem[] footParticles = new ParticleSystem[0];
    private float[] footParticleEmission = new float[0];

    [Header("State Machine")]
    public PlayerState _currentState;

    [Header("Physics Materials")]
    public PhysicsMaterial2D physMatPlatform;
    public PhysicsMaterial2D physMatBall;

    [Header("Sling Logic")]
    public bool canSling = true;
    public bool playerIsAiming = false;

    [Header("Time Dilation")]
    public float slowdownFactor = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        //Sets reference to components within script on start function.
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        jumpTimerRestart = jumpTimer;
        capsuleCollider = this.GetComponent<CapsuleCollider2D>();
        circleCollider = this.GetComponent<CircleCollider2D>();
        capsuleCollider.enabled = true;
        circleCollider.enabled = false;
        rb.sharedMaterial = physMatPlatform;
    }

    // Update is called once per frame
    void Update()
    {
        oldPosition = this.transform.position;

        switch (_currentState)
        {
            case PlayerState.Platformer:
                {
                    getInput();
                    UpdateAnimator();
                    evaluateJumpTimer();
                    break;
                }
            case PlayerState.Physics:
                {
                    getPhysInput();
                    //UpdateAnimator();
                    break;
                }
        }
        playerVelocity = (oldPosition - this.transform.position) / Time.deltaTime;
        playerVelocity.x = -playerVelocity.x;
        evaluatePlayerFlip();
        Debug.Log("playerVelocity = " + playerVelocity);

    }


    private void evaluatePlayerFlip()
    {
        if (Mathf.Abs(playerVelocity.x)>0.1f)
        {
            if (playerVelocity.x > 0.0f)
            {
                playerSpriteObject.transform.localScale = playerUnflippedScale;
                isFlipped = false;
            }
            else
            {
                playerSpriteObject.transform.localScale = playerFlippedScale;
                isFlipped = false;
            }
        }

    }

    public void switchStatePhysics(bool isAiming)
    {
        _currentState = PlayerState.Physics;
        animator.SetBool("isPhysBall", true);
        rb.sharedMaterial = physMatBall;
        capsuleCollider.enabled = false;
        circleCollider.enabled = true;

        if (isAiming)
        {
            aimSlowMo();
            ResetRBForces();
            rb.velocity = playerVelocity;
            playerIsAiming = true;
        }
    }

    public void additionalSling()
    {
            aimSlowMo();
            playerIsAiming = true;
    }

    public void SwitchStatePlatform()
    {
        _currentState = PlayerState.Platformer;
        animator.SetBool("isPhysBall", false);
        isGrounded = false;
        animator.SetTrigger("platSwitch");
        ResetRBForces();
        resetTimeScale();
        rb.sharedMaterial = physMatPlatform;
        playerIsAiming = false;
        capsuleCollider.enabled = true;
        circleCollider.enabled = false;

    }

    public void aimSlowMo()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void resetTimeScale()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void getPhysInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SwitchStatePlatform();
        }
    }

    private void getInput()
    {

        // Gets horizontal axis input and applies to RigidBody2D Component on Character
        if (Mathf.Abs(Input.GetAxis("Horizontal"))>0.0f)
        {
            this.transform.position += Vector3.right * Input.GetAxis("Horizontal") * runSpeed;
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump();
        }

    }

    public void ResetRBForces()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = 0.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    private void evaluateJumpTimer()
    {
        if (jumpTimer > 0.0f)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void jump()
    {
        animator.SetTrigger("jump");
        isJumping = true;
        isGrounded = false;
        rb.AddForce(Vector2.up * jumpForce);
    }
    
    private void UpdateAnimator()
    {
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
    }

    public enum PlayerState
    {
        Platformer,
        Physics
    }

    public void resetSlingBool()
    {
        canSling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (!canSling)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    resetSlingBool();
                }
            }
    }

}
