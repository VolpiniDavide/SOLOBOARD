using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask characterLayers;
    [SerializeField] private float transparentAlpha = 0.4f;
    [SerializeField] private float normalAlpha = 1f;
    [SerializeField] private float fadeSpeed = 8f;

    [Header("Detection Area")]
    [SerializeField] private bool useColliderBounds = true;
    [SerializeField] private float manualDetectionRadius = 1.5f;
    [SerializeField] private float boundsExpansion = 0.2f;

    [Header("Occlusion Settings")]
    [SerializeField] private float yOffsetTolerance = 0.1f; // Tolleranza per evitare flickering

    [Header("Debug")]
    [SerializeField] private bool showDetectionArea = true;
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

        if (wallCollider == null)
        {
            Debug.LogWarning($"WallTransparency su {gameObject.name}: Nessun Collider2D trovato!", this);
        }

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
            detectionBounds.Expand(boundsExpansion * 2f);
        }
    }

    private bool CheckForCharactersBehind()
    {
        Collider2D[] characters;

        if (useColliderBounds && wallCollider != null)
        {
            Vector2 center = detectionBounds.center;
            Vector2 size = detectionBounds.size;

            characters = Physics2D.OverlapBoxAll(center, size, 0f, characterLayers);
        }
        else
        {
            characters = Physics2D.OverlapCircleAll(transform.position, manualDetectionRadius, characterLayers);
        }

        foreach (Collider2D character in characters)
        {
            if (IsCharacterBehindWall(character))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsCharacterBehindWall(Collider2D character)
    {
        Vector2 characterPos = character.transform.position;

        // 1. Il personaggio deve avere Y minore del bordo inferiore del muro
        float wallBottomY = wallCollider.bounds.min.y;

        if (characterPos.y < wallBottomY - yOffsetTolerance)
        {
            // Il personaggio è troppo in alto, probabilmente davanti al muro
            return false;
        }

        // 2. Il personaggio deve essere dentro i bounds X del muro
        float wallMinX = wallCollider.bounds.min.x - boundsExpansion;
        float wallMaxX = wallCollider.bounds.max.x + boundsExpansion;

        if (characterPos.x < wallMinX || characterPos.x > wallMaxX)
        {
            // Il personaggio è fuori dall'area orizzontale del muro
            return false;
        }

        // 3. Controlla anche la distanza verticale per evitare che personaggi molto lontani
        //    rendano trasparente il muro
        float wallTopY = wallCollider.bounds.max.y;
        float verticalDistance = wallTopY - characterPos.y;

        // Se il personaggio è troppo lontano verticalmente, non è dietro questo muro
        if (verticalDistance > detectionBounds.size.y + boundsExpansion)
        {
            return false;
        }

        // Tutte le condizioni soddisfatte: il personaggio è effettivamente dietro
        return true;
    }

    private void OnDrawGizmos()
    {
        if (!showDetectionArea) return;

        if (wallCollider == null)
        {
            wallCollider = GetComponent<Collider2D>();
        }

        if (wallCollider != null && useColliderBounds)
        {
            Bounds bounds = wallCollider.bounds;
            Bounds expandedBounds = bounds;
            expandedBounds.Expand(boundsExpansion * 2f);

            // Area di detection (verde/rosso)
            Gizmos.color = hasCharacterBehind ? gizmoColorWithCharacter : gizmoColorNoCharacter;
            Gizmos.DrawWireCube(expandedBounds.center, expandedBounds.size);

            // Bounds originali (blu)
            Gizmos.color = new Color(0.3f, 0.5f, 1f, 0.5f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            // Linea che indica il bordo inferiore del muro (importante per debug)
            Gizmos.color = Color.yellow;
            Vector3 leftPoint = new Vector3(bounds.min.x, bounds.min.y, 0);
            Vector3 rightPoint = new Vector3(bounds.max.x, bounds.min.y, 0);
            Gizmos.DrawLine(leftPoint, rightPoint);
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

        if (wallCollider == null)
        {
            wallCollider = GetComponent<Collider2D>();
        }

        if (wallCollider != null && useColliderBounds)
        {
            Bounds bounds = wallCollider.bounds;
            Bounds expandedBounds = bounds;
            expandedBounds.Expand(boundsExpansion * 2f);

            // Area riempita semi-trasparente
            Gizmos.color = new Color(
                hasCharacterBehind ? 1f : 0f,
                hasCharacterBehind ? 0f : 1f,
                0f,
                0.2f
            );
            Gizmos.DrawCube(expandedBounds.center, expandedBounds.size);

            // Bordo più evidente
            Gizmos.color = hasCharacterBehind ? Color.red : Color.green;
            Gizmos.DrawWireCube(expandedBounds.center, expandedBounds.size);

            // Evidenzia il bordo inferiore
            Gizmos.color = Color.yellow;
            Vector3 leftPoint = new Vector3(bounds.min.x - 0.5f, bounds.min.y, 0);
            Vector3 rightPoint = new Vector3(bounds.max.x + 0.5f, bounds.min.y, 0);
            Gizmos.DrawLine(leftPoint, rightPoint);
        }
    }
}