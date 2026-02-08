using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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

    public void OnPointerDown(PointerEventData eventData)
    {
        keyNotPressed.SetActive(false);
        keyPressed.SetActive(true);

        if (keyNotPressedText != null)
            keyNotPressedText.color = pressedTextColor;

        keyboard.DeleteLast();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        keyPressed.SetActive(false);
        keyNotPressed.SetActive(true);

        if (keyNotPressedText != null)
        {
            // keyNotPressedText.color = normalTextColor;
        }
    }
}
