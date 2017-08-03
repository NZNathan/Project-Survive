using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CMoveCombatable {

    protected override void movement()
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

        if (shiftKeyDown)
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

        animator.SetFloat("movementSpeed", movement.magnitude);

        if (movement == Vector3.zero)
            return;

        //Flip player sprite if not looking the right way
        if (movement.x < 0 && transform.localScale.x != -1)
            faceLeft();
        else if (movement.x > 0 && transform.localScale.x != 1)
            faceRight();

        /*
    //Flip player sprite if not looking the right way
    //Times by facing front so if player is facing backwards the X scale is inverted
    if (movement.x < 0 && transform.localScale.x != -1 * facingFront)
        faceLeft();
    else if (movement.x > 0 && transform.localScale.x != 1 * facingFront)
        faceRight();

    //Flip player head if not looking the right way
    //NEEDTOFIX: need to flip player sword around and add backward animations 

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

        rb2D.AddForce(movement * movementSpeed);
    }

    void attack()
    {
        //Get mouse position in relation to the world
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = getDirection(mousePos, 0);

        attack(mousePos, direction);

    }

    protected override void removeDeadBody()
    {
        WorldManager.instance.playerDied(this);

        Destroy(healthBar.gameObject);
        Destroy(this.gameObject);
    }

    void keyPresses()
    {
        bool qKeyDown = Input.GetKeyDown(KeyCode.Q);

        if (qKeyDown)
            drawWeapon();
    }
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();

        if (dead)
            return;

        keyPresses();

        if (Input.GetMouseButtonDown(0) && !attacking && weapon.activeInHierarchy)
            attack();

    }
}
