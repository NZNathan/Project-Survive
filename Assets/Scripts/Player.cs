using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CMoveCombatable
{
    public static Player instance;

    //Experiance Variables
    public static int pointsOnLevelUp = 3;
    private int levelUpPoints = 0;
    private int xpPerLevelIncrease = 5;
    private int xpPerLevel = 5; //Reach this to level up
    private int xp = 0;

    //Input Variables
    private bool chargingAttack = false;

    //Menu Variables
    private bool inMenu = false;
    private bool inBagMenu = false;
    private bool inLevelMenu = false;

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

        //Set up sprites to include weapon
        weapon.SetActive(true);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        weapon.SetActive(false);

        UIManager.instance.setAbilities(abilities); //REmove when player is generated
    }

    public bool isInMenu()
    {
        return inMenu;
    }

    public void setInMenu(bool inMenu)
    {
        this.inMenu = inMenu;
    }

    public void closeBagMenu()
    {
        inBagMenu = false;
        Invoke("closeMenu", 0.1f);
    }

    public void closeLevelupMenu()
    {
        inLevelMenu = false;
        Invoke("closeMenu", 0.1f);
    }

    public void closeMenu()
    {
        inMenu = false;
    }

    public void levelup(int pointsLeft, int[] stats)
    {
        levelUpPoints = pointsLeft;

        strength += stats[0];
        agility += stats[1];
        endurance += stats[2];
    }

    internal int getLevelUpPoints()
    {
        return levelUpPoints;
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
                itemInRange = null;
            }
        }
    }

    public override void attackHit()
    {
        CameraFollow.cam.GetComponentInParent<CameraShake>().shake = .5f;
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
        Vector2 mousePos = CameraFollow.cam.ScreenToWorldPoint(Input.mousePosition);

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
        bool cKeyDown = Input.GetKeyDown(KeyCode.C);

        if (bKeyDown && !attacking && !chargingAttack && (!inMenu || inBagMenu))
        {
            inMenu = !inMenu;
            inBagMenu = !inBagMenu;
            bag.input();
        }

        if (cKeyDown && !attacking && !chargingAttack && (!inMenu || inLevelMenu))
        {
            inMenu = !inMenu;
            inLevelMenu = !inLevelMenu;
            UIManager.instance.toggleLevelUpWindow();
        }

        //Only take menu inputs if in menu
        if (inMenu)
            yield break;

        //Weapon Inputs
        //Attack
        bool leftClickDown = Input.GetMouseButtonDown(0);
        bool leftClickHeld = Input.GetMouseButton(0);
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

        if (spaceKeyDown && !attacking && !jumping && !chargingAttack && !weaponDrawn)
        {
            jump();
        }

        //Call movement function to handle movements
        Vector3 movementVector = Vector3.zero;

        if (!attacking)
            movementVector = movement();

        yield return new WaitForFixedUpdate(); //For rigidbody interactions

        //If left click with weapon out and not already attacking, then start charging
        if (((leftClickHeld && !chargingAttack && !stunned) || leftClickDown) && !attacking && weaponDrawn)
        {
            startedHolding = Time.time;
            chargingAttack = true;
        }
        //If left click with no weapon out and not attacking or charging, then draw weapon
        else if (leftClickUp && !attacking && !chargingAttack && !weaponDrawn)
        {
            drawWeapon();
        }
        //If releasing left click after charging up and not already attacking then execute either a heavy or light attack based on charge time
        else if (leftClickUp && !attacking && weaponDrawn && chargingAttack)
        {
            if (startedHolding + chargeTime < Time.time)
                attack(heavyAttack); 
            else
                attack(basicAttack);

            chargingAttack = false;
        }
        //If you are attacking and can combo then attack with basic attack
        else if (leftClickUp && canCombo && weaponDrawn)
        {
            attack(basicAttack);
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
            CameraFollow.cam.GetComponentInParent<CameraShake>().shake = .5f;
    }

    public void addXp(int xpGained)
    {
        xp += xpGained;

        if(xp >= xpPerLevel)
        {
            levelUpPoints += pointsOnLevelUp;
            xp -= xpPerLevel;
            level++;
            xpPerLevel += xpPerLevelIncrease;
        }

        //Update UI xp bar
        UIManager.instance.addXp(xp, xpPerLevel);
    }

    public override void applyStun(float stunTime)
    {
        base.applyStun(stunTime);

        startedHolding = Mathf.Infinity;
        animator.SetBool("charged", false);
        chargingAttack = false;
    }

    public int[] getStats()
    {
        int[] stats = { strength, agility, endurance };
        return stats;
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!dead && !stunned)
             StartCoroutine("inputHandler"); //Alternte than coroutine??

        if (startedHolding + chargeTime < Time.time)
            animator.SetBool("charged", true);

        if (inMenu)
            animator.SetFloat("movementSpeed", 0);
    }

}