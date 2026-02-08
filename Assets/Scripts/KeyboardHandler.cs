using TMPro;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    [Header("Display")]
    public TextMeshProUGUI screenText;

    public GameManager gameManager;

    private string currentText = "";

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void AddCharacter(string c)
    {
        currentText += c;
        UpdateScreen();
    }

    public void AddSpace()
    {
        currentText += " ";
        UpdateScreen();
    }

    public void DeleteLast()
    {
        if (currentText.Length == 0) return;

        currentText = currentText.Substring(0, currentText.Length - 1);
        UpdateScreen();
    }

    public void Submit()
    {
        Debug.Log("Typed phrase: " + currentText);


        //TODO:
        //check parole/frase
        //currentText = ""; 
        //UpdateScreen();

        //if(tutti i controlli sono positivi )
        gameManager.OnEnterClick();
    }

    private void UpdateScreen()
    {
        screenText.text = currentText;
    }

    public void cleanText()
    {
        currentText = "";
    }
}
