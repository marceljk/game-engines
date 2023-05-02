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
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    public LayerMask trampolinMask;

    public Camera camera;
    public float maxDist;
    public LayerMask grapplingMask;
    public GameObject hand;
    public Vector3 grapplePoint;

    LineRenderer rope;
    Vector3 velocity;
    bool isGrounded;
    bool isOnTrampolin;
    bool firstJump = true;

    public float dashSpeed = 7f;
    bool dashActive = false;
    int dashAmount = 2;
    int dashAvailable = 2;

    // Start is called before the first frame update
    void Start()
    {
        rope = hand.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnTrampolin = Physics.CheckSphere(groundCheck.position, groundDistance, trampolinMask);

        Debug.Log("IsGrounded: " + isGrounded + ". Velocity.y: " + velocity.y);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            firstJump = true;
        }
        else if (grapplePoint == Vector3.zero)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        HandleMovement();

        HandleJump();

        // rope.SetPosition(0, hand.transform.position);
        HandleGrapplingHook();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        if (Input.GetKeyDown(KeyCode.E) && move != Vector3.zero)
            ActivateDash();

        if (dashActive)
            speed = walkSpeed * dashSpeed;

        controller.Move(move * speed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            float trampolinBoost = isOnTrampolin ? 5 : 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity * trampolinBoost);
        }
        else if (Input.GetButtonDown("Jump") && firstJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            firstJump = false;
        }
    }

    /*void HandleGrapplingHook()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryGrapple();
        }

        if (joint == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            ReleaseGrapple();
            return;
        }

        UpdateGrapplePosition();
    }*/

    void HandleGrapplingHook()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryGrapple();
        }

        if (grapplePoint == Vector3.zero) return;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            ReleaseGrapple();
            return;
        }

        float distanceToGrapplePoint = Vector3.Distance(transform.position, grapplePoint);

        if (distanceToGrapplePoint > 2)
        {
            Vector3 grappleDir = (grapplePoint - hand.transform.position).normalized;
            Vector3 grappleVelocity = grappleDir * 20f;
            controller.Move(grappleVelocity * Time.deltaTime);
        }

        UpdateGrapplePosition();
    }


    void TryGrapple()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawLine(ray.origin, ray.GetPoint(maxDist));

        if (Physics.Raycast(ray, out RaycastHit grappleHit, maxDist, grapplingMask))
        {
            grapplePoint = grappleHit.point;

            rope.enabled = true;
            rope.SetPosition(0, hand.transform.position);
            rope.SetPosition(1, grappleHit.point);

        }
        else
        {
            ReleaseGrapple();
        }
    }

    /*void TryGrapple()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawLine(ray.origin, ray.GetPoint(maxDist));

        if (Physics.Raycast(ray, out RaycastHit grappleHit, maxDist, grapplingMask))
        {
            Destroy(joint);
            joint = gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grappleHit.point;

            Vector3 distanceFromPoint = hand.transform.position - grappleHit.point;

            joint.maxDistance = distanceFromPoint.magnitude * 0.5f;
            joint.minDistance = distanceFromPoint.magnitude * 0.1f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            rope.enabled = true;
            rope.SetPosition(0, hand.transform.position);
            rope.SetPosition(1, grappleHit.point);

            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            ReleaseGrapple();
        }
    }*/

    void ReleaseGrapple()
    {
        rope.enabled = false;
        // GetComponent<Rigidbody>().useGravity = true;
        grapplePoint = Vector3.zero;
    }

    void UpdateGrapplePosition()
    {
        rope.SetPosition(0, hand.transform.position);
        rope.SetPosition(1, grapplePoint);
    }

    void ActivateDash()
    {
        if (dashAvailable > 0)
        {
            dashActive = true;
            dashAvailable--;
            Invoke("DeactivateDash", 0.2f);
        }
    }

    void DeactivateDash()
    {
        dashActive = false;
        Invoke("FillDash", 2.5f);
    }

    void FillDash()
    {
        if (dashAvailable < dashAmount)
        {
            dashAvailable = dashAmount;
        }
    }
}
