using UnityEngine;
using TMPro;
using System.Collections;

public class SentenceUIFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float duration = 2f;

    Coroutine routine;

    private void OnEnable()
    {
        SentenceEvents.OnWordError += ShowError;
        SentenceEvents.OnSentenceError += ShowError;
        SentenceEvents.OnSentenceWarning += ShowWarning;
    }

    private void OnDisable()
    {
        SentenceEvents.OnWordError -= ShowError;
        SentenceEvents.OnSentenceError -= ShowError;
        SentenceEvents.OnSentenceWarning -= ShowWarning;
    }

    void ShowError(string msg)
    {
        Show(msg, Color.red);
    }

    void ShowWarning(string msg)
    {
        Show(msg, Color.yellow);
    }

    void Show(string msg, Color color)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine(msg, color));
    }

    IEnumerator ShowRoutine(string msg, Color color)
    {
        feedbackText.text = msg;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        feedbackText.gameObject.SetActive(false);
    }
}
