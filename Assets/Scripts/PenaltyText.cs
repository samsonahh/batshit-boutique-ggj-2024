using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PenaltyText : MonoBehaviour
{
    TMP_Text text;
    float timer = 0;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (!GameManager.Instance.b_isWorkingOnClient) Destroy(gameObject);
        timer += Time.deltaTime;

        if (timer > 1f) Destroy(gameObject);

    }
}
