using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    //Components
    private Animator animator;
    private Rigidbody2D rb2D;

    //Extract out to Character Class and inherit from it? Character -> Player -> PlayerCombat?
    public GameObject weapon;

    //Weapon Variables
    bool weaponDrawn = false;

    //Basic Attack Variables
    public float attackVelocity = 10f;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void drawWeapon()
    {
        weaponDrawn = !weaponDrawn;
        weapon.SetActive(weaponDrawn);
    }

    void attack()
    {
        animator.SetTrigger("attack");
        //Get mouse position in relation to the world
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Get direction by subtracting player location
        mousePos = (new Vector3(mousePos.x, mousePos.y, 0) - transform.position); //Minus y so taking point on middle of character?

        //Normalize the direction so mouse distance doesn't affect it
        var distance = mousePos.magnitude;
        var direction = mousePos / distance; // This is now the normalized direction.

        rb2D.AddForce(direction * attackVelocity);

    }

    void keyPresses()
    {
        bool qKeyDown = Input.GetKeyDown(KeyCode.Q);

        if (qKeyDown)
            drawWeapon();
    }
	
	// Update is called once per frame
	void Update ()
    {
        keyPresses();
        Debug.Log(rb2D.velocity);
        if (Input.GetMouseButtonDown(0))
            attack();

    }
}
