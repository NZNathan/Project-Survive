using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Sets the images to a certain character for use in the UI
/// </summary>
public class CharacterUI : MonoBehaviour {

    private Image image;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setSpriteController(RuntimeAnimatorController spriteController)
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = spriteController;
    }

    private void Update()
    {
        image.sprite = spriteRenderer.sprite;
    }
}
