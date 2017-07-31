using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    //Components
    protected Animator animator;
    protected AudioSource audioSource;

    //AI Variables
    public float aggroRange = 5f;
    public float attackRange = 0.9f;
    private float pauseAfterAttack = 0.7f;

    //Raycast Variables
    private LayerMask attackMask; //Layer that all attackable objects are on
    private float attackRayRange = 0.9f;
    private IEnumerator attackAction;

    //Attack Variables
    public float attackVelocity = 350f;
    private bool attacking = false;
    private int attackDamage = 3;
    private float attackPause = 0.25f;
    private float attackForce = 500f; //knockback

    //Stun Variables
    private bool stunned = false;
    private float stunTime = 0.5f;

    //Audio Variables
    public AudioClip attackSound;
    public AudioClip missSound;

    //Movement Variables
    public float movementSpeed = 0.8f;
    private Transform target;

    private GameObject player;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        attackMask = LayerMask.GetMask("Hitable");
        player = GameObject.Find("Player");
    }

    void moveTowardTarget()
    {
        Vector3 dir = getDirection(target.position, target.gameObject.GetComponent<Hitable>().objectHeight);

        if (dir.x < 0 && transform.localScale.x != -1 * facingFront)
            faceLeft();
        else if (dir.x > 0 && transform.localScale.x != 1 * facingFront)
            faceRight();

        animator.SetInteger("movementSpeed", 1);
        rb2D.MovePosition(transform.position + dir * movementSpeed * Time.deltaTime);

    }

    void attackTarget()
    {
        //Stop movement and call attack animataion
        attacking = true;
        animator.SetInteger("movementSpeed", 0);
        animator.ResetTrigger("stopAttack");
        animator.SetTrigger("attack");

        Vector2 direction = getDirection(target.position, target.gameObject.GetComponent<Hitable>().objectHeight);

        //Get position of player
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

        //Face attack direction
        if (target.position.x < pos.x)
            faceLeft();
        else
            faceRight();

        attackAction = rayCastAttack(pos, direction, attackPause);
        StartCoroutine(attackAction);

        

    }

    IEnumerator rayCastAttack(Vector2 pos, Vector2 direction, float pause)
    {
        rb2D.AddForce(direction * attackVelocity);

        yield return new WaitForSeconds(pause);

        RaycastHit2D[] hitObject = Physics2D.RaycastAll(pos, direction, attackRayRange, attackMask, -10, 10);
        Debug.DrawRay(pos, direction * attackRayRange, Color.blue, 3f);

        bool hitTarget = false;

        //If the Raycast hits an object on the layer Enemy
        foreach (RaycastHit2D r in hitObject)
        {
            if (r && r.transform.gameObject != this.gameObject && attacking)
            {
                //Hit attack
                Hitable objectHit = r.transform.gameObject.GetComponentInParent<Hitable>();

                //Apply damage and knockback
                objectHit.loseHealth(attackDamage);
                objectHit.knockback(pos, attackForce, objectHit.objectHeight);

                audioSource.clip = attackSound;
                audioSource.Play();

                hitTarget = true;
                break;
            }
        }

        if (!hitTarget)
        {
            audioSource.clip = missSound;
            audioSource.Play();
        }

        yield return new WaitForSeconds(pauseAfterAttack);

        attacking = false;
    }

    public override void loseHealth(int damage)
    {
        base.loseHealth(damage);

        //Stop enemy if they are currently attacking and stun them 
        StopCoroutine(attackAction);
        StopCoroutine("stun");
        attacking = false;
        animator.SetTrigger("stopAttack");

        StartCoroutine("stun");
    }

    public IEnumerator stun()
    {
        stunned = true;
        animator.SetInteger("movementSpeed", 0);

        yield return new WaitForSeconds(stunTime);

        stunned = false;
    }

    //Merge with player movement into character??
    public void faceLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void faceRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    new void Update ()
    {
        base.Update();

        if (stunned)
            return;

        //Abstract out a get target method for futre enemies?
        if ((player.transform.position - transform.position).magnitude < aggroRange)
            target = player.transform;
        else
            target = null;

        if (target != null && (player.transform.position - transform.position).magnitude > attackRange)
            moveTowardTarget();
        else
            animator.SetInteger("movementSpeed", 0);

        if (target != null && (player.transform.position - transform.position).magnitude < attackRange && !attacking)
            attackTarget();
    }
}
