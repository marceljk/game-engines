using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject ballPrefab;
    public GameObject bombPrefab;

    public GameObject hand;
    public Camera camera;
    public LayerMask interactionMask;
    public float maxDist;
    public float throwForce;
    public float grabDist;
    public float attrForce;
    public float slowScale;

    private GameObject objInHand;
    private float originFixedDeltaTime;
    // Start is called before the first frame update
    void Awake()
    {
        originFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = slowScale;
                Time.fixedDeltaTime = originFixedDeltaTime * slowScale;
            } else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = originFixedDeltaTime;
            }
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (objInHand == null)
            {
                // Instantiate(boxPrefab, hand.transform.position, Quaternion.identity);
                Ray ray = new Ray(camera.transform.position, camera.transform.forward);
                Debug.DrawLine(ray.origin, ray.GetPoint(maxDist));
                RaycastHit obj;
                if (Physics.Raycast(ray, out obj, maxDist, interactionMask))
                {
                    if (Vector3.Distance(hand.transform.position, obj.transform.position) < grabDist)
                    {
                        objInHand = obj.transform.gameObject;
                        obj.transform.position = hand.transform.position;
                        obj.transform.parent = hand.transform;
                        obj.transform.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    else
                    {
                        Vector3 dir = (hand.transform.position - obj.transform.position).normalized;
                        obj.rigidbody.AddForce(dir * attrForce, ForceMode.Impulse);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (objInHand != null)
            {
                objInHand.transform.parent = null;
                objInHand.transform.GetComponent<Rigidbody>().isKinematic = false;
                objInHand.GetComponent<Rigidbody>().AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
                objInHand = null;
            }
            else
            {
                GameObject ball = Instantiate(ballPrefab, hand.transform.position, Quaternion.identity);
                Rigidbody ballRB = ball.GetComponent<Rigidbody>();
                ballRB.AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject ball = Instantiate(bombPrefab, hand.transform.position, Quaternion.identity);
            Rigidbody ballRB = ball.GetComponent<Rigidbody>();
            ballRB.AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
        }
    }
}
