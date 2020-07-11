using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_Waypoint : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public MNGR_LevelManager levelManager;
    public SpriteRenderer spriteRend;

    [Header("Waypoint Variables")]
    public int waypointIndex;
    private bool isActive;

    [Header("Art/Prototyping")]
    public Color inactiveColor;
    public Color activeColor;

    private void Start()
    {
        spriteRend = this.GetComponent<SpriteRenderer>();
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
        spriteRend.color = activeColor;
    }

    public void setInactive()
    {
        spriteRend.color = inactiveColor;
    }

}
