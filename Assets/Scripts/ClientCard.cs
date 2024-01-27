using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientCard : MonoBehaviour
{
    public TMP_Text clientName;
    public TMP_Text clientTimer;
    public Image clientImage;

    private void Update()
    {
        clientName.text = "Shrek";
        clientTimer.text = "0:00";
    }

    public void AcceptClient()
    {
        Debug.Log(clientName.text + " ACCEPTED");
    }
}
