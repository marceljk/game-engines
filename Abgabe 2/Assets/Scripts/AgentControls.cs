using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControls : MonoBehaviour
{
    public GameObject player;
    public float followDistance;
    public GameObject bomb;
    public float bombForce;

    [SerializeField] GameObject rightHand;
    float health;
    float distance;
    float timestampLastBomb;
    bool dead;

    NavMeshAgent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        health = 100f;
        timestampLastBomb = Time.time;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (dead) return;
        FollowPlayer();
        ThrowBomb();
        CheckHealth();
    }

    void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);
        if (distance < 5f)
        {
            agent.SetDestination(transform.position);
        }
        else if (distance < 10f)
        {
            agent.SetDestination(player.transform.position + (transform.position - player.transform.position).normalized * 0.5f);
        }

        if (agent.velocity.magnitude > 0)
        {
            float percentVelocity = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Velocity", percentVelocity);
        }
        else
        {
            animator.SetFloat("Velocity", 0);
        }
    }

    void ThrowBomb()
    {
        if (distance < 15 && (Time.time - 2) > timestampLastBomb)
        {
            timestampLastBomb = Time.time;
            animator.SetTrigger("Throw");
        }
    }

    public void InstantiateBomb()
    {
        Vector3 handPosition = rightHand.transform.position;
        Vector3 direction = player.transform.position - handPosition;
        direction.Normalize();
        GameObject bombInstance = Instantiate(bomb, handPosition, Quaternion.identity);
        bombInstance.GetComponent<Rigidbody>().AddForce(direction * bombForce, ForceMode.Impulse);
    }

    public void AddDamage(float damage = 25)
    {
        health -= Mathf.Abs(damage);
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            dead = true;
            agent.isStopped = true;
            animator.SetLayerWeight(animator.GetLayerIndex("Death"), 1);
            animator.SetBool("Death", true);
        }
    }

    public void Dead()
    {
        Invoke(nameof(Spawn), 3f);
    }

    void Spawn()
    {
        if (!dead) return;
        bool respawn = true;
        animator.SetBool("Death", false);

        if (respawn)
        {
            Start();
            agent.isStopped = false;
            animator.SetLayerWeight(animator.GetLayerIndex("Death"), 0);

            float x = Random.Range(-10f, 11f);
            float z = Random.Range(15f, 31f);
            int randomSign = Random.Range(0, 2) * 2 - 1;
            transform.position = new Vector3(x, 0.5f, z * randomSign);
        }
        else
        {
            Destroy(gameObject);
        }

    }

}
