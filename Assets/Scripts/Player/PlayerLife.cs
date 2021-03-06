using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public HeighLightText heighlightText;
    public GameObject brokenEffect;
    public GameObject mainCamera;
    public GameObject timeManager;

    public void DeadMotion()
    {
        AudioManager.instance.PlaySound("BrokenSFX", mainCamera);
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

        Invoke(nameof(ShowIgnoreText), 1f);
        Invoke(nameof(ReloadScene), 1.1f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    private void ShowIgnoreText()
    {
        heighlightText.StartShowText(heighlightText.focusOnIdiot);
    }
}
