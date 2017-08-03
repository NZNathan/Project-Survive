using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : CHitable {

    //Components
    private SpriteRenderer[] spriteRenderers;
    protected Animator animator;

    //Sprite Variables
    protected int facingFront = 1;

    //Flash Variables
    private Material whiteMat;
    private Material defaultMat;
    private float flashDuration = 0.1f;

    //Movement Variables
    protected bool dead = false;

    //Sprites
    Sprite[] sprites;

    public new void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        objectHeight = 0.48f;
    }

    // Runs as soon as Instantiate
    void Awake ()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        defaultMat = Resources.Load<Material>("Materials/Light_Shader");
        whiteMat = Resources.Load<Material>("Materials/SolidWhite");
    }

    public void faceBack()
    {
        spriteRenderers[(int) SpriteSet.Part.HEAD].sprite = sprites[(int)SpriteSet.Part.HEADBACKWARDS];
    }

    public void faceFront()
    {
        spriteRenderers[(int)SpriteSet.Part.HEAD].sprite = sprites[(int)SpriteSet.Part.HEAD];
    }

    public void setSpriteSet(Sprite[] spriteSet)
    {
        sprites = spriteSet;

        setSprites();
    }

    void setSprites()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprites[i];
        }
    }

    protected override void death()
    {
        //StopAllCoroutines();

        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }

        gameObject.layer = 0;
        animator.SetBool("dead", true);
        Invoke("removeDeadBody", 4);
        dead = true;
    }

    protected void removeDeadBody()
    {
        Destroy(healthBar.gameObject);
        Destroy(this.gameObject);
    }

    protected override IEnumerator flash()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material = whiteMat;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material = defaultMat;
        }
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        //Optimise so only runs while moving?
		foreach(var sr in spriteRenderers)
        {
            sr.sortingOrder = (int) (transform.position.y*10 *-1);
        }
	}

}
