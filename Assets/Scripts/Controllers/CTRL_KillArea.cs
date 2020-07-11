using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_KillArea : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public MNGR_LevelManager levelManager;
    public CTRL_Player playerCtrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerCtrl.playerDeath();
        }
    }

}
