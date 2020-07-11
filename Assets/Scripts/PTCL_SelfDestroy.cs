using UnityEngine;
using System.Collections;

public class PTCL_SelfDestroy : MonoBehaviour
{
    public bool deleteParent;
    private void Start()
    {
        if (this.gameObject.transform.parent != null && deleteParent)

        {
            Destroy(this.gameObject.transform.parent.gameObject, GetComponent<ParticleSystem>().duration);
        }
        else
        {
            Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }

    }

}