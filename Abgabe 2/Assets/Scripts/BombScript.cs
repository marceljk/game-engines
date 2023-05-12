using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float delay;
    public LayerMask interactionMask;
    public float explosionRadius;
    public float explosionForce;
    public float damage;
    public GameObject particlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    void Explode()
    {
        Collider[] objs;
        objs = Physics.OverlapSphere(transform.position, explosionRadius);

        Debug.Log(objs.Length);
        foreach(Collider c in objs)
        {
            if (c.gameObject == null) return;

            Rigidbody rb = c.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
            }
            float distance = Vector3.Distance(transform.position, c.transform.position);
            float damageFactor = 1 - (distance / explosionRadius);

            PlayerMovement movement = c.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.AddDamage(damageFactor * damage);
            }

            AgentControls controls = c.GetComponent<AgentControls>();
            if (controls != null)
            {
                controls.AddDamage(damageFactor * damage);
            }
        }
        // Instantiate(particlePrefab, transform.position, Quaternion.identity);
        // GetComponent<AudioSource>().Play();

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
;    }
}