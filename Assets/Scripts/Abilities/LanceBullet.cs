using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceBullet : MonoBehaviour {

    //Components
    private Rigidbody2D rb2D;
    new private SpriteRenderer renderer;

    //Information Variables
    private CMoveCombatable caster;
    public int damage;
    private float stunTime;
    private List<CHitable> targetsHit;
    private Faction faction;

    //Lifetime Variables
    private float lifespan = 0.3f;
    private float timeShot = 0f;

    //Speed Variables
    private float velocity = .8f;
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
        faction = caster.faction;

		targetsHit = new List<CHitable>();
        timeShot = Time.time;

        Vector3 spawnPos = new Vector3(caster.transform.position.x, caster.transform.position.y + caster.objectHeight / 2, caster.transform.position.z);
        transform.position = spawnPos + (dir * 0.4f) ;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //If an object has been hit first, destroy the bullet
        if (collider.transform.gameObject.tag == "Object")
        {
            if (!collider.isTrigger)
                this.gameObject.SetActive(false);
            //Add destoryed particle effect here
        }

        CHitable targetHit = collider.GetComponentInParent<CHitable>();

        //If object hit is hitable, and this bullet hasn't hit anything else this life
        if (targetHit != null && !targetsHit.Contains(targetHit))
        {

            if (targetHit.isInvuln() || targetHit.isKnockedback())
                return;

            CMoveCombatable enemy = collider.GetComponentInParent<CMoveCombatable>();

            if(enemy != null && !FactionManager.instance.isHostile(enemy.faction, faction))
            {
                return;
            }

            targetsHit.Add(targetHit);

            //Apply damage and knockback
            targetHit.setAttacker(caster);
            //objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
            targetHit.loseHealth(damage);

            //Apply stun to the target
            targetHit.applyStun(stunTime);

            //TODO: Play audio sound
            caster.attackHit();
        }
    }

    private void Update()
    {
        if(Time.time - lifespan > timeShot)
            this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        rb2D.MovePosition(transform.position + (dir * velocity));
    }
}
