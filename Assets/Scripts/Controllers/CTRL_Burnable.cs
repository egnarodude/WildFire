using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL_Burnable : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;

    [Header("Burn Variables")]
    public float timeToBurn;
    private float burnTime = 0.0f;
    private float timeToBurnStart;
    public AnimationCurve burnCurve;
    private float curveEval;
    public Color burnColor;
    public Color startColor;
    private bool isBurning = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        startColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurning)
        {
            burnTime += Time.deltaTime;
            curveEval = burnTime / timeToBurn;
            Debug.Log("curveEval = " + curveEval);
            spriteRenderer.color = Color.Lerp(startColor,burnColor, burnCurve.Evaluate(curveEval));

            if (burnTime >= timeToBurn)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isBurning = true;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //}

}
