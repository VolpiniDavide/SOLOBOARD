using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask characterLayers;
    [SerializeField] private float transparentAlpha = 0.4f;
    [SerializeField] private float normalAlpha = 1f;
    [SerializeField] private float fadeSpeed = 8f;

    [Header("Detection Area")]
    [SerializeField] private bool useColliderBounds = true; // Usa automaticamente le dimensioni del collider
    [SerializeField] private float manualDetectionRadius = 1.5f; // Usato solo se useColliderBounds = false
    [SerializeField] private float boundsExpansion = 0.2f; // Espande leggermente i bounds per evitare bordi troppo stretti

    [Header("Debug")]
    [SerializeField] private bool showDetectionArea = true; // Mostra l'area di detection
    [SerializeField] private Color gizmoColorNoCharacter = Color.green;
    [SerializeField] private Color gizmoColorWithCharacter = Color.red;

    private SpriteRenderer spriteRenderer;
    private Collider2D wallCollider;
    private Color currentColor;
    private bool hasCharacterBehind = false;
    private Bounds detectionBounds;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallCollider = GetComponent<Collider2D>();

        currentColor = spriteRenderer.color;
        currentColor.a = normalAlpha;
        spriteRenderer.color = currentColor;

        UpdateDetectionBounds();
    }

    private void Update()
    {
        if (useColliderBounds)
        {
            UpdateDetectionBounds();
        }

        hasCharacterBehind = CheckForCharactersBehind();

        float targetAlpha = hasCharacterBehind ? transparentAlpha : normalAlpha;
        currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * fadeSpeed);

        spriteRenderer.color = currentColor;
    }

    private void UpdateDetectionBounds()
    {
        if (wallCollider != null)
        {
            detectionBounds = wallCollider.bounds;
            // Espande leggermente i bounds
            detectionBounds.Expand(boundsExpansion * 2f);
        }
    }

    private bool CheckForCharactersBehind()
    {
        Collider2D[] characters;

        if (useColliderBounds && wallCollider != null)
        {
            // Usa OverlapBox con le dimensioni esatte del collider
            Vector2 center = detectionBounds.center;
            Vector2 size = detectionBounds.size;

            characters = Physics2D.OverlapBoxAll(center, size, 0f, characterLayers);
        }
        else
        {
            // Fallback a OverlapCircle
            characters = Physics2D.OverlapCircleAll(transform.position, manualDetectionRadius, characterLayers);
        }

        foreach (Collider2D character in characters)
        {
            // Personaggio è dietro se ha Y minore del muro
            if (character.transform.position.y > transform.position.y)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (!showDetectionArea) return;

        // Aggiorna i bounds anche in editor mode
        if (wallCollider != null && useColliderBounds)
        {
            Bounds bounds = wallCollider.bounds;
            bounds.Expand(boundsExpansion * 2f);

            Gizmos.color = hasCharacterBehind ? gizmoColorWithCharacter : gizmoColorNoCharacter;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            // Disegna anche i bounds originali del collider per confronto
            Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.3f);
            Gizmos.DrawWireCube(wallCollider.bounds.center, wallCollider.bounds.size);
        }
        else if (!useColliderBounds)
        {
            Gizmos.color = hasCharacterBehind ? gizmoColorWithCharacter : gizmoColorNoCharacter;
            Gizmos.DrawWireSphere(transform.position, manualDetectionRadius);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDetectionArea) return;

        // Versione più visibile quando selezionato
        if (wallCollider != null && useColliderBounds)
        {
            Bounds bounds = wallCollider.bounds;
            bounds.Expand(boundsExpansion * 2f);

            Gizmos.color = new Color(hasCharacterBehind ? 1f : 0f, hasCharacterBehind ? 0f : 1f, 0f, 0.5f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
    }
}