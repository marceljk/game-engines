using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDebugLine : MonoBehaviour
{
    SpringJoint sj;
    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        sj = GetComponent<SpringJoint>();
        origin = transform.position + sj.anchor;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(sj.connectedBody.transform.position, sj.transform.position);
    }
}
