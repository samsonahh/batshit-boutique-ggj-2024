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

    [Header("Salon Settings")]
    public string salonName;
    public bool b_gameStarted;

    public GameObject clientCardPrefab;
    public string[] possibleNames;
    public bool b_isWorkingOnClient;
    public Client currentClient;
    public string currentClientRequest;

    public Button clientFace;
    public Button clientEyes;
    public Button clientNose;
    public Button clientMouth;
    public Button clientEyebrows;
    public Button clientChin;
    public Button clientHair;
    public Button clientEars;
    List<string> clientParts;

    public Sprite floorBufferSprite;
    public Sprite chiselSprite;
    public Sprite handsawSprite;
    public Sprite needleSprite;
    public Sprite magicWandSprite;
    public Sprite icePackSprite;
    Dictionary<string, Sprite> toolSpritesDict;

    public bool b_isSelectingTool;
    public string selectedTool;
    public Image selectedToolSprite;

    public float rating;
    public List<float> clientRatings;



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

        // Prepopulate tool sprites and names
        toolSpritesDict = new Dictionary<string, Sprite>()
        {
            { "Floor Buffer", floorBufferSprite },
            { "Chisel", chiselSprite},
            { "Handsaw", handsawSprite},
            { "Needle", needleSprite},
            { "Magic Wand", magicWandSprite},
            { "Ice Pack", icePackSprite}
        };

        possibleNames = new string[] { "Deez N.", "Hughe G. Rection", "Steve Diamondpick", "Steve Steve", "Charlie Cheese", "Ieluv Ore", "Kranken Nein Dees", "Bertha Khunt", "Terry Tootits", "Pablo Poopenfarten", "Stinky McBinkleskin", "The Less Than Long Johnson", "Baskin The Bob Robin", "Tomasz", "Schmorgasboard Henry", "Stanley Peenspleen", "Vicky Yaborger", "Krangis Mcbasketball", "Jack Bubigbottomhank", "Michael Hamsandwich", "Al Beback", "Anita Bath", "Bea O'Problem", "Ben Dover", "Bill Board", "Brock Lee", "Ben Dover", "Chris P. Bacon", "Crystal Clear", "Dee Zyner", "Drew Peacock", "Ella Vator", "Frank N. Stein", "Gail Forcewind", "Hugh Jass", "Ima Pigg", "Ivana Tinkle", "Justin Time", "Ken Tucky", "Les Ismore", "Lou Natic", "Luke Warm", "Moe Lester", "Neil Down", "Paige Turner", "Pat Myback", "Ray N. Carnation", "Rob Banks", "Rusty Nail", "Seymour Butz", "Stan Still", "Tess Tickle", "Upton O'Good", "Wayne Dwops", "Will Power", "Yul B. Next", "Al O'Moaney", "Al Bequerque", "Barb Dwyer", "Beau Vine", "Brock O'Bama", "Cam O'Flage", "Candi Barr", "Dee Sember", "Don Keigh", "Ella Vator", "Fran Tick", "Gene Poole", "Gail Forcewind" };

        ResetClientParts();
    }

    private void Update()
    {
        if (!b_gameStarted) return;

        rating = CalculateRating();
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

            clientRatings.Add(0);
            alertText.text = currentClient.name + " left your salon and gave you a rating of 0";

            ResetClientParts();
            b_isSelectingTool = false;
            b_isWorkingOnClient = false;
        }

        HandleSelectedToolSprite();
    }

    public void SetSelectedTool(string tool)
    {
        b_isSelectingTool = true;
        selectedTool = tool;

        selectedToolSprite.sprite = toolSpritesDict[selectedTool];
    }

    void HandleSelectedToolSprite()
    {
        selectedToolSprite.gameObject.SetActive(b_isSelectingTool);
        Cursor.visible = !b_isSelectingTool;

        selectedToolSprite.transform.position = Vector2.Lerp(selectedToolSprite.transform.position, Input.mousePosition, 40 * Time.deltaTime);
    }

    public float CalculateRating()
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

    public void OnFacePartPressed(string part)
    {
        if (!b_isSelectingTool)
        {
            Debug.Log("Youre not holding a tool");
            return;
        }

        for(int i = 0; i < clientParts.Count; i++)
        {
            string clientPart = clientParts[i];

            if (part == clientPart)
            {
                int randomIndex = Random.Range(0, clientParts.Count);
                Debug.Log(clientParts[randomIndex]);
                // change the sprite of clientParts[randomIndex];
                switch (clientParts[randomIndex])
                {
                    case "Face":
                        clientFace.interactable = false;
                        break;
                    case "Ear":
                        clientEars.interactable = false;
                        break;
                    case "Mouth":
                        clientMouth.interactable = false;
                        break;
                    case "Chin":
                        clientChin.interactable = false;
                        break;
                    case "Hair":
                        clientHair.interactable = false;
                        break;
                    case "Nose":
                        clientNose.interactable = false;
                        break;
                    case "Eye":
                        clientEyes.interactable = false;
                        break;
                    case "Eyebrow":
                        clientEyebrows.interactable = false;
                        break;
                    case null:
                        break;
                }
                clientParts.RemoveAt(randomIndex);
                b_isSelectingTool = false;
            }
        }
    }

    public void ResetClientParts()
    {
        MakeClientPartsInteractable(true);

        clientParts = new List<string> { "Face", "Ear", "Mouth", "Chin", "Hair", "Nose", "Eye", "Eyebrow" };
    }

    public void MakeClientPartsInteractable(bool b)
    {
        clientFace.interactable = b;
        clientEars.interactable = b;
        clientMouth.interactable = b;
        clientChin.interactable = b;
        clientHair.interactable = b;
        clientNose.interactable = b;
        clientEyes.interactable = b;
        clientEyebrows.interactable = b;
    }
}
