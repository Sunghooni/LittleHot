using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    private PlayerState playerState;

    private int vert;
    private int horz;

    private const int moveSpeed = 3;
    private const int motionChangeSpeed = 3;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        playerState = PlayerState.GetInstance();
    }

    private void Update()
    {
        GetInput();
        WalkMotion();
        AimWalkMotion();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        vert = int.Parse(Input.GetAxisRaw("Vertical").ToString());
        horz = int.Parse(Input.GetAxisRaw("Horizontal").ToString());
    }

    private void MovePlayer()
    {
        Vector3 movePos = new Vector3(horz, 0, vert).normalized;
        gameObject.transform.Translate(movePos * moveSpeed * Time.deltaTime);
    }

    private void WalkMotion()
    {
        if (playerState.isHolding == false)
        {
            if (vert != 0 || horz != 0)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }
    }

    private void AimWalkMotion()
    {
        float progress = animator.GetFloat("AimWalk");
        animator.SetBool("Holding", playerState.isHolding);

        if (playerState.isHolding == true)
        {
            if (vert == 0 && progress > 0f)
            {
                animator.SetFloat("AimWalk", progress - Time.deltaTime * motionChangeSpeed);
            }
            if (vert != 0 && progress < 1f)
            {
                animator.SetFloat("AimWalk", progress + Time.deltaTime * motionChangeSpeed);
            }
        }
    }
}