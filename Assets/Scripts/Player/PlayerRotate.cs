using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public GameObject Camera;
    public bool isReplaying = false;
    public float mouseY;
    public float mouseX;

    private PlayerState playerState;
    private float cameraX;
    private const float playerRotSpeed = 2;
    private const float cameraRotSpeed = 1f;

    private void Awake()
    {
        playerState = PlayerState.GetInstance();
    }

    private void Update()
    {
        if (!isReplaying)
        {
            GetInput();
        }
        CheckRotate();
    }

    private void FixedUpdate()
    {
        if (!isReplaying)
        {
            RotatePlayer();
            CameraMove();
        }
    }

    private void GetInput()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    private void RotatePlayer()
    {
        gameObject.transform.Rotate(Vector3.up * playerRotSpeed * mouseX);
    }

    private void CameraMove()
    {
        float maxRange = 30f;
        float minRange = -40f;

        cameraX += -mouseY * cameraRotSpeed;
        cameraX = Mathf.Clamp(cameraX, minRange, maxRange);
        Camera.transform.localEulerAngles = new Vector3(cameraX, 0, 0);
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
