using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : MonoBehaviour {

    //Components
    private Rigidbody2D rb2D;
    new private SpriteRenderer renderer;

    //Information Variables
    private CMoveCombatable caster;
    public int damage;
    private float stunTime;

	//Target Variables
	private CMoveCombatable target;
    private List<CMoveCombatable> targetsHit;

	//Lifetime Variables
    private float durability = 3;

    //Rebound Variables
    private float rebounded = 0;
    private float reboundTime = 0.1f;

    //Death variables
    private string causeOfDeath = "Caught by a ricocheting bullet";

    //Speed Variables
    private float velocity = .25f;
    private Vector3 dir;

	// Use this for initialization
	public void Setup(CMoveCombatable caster, int damage, float stunTime, Vector3 dir)
    {
        rb2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        this.caster = caster;
        this.damage = damage;
        this.stunTime = stunTime;
        this.dir = dir;

        durability = 3;

        transform.rotation = Quaternion.Euler(0,0,0);
        target = null;
        targetsHit = new List<CMoveCombatable>();

        Vector3 spawnPos = new Vector3(caster.transform.position.x, caster.transform.position.y + caster.objectHeight / 2, caster.transform.position.z);
        transform.position = spawnPos + (dir * 0.4f) ;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //If bullet is rebounding then don't detect collisions
        if(rebounded + reboundTime > Time.time)
            return;
        //If an object has been hit first, destroy the bullet
        if (collider.transform.gameObject.tag == "Object")
        {
            if (!collider.isTrigger)
                this.gameObject.SetActive(false);
            //Add destoryed particle effect here
        }

        CHitable targetHit = collider.GetComponentInParent<CHitable>();
        CMoveCombatable enemy = collider.GetComponentInParent<CMoveCombatable>();

        //If object hit is hitable, and this bullet hasn't already hit the target
        if (targetHit != null && !targetsHit.Contains(enemy))
        {

            if (targetHit.isInvuln() || targetHit.isKnockedback())
                return;

            if(enemy != null && enemy.parrying){
                this.gameObject.SetActive(false);
            }

            //Apply damage and knockback
            targetHit.setAttacker(caster);
            //objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
            targetHit.loseHealth(damage);

            //Apply stun to the target
            targetHit.applyStun(stunTime);

            if (enemy != null)
                enemy.causeOfDeath = causeOfDeath;

            //TODO: Play audio sound
            caster.attackHit();
			durability--;

			if(durability <= 0)
            	this.gameObject.SetActive(false);
			else
			{
                targetsHit.Add(enemy);
				findNewTarget();
				if(target == null)
            		this.gameObject.SetActive(false);
			}
        }
    }

	private void findNewTarget()
	{
		target = null;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 5f, CMoveCombatable.attackMask);

		int i = 0;
        while (i < hitColliders.Length && target == null)
        {
            CMoveCombatable character = hitColliders[i].GetComponentInParent<CMoveCombatable>();
            if (character != null && FactionManager.instance.isHostile(character.faction, caster.faction) && !targetsHit.Contains(character))
				target = character;
            i++;
        }
		Debug.Log(target);
	}

    private void Update()
    {
        if(!renderer.isVisible)
            this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            //Get new direction to target
            dir = target.getDirection(transform.position, target.objectHeight) * -1;

            //Angle toward new target
            transform.right = target.transform.position - transform.position;
        }

        rb2D.MovePosition(transform.position + (dir * velocity));
    }
}
