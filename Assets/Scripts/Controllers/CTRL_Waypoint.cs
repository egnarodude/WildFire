using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_Waypoint : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public MNGR_LevelManager levelManager;

    [Header("Waypoint Variables")]
    public int waypointIndex;
    public bool isActive;

    [Header("Art/Prototyping")]
    public Color inactiveColor;
    public Color activeColor;

    [Header("Animator Variables")]
    public Animator animator;
    public string activateTrigger;
    public string deactivateTrigger;

    private void Start()
    {
        if (isActive)
        {
            animator.SetTrigger(activateTrigger);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (levelManager.curSpawnPointIndex != waypointIndex)
            {
                levelManager.spawnPoints[levelManager.curSpawnPointIndex].setInactive();
                setActive();
                levelManager.curSpawnPointIndex = waypointIndex;
            }

        }
    }

    public void setActive()
    {
        animator.SetTrigger(activateTrigger);
    }

    public void setInactive()
    {
        animator.SetTrigger(deactivateTrigger);
    }

}
