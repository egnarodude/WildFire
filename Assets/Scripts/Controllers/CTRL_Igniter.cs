using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class CTRL_Igniter : MonoBehaviour
{

    [Header("GameObjects/Components")]
    public ParticleSystem fireParticles;
    private Rigidbody2D rb;
    public CircleCollider2D circleCollider;
    public float particleEmission;
    public Light2D light;
    private float lightIntensityStart;
    private float lightIntensity;
    public float spawnIgniterForce;
    private float lifeTimer = 0.0f;
    public float lifeTime = 2.0f;
    private float lifeTimerStart;
    private int bounceSpread = 1;
    public bool isChild = false;
    private float curveEval;
    private bool isDead = false;
    public float deathTimer = 2.0f;


    [Header("Fire Spread Variables")]
    public GameObject igniterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimerStart = lifeTimer;
        lightIntensity = light.intensity;
        lightIntensityStart = lightIntensity;
        circleCollider = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;

        curveEval = lifeTimer / lifeTime;
        lightIntensity = Mathf.Lerp(lightIntensityStart, 0.0f, curveEval);
        light.intensity = lightIntensity;

        var emission = fireParticles.emission;
        emission.rateOverTime = Mathf.Lerp(particleEmission, 0.0f, curveEval);

        if (curveEval > 1.0f)
        {
            isDead = true;
            circleCollider.enabled = false;
        }

        if (isDead)
        {
            deathTimer -= Time.deltaTime;
        }

        if (deathTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (bounceSpread > 0 && !isChild)
        {
            spawnIgniters(collision.contacts[0].point, collision.contacts[0].normal);
            bounceSpread -= 1;
        }

    }

    private void spawnIgniters(Vector3 contactPosition, Vector3 contactNormal)
    {
        GameObject newIgniter = Instantiate(igniterPrefab, contactPosition, Quaternion.identity);
        newIgniter.GetComponent<Rigidbody2D>().AddForce(contactNormal * spawnIgniterForce);

    }
}
