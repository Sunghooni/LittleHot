using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrl : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public GameObject ShotRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.transform.TryGetComponent(out Gun gun))
            {
                gun.HoldedToHand();
                return hit.transform.gameObject;
            }
        }

        return null;
    }
}
