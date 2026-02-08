using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordPool : MonoBehaviour
{
    [Header("Initial Words")]
    [SerializeField] private List<WordData> startingWords;

    private List<WordInstance> activeWords = new();

    public IReadOnlyList<WordInstance> ActiveWords => activeWords;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        activeWords.Clear();

        foreach (var word in startingWords)
        {
            activeWords.Add(new WordInstance(word));
        }
    }

    // =========================
    // POOL MANAGEMENT
    // =========================

    public void AddWord(WordData data)
    {
        activeWords.Add(new WordInstance(data));
    }

    public void RemoveWord(string wordId)
    {
        activeWords.RemoveAll(w => w.data.id == wordId);
    }

    public bool ContainsWord(string value)
    {
        return activeWords.Any(w => w.data.value == value);
    }

    public WordInstance GetWord(string value)
    {
        return activeWords.FirstOrDefault(w => w.data.value == value);
    }

    public List<WordInstance> GetByType(WordType type)
    {
        return activeWords.Where(w => w.data.wordType == type).ToList();
    }

    public List<WordInstance> GetAllWords()
    {
        return new List<WordInstance>(activeWords);
    }

    public List<WordData> GetAllWordData()
    {
        return activeWords
                .Select(w => w.data)
                .ToList();
    }


}
