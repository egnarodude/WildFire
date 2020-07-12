using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_GroundCheck : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public CTRL_Player playerCtrl;

    [Header("Raycast Check")]
    public float groundDistance;
    public LayerMask layerMask;


    private void FixedUpdate()
    {

        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance, ~layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Burnable")
            {

                if (playerCtrl.jumpTimer <= 0.0f)
                {
                    playerCtrl.isGrounded = true;
                    playerCtrl.isJumping = false;
                    playerCtrl.jumpTimer = playerCtrl.jumpTimerRestart;
                }
            }
            else
            {
                playerCtrl.isGrounded = false;
            }
        }
        else
        {
            playerCtrl.isGrounded = false;
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, (this.transform.position + -Vector3.up*groundDistance));
    }


}
