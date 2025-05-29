using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class RuneTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite unlitSprite;
    [SerializeField] private Sprite litSprite;
    [SerializeField] private Sprite finishSprite;

    public Vector2Int gridPosition; // e.g. (0,1)
    public bool isLit = false;

    private PuzzleController controller;

    public void Init(PuzzleController puzzleController, Vector2Int position)
    {
        controller = puzzleController;
        gridPosition = position;
        SetLit(false);
    }

    public void Toggle()
    {
        SetLit(!isLit);
        controller.CheckPuzzleSolved();
    }

    public void SetLit(bool state)
    {
        isLit = state;
        spriteRenderer.sprite = isLit ? litSprite : unlitSprite;
    }

    public void SetFinishState()
    {
        spriteRenderer.sprite = finishSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        controller.OnTileStepped(gridPosition);
    }
}
