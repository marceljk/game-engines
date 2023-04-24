using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControls : MonoBehaviour
{
    public Animator animator;
    public Transform lookAtTarget;
    public Transform leftHandTarget;

    [Range(0, 1)] public float lookAtWeight;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnAnimatorIK()
    {
        animator.SetLookAtPosition(lookAtTarget.position);
        animator.SetLookAtWeight(lookAtWeight);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
    }
}
