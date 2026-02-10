using System.Collections.Generic;
using UnityEngine;

public class SentenceValidator : MonoBehaviour
{
    [SerializeField] private WordRepository repository;

    private List<WordData> currentWords = new();

    //----------------------------------

    public void ValidateWord(string raw)
    {
        if (!repository.TryGet(raw, out var word))
        {
            SentenceEvents.OnWordError?.Invoke($"'{raw}' is not a valid word.");
            return;
        }

        currentWords.Add(word);
    }

    //----------------------------------

    public void ValidateSentence()
    {
        if (currentWords.Count < 3)
        {
            SentenceEvents.OnSentenceError?.Invoke("A sentence must contain at least 3 words.");
            Reset();
            return;
        }

        var subject = currentWords[0];
        var predicate = currentWords[1];
        var obj = currentWords[2];

        if (subject.wordType != WordType.Subject)
        {
            Error("First word must be a SUBJECT");
            return;
        }

        if (predicate.wordType != WordType.Predicate)
        {
            Error("Second word must be a PREDICATE");
            return;
        }

        if (obj.wordType != WordType.Object)
        {
            Error("Third word must be an OBJECT");
            return;
        }

        bool boost = predicate.relatedStat == obj.relatedStat;

        if (!boost)
        {
            SentenceEvents.OnSentenceWarning?.Invoke(
                "Stat mismatch! Ally will spawn with a penalty."
            );
        }

        SentenceEvents.OnSentenceValidated?.Invoke(
            new ValidatedSentence
            {
                subject = subject,
                predicate = predicate,
                obj = obj,
                statBoost = boost
            });

        Reset();
    }

    //----------------------------------

    private void Error(string msg)
    {
        SentenceEvents.OnSentenceError?.Invoke(msg);
        Reset();
    }

    private void Reset()
    {
        currentWords.Clear();
    }
}
