using UnityEngine;

public class RuneTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite unlitSprite;
    [SerializeField] private Sprite litSprite;
    [SerializeField] private Sprite finishSprite;

    public Vector2Int gridPosition;
    public bool isLit = false;

    private PuzzleController controller;
    private bool isSteppedOn = false; // prevent re-toggling too rapidly

    public void Init(PuzzleController puzzleController, Vector2Int position)
    {
        controller = puzzleController;
        gridPosition = position;
        SetLit(false);
    }

    public void Toggle()
    {
        SetLit(!isLit);
        // Removed: controller.CheckPuzzleSolved(); — we'll control this from PuzzleController only
    }


    public void SetLit(bool state)
    {
        isLit = state;
        spriteRenderer.sprite = isLit ? litSprite : unlitSprite;
    }

    public void SetFinishState()
    {
        isLit = true;
        spriteRenderer.sprite = null; // Force refresh
        spriteRenderer.sprite = finishSprite;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isSteppedOn || controller.IsPuzzleComplete())
            return;

        isSteppedOn = true;
        controller.OnTileStepped(gridPosition);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isSteppedOn = false;
    }
}
