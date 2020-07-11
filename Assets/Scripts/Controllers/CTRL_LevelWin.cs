using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_LevelWin : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public MNGR_LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelManager.winLevel();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        levelManager.winLevel();
    //    }
    //}

}
