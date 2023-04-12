using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay;
    public LayerMask interactionMask;
    public float explosionRadius;
    public float explosionForce;
    public GameObject particlePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    void Explode()
    {
        Collider[] objs;
        objs = Physics.OverlapSphere(transform.position, explosionRadius, interactionMask);
        foreach(Collider c in objs)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            // rb.AddForce(new Vector3(0, explosionForce, 0), ForceMode.Impulse);
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
        }
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        GetComponent<AudioSource>().Play();

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
;    }
}
