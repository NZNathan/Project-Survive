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

    [HideInInspector]
    public bool canMove = true;

    //Movement
    protected abstract void movement();

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

    protected void FixedUpdate()
    {
        //Call movement function to handle movements
        if (!dead && canMove)
            movement();
    }

    protected new void Update ()
    {
        base.Update();
	}
}
