using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Menus")]
    public TMP_Text alertText;
    public GameObject namingCanvas;
    public GameObject clientSelectCanvas;
    public TMP_Text clientSelectSalonName;
    public TMP_Text clientSelectRatingText;
    public Transform clientSelectContentTransform;
    public GameObject gameplayCanvas;
    public TMP_Text clientGameplayNameText;
    public TMP_Text clientGameplayTimerText;
    public TMP_Text clientPromptText;

    [Header("Salon Settings")]
    public string salonName;
    public bool b_gameStarted;

    public GameObject clientCardPrefab;
    public string[] possibleNames;
    public bool b_isWorkingOnClient;
    public Client currentClient;
    public string currentClientRequest;

    List<string> clientParts;
    public Dictionary<string, string[]> possiblePrompts;
    public string[] toolNames;

    public Sprite floorBufferSprite;
    public Sprite chiselSprite;
    public Sprite handsawSprite;
    public Sprite needleSprite;
    public Sprite magicWandSprite;
    public Sprite icePackSprite;

    public bool b_isDraggingTool;
    public string selectedTool;

    public float rating;
    public List<float> clientRatings;
    public int satisfiedClients;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Start with the player naming the salon
        namingCanvas.SetActive(true);
        clientSelectCanvas.SetActive(false);
        gameplayCanvas.SetActive(false);

        // Makes sure the game isnt started yet until the player names the salon
        b_gameStarted = false;
        b_isWorkingOnClient = false;

        possibleNames = new string[] { "Deez N.", "Hughe G. Rection", "Steve Diamondpick", "Steve Steve", "Charlie Cheese", "Ieluv Ore", "Kranken Nein Dees", "Bertha Khunt", "Terry Tootits", "Pablo Poopenfarten", "Stinky McBinkleskin", "The Less Than Long Johnson", "Baskin The Bob Robin", "Tomasz", "Schmorgasboard Henry", "Stanley Peenspleen", "Vicky Yaborger", "Krangis Mcbasketball", "Jack Bubigbottomhank", "Michael Hamsandwich", "Al Beback", "Anita Bath", "Bea O'Problem", "Ben Dover", "Bill Board", "Brock Lee", "Ben Dover", "Chris P. Bacon", "Crystal Clear", "Dee Zyner", "Drew Peacock", "Ella Vator", "Frank N. Stein", "Gail Forcewind", "Hugh Jass", "Ima Pigg", "Ivana Tinkle", "Justin Time", "Ken Tucky", "Les Ismore", "Lou Natic", "Luke Warm", "Moe Lester", "Neil Down", "Paige Turner", "Pat Myback", "Ray N. Carnation", "Rob Banks", "Rusty Nail", "Seymour Butz", "Stan Still", "Tess Tickle", "Upton O'Good", "Wayne Dwops", "Will Power", "Yul B. Next", "Al O'Moaney", "Al Bequerque", "Barb Dwyer", "Beau Vine", "Brock O'Bama", "Cam O'Flage", "Candi Barr", "Dee Sember", "Don Keigh", "Ella Vator", "Fran Tick", "Gene Poole", "Gail Forcewind" };

        possiblePrompts = new Dictionary<string, string[]>()
        {
            { "FloorBuffer", new string[] { "My poor face is too bumpy. Smooth it out!" } },
            { "Chisel", new string[] { "I’m looking for a more defined look. Make me as chiseled as a Greek god!" } },
            { "Handsaw", new string[] { "I’ve got a hot date at 5. I wanna look sharp." } },
            { "Needle", new string[] { "EMERGENCY QUICK FIX NOW!" } },
            { "MagicWand", new string[] { "I’m feeling spontaneous. Surprise me." } },
            { "IcePack", new string[] { "OW MY FACE! HELP ME!" } },
        };

        toolNames = new string[] { "FloorBuffer", "Chisel", "Handsaw", "Needle", "MagicWand", "IcePack" };

        ResetClientParts();
    }

    private void Update()
    {
        if (!b_gameStarted) return;

        rating = CalculateFinalRating();
        clientSelectRatingText.text = "Rating: " + (Mathf.Round(rating * 10f)/10f).ToString() + "/5";

        if (!b_isWorkingOnClient) return;

        HandleClientMakeOverGameplay();
    }

    public void StartGame()
    {
        string inputName = namingCanvas.GetComponentInChildren<TMP_InputField>().text;

        if (!IsNameValid(inputName)) return;

        salonName = inputName;

        namingCanvas.SetActive(false);
        clientSelectCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);

        clientSelectSalonName.text = salonName;
        b_gameStarted = true;

        StartCoroutine(ClientSpawnHandler());
    }

    private bool IsNameValid(string name)
    {
        if (name.Length < 1) return false;

        if (name.Length > 45) return false;

        return true;
    }

    private IEnumerator ClientSpawnHandler()
    {
        while (b_gameStarted)
        {
            Instantiate(clientCardPrefab, clientSelectContentTransform);

            float randomWaitTime = Random.Range(10, 20);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    public void StartClientMakeover(Client client)
    {
        currentClient = client;

        clientSelectCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);

        b_isWorkingOnClient = true;
    }

    void HandleClientMakeOverGameplay()
    {
        if (!b_isWorkingOnClient) return;

        clientGameplayNameText.text = currentClient.name;
        clientGameplayTimerText.text = Client.StringifyTime(currentClient.currentTime);

        // when you run out of time
        if(currentClient.currentTime < 0)
        {
            clientSelectCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);

            ResetClientParts();
            b_isDraggingTool = false;
            b_isWorkingOnClient = false;
        }
    }

    public void ApplyRandomMakeup(string toolName)
    {
        int randomIndex = Random.Range(0, clientParts.Count);

        GameObject randomPart = GameObject.Find(clientParts[randomIndex]);

        clientParts.RemoveAt(randomIndex);

        if(clientParts.Count == 1)
        {
            clientSelectCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);

            float rating = CalculateRating();
            clientRatings.Add(rating);
            alertText.text = currentClient.name + " left your salon and gave you a rating of " + (rating).ToString();
            alertText.color = Color.green;

            ResetClientParts();
            b_isDraggingTool = false;
            b_isWorkingOnClient = false;

            Destroy(currentClient.card.gameObject);
            Destroy(currentClient.gameObject);
            currentClient = null;

            satisfiedClients++;
        }
    }

    public float CalculateFinalRating()
    {
        float totalClientsRated = clientRatings.Count;

        if (totalClientsRated < 1) return 0; // divide by zero proof

        float totalRating = 0f;
        foreach(float rating in clientRatings)
        {
            totalRating += rating;
        }

        return totalRating / totalClientsRated;
    }

    public float CalculateRating()
    {
        return Mathf.Round((currentClient.currentTime / currentClient.timeLimit) * 5f * 10f)/10f;
    }

    public void ResetClientParts()
    {
        clientParts = new List<string> { "ClientFace", "ClientEar", "ClientMouth", "ClientChin", "ClientHair", "ClientNose", "ClientEye", "ClientEyebrows" };
    }
}
