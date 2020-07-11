using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_GroundCheck : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public CTRL_Player playerCtrl;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (playerCtrl.isGrounded == false)
            {
                playerCtrl.isGrounded = true;
                playerCtrl.enableFootParticles();
            }

        }
    }

}
