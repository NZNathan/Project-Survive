using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CMoveCombatable
{
    //Static Variables
    public static Player instance;
    public static string familyName = "";
    public static Vector3 spawmPos = new Vector3(-13f, -0.2f, 0);

    public new static HealthBar healthBar;

    //Experiance Variables
    public static int pointsOnLevelUp = 3;
    private int levelUpPoints = 0;
    private int xpPerLevelIncrease = 5;
    private int xpPerLevel = 5; //Reach this to level up
    private int xp = 0;

    //Equipment Variables
    private Equipment[] equipment = new Equipment[3];

    //Menu Variables
    private bool inMenu = false;
    private bool inBagMenu = false;
    private bool inLevelMenu = false;

    //Inventory Variables
    [HideInInspector]
    public Bag bag;
    public ItemsInRange itemsInRange;
    private int coins = 900;

    public new void Start()
    {
        healthBar.resetFill();
        healthBar.setActive(true);
        base.healthBar = healthBar;

        base.Start();

        //Set up family Name
        if (familyName == "")
            familyName = lastName;
        else
            lastName = familyName;

        bag = new Bag(UIManager.instance.bagGUIObject.GetComponent<BagGUI>());

        itemsInRange = new ItemsInRange(this);

        //traits[0] = Trait.getTrait();
        //traits[0].applyTrait(this);

        //Singleton
        if (instance == null)
            instance = this;

        animator = GetComponentInChildren<Animator>();

        UIManager.instance.setAbilities(characterClass.abilities); //REmove when player is generated
    }

    public void setSingleton()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
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

    public int getCoinsAmount()
    {
        return coins;
    }

    public void addCoins(int coinsAmount)
    {
        coins += coinsAmount;
    }

    public void removeCoins(int coinsAmount)
    {
        coins -= coinsAmount;
    }

    public void levelup(int pointsLeft, int[] stats)
    {
        levelUpPoints = pointsLeft;

        strength += stats[0];
        agility += stats[1];
        endurance += stats[2];

        //Update health
        currentHealth += stats[2] * endMod;
        maxHealth += stats[2] * endMod;

        //Update Health bar
        healthBar.updateFill((float)currentHealth / (float)maxHealth);
    }

    internal int getLevelUpPoints()
    {
        return levelUpPoints;
    }

    public void useItem(int i)
    {
        bag.useItem(i);
    }

    public override void attackHit()
    {
        CameraFollow.cam.GetComponentInParent<CameraShake>().shake = .5f;
    }

    public override Vector2 movement()
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

        //If movement amount is zero
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

    public override bool attack(Ability action)
    {
        //Reset charge attack time, so players can't hold it while using an ability and come straight out for a follow up heavy attack
        animator.SetBool("charged", false);

        //Get mouse position in relation to the world
        /*Vector2 mousePos = CameraFollow.cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = getDirection(mousePos, 0);*/

        return base.attack(action);
    }

    protected override void death()
    {
        base.death();

        WorldManager.instance.playerDied(this);
    }

    private Vector3 checkBounds(Vector3 movement)
    {
        //If trying to move off screen to the left
        if ((atLeftEdgeOfScreen() && rb2D.velocity.x <= 0))
        {
            moving = false;
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            
            if (movement.x <= 0)
                movement = new Vector2(0, movement.y);
        }
        //If trying to move screen to the right
        else if ((atRightEdgeOfScreen() && rb2D.velocity.x >= 0))
        {
            moving = false;
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);

            if (movement.x >= 0)
                movement = new Vector2(0, movement.y);
        }

        return movement;
    }

    /// <summary>
    /// If player is at the edge of the screen and the screen is locked return true
    /// </summary>
    /// <returns></returns>
    private bool atLeftEdgeOfScreen()
    {
        CameraFollow cam = Camera.main.GetComponentInParent<CameraFollow>();

        if (cam == null)
            return false;

        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        var dist = rightBorder - leftBorder;

        return transform.position.x <= cam.transform.position.x - dist / 2 + .2;
    }

    private bool atRightEdgeOfScreen()
    {
        CameraFollow cam = Camera.main.GetComponentInParent<CameraFollow>();

        if (cam == null)
            return false;

        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        var dist = rightBorder - leftBorder;

        return transform.position.x >= cam.transform.position.x + dist / 2 - .2;
    }

    protected override void removeDeadBody()
    {
        return;
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

        if (eKeyDown && !attacking)
            itemsInRange.pickupItem();     

        //Call movement function to handle movements
        Vector3 movementVector = Vector3.zero;

        if (!attacking)
            movementVector = movement();

        movementVector = checkBounds(movementVector);

        yield return new WaitForFixedUpdate(); //For rigidbody interactions

        //If left click with weapon out and not already attacking, then start charging
        if (((leftClickHeld && !chargingAttack && !stunned) || leftClickDown) && !attacking)
        {
            animator.SetTrigger("charging");
            chargingAttack = true;
        }
        //If releasing left click after charging up and not already attacking then execute either a heavy or light attack based on charge time
        else if (leftClickUp && !attacking && chargingAttack)
        {
            if (chargedHeavy)
                attack(characterClass.heavyAttack);
            else
            {
                //animator.SetTrigger("stopCharge");
                attack(characterClass.basicAttack);
            }

            chargingAttack = false;
        }
        //If you are attacking and can combo then attack with basic attack
        else if (leftClickUp && attacking)
        {
            canCombo = true;
            chargingAttack = false;
        }


        else if (rightClick && !attacking)
        {
            if (attack(characterClass.abilities[0]))
                UIManager.instance.usedAbility(0);
        }
        else if (spaceKeyDown && !attacking)
        {
            if (attack(characterClass.abilities[1]))
                UIManager.instance.usedAbility(1);
        }

        rb2D.AddForce(movementVector);
    }


    public override void loseHealth(int damage)
    {
        base.loseHealth(damage);
        StopCoroutine("showHealth");
        animator.ResetTrigger("charging");
        //attackTrigger.idleState();

        if (!dead)
            CameraFollow.cam.GetComponentInParent<CameraShake>().shake = .5f;
    }

    public void addXp(int xpGained)
    {
        xp += xpGained;

        if (xp >= xpPerLevel)
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

        animator.SetBool("charged", false);
        chargingAttack = false;
    }

    public int[] getStats()
    {
        int[] stats = { strength, agility, endurance };
        return stats;
    }

    /// <summary>
    /// Equips passed in item to an empty equipment slot or replaces the item in equipment slot 1
    /// </summary>
    /// <param name="item"></param>
    public void equipItem(Equipment item)
    {
        int i = 2;
        if (item.isWeapon)
        {
            i = 1;

            //If already have a weapon equipped
            if (equipment[1] != null)
            {
                Equipment eq = unequipItem(1);
                int index = bag.findIndex(item);
                StartCoroutine(setItem(eq, index));
            }
        }
        else if (equipment[0] == null)
        {
            i = 0;
        }
        else if (equipment[2] != null)
        {
            Equipment eq = unequipItem(0);
            
            int index = bag.findIndex(item);
            StartCoroutine(setItem(eq, index));
            i = 0;
        }

        equipment[i] = item;
        UIManager.instance.equipItem(i, item);

        //Update Stats
        strength += item.strMod;
        agility += item.aglMod;
        endurance += item.endMod;

        //Update health
        maxHealth += equipment[i].endMod * endMod;

        healthBar.updateFill((float)currentHealth / (float)maxHealth);
    }


    private IEnumerator setItem(Item item, int i)
    {
        yield return new WaitForEndOfFrame();

        bag.addItem(item, i);
    }

    /// <summary>
    /// Unequips the item at index i and adds it to the bag if there is room, returns true if operation was successful
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Equipment unequipItem(int i)
    {
        Equipment eq = equipment[i];

        //Update Stats
        strength -= equipment[i].strMod;
        agility -= equipment[i].aglMod;
        endurance -= equipment[i].endMod;

        //Update health
        maxHealth -= equipment[i].endMod * endMod;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        healthBar.updateFill((float)currentHealth / (float)maxHealth);

        UIManager.instance.updateLevelUpWindow();
        equipment[i] = null;

        return eq;
    }

    // Update is called once per frame
    new void Update()
    {
        if (!WorldManager.isPaused)
        {
            base.Update();

            if (!dead && !stunned)
                StartCoroutine("inputHandler"); //Alternte than coroutine??

            if (!chargingAttack && chargedHeavy)
            {
                chargedHeavy = false;
                animator.SetBool("charged", false);           
            }

            if (inMenu)
                animator.SetFloat("movementSpeed", 0);
        }
    }
}