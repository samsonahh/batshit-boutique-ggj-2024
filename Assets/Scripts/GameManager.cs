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
    public GameObject namingCanvas;
    public GameObject clientSelectCanvas;
    public TMP_Text clientSelectSalonName;
    public GameObject gameplayCanvas;

    [Header("Salon Settings")]
    public string salonName;
    public bool b_gameStarted;



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
    }

    private void Update()
    {
        if (!b_gameStarted) return;
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
    }

    private bool IsNameValid(string name)
    {
        if (name.Length < 1) return false;

        if (name.Length > 15) return false;

        return true;
    }
}
