using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Client: MonoBehaviour
{
    public ClientCard card;

    public new string name;
    public float timeLimit; // in seconds
    public float currentTime; // in seconds
    public List<string> toolRequestOrder;

    public Sprite photo;
    public Sprite face;
    public Sprite ear;
    public Sprite mouth;
    public Sprite chin;
    public Sprite hair;
    public Sprite nose;
    public Sprite eye;
    public Sprite eyebrows;

    private void Update()
    {
        currentTime -= Time.deltaTime;
    }

    public static Client GenerateRandomClient(Transform clientListParent)
    {
        GameObject tempObj = new GameObject("Client Object");
        tempObj.AddComponent<Client>();

        GameObject clientObject = Instantiate(tempObj, clientListParent);
        Destroy(tempObj);

        Client randomClient = clientObject.GetComponent<Client>();

        randomClient.name = GameManager.Instance.possibleNames[UnityEngine.Random.Range(0, GameManager.Instance.possibleNames.Length)];
        randomClient.timeLimit = UnityEngine.Random.Range(15, 60);
        randomClient.currentTime = randomClient.timeLimit;
        
        

        clientObject.name = randomClient.name;

        return randomClient;
    }

    public static string StringifyTime(float time)
    {
        return string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(time).Minutes, TimeSpan.FromSeconds(time).Seconds);
    }
}
