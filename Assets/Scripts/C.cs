using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : CHitable {

    //Components
    protected SpriteRenderer[] spriteRenderers;
    [HideInInspector]
    public Animator animator;

    [Header("Faction Variables")]
    public Faction faction;

    [Header("Character Variables")]
    public string firstName;
    public string lastName;

    //Collision Variables
    protected int originalLayer;
    public static readonly int noCollisionLayer = 9;// LayerMask.NameToLayer("NoCharacterCollisions"); //readonly = const?

    //Sprite Variables
    protected int facingFront = 1;

    //Flash Variables
    private Material whiteMat;
    private Material defaultMat;
    private float flashDuration = 0.1f;

    //Movement Variables
    protected bool dead = false;
    protected Coroutine fallingCo;
    protected bool falling = false;

    //Sprites
    private Sprite[] sprites;

    public new void Start()
    {
        base.Start();
        originalLayer = gameObject.layer;

        animator = GetComponentInChildren<Animator>();
        objectHeight = 0.48f;
    }

    // Runs as soon as Instantiate
    void Awake ()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        defaultMat = Resources.Load<Material>("Materials/Light_Shader");
        whiteMat = Resources.Load<Material>("Materials/SolidWhite");
    }

    public bool isDead()
    {
        return dead;
    }

    public bool isFalling()
    {
        return falling;
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

    public Sprite[] getSprites()
    {
        return sprites;
    }

    void setSprites()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprites[i];
        }
    }

    public override void applyStun(float stunTime)
    {
        return;
    }

    //force is knockback force of the attack
    public override void knockUp(Vector2 target, int knockbackForce, int knockupForce, float targetHeight)
    {
        gameObject.layer = noCollisionLayer;
        falling = true;

        rb2D.AddForce(Vector2.up * knockupForce);

        animator.SetTrigger("inAir");

        Vector3 dir = getDirection(target, targetHeight) * -1;
        float startPos = (transform.position + dir * knockbackForce / 1000).y; //What if hit vertically

        if (fallingCo != null)
            StopCoroutine(fallingCo);

        fallingCo = StartCoroutine("fallDown", startPos);
    }

    IEnumerator fallDown(float floorY)
    {
        gameObject.layer = noCollisionLayer;

        yield return new WaitForSeconds(.1f);

        float fallVelocity = 35;

        while (transform.position.y > floorY + 0.03f && transform.position.y > WorldManager.lowerBoundary)
        {
            rb2D.AddForce(Vector2.down * fallVelocity);
            yield return new WaitForFixedUpdate();
        }

        rb2D.velocity = new Vector2(0, 0);
        animator.SetTrigger("hitFloor");

        yield return new WaitForSeconds(.6f);

        animator.SetTrigger("getUp");
        gameObject.layer = originalLayer;

        //yield return new WaitForSeconds(.2f); //Wait till up

        falling = false;
    }

    protected override void death()
    {
        //StopAllCoroutines();
        gameObject.layer = noCollisionLayer;
        GetComponentInChildren<BoxCollider2D>().gameObject.layer = noCollisionLayer; //Make sure object with collider on it can no longer be hit 
        StopCoroutine("showHealth");

        /* WHY????
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }

        gameObject.layer = 0;*/
        animator.SetBool("dead", true);
        Invoke("removeDeadBody", 1);
        dead = true;
    }

    protected virtual void removeDeadBody()
    {
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
