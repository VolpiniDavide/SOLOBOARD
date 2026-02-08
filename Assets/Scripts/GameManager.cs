using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        RoundStart,
        Playing,
        RoundEnd,
        Shop
    }

    [Header("Round Settings")]
    [SerializeField] private float roundDuration = 60f;

    [Header("References")]
    [SerializeField] private WordGridSpawner gridSpawner;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("UI Panels")]
    [SerializeField] private GameObject keyboardPanel;
    [SerializeField] private GameObject shopPanel;

    [Header("Timer Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color dangerColor = Color.red;

    [Header("Display")]
    public TextMeshProUGUI screenText;

    [Header("Keyboard")]
    public KeyboardHandler keyboardHandler;

    [Header("Test Words")]
    public WordData subject;
    public WordData predicate;
    public WordData obj;

    // STATE
    private GameState currentState = GameState.None;
    private int currentRound = 0;
    public int CurrentRound => currentRound;
    public GameState CurrentState => currentState;


    // TIMER
    private float timer;
    private int lastShownSecond;

    private void Start()
    {
        ChangeState(GameState.RoundStart);
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            UpdateTimer();
        }
    }

    // =========================
    // STATE MACHINE
    // =========================

    private void ChangeState(GameState newState)
    {
        if (currentState == newState)
            return;

        ExitState(currentState);
        currentState = newState;
        EnterState(currentState);
    }

    private void EnterState(GameState state)
    {
        Debug.Log($"ENTER STATE: {state}");

        switch (state)
        {
            case GameState.RoundStart:
                StartNewRound();
                break;

            case GameState.Playing:
                StartTimer();
                break;

            case GameState.RoundEnd:
                HandleRoundEnd();
                break;

            case GameState.Shop:
                OpenShop();
                break;
        }
    }

    private void ExitState(GameState state)
    {
        Debug.Log($"EXIT STATE: {state}");

        switch (state)
        {
            case GameState.Playing:
                // qui in futuro: bloccare input, fermare suoni, ecc
                break;
        }
    }

    // =========================
    // ROUND LOGIC
    // =========================

    private void StartNewRound()
    {
        currentRound++;
        Debug.Log($"ROUND {currentRound} START");

        keyboardPanel.SetActive(true);
        shopPanel.SetActive(false);

        gridSpawner.GenerateGrid();

        ChangeState(GameState.Playing);
    }

    private void StartTimer()
    {
        timer = roundDuration;
        lastShownSecond = -1;
        UpdateTimerUI();
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            ChangeState(GameState.RoundEnd);
            return;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(timer);

        if (seconds == lastShownSecond)
            return;

        lastShownSecond = seconds;
        timerText.text = seconds.ToString();
        timerText.color = seconds <= 10 ? dangerColor : normalColor;
    }

    private void HandleRoundEnd()
    {
        Debug.Log($"ROUND {currentRound} END");

        keyboardPanel.SetActive(false);
        UpdateScreen("");
        cleanTextFormKeyboardHandler();

        // qui più avanti:
        // spawn personaggi
        // calcolo reward

        ChangeState(GameState.Shop);
    }

    private void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    // =========================
    // UI CALLBACKS
    // =========================

    public void OnContinueButtonPressed()
    {
        if (currentState != GameState.Shop)
            return;

        ChangeState(GameState.RoundStart);
    }

    public void OnEnterClick()
    {
        Debug.Log($"ROUND {currentRound} END because player clicked enter and all checks successfull");



        // qui più avanti:
        // spawn personaggi
        // calcolo reward

        //keyboardPanel.SetActive(false);
        //ChangeState(GameState.RoundEnd);
        SentenceEvents.OnSentenceValidated?.Invoke(
        new ValidatedSentence(subject, predicate, obj)
);
    }

    private void UpdateScreen(string text)
    {
        screenText.text = text;
    }

    private void cleanTextFormKeyboardHandler()
    {
        keyboardHandler.cleanText();
    }

    // =========================
    // DEBUG / INFO
    // =========================

}
