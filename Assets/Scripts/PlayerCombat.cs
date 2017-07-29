using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : PlayerMovement {

    //Extract out to Character Class and inherit from it? Character -> Player -> PlayerCombat?
    public GameObject weapon;

    //Weapon Variables
    private bool weaponDrawn = false;

    //Raycast Variables
    private LayerMask attackMask; //Layer that all attackable objects are on
    private float attackRange = 0.9f;

    private bool attacking = false;

    //Basic Attack Variables
    public float attackVelocity = 10f;
    private int attackDamage = 5;
    private float attackPause = 0.25f;
    private float attackForce = 500f;

	// Use this for initialization
	new void Start ()
    {
        base.Start();
        attackMask = LayerMask.GetMask("Hitable");
    }

    void drawWeapon()
    {
        weaponDrawn = !weaponDrawn;
        weapon.SetActive(weaponDrawn);
    }

    void attack()
    {
        //Stop movement and call attack animataion
        canMove = false;
        attacking = true;
        animator.SetInteger("movementSpeed", 0);
        animator.SetTrigger("attack");

        //Get mouse position in relation to the world
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = getDirection(mousePos);
        
        //Get position of player
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

        //Face attack direction
        if (mousePos.x < playerPos.x)
            faceLeft();
        else
            faceRight();

        /*
        //Get direction by subtracting player location
        mousePos = (new Vector3(mousePos.x, mousePos.y, 0) - (Vector3) playerPos); //Minus y so taking point on middle of character?

        //Normalize the direction so mouse distance doesn't affect it
        float distance = mousePos.magnitude;
        Vector2 direction = mousePos / distance; // This is now the normalized direction.
        */
        StartCoroutine(rayCast(playerPos, direction, attackPause));

        rb2D.AddForce(direction * attackVelocity);

    }

    IEnumerator rayCast(Vector2 playerPos, Vector2 direction, float pause)
    {
        yield return new WaitForSeconds(pause);

        RaycastHit2D hitObject = Physics2D.Raycast(playerPos, direction, attackRange, attackMask,-10, 10);
        Debug.DrawRay(playerPos, direction * attackRange, Color.blue, 3f);
        
        Debug.Log(hitObject);

        //If the Raycast hits an object on the layer Enemy
        if (hitObject)
        {
            //Hit attack
            Hitable objectHit = hitObject.transform.gameObject.GetComponentInParent<Hitable>();

            //Apply damage and knockback
            objectHit.loseHealth(attackDamage);
            objectHit.knockback(playerPos, attackForce);
        }

        canMove = true;
        attacking = false;
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
        keyPresses();

        if (Input.GetMouseButtonDown(0) && !attacking)
            attack();

    }
}
