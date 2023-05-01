using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 12f;
    public float runSpeed = 24f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Camera camera;
    public float maxDist;
    public LayerMask grapplingMask;
    public GameObject hand;

    LineRenderer robe;
    SpringJoint joint;
    Vector3 velocity;
    bool isGrounded;
    bool firstJump = true;

    public float dashSpeed = 7f;
    bool dashActive = false;
    int dashAmount = 2;
    int dashAvailable = 2;

    // Start is called before the first frame update
    void Start()
    {
        robe = hand.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            firstJump = true;
        } else if (joint == null)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        if (Input.GetKeyDown(KeyCode.E) && move != Vector3.zero)
        {
            activateDash();
        }

        if (dashActive) speed = walkSpeed * dashSpeed;

        if (joint == null)
        {
            controller.Move(move * speed * Time.deltaTime);
        }
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        } else if (Input.GetButtonDown("Jump") && firstJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            firstJump = false;
        }


        robe.SetPosition(0, hand.transform.position);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            Debug.DrawLine(ray.origin, ray.GetPoint(maxDist));
            RaycastHit grapple;
            Destroy(joint);
            if (Physics.Raycast(ray, out grapple, maxDist, grapplingMask))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                Vector3 grapplePoint = grapple.point;

                robe.enabled = true;
                robe.SetPosition(1, grapplePoint);
                joint = transform.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(hand.transform.position, grapplePoint);

                //The distance grapple will try to keep from grapple point. 
                joint.maxDistance = distanceFromPoint * 0.5f;
                joint.minDistance = distanceFromPoint * 0.1f;

                //Adjust these values to fit your game.
                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

            }
            else
            {
                robe.enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void activateDash()
    {
        if (dashAvailable > 0)
        {
            dashActive = true;
            dashAvailable--;
            Invoke("deactivateDash", 0.2f);
        }
    }

    void deactivateDash()
    {
        dashActive = false;
        Invoke("fillDash", 2.5f);
    }

    void fillDash()
    {
        if (dashAvailable < dashAmount)
        {
            dashAvailable = dashAmount;
        }
    }
}
