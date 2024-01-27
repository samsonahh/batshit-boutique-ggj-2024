using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientCard : MonoBehaviour
{
    private Client client;
    private Transform clientListTransform;

    public TMP_Text clientNameText;
    public TMP_Text clientTimerText;
    public Image clientImage;

    private void Start()
    {
        clientListTransform = GameObject.Find("ClientList").transform;
        if (client == null) client = Client.GenerateRandomClient(clientListTransform);

        clientNameText.text = client.name;
        clientImage.sprite = client.photo;
    }

    private void Update()
    {
        clientTimerText.text = Client.StringifyTime(client.currentTime);

        if (client.currentTime < 0)
        {
            Destroy(client.gameObject);
            Destroy(gameObject);
        }
    }

    public void AcceptClient()
    {
        if (GameManager.Instance.b_isWorkingOnClient) return;

        GameManager.Instance.StartClientMakeover(client);
    }
}
