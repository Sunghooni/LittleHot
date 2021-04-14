using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    private PlayerState playerState;

    private int vert;
    private int horz;
    private float mouseX;

    private const int moveSpeed = 2;
    private const int rotSpeed = 2;
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
        RotatePlayer();
    }

    private void GetInput()
    {
        vert = int.Parse(Input.GetAxisRaw("Vertical").ToString());
        horz = int.Parse(Input.GetAxisRaw("Horizontal").ToString());
        mouseX = Input.GetAxisRaw("Mouse X");
    }

    private void MovePlayer()
    {
        int vertSpeed = vert * moveSpeed;
        int horzSpeed = horz * moveSpeed;

        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * vertSpeed);
        gameObject.transform.Translate(Vector3.right * Time.deltaTime * horzSpeed);
    }

    private void RotatePlayer()
    {
        gameObject.transform.Rotate(Vector3.up * rotSpeed * mouseX);
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