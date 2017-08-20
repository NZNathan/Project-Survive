using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CMoveCombatable
{
    public static Player instance;
    private bool chargingAttack = false;

    //Inventory Variables
    [HideInInspector]
    public Bag bag;
    private Item itemInRange;

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

    public void itemEnterProximity(Item item)
    {
        UIManager.instance.newPopup(item.gameObject);
        itemInRange = item;
    }

    public void itemLeaveProximity(Item item)
    {
        UIManager.instance.closePopup();
        itemInRange = null;
    }

    public void useItem(int i)
    {
        bag.useItem(i);
    }

    void pickupItem()
    {
        if(itemInRange != null)
        {
            if (bag.addItem(itemInRange))
            {
                itemInRange.gameObject.SetActive(false);
                Debug.Log("Item in range: " + itemInRange);
                itemInRange = null;
            }
        }
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

        if (shiftKeyDown && !weaponDrawn)
            movementSpeed = sprintSpeed;

        if (wKeyDown && !sKeyDown && !jumping)
        {
            movement.y += movementSpeed;
        }
        if (sKeyDown && !wKeyDown && !jumping)
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

        return movement * movementSpeed;// * Time.deltaTime;
    }

    bool attack(Ability action)
    {
        //Reset charge attack time, so players can't hold it while using an ability and come straight out for a follow up heavy attack
        startedHolding = float.MaxValue;
        animator.SetBool("charged", false);

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

        if (bKeyDown && !attacking && !chargingAttack)
            bag.input();

        if(bag.isOpen())
            yield break;

        //Weapon Inputs
        //Attack
        bool leftClickDown = Input.GetMouseButtonDown(0);
        bool leftClickUp = Input.GetMouseButtonUp(0);
        //Ability #1
        bool rightClick = Input.GetMouseButtonDown(1);
        //Ability #2
        bool spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        //Draw Weapon
        bool qKeyDown = Input.GetKeyDown(KeyCode.Q);
        //Pickup Item
        bool eKeyDown = Input.GetKeyDown(KeyCode.E);

        if (qKeyDown && !attacking && !chargingAttack)
            drawWeapon();

        if (eKeyDown && !attacking)
            pickupItem();

        if (spaceKeyDown && !attacking && !jumping && !chargingAttack)
        {
            jump();
        }

        //Call movement function to handle movements
        Vector3 movementVector = Vector3.zero;

        if (!attacking)
            movementVector = movement();

        yield return new WaitForFixedUpdate(); //For rigidbody interactions

        if (leftClickDown && !attacking && weaponDrawn)
        {
            startedHolding = Time.time;
            chargingAttack = true;
        }

        if (leftClickUp && !attacking && weaponDrawn)
        {
            if (startedHolding + chargeTime < Time.time)
                attack(heavyAttack); 
            else
                attack(basicAttack);

            chargingAttack = false;
        }
        else if (leftClickUp && canCombo && weaponDrawn)
        {
            attack(basicAttack);
            Debug.Log("Combo");
            chargingAttack = false;
        }


        else if (rightClick && !attacking && weaponDrawn)
        {
            if (attack(abilities[0]))
                UIManager.instance.usedAbility(0);
        }
        else if (spaceKeyDown && !attacking && weaponDrawn)
        {
            if (attack(abilities[1]))
                UIManager.instance.usedAbility(1);
        }

        rb2D.AddForce(movementVector);
    }

    //Set Active
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

        if (!dead && !stunned)
             StartCoroutine("inputHandler"); //Alternte than coroutine??

        if (startedHolding + chargeTime < Time.time)
            animator.SetBool("charged", true);
    }

}