using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CMoveCombatable {


    [Header("AI Variables")]
    public float aggroRange = 5f;
    public float attackRange = 0.9f;
    AIState state;

    //Stun Variables
    public float stunTime = 0.5f;
    private bool stunned = false;

    //Movement Variables
    private Transform target;
    private Player player;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        player = GameObject.Find("Player").GetComponent<Player>();
        target = player.transform;
    }

    protected override void movement()
    {
        float movementSpeed = walkSpeed;

        Vector3 dir = getDirection(target.position, target.gameObject.GetComponent<CHitable>().objectHeight);

        if (dir.x < 0 && transform.localScale.x != -1 * facingFront)
            faceLeft();
        else if (dir.x > 0 && transform.localScale.x != 1 * facingFront)
            faceRight();

        animator.SetFloat("movementSpeed", 4f);
        //rb2D.MovePosition(transform.position + dir * movementSpeed * Time.deltaTime);
        rb2D.AddForce(dir * movementSpeed);

    }

    void attackTarget()
    {
        Vector2 direction = getDirection(target.position, target.gameObject.GetComponent<CHitable>().objectHeight);

        attack(target.position, direction, abilities[0]);
    }

    public override void loseHealth(int damage)
    {
        base.loseHealth(damage);

        //Stop enemy if they are currently attacking and stun them 
        if(attackAction != null)
            StopCoroutine(attackAction);

        StopCoroutine("stun");
        attacking = false;
        animator.SetTrigger("stopAttack");

        StartCoroutine("stun");
    }

    public IEnumerator stun()
    {
        stunned = true;
        animator.SetFloat("movementSpeed", 0);

        yield return new WaitForSeconds(stunTime);

        stunned = false;
    }

    // Update is called once per frame
    new void FixedUpdate ()
    {
        if (stunned || dead || player == null || player.isDead())
            return;

        //Abstract out a get target method for futre enemies?
        if ((player.transform.position - transform.position).magnitude < aggroRange)
            target = player.transform;
        else
            target = null;

        if (target != null && (player.transform.position - transform.position).magnitude > attackRange)
            movement();
        else
            animator.SetFloat("movementSpeed", 0);

        if (target != null && (player.transform.position - transform.position).magnitude < attackRange && !attacking)
            attackTarget();
    }
}
