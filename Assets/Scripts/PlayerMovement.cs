using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Character {

    //Components
    protected Animator animator;
    protected BoxCollider2D col2d;

    //Movement Variables
    public float walkSpeed = 1f;
    public float sprintSpeed = 2f;
    protected bool canMove = true;

    // Use this for initialization
    public new void Start ()
    {
        base.Start();

        //Get Components
        animator = GetComponent<Animator>();
        col2d = GetComponent<BoxCollider2D>();
    }

    void movement()
    {
        // read key inputs
        bool wKeyDown = Input.GetKey(KeyCode.W);
        bool aKeyDown = Input.GetKey(KeyCode.A);
        bool sKeyDown = Input.GetKey(KeyCode.S);
        bool dKeyDown = Input.GetKey(KeyCode.D);
        bool shiftKeyDown = Input.GetKey(KeyCode.LeftShift);

        //Movement Vector
        Vector3 movement = Vector3.zero;
        float movementSpeed = walkSpeed;

        if(shiftKeyDown)
            movementSpeed = sprintSpeed;

        if (wKeyDown && !sKeyDown)
        {
            movement.y += movementSpeed;
        }
        if (sKeyDown && !wKeyDown)
        {
            movement.y -= movementSpeed;
        }
        if (aKeyDown && !dKeyDown)
        {
            movement.x -= movementSpeed;
        }
        if (dKeyDown && !aKeyDown)
        {
            movement.x += movementSpeed;
        }

        animator.SetInteger("movementSpeed", (int)movement.magnitude);

        if (movement == Vector3.zero)
            return;

        //Flip player sprite if not looking the right way
        //Times by facing front so if player is facing backwards the X scale is inverted
        if (movement.x < 0 && transform.localScale.x != -1 * facingFront)
            faceLeft();
        else if (movement.x > 0 && transform.localScale.x != 1 * facingFront)
            faceRight();

        //Flip player head if not looking the right way
        //NEEDTOFIX: need to flip player sword around and add backward animations 
        /*
        if (movement.y < 0 && facingFront == -1)
        {
            faceFront();
            facingFront = 1;
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
        else if (movement.y > 0 && facingFront == 1)
        {
            faceBack();
            facingFront = -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
        */

        rb2D.MovePosition(transform.position + movement * Time.deltaTime);
    }

    public void faceLeft()
    {
        transform.localScale = new Vector3(-1 * facingFront, 1, 1);
    }

    public void faceRight()
    {
        transform.localScale = new Vector3(1 * facingFront, 1, 1);
    }

    // Fixed update so is consistant with frames
    void FixedUpdate ()
    {
        //Call movement function to handle player movements
        if(canMove)
            movement();
	}
}
