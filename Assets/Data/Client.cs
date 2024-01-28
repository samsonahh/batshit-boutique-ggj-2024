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
    public List<string> requiredTools;

    public Sprite photo;
    public Sprite ear;
    public Sprite mouth;
    public Sprite chin;
    public Sprite nose;
    public Sprite eye;
    public Sprite eyebrows;

    private void Start()
    {
    }

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
        randomClient.timeLimit = UnityEngine.Random.Range(30, 60);
        randomClient.currentTime = randomClient.timeLimit;
        randomClient.requiredTools = new List<string>();

        int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.eyeSprites.Length);
        randomClient.eye = GameManager.Instance.eyeSprites[randomIndex];
        if(randomIndex != 0) randomClient.requiredTools.Add("FloorBuffer");

        randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.eyebrowSprites.Length);
        randomClient.eyebrows = GameManager.Instance.eyebrowSprites[randomIndex];
        if (randomIndex != 0) randomClient.requiredTools.Add("Chisel");

        randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.chinSprites.Length);
        randomClient.chin = GameManager.Instance.chinSprites[randomIndex];
        if (randomIndex != 0) randomClient.requiredTools.Add("Handsaw");

        randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.mouthSprites.Length);
        randomClient.mouth = GameManager.Instance.mouthSprites[randomIndex];
        if (randomIndex != 0) randomClient.requiredTools.Add("Needle");

        randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.noseSprites.Length);
        randomClient.nose = GameManager.Instance.noseSprites[randomIndex];
        if (randomIndex != 0) randomClient.requiredTools.Add("MagicWand");

        randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.earSprites.Length);
        randomClient.ear = GameManager.Instance.earSprites[randomIndex];
        if (randomIndex != 0) randomClient.requiredTools.Add("IcePack");

        clientObject.name = randomClient.name;

        return randomClient;
    }

    public static string StringifyTime(float time)
    {
        return string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(time).Minutes, TimeSpan.FromSeconds(time).Seconds);
    }
}
