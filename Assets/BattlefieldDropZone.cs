using UnityEngine;

public class BattlefieldDropZone : MonoBehaviour
{
    public static BattlefieldDropZone Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryGetDropPosition(Vector2 screenPosition, out Vector3 worldPos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(screenPosition);
        wp.z = 0;

        Collider2D hit = Physics2D.OverlapPoint(wp, LayerMask.GetMask("Battlefield"));

        if (hit != null)
        {
            worldPos = wp;
            return true;
        }

        worldPos = Vector3.zero;
        return false;
    }
}
