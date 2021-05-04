using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public GameObject Camera;

    private PlayerState playerState;
    private float mouseY;
    private float mouseX;
    private float cameraX;
    private readonly float rotSpeed = 3;

    public float CameraX
    {
        get
        {
            cameraX = cameraX <= 0 ? cameraX + 360 : cameraX;
            cameraX = cameraX > 360 ? cameraX - 360 : cameraX;

            if (cameraX > 30 && cameraX < 180)
            {
                cameraX = 30;
            }
            else if (cameraX < 330 && cameraX > 180)
            {
                cameraX = 330;
            }
            return cameraX;
        }
        set
        {
            cameraX = value;
        }
    }

    private void Awake()
    {
        playerState = PlayerState.GetInstance();
    }

    private void Update()
    {
        GetInput();
        CheckRotate();
    }

    private void FixedUpdate()
    {
        RotatePlayer();
        CameraMove();
    }

    private void GetInput()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    private void RotatePlayer()
    {
        gameObject.transform.Rotate(Vector3.up * rotSpeed * mouseX);
    }

    private void CameraMove()
    {
        var cameraRot = gameObject.transform.eulerAngles;

        CameraX += -mouseY * rotSpeed;
        Camera.transform.eulerAngles = new Vector3(CameraX, cameraRot.y, cameraRot.z);
    }

    private void CheckRotate()
    {
        if (mouseX != 0 || mouseY != 0)
        {
            playerState.isRotate = true;
        }
        else
        {
            playerState.isRotate = false;
        }
    }
}
