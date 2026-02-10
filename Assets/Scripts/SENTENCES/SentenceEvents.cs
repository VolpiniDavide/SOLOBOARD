using System;
using UnityEngine;

public static class SentenceEvents
{
    public static Action<ValidatedSentence> OnSentenceValidated;
    public static Action<string> OnSentenceError;
    public static Action<string> OnWordError;
    public static Action<string> OnSentenceWarning;
    public static Action<string> OnSentenceSuccess;
}
