﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using TMPro;

public class CTRL_Player : MonoBehaviour
{

    [Header("GameObjects/Components")]
    public GameObject playerSpriteObject;
    public Animator animator;
    public Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private CapsuleCollider2D capsuleCollider;
    public Util_Trajectory trajectory;
    public MNGR_LevelManager levelManager;

    [Header("Movement Variables")]
    public float runSpeed;
    public float jumpForce;
    private Vector2 movementForce;
    public bool isGrounded;
    public bool moving = false;

    [Header("Art/Animation Variables")]
    private bool isFlipped = false;
    private bool isRunning = false;
    public bool isJumping = false;
    public float jumpTimer;
    public float jumpTimerRestart;
    public Light2D playerLight;
    public GameObject glowSprite;
    private Vector3 oldPosition;
    private Vector3 playerVelocity;

    private Vector3 playerUnflippedScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 playerFlippedScale = new Vector3(-1.0f, 1.0f, 1.0f);

    [Header("Particle FX")]
    public ParticleSystem[] footParticles = new ParticleSystem[0];
    private float[] footParticleEmission = new float[0];

    [Header("IgniterVariables")]
    public GameObject igniterPrefab;
    public bool ignitersLoaded = false;
    public float igniterSpawnForce = 50.0f;

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

    [Header("Death/Respawn Variables")]
    public float deathPauseTime = 1.0f;
    public float tweenToSpawnTime = 2.0f;
    public int deathCounter = 0;
    public TextMeshProUGUI deathsText;
    private string deathsTextString;

    [Header("Particle Systems")]
    public GameObject deathParticles;
    public GameObject slingFireParticles;
    public GameObject slingResetParticles;
    public GameObject switchToPlatParticles;
    public GameObject switchToPhysParticles;

    [Header("Particle Systems")]
    public Vector2 colliderSize;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private LayerMask whatIsGround;
    private float xInput;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private Vector2 slopeNormalPerp;

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

        colliderSize = capsuleCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        oldPosition = this.transform.position;

        if (levelManager.isPaused == false)
        {
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
                case PlayerState.Respawning:
                    {
                        break;
                    }
            }
        }

        if (levelManager.isPaused == false)
        {
            playerVelocity = (oldPosition - this.transform.position) / Time.deltaTime;
            playerVelocity.x = -playerVelocity.x;
            evaluatePlayerFlip();
        }

    }

    private void FixedUpdate()
    {
        SlopeCheck();
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal);

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
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
        GameObject physParticles = Instantiate(switchToPhysParticles, rb.position, Quaternion.identity);
        physParticles.transform.parent = this.gameObject.transform;
        animator.SetBool("isPhysBall", true);
        rb.sharedMaterial = physMatBall;
        capsuleCollider.enabled = false;
        circleCollider.enabled = true;
        glowSprite.SetActive(true);

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
        ignitersLoaded = false;
        glowSprite.SetActive(false);
        GameObject platPartices = Instantiate(switchToPlatParticles, rb.position, Quaternion.identity);
        platPartices.transform.parent = this.gameObject.transform;
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
            if (levelManager.isPaused == false)
            {
                this.transform.position += Vector3.right * Input.GetAxis("Horizontal") * runSpeed;
            }

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

    public void playerDeath()
    {
        updateDeathCount();

        Instantiate(deathParticles, this.transform.position, Quaternion.identity);
        glowSprite.SetActive(false);
        playerSpriteObject.SetActive(false);
        _currentState = PlayerState.Respawning;
        playerLight.enabled = false;
        circleCollider.enabled = false;
        capsuleCollider.enabled = false;
        ResetRBForces();
        rb.isKinematic = true;
        StartCoroutine("respawnCo");
    }

    private void updateDeathCount()
    {
        deathCounter++;
        deathsTextString = deathCounter.ToString();
        deathsText.text = deathsTextString;
    }

    private void playerRespawn()
    {
        SwitchStatePlatform();
        playerSpriteObject.SetActive(true);
        playerLight.enabled = true;
        capsuleCollider.enabled = true;
        ResetRBForces();
        rb.isKinematic = false;
    }

    public IEnumerator respawnCo()
    {
        yield return new WaitForSeconds(deathPauseTime);
        float t = 0.0f;
        Vector3 deathPosition = this.transform.position;
        while(t < tweenToSpawnTime)
        {
            this.transform.position = Vector3.Lerp(deathPosition, levelManager.spawnPoints[levelManager.curSpawnPointIndex].transform.position, t / tweenToSpawnTime);
            t += Time.deltaTime;
            yield return null;
        }
        playerRespawn();
        yield return null;
    }

    public enum PlayerState
    {
        Platformer,
        Physics,
        Respawning
    }

    public void resetSlingBool()
    {
        canSling = true;
        GameObject rechargeParticles = Instantiate(slingResetParticles, rb.position, Quaternion.identity);
        rechargeParticles.transform.parent = this.gameObject.transform;

        if (_currentState == PlayerState.Physics)
        {
            glowSprite.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentState == PlayerState.Physics)
        {
            ignitersLoaded = false;
            Debug.Log("collision Velocity magnitude = " + collision.relativeVelocity.magnitude);

            int igniterSpawns = (int)Random.Range(1.0f, 4.0f);
            for (int i = 0; i <= igniterSpawns; i++)
            {
                GameObject newIgniter = Instantiate(igniterPrefab, rb.position, Quaternion.identity);
                newIgniter.GetComponent<Rigidbody2D>().AddForce((collision.contacts[0].normal + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f))) * igniterSpawnForce*(collision.relativeVelocity.magnitude/4.0f));
            }
        }

         if (!canSling)
         {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Burnable")
            {
                resetSlingBool();
            }
         }
    }

}
