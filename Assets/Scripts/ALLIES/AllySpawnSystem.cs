using UnityEngine;

public class AllySpawnSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AllyDatabase allyDatabase;
    [SerializeField] private Transform uiSpawnBox;

    private void OnEnable()
    {
        SentenceEvents.OnSentenceValidated += HandleSentence;
    }

    private void OnDisable()
    {
        SentenceEvents.OnSentenceValidated -= HandleSentence;
    }

    private void HandleSentence(ValidatedSentence sentence)
    {
        SpawnAllyFromSubject(sentence.subject);
    }

    private void SpawnAllyFromSubject(WordData subject)
    {
        AllyData allyData = allyDatabase.Get(subject.id);

        GameObject allyUI = Instantiate(
            allyData.uiPrefab,
            uiSpawnBox
        );

        allyUI.transform.localPosition = Vector3.zero;
    }
}
