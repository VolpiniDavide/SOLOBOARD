using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableAlly : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private RectTransform rect;
    private Canvas canvas;
    private Vector3 startPos;

    private AllyData allyData;

    public void Init(AllyData data)
    {
        allyData = data;
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (BattlefieldDropZone.Instance.TryGetDropPosition(
            eventData.position,
            out Vector3 worldPos))
        {
            AllyWorldSpawner.Instance.SpawnWorldAlly(allyData, worldPos);
            Destroy(gameObject);
        }
        else
        {
            rect.position = startPos;
        }
    }
}
