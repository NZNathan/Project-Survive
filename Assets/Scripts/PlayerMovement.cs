using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Components
    private Animator animator;
    private Rigidbody2D rb2D;
    private BoxCollider2D col2d;

    //Movement Variables
    public float walkSpeed = 1f;
    public float sprintSpeed = 2f;

    // Use this for initialization
    void Start ()
    {
        //Get Components
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
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

        //Flip player sprite if not looking the right way
        if (movement.x < 0 && transform.localScale.x != -1)
            transform.localScale = new Vector3(-1,1,1);
        else if (movement.x > 0 && transform.localScale.x != 1)
            transform.localScale = new Vector3(1, 1, 1);

        animator.SetInteger("movementSpeed", (int) movement.magnitude);
        rb2D.MovePosition(transform.position + movement * Time.deltaTime);
    }

	// Fixed update so is consistant with frames
	void FixedUpdate ()
    {
        //Call movement function to handle player movements
        movement();
	}
}
