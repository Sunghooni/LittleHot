using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingOperate : MonoBehaviour
{
    public string text;
    public float delay;
    private string currentText;
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= text.Length; i++)
        {
            currentText = text.Substring(0, i);
            tmp.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
