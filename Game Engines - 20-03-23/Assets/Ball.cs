using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float maxX;
    public float maxZ;
    public Transform playArea;

    private Vector3 velocity;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit");
        //Destroy(other.gameObject);
        if (other.CompareTag("Paddle"))
        {
            float maxDist = 0.5f * other.transform.localScale.x + 0.5f * transform.localScale.x;
            float actualDist = transform.position.x - other.transform.position.x;

            float distNorm = actualDist / maxDist;
            velocity.x = distNorm * maxX;
            velocity.z *= -1;
        }
        else if (other.CompareTag("Wall"))
        {
            velocity.x *= -1;
        }
        GetComponent<AudioSource>().Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, maxZ);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Score: " + GameManager.instance.P1Score + " : " + GameManager.instance.P2Score);
        }

        transform.position += velocity * Time.deltaTime;
        float maxZPosition = playArea.localScale.z * 0.5f * 10 + 2;

        if (transform.position.z > maxZPosition)
        {
            transform.position = new Vector3(0, 0.5f, 0);
            velocity = new Vector3(0, 0, -maxZ);
            GameManager.instance.P1Score += 1;
        }
        else if (transform.position.z < -maxZPosition) {
            transform.position = new Vector3(0, 0.5f, 0);
            velocity = new Vector3(0, 0, maxZ);
            GameManager.instance.P2Score += 1;
        }
    }
}
