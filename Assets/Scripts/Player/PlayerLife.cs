using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public GameObject brokenEffect;
    public GameObject mainCamera;
    public GameObject timeManager;

    public void DeadMotion()
    {
        PlayerState.GetInstance().isActing = false;
        PlayerState.GetInstance().isRotate = false;

        timeManager.SetActive(false);

        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        brokenEffect.SetActive(true);
        Destroy(gameObject.GetComponent<CapsuleCollider>());

        brokenEffect.transform.SetParent(null);
        mainCamera.transform.SetParent(null);

        mainCamera.AddComponent<BoxCollider>();
        mainCamera.AddComponent<Rigidbody>();

        gameObject.SetActive(false);

        if (PlayerState.GetInstance().isHolding)
        {
            Gun gun = PlayerState.GetInstance().holdingObj.GetComponent<Gun>();
            gun.isHolded = false;
            gun.ThrowGunByAttack();
        }
    }
}
