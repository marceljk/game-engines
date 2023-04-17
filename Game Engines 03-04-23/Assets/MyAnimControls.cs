using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimControls : MonoBehaviour
{
    public float myValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(myValue, 1, 1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetFloat("Circular", Random.Range(0f, 1f));
        }
    }
}
