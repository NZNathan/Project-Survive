using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMoveable : C {

    //Components
    [HideInInspector]
    public CircleCollider2D col2D;
    [HideInInspector]
    public AudioSource audioSource;

    //Movement Variables
    [Header("Movement Variables")]
    public float walkSpeed = 1f;
    public float sprintSpeed = 2f;
    public float jumpForce = 600;
    protected bool jumping = false;

    //Jump Variables
    private float jumpStartY = 0;

    [HideInInspector]
    public bool canMove = true;
    protected bool moving = false;
    //Movement
    protected abstract Vector2 movement();

    protected new void Start()
    {
        base.Start();

        //Get Components
        col2D = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void faceLeft()
    {
        transform.localScale = new Vector3(-1 * facingFront, 1, 1);
    }

    public void faceRight()
    {
        transform.localScale = new Vector3(1 * facingFront, 1, 1);
    }

    public void jump()
    {
        jumping = true;
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);

        rb2D.AddForce(Vector2.up * jumpForce);

        jumpStartY = transform.position.y; //What if hit vertically

        StartCoroutine("jumpDown");
    }

    IEnumerator jumpDown()
    {
        //gameObject.layer = LayerMask.NameToLayer("NoCharacterCollisions");
        col2D.isTrigger = true;

        yield return new WaitForSeconds(.1f);

        float fallVelocity = 35;

        while (transform.position.y > jumpStartY + 0.03f && transform.position.y > WorldManager.lowerBoundary)
        {
            rb2D.AddForce(Vector2.down * fallVelocity);
            yield return new WaitForFixedUpdate();
        }

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);

        col2D.isTrigger = false;

        jumping = false;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (!jumping)
        {
            //Optimise so only runs while moving?
            foreach (var sr in spriteRenderers)
            {
                sr.sortingOrder = (int)(transform.position.y * 10 * -1);
            }
        }
    }

}
