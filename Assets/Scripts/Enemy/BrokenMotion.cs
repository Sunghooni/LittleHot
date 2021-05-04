using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMotion : MonoBehaviour
{
    public GameObject bodyParts;
    private Animator animator;
    private EnemyLife _EnemyLife;

    private bool isMotionplayed = false;
    private float leftHandMovement = 3f;
    private float rightHandMovement = 3f;
    private float spineRotateX = 45f;
    private float spineRotateY = 0f;

    private void Awake()
    {
        animator = gameObject.transform.GetComponent<Animator>();
        _EnemyLife = gameObject.transform.GetComponent<EnemyLife>();
    }

    private void Start()
    {
        float fixedMovement = -2f;
        float spineYRange = 30;

        leftHandMovement = Random.Range(leftHandMovement + fixedMovement, leftHandMovement);
        rightHandMovement = Random.Range(rightHandMovement + fixedMovement, rightHandMovement);
        spineRotateY = Random.Range(-spineYRange, spineYRange);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_EnemyLife.isDead)
        {
            leftHandMovement = _EnemyLife.deadByGun ? leftHandMovement : -leftHandMovement;
            rightHandMovement = _EnemyLife.deadByGun ? rightHandMovement : -rightHandMovement;

            spineRotateX = _EnemyLife.deadByGun ? spineRotateX : -spineRotateX;

            Vector3 leftHandPos = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            Vector3 rightHandPos = animator.GetIKPosition(AvatarIKGoal.RightHand);
            Vector3 spineRotation = animator.GetBoneTransform(HumanBodyBones.Spine).eulerAngles;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

            leftHandPos += transform.forward * leftHandMovement + transform.up * leftHandMovement;
            rightHandPos += transform.forward * rightHandMovement + transform.up * rightHandMovement;
            spineRotation += transform.right * spineRotateX + transform.up * spineRotateY;

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
            animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(spineRotation));

            if (!isMotionplayed)
            {
                bodyParts.GetComponent<BrokenEffects>().ShowBrokenParts();
            }
            isMotionplayed = true;
        }
    }
}
