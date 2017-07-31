using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : PlayerMovement {

    //Extract out to Character Class and inherit from it? Character -> Player -> PlayerCombat?
    [Header("Combat Variables")]
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
    private float attackForce = 500f; //knockback

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

        Vector2 direction = getDirection(mousePos, 0);
        
        //Get position of player
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

        //Face attack direction
        if (mousePos.x < playerPos.x)
            faceLeft();
        else
            faceRight();

        StartCoroutine(rayCastAttack(playerPos, direction, attackPause));

        rb2D.AddForce(direction * attackVelocity);

    }

    IEnumerator rayCastAttack(Vector2 playerPos, Vector2 direction, float pause)
    {
        yield return new WaitForSeconds(pause);

        RaycastHit2D[] hitObject = Physics2D.RaycastAll(playerPos, direction, attackRange, attackMask,-10, 10);
        Debug.DrawRay(playerPos, direction * attackRange, Color.blue, 3f);

        //If the Raycast hits an object on the layer Enemy
        foreach (RaycastHit2D r in hitObject) {
            if (r && r.transform.gameObject != this.gameObject)
            {
                //Hit attack
                Hitable objectHit = r.transform.gameObject.GetComponentInParent<Hitable>();

                //Apply damage and knockback
                objectHit.loseHealth(attackDamage);
                objectHit.knockback(playerPos, attackForce, objectHit.objectHeight);
                break;
            } 
        }

        yield return new WaitForSeconds(0.2f);

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

        if (Input.GetMouseButtonDown(0) && !attacking && weapon.activeInHierarchy)
            attack();

    }
}
