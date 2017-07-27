using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Character : MonoBehaviour {

    //Components
    private SpriteRenderer[] spriteRenderers;

    //Sprite Variables
    protected int facingFront = 1;

    //Sprites
    Sprite[] sprites;

    // Runs as soon as Instantiate
    void Awake ()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}

    public void faceBack()
    {
        spriteRenderers[(int) SpriteSet.Part.HEAD].sprite = sprites[(int)SpriteSet.Part.HEADBACKWARDS];
    }

    public void faceFront()
    {
        spriteRenderers[(int)SpriteSet.Part.HEAD].sprite = sprites[(int)SpriteSet.Part.HEAD];
    }

    public void setSprites(Sprite[] spriteSet)
    {
        sprites = spriteSet;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = spriteSet[i];
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Optimise so only runs while moving?
		foreach(var sr in spriteRenderers)
        {
            sr.sortingOrder = (int) (transform.position.y*10 *-1);
        }
	}
}
