using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Transform playArea;
    public float speed;
    private float dir = 0;
    // Start is called before the first frame update
    void Start()
    {
        /*this.GetComponent<Transform>().position = new Vector3();
        transform.position = new Vector3(0, 0, 0);
        Debug.Log("Hello");*/
    }

    // Update is called once per frame
    void Update()
    {
        /*dir = 0;
        if(Input.GetKey(KeyCode.A))
        {
            dir = -1;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            dir = 1;
        }*/
        dir = Input.GetAxis("Horizontal");

        float limit = playArea.localScale.x * 10 * 0.5f - transform.localScale.x * 0.5f;
        float newX = transform.position.x + Time.deltaTime * speed * dir;
        float clampedX = Mathf.Clamp(newX, -limit, limit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        //transform.position += new Vector3(Time.deltaTime * speed * dir, 0, 0);
    }
}
