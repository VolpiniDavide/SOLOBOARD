using System.Collections.Generic;
using UnityEngine;

public class WordRepository : MonoBehaviour
{
    [SerializeField] private WordPool wordPool;

    private Dictionary<string, WordData> lookup;

    private void Awake()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, WordData>();

        foreach (var w in wordPool.ActiveWords) // <-- meglio esporla con proprietà
        {
            lookup[w.data.value.ToUpper()] = w.data;
        }
    }

    public bool TryGet(string value, out WordData word)
    {
        return lookup.TryGetValue(value.ToUpper(), out word);
    }
}
