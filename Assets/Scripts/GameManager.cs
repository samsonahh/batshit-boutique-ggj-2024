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
    public GameObject alertPanel;
    public GameObject namingCanvas;
    public GameObject clientSelectCanvas;
    public TMP_Text clientSelectSalonName;
    public TMP_Text clientSelectRatingText;
    public Transform clientSelectContentTransform;
    public GameObject gameplayCanvas;
    public TMP_Text clientGameplayNameText;
    public TMP_Text clientGameplayTimerText;
    public Transform iconsParent;

    [Header("Salon Settings")]
    public string salonName;
    public bool b_gameStarted;

    public GameObject clientCardPrefab;
    public string[] possibleNames;
    public bool b_isWorkingOnClient;
    public Client currentClient;

    public string[] toolNames;

    public Sprite[] perfectFaceSprites;
    public Sprite[] faceSprites;
    public Sprite[] eyeSprites;
    public Sprite[] eyebrowSprites;
    public Sprite[] mouthSprites;
    public Sprite[] earSprites;
    public Sprite[] noseSprites;
    public Sprite[] chinSprites;

    public Image clientFace;
    public Image clientEye;
    public Image clientEyebrow;
    public Image clientMouth;
    public Image clientEar;
    public Image clientNose;
    public Image clientChin;

    public GameObject penaltyTextPrefab;

    public bool b_isDraggingTool;
    public bool b_canDragTool;
    public bool b_isOverFace;

    public float rating;
    public List<float> clientRatings;
    public int satisfiedClients;

    [Header("Audio")]
    public AudioClip vineBoom;
    public AudioClip correctSound;
    public AudioClip handsawSound;
    public AudioClip jackHammerSound;
    public AudioClip lawnMowerSound;
    public AudioClip sparkleSound;
    public AudioClip needleSound;
    public AudioClip iceSound;
    public AudioClip bellSound;
    public AudioClip poofSound;


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
        alertPanel.SetActive(false);

        // Makes sure the game isnt started yet until the player names the salon
        b_gameStarted = false;
        b_isWorkingOnClient = false;

        alertText.text = "";

        possibleNames = new string[] { "Deez N.", "Hugh G. Rection", "Steve Diamondpick", "Steve Steve", "Charlie Cheese", "Ieluv Ore", "Kranken Nein Dees", "Bertha Khunt", "Terry Tootits", "Pablo Poopenfarten", "Stinky McBinkleskin", "The Less Than Long Johnson", "Baskin The Bob Robin", "Tomasz", "Schmorgasboard Henry", "Stanley Peenspleen", "Vicky Yaborger", "Krangis Mcbasketball", "Jack Bubigbottomhank", "Michael Hamsandwich", "Al Beback", "Anita Bath", "Bea O'Problem", "Ben Dover", "Bill Board", "Brock Lee", "Ben Dover", "Chris P. Bacon", "Crystal Clear", "Dee Zyner", "Drew Peacock", "Ella Vator", "Frank N. Stein", "Gail Forcewind", "Hugh Jass", "Ima Pigg", "Ivana Tinkle", "Justin Time", "Ken Tucky", "Les Ismore", "Lou Natic", "Luke Warm", "Moe Lester", "Neil Down", "Paige Turner", "Pat Myback", "Ray N. Carnation", "Rob Banks", "Rusty Nail", "Seymour Butz", "Stan Still", "Tess Tickle", "Upton O'Good", "Wayne Dwops", "Will Power", "Yul B. Next", "Al O'Moaney", "Al Bequerque", "Barb Dwyer", "Beau Vine", "Brock O'Bama", "Cam O'Flage", "Candi Barr", "Dee Sember", "Don Keigh", "Ella Vator", "Fran Tick", "Gene Poole", "Gail Forcewind" };

        toolNames = new string[] { "FloorBuffer", "Chisel", "Handsaw", "Needle", "MagicWand", "IcePack" };
    }

    private void Update()
    {
        if (!b_gameStarted) return;

        rating = CalculateFinalRating();
        clientSelectRatingText.text = "Rating: " + (Mathf.Round(rating * 10f)/10f).ToString() + "/5" + ", Satisfied Clients: " + satisfiedClients;

        if(rating >= 4.5f && satisfiedClients >= 15)
        {
            SceneManager.LoadScene("End");
        }

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
        alertPanel.SetActive(true);

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
            AudioSource.PlayClipAtPoint(bellSound, Vector3.zero);

            float randomWaitTime = Random.Range(1, 15);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    public void StartClientMakeover(Client client)
    {
        currentClient = client;

        clientSelectCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);

        TurnIconsInvisible();
        FacialFeaturesEnabled(true);

        b_isWorkingOnClient = true;
        b_canDragTool = true;
    }

    void HandleClientMakeOverGameplay()
    {
        if (!b_isWorkingOnClient) return;

        clientGameplayNameText.text = currentClient.name;
        clientGameplayTimerText.text = Client.StringifyTime(currentClient.currentTime);

        clientFace.sprite = currentClient.face;
        clientEye.sprite = currentClient.eye;
        clientEyebrow.sprite = currentClient.eyebrows;
        clientMouth.sprite = currentClient.mouth;
        clientNose.sprite = currentClient.nose;
        clientEar.sprite = currentClient.ear;
        clientChin.sprite = currentClient.chin;

        foreach(string tool in currentClient.requiredTools)
        {
            Image iconImage = GameObject.Find(tool + "Icon").GetComponent<Image>();
            iconImage.color = Color.white;
        }

        // when you run out of time
        if(currentClient.currentTime < 0)
        {
            clientSelectCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);

            b_isDraggingTool = false;
            b_isWorkingOnClient = false;
        }
    }

    public void ApplyMakeup(string toolName)
    {
        if (!currentClient.requiredTools.Contains(toolName))
        {
            AudioSource.PlayClipAtPoint(vineBoom, Vector3.zero);
            Instantiate(penaltyTextPrefab, gameplayCanvas.transform);
            currentClient.currentTime -= 2f;
            return;
        }

        currentClient.requiredTools.Remove(toolName);

        Image iconImage = GameObject.Find(toolName + "Icon").GetComponent<Image>();
        iconImage.color = Color.clear;

        PlayToolSound(toolName);
        ReplaceFacePart(toolName);

        if(currentClient.requiredTools.Count == 0)
        {
            StartCoroutine(FinishClient());
            return;
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
        return Mathf.Round((currentClient.currentTime / currentClient.timeLimit) * 4f * 10f)/10f + 1f;
    }

    public void ReplaceFacePart(string toolName)
    {
        switch (toolName)
        {
            case "FloorBuffer":
                currentClient.eye = eyeSprites[0];
                break;
            case "Chisel":
                currentClient.eyebrows = eyebrowSprites[0];
                break;
            case "Handsaw":
                currentClient.chin = chinSprites[0];
                break;
            case "Needle":
                currentClient.mouth = mouthSprites[0];
                break;
            case "MagicWand":
                currentClient.nose = noseSprites[0];
                break;
            case "IcePack":
                currentClient.ear = earSprites[0];
                break;
            case null:
                break;
        }
    }

    IEnumerator FinishClient()
    {
        b_canDragTool = false;
        
        yield return new WaitForSeconds(0.25f);

        AudioSource.PlayClipAtPoint(poofSound, Vector3.zero);

        yield return new WaitForSeconds(0.75f);

        AudioSource.PlayClipAtPoint(correctSound, Vector3.zero);

        float rating = CalculateRating();
        clientRatings.Add(rating);
        alertText.text = currentClient.name + " left your salon and gave you a rating of " + (rating).ToString();

        FacialFeaturesEnabled(false);
        currentClient.face = perfectFaceSprites[currentClient.faceIndex];

        yield return new WaitForSeconds(2);

        clientSelectCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);

        b_isDraggingTool = false;
        b_isWorkingOnClient = false;

        Destroy(currentClient.card.gameObject);
        Destroy(currentClient.gameObject);
        currentClient = null;

        satisfiedClients++;
    }

    public void ChangeIsOverFaceBool(bool b)
    {
        b_isOverFace = b;
    }

    public void TurnIconsInvisible()
    {
        for(int i = 0; i < iconsParent.childCount; i++)
        {
            iconsParent.GetChild(i).GetComponent<Image>().color = Color.clear;
        }
    }

    public void PlayToolSound(string toolName)
    {
        switch (toolName)
        {
            case "FloorBuffer":
                AudioSource.PlayClipAtPoint(lawnMowerSound, Vector3.zero);
                break;
            case "Chisel":
                AudioSource.PlayClipAtPoint(jackHammerSound, Vector3.zero);
                break;
            case "Handsaw":
                AudioSource.PlayClipAtPoint(handsawSound, Vector3.zero);
                break;
            case "Needle":
                AudioSource.PlayClipAtPoint(needleSound, Vector3.zero);
                break;
            case "MagicWand":
                AudioSource.PlayClipAtPoint(sparkleSound, Vector3.zero);
                break;
            case "IcePack":
                AudioSource.PlayClipAtPoint(iceSound, Vector3.zero);
                break;
            case null:
                break;
        }
    }

    public void FacialFeaturesEnabled(bool b)
    {
        clientEar.gameObject.SetActive(b);
        clientEye.gameObject.SetActive(b);
        clientNose.gameObject.SetActive(b);
        clientMouth.gameObject.SetActive(b);
        clientEyebrow.gameObject.SetActive(b);
        clientChin.gameObject.SetActive(b);
    }
}
