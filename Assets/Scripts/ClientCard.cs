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

    public Image face;
    public Image ear;
    public Image mouth;
    public Image chin;
    public Image eye;
    public Image nose;
    public Image eyebrow;

    private void Start()
    {
        clientListTransform = GameObject.Find("ClientList").transform;
        if (client == null) client = Client.GenerateRandomClient(clientListTransform);
        client.card = this;

        clientNameText.text = client.name;

        face.sprite = client.face;
        ear.sprite = client.ear;
        nose.sprite = client.nose;
        mouth.sprite = client.mouth;
        eyebrow.sprite = client.eyebrows;
        chin.sprite = client.chin;
    }

    private void Update()
    {
        clientTimerText.text = Client.StringifyTime(client.currentTime);

        if (client.currentTime < 0)
        {
            AudioSource.PlayClipAtPoint(GameManager.Instance.vineBoom, Vector3.zero);
            GameManager.Instance.clientRatings.Add(0);
            GameManager.Instance.alertText.text = client.name + " left your salon and gave you a rating of 0";

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
