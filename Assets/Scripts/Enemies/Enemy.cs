using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CMoveCombatable {

    //Components
    private new SpriteRenderer renderer;

    //Revenge Target Variables
    public bool isBoss = false;
    public bool revengeTarget = false;

    [Header("AI Variables")]
    public float aggroRange = 5f;
    public float attackRange = 1.45f;
    private Stack<AIState> state;

    [Header("Drop On Death Variables")]
    public DropableItem[] dropableItems;
    public int xpWorth;

    //Knockback collisions off time
    protected float collisionOffTime = 0.3f;

    //Movement Variables
    //[HideInInspector]
    public Transform target;
    private float stopDistance = 0.5f; //Distance to stop away from target by defualt

    // Use this for initialization
    new void Start()
    {
        base.Start();

        animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();


        if(characterClass.name == "Gunner")
            attackRange = 4f;

        //Manage State
        state = new Stack<AIState>();
        state.Push(new IdleState(this));
    }

    #region States

    public AIState peekState()
    {
        if(state.Count > 0)
            return state.Peek();

        return null;
    }

    public void popState()
    {
        state.Pop();
    }

    public void pushState(AIState aiState)
    {
        state.Push(aiState);
    }

    #endregion

    public int getLevel()
    {
        return level;
    }

    public void setupRevengeTarget(RevengeTarget self)
    {
        //Set Details
        firstName = self.firstName;
        lastName = self.lastName;
        faction = self.faction;

        //Set Stats
        level = self.level;
        strength = self.strength;
        agility = self.agility;
        endurance = self.endurance;

        traits = self.traits;

        //Set animator
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = self.spriteController;
    }

    public Vector3 getTargetPositon()
    {
        Vector3 targetOffset = new Vector3(-stopDistance * transform.localScale.x, 0, 0);
        
        return target.position + targetOffset;
    }

    public override Vector2 movement()
    {//Abstract out different move types?

        float movementSpeed = sprintSpeed;

        if (target.transform.position.x < transform.position.x && transform.localScale.x != -1)
            faceLeft();
        else if (target.transform.position.x > transform.position.x && transform.localScale.x != 1)
            faceRight();

        Vector3 dir = getDirection(getTargetPositon(), target.gameObject.GetComponent<CHitable>().objectHeight);

        if (movementSpeed == walkSpeed)
            animator.SetFloat("movementSpeed", 2.5f);
        else
            animator.SetFloat("movementSpeed", 5f);

        return (dir * movementSpeed);

    }

    public void attackTarget()
    {
        if (true)
        {
            if (!characterClass.abilities[0].onCooldown())
            {
                attack(characterClass.abilities[0]);
                Debug.Log(characterClass.abilities[0].name);
            }
            else
                attack(characterClass.basicAttack);
        }
        else
            attack(characterClass.basicAttack);
    }

    public override void loseHealth(int damage)
    {
        StartCoroutine("flash");
        currentHealth -= damage;

        //Stop showHealth so it doesn't remove the health bar off an earilier call
        if (!isBoss)
        {
            StopCoroutine("showHealth");
            StartCoroutine("showHealth");
        }

        healthBar.loseHealth((float)currentHealth / (float)maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            death();
        }
        else
        {
            setInvulnerable(invulnTime);
        }

        //If idle and attacked, move toward new attack target
        popState();
        pushState(new MoveState(this));

        target = lastAttacker.transform;
    }

    protected override void death()
    {
        base.death();

        //If the player killed this enemy, award xp to player
        if(lastAttacker.tag == "Player")
            ((Player) lastAttacker).addXp(xpWorth);

        //If enemy was a boss, then turn off boss GUI and change music
        if (isBoss)
        {
            UIManager.instance.closeBossGUI();
            MusicManager.instance.stopBossMusic();
            CameraFollow.screenLocked = false;
        }

        //Item drops
        foreach(DropableItem dropableItem in dropableItems)
        {
            dropableItem.dropItem(transform.position, transform.parent); 
        }
    }

    public override void knockback(Vector2 target, int force, float targetHeight)
    {
        rb2D.velocity = Vector2.zero;
        base.knockback(target, force, targetHeight);

        startCollisionsOff(collisionOffTime);
    }

    private new void Update()
    {
        if (!WorldManager.isPaused)
        {
            base.Update();
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!WorldManager.isPaused)
            state.Peek().action();
    }

    //Just landed a hit on an enemy
    public override void attackHit()
    {
        //AI Reaaction here
    }
}
