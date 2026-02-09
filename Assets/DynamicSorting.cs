using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    [SerializeField] private string sortingLayerName = "Default";
    [SerializeField] private int sortingOrderBase = 5000;
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = sortingLayerName;
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y * 100) + offset;

        if (runOnlyOnce)
        {
            Destroy(this);
        }
    }
}