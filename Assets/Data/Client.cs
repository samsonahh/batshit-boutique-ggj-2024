using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client: MonoBehaviour
{
    public new string name;
    public float timeLimit; // in seconds
    public float currentTime; // in seconds

    public Sprite photo;

    private void Update()
    {
        currentTime -= Time.deltaTime;
    }

    public static Client GenerateRandomClient()
    {
        Client randomClient = new Client();

        randomClient.name = GameManager.Instance.possibleNames[Random.Range(0, GameManager.Instance.possibleNames.Length)];
        randomClient.timeLimit = Random.Range(15, 300);
        randomClient.currentTime = randomClient.timeLimit;

        return randomClient;
    }
}
