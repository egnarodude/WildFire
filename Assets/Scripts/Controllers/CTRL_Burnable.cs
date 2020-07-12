﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;

public class CTRL_Burnable : MonoBehaviour
{
    [Header("GameObjects/Components")]
    public SpriteRenderer spriteRenderer;
    private Material spriteMaterial;
    public BoxCollider2D boxCollider;

    [Header("ParticleSystem Variables")]
    public float maxEmissionValue;
    public ParticleSystem fireParticles;
    public AnimationCurve fireParticleEmissCurve;

    [Header("Lighting 2D Variables")]
    public Light2D fireLight;
    public float maxIntensity;

    [Header("Collider Removal")]
    public float colliderRemoveThreshold = 0.8f;

    [Header("Shader Variables")]
    public AnimationCurve shaderBurnCurve;

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
        spriteMaterial = spriteRenderer.material;
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
            spriteMaterial.SetFloat("_Dissolve", shaderBurnCurve.Evaluate(burnCurve.Evaluate(curveEval)));
            var emission = fireParticles.emission;

            emission.rateOverTime = fireParticleEmissCurve.Evaluate(burnCurve.Evaluate(curveEval)) * maxEmissionValue;
            fireLight.intensity = fireParticleEmissCurve.Evaluate(burnCurve.Evaluate(curveEval)) * maxIntensity;

            if (curveEval >= colliderRemoveThreshold)
            {
                boxCollider.enabled = false;
            }

            //spriteRenderer.color = Color.Lerp(startColor,burnColor, burnCurve.Evaluate(curveEval));

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
