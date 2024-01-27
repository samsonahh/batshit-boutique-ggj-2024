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
        client.card = this;

        clientNameText.text = client.name;
        clientImage.sprite = client.photo;
    }

    private void Update()
    {
        clientTimerText.text = Client.StringifyTime(client.currentTime);

        if (client.currentTime < 0)
        {
            GameManager.Instance.clientRatings.Add(0);
            GameManager.Instance.alertText.text = client.name + " left your salon and gave you a rating of 0";
            GameManager.Instance.alertText.color = Color.red;

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
