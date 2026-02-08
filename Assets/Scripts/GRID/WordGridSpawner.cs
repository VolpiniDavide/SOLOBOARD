using System.Collections.Generic;
using UnityEngine;

public class WordGridSpawner : MonoBehaviour
{
    [Header("Grid Size")]
    public int sizeX = 10;
    public int sizeY = 10;

    [Header("Word Constraints")]
    public int subjects;
    public int objects;
    public int predicate;

    public int level1;
    public int level2;
    public int level3;

    [Header("References")]
    public GameObject keyPrefab;
    public Transform gridParent; // il transform con GridLayoutGroup

    [Header("Word Pool")]
    public List<Word> words;

    [Header("Word Pool")]
    public WordPool WordPool;

    private void Awake()
    {
        WordPool = FindObjectOfType<WordPool>();
    }

    private void Start()
    {
        //GenerateGrid();
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        ClearGrid();

        var words = WordPool.GetAllWordData();



        char[] letters = WordGridGenerator.GetLettersList(
            sizeX,
            sizeY,
            subjects,
            objects,
            predicate,
            level1,
            level2,
            level3,
            words
        );

        foreach (char c in letters)
        {
            GameObject keyGO = Instantiate(keyPrefab, gridParent);

            // IMPORTANTISSIMO:
            // il prefab deve avere NormalKey
            NormalKey key = keyGO.GetComponent<NormalKey>();

            if (key != null)
                key.SetLetter(c);
            else
                Debug.LogError("Il prefab non ha NormalKey!");
        }
    }

    private void ClearGrid()
    {
        for (int i = gridParent.childCount - 1; i >= 0; i--)
        {
            Destroy(gridParent.GetChild(i).gameObject);
        }
    }
}
