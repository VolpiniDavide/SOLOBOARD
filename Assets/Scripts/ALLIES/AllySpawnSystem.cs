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
        SpawnAllyFromSubject(sentence);
    }

    private void SpawnAllyFromSubject(ValidatedSentence sentence)
    {
        AllyData allyData = allyDatabase.Get(sentence.subject.id);

        GameObject allyUI = Instantiate(
            allyData.uiPrefab,
            uiSpawnBox
        );

        var draggable = allyUI.GetComponent<DraggableAlly>();
        draggable.Init(allyData);

        //ApplyStatModifier(draggable, sentence);

        allyUI.transform.localPosition = Vector3.zero;
    }

    
}
