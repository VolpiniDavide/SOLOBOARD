using TMPro;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    [Header("Display")]
    public TextMeshProUGUI screenText;

    private string currentText = "";

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

        // opzionale:
        // currentText = "";
        // UpdateScreen();
    }

    private void UpdateScreen()
    {
        screenText.text = currentText;
    }
}
