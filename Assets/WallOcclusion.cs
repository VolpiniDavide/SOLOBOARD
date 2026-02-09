using UnityEngine;

public class WallOcclusion : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float behindWallAlpha = 0.5f;
    [SerializeField] private float normalAlpha = 1f;
    [SerializeField] private float fadeSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Color targetColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetColor = spriteRenderer.color;
    }

    private void Update()
    {
        // Controlla se c'è un muro davanti al personaggio (Y maggiore)
        bool isBehindWall = CheckIfBehindWall();

        float targetAlpha = isBehindWall ? behindWallAlpha : normalAlpha;
        targetColor.a = targetAlpha;

        // Transizione smooth
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * fadeSpeed);
    }

    private bool CheckIfBehindWall()
    {
        // Raycast verso l'alto per rilevare muri
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 5f, wallLayer);

        if (hit.collider != null)
        {
            // Se c'è un muro con Y maggiore del personaggio, è dietro
            return hit.collider.transform.position.y > transform.position.y;
        }

        return false;
    }
}