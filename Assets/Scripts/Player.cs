using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CMoveCombatable
{
    public static Player instance;
    [HideInInspector]
    public Bag bag;

    public new void Start()
    {
        base.Start();
        bag = new Bag(UIManager.instance.bagGUIObject.GetComponent<BagGUI>());

        //Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        animator = GetComponentInChildren<Animator>();

        UIManager.instance.setAbilities(abilities); //REmove when player is generated
    }

    public override void attackHit()
    {
        Camera.main.GetComponentInParent<CameraShake>().shake = .5f;
    }

    protected override Vector2 movement()
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

        

        if (movement == Vector3.zero)
        {
            moving = false;
            animator.SetFloat("movementSpeed", 0);
            return movement;
        }
        else
            moving = true;

        animator.SetFloat("movementSpeed", movementSpeed);

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

        return movement * movementSpeed;// * Time.deltaTime;
    }

    bool attack(Ability action)
    {
        //Get mouse position in relation to the world
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = getDirection(mousePos, 0);

        return attack(mousePos, direction, action);
    }

    protected override void death()
    {
        base.death();

        WorldManager.instance.playerDied(this);
    }

    IEnumerator inputHandler()
    {
        //Bag Input
        bool bKeyDown = Input.GetKeyDown(KeyCode.B);

        if (bKeyDown)
            bag.input();

        if(bag.isOpen())
            yield break;

        //Weapon Inputs
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);
        bool qKeyDown = Input.GetKeyDown(KeyCode.Q);
        

        if (qKeyDown)
            drawWeapon();

        //Call movement function to handle movements
        Vector3 movementVector = Vector3.zero;

        movementVector = movement();

        yield return new WaitForFixedUpdate(); //For rigidbody interactions

        if (leftClick && !attacking && weapon.activeInHierarchy)
            attack(abilities[0]);

        else if (rightClick && !attacking && weapon.activeInHierarchy)
        {
            if (attack(abilities[1]))
                UIManager.instance.usedAbility(1);
        }

        rb2D.AddForce(movementVector);
    }

    //Set AActive
    public override void loseHealth(int damage)
    {
        base.loseHealth(damage);

        if(!dead)
            Camera.main.GetComponentInParent<CameraShake>().shake = .5f;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!dead && canMove && !attacking)
             StartCoroutine("inputHandler"); //Alternte that coroutine??

    }

    void LateUpdate()
    {

    }
}