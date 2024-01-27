using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClientCard : MonoBehaviour
{
    private Client client;

    public TMP_Text clientNameText;
    public TMP_Text clientTimerText;
    public Image clientImage;

    private void Start()
    {
        if (client == null) client = Client.GenerateRandomClient();

        clientNameText.text = client.name;
        clientImage.sprite = client.photo;
    }

    private void Update()
    {
        clientTimerText.text = string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(client.currentTime).Minutes, TimeSpan.FromSeconds(client.currentTime).Seconds);

        if (client.currentTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void AcceptClient()
    {
        Debug.Log(clientNameText.text + " CLICKED");

        if (GameManager.Instance.b_isWorkingOnClient) return;

        GameManager.Instance.StartClientMakeover(client);
    }
}
