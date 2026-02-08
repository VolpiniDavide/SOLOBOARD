using UnityEngine;

[CreateAssetMenu(menuName = "SOLOBOARD/Word")]
public class WordData : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string value;

    [Header("Type")]
    public WordType wordType;

    [Header("Gameplay")]
    public StatType relatedStat;
    public int level;              // 0 per subject/predicate
    public int economicValue;
}
