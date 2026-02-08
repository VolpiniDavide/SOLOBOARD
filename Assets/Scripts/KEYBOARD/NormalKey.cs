using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NormalKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string keyChar;
    public TextMeshProUGUI keyNotPressedText;
    public TextMeshProUGUI keyPressedText;
    public GameObject keyNotPressed;
    public GameObject keyPressed;
    public Color normalTextColor = Color.white;
    public Color pressedTextColor = Color.red;

    private KeyboardHandler keyboard;

    private void Awake()
    {
        keyboard = FindObjectOfType<KeyboardHandler>();
    }

    void Start()
    {
        if (keyNotPressedText != null)
        {
            keyNotPressedText.text = keyChar;
            keyPressedText.text = keyChar;
        }
    }

    public void SetLetter(char c)
    {
        keyChar = c.ToString();

        if (keyNotPressedText != null)
            keyNotPressedText.text = c.ToString();

        if (keyPressedText != null)
            keyPressedText.text = c.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        keyNotPressed.SetActive(false);
        keyPressed.SetActive(true);

        if (keyNotPressedText != null)
            keyNotPressedText.color = pressedTextColor;

        keyboard.AddCharacter(keyChar);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        keyPressed.SetActive(false);
        keyNotPressed.SetActive(true);

        if (keyNotPressedText != null) { 
           // keyNotPressedText.color = normalTextColor;
           }
    }
}
