using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ally Database")]
public class AllyDatabase : ScriptableObject
{
    [SerializeField] private AllyData[] allies;

    private Dictionary<string, AllyData> lookup;

    private void OnEnable()
    {
        lookup = allies.ToDictionary(a => a.wordId);
    }

    public AllyData Get(string wordId)
    {
        return lookup[wordId];
    }
}
