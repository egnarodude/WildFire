using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class ART_2DLightFlicker : MonoBehaviour
{

    Light2D lt;

    float originalRange;



    [Space(10)]



    [Header("Light customization")]

    [Space(10)]

    [SerializeField]

    [Tooltip("The color of your light.")]

    Color lightColor = Color.yellow;



    [SerializeField]

    [Tooltip("The min intensity of your light.")]

    float minIntensity = 8f;

    [SerializeField]

    [Tooltip("The max intensity of your light.")]

    float maxIntensity = 3f;

    [SerializeField]

    [Tooltip("The minimal range of your light (radius).")]

    float minRange = 79f;

    [SerializeField]

    [Tooltip("The maximum range of your light (radius).")]

    float maxRange = 100f;



    void Start()
    {

        lt = this.GetComponent<Light2D>();

    }

    void FixedUpdate()
    {

        lt.intensity = Random.Range(minIntensity, maxIntensity);

        lt.pointLightOuterRadius = Random.Range(minRange, maxRange);

        lt.color = lightColor;

    }
}
