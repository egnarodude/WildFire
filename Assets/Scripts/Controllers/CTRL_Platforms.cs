using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_Platforms : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public MNGR_LevelManager levelManager;

    [Header("Platform Variables")]
    private Vector3 startPos;
    public Transform target;
    public float speed;
    private bool moveRight;

    [Header("Art/Prototyping")]
    public Color inactiveColor;
    public Color activeColor;

    private void Start()
    {
        startPos = transform.position;
        moveRight = true;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        if (transform.position == target.position)
        {
            moveRight = false;
        }
        else if (transform.position == startPos)
        {
            moveRight = true;
        }
        if (moveRight == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, step);
        }
        else if (moveRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

}
