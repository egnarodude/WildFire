using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_PlayerSling : MonoBehaviour
{
    [Header("Game Objects/Components")]
    public CTRL_Player playerCtrl;
    public Util_Trajectory trajectory;
    public MNGR_LevelManager levelManager;

    [Header("Sling Variables")]
    public float slingForce;
    public float slingThreshold = 0.5f;
    public bool isSlinging = false;
    private Vector3 slingStartPos;
    private Vector3 slingCurPos;
    private Vector3 slingAimVector;
    private float slingDistance;

    [Header("Sling Art Variables")]
    public GameObject aimReticle;
    private Quaternion aimReticleRot;
    public float trajectoryMagnitudeMultiplier;
    private Vector3 aimreticleScale = new Vector3(1.0f,1.0f,1.0f);

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = this.GetComponent<CTRL_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.isPaused == false)
        {
            getInput();
        }


    }

    private void getInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (playerCtrl.canSling)
            {
                slingStartPos = Input.mousePosition;
                isSlinging = true;
                aimreticleScale.x = 0.0f;

            }

        }

        if (isSlinging)
        {
            if (playerCtrl.playerIsAiming)
            {
                calculateTrajectory();
            }
            slingCurPos = Input.mousePosition;
            slingAimVector = slingStartPos - slingCurPos;
            slingDistance = (Vector3.Distance(slingStartPos, slingCurPos) / 100.0f);

            if (slingDistance > 1.0f)
            {
                slingDistance = 1.0f;
            }

            if (slingDistance >= slingThreshold && !playerCtrl.playerIsAiming)
            {

                aimReticle.SetActive(true);
                if (playerCtrl._currentState == CTRL_Player.PlayerState.Platformer)
                {
                    playerCtrl.switchStatePhysics(true);
                }
                else
                {
                    playerCtrl.additionalSling();
                }

            }

            aimreticleScale.x = slingDistance;

            aimReticle.transform.localScale = aimreticleScale;

            float angle = Mathf.Atan2(slingAimVector.y, slingAimVector.x) * Mathf.Rad2Deg;
            aimReticleRot = Quaternion.AngleAxis(angle, Vector3.forward);
            aimReticle.transform.rotation = Quaternion.Slerp(transform.rotation, aimReticleRot, 10.0f);

        }

        if (Input.GetMouseButtonUp(0))
        {
            isSlinging = false;
            trajectory.lineRendere.enabled = false;
            if (playerCtrl.canSling && playerCtrl.playerIsAiming)
            {

                playerCtrl.playerIsAiming = false;
                playerCtrl.glowSprite.SetActive(false);
                playerCtrl.canSling = false;
                aimReticle.SetActive(false);
                playerCtrl.ResetRBForces();
                Instantiate(playerCtrl.slingFireParticles, playerCtrl.rb.position, Quaternion.LookRotation(slingAimVector));
                playerCtrl.rb.AddForce(slingAimVector.normalized * slingForce);
                playerCtrl.resetTimeScale();
            }

        }

    }

    public void calculateTrajectory()
    {
        trajectory.velocity = ((slingAimVector.normalized * slingForce * trajectoryMagnitudeMultiplier));
        trajectory.drawTrajectory();

        trajectory.Calculate_Trajectory(this.transform.position);
        trajectory.lineRendere.enabled = true;
    }



    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        if (isSlinging)
        {
            Debug.DrawLine(this.transform.position, this.transform.position-(slingAimVector));
        }

    }

}
