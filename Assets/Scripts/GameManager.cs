using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Boot,
        Round,
        Shop,
        GameOver
    }

    public static GameManager Instance;

    public GameState State;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        State = GameState.Round;

        Debug.Log("Round Started");

        // TODO:
        // generate grid
        // start timer
        // spawn enemies
    }

    public void EndRound()
    {
        State = GameState.Shop;

        Debug.Log("Round Ended");
    }

    public void GameOver()
    {
        State = GameState.GameOver;

        Debug.Log("GAME OVER");
    }
}
