using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControls : MonoBehaviour
{
    public GameObject player;
    public float followDistance;

    float health;
    NavMeshAgent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        CheckHealth();
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < followDistance)
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

    public void AddDamage(float damage = 25)
    {
        health -= Mathf.Abs(damage);
        Debug.Log("Bot health" + health);
    }

    void CheckHealth()
    {
        // Needs to be changed into a dead animation & respawn
        if (health <= 0) Destroy(this.gameObject);
    }

}
