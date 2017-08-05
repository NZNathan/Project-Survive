using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CHitable : MonoBehaviour {

    //Components
    [HideInInspector]
    public Rigidbody2D rb2D;
    protected HealthBar healthBar;

    [HideInInspector]
    public float objectHeight;

    //Health Variables
    [Header("Health Variables")]
    public int maxHealth = 100;
    protected int currentHealth;

    //Invulnerable Variables
    protected bool invulnerable = false;
    public float invulnTime = 0.3f;
    public bool knockedback = false;
    protected CMoveCombatable lastAttacker;

    //Collisions
    public static int hitabelLayer = 5;
    public static int noCollisionsLayer = 6;

    //Abstract Functions
    protected abstract IEnumerator flash();
    protected abstract void death();
    

    public void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        

        currentHealth = maxHealth;

        healthBar = UIManager.instance.newHealthBar();
        healthBar.setTarget(transform);
        healthBar.setActive(false);
    }

    public bool isInvuln()
    {
        return invulnerable;
    }

    public bool isKnockedback()
    {
        return knockedback;
    }

    public void setAttacker(CMoveCombatable attacker)
    {
        lastAttacker = attacker;
    }

    public CMoveCombatable getAttacker()
    {
        return lastAttacker;
    }

    public virtual void knockback(Vector2 target, int force, float targetHeight)
    {
        knockedback = true;
        rb2D.AddForce(getDirection(target, targetHeight) * force * -1); //Added object height?
        StartCoroutine("beingKnockedBack");
    }

    protected Vector2 getDirection(Vector2 target, float targetHeight)
    {
        //Get position of this
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

        //Get direction by subtracting player location
        target = (new Vector3(target.x, target.y + targetHeight/2, 0) - (Vector3)pos);
        //Normalize the direction so mouse distance doesn't affect it
        float distance = target.magnitude;

        Vector2 direction = target / (distance + 0.0001f); // This is now the normalized direction. add 0.001f to avoid dividing by 0

        return direction;
    }

    //Set AActive
    public virtual void loseHealth(int damage)
    {
        StartCoroutine("flash");
        currentHealth -= damage;

        //Stop showHealth so it doesn't remove the health bar off an earilier call
        StopCoroutine("showHealth");
        StartCoroutine("showHealth");

        healthBar.healthBar.fillAmount = (float) currentHealth /  (float) maxHealth;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            death();
        }
        else
        {
            invulnerable = true;
            Invoke("cancelInvuln", invulnTime);
        }
    }

    void cancelInvuln()
    {
        invulnerable = false;
        
    }

    IEnumerator beingKnockedBack()
    {
        knockedback = true;

        yield return new WaitForSeconds(0.001f);

        while (rb2D.velocity.magnitude > 0.6f) //Alter??
        {
            yield return new WaitForSeconds(0.001f);
        }

        knockedback = false;
    }

    IEnumerator showHealth()
    {
        healthBar.setActive(true);

        yield return new WaitForSeconds(1f);

        healthBar.setActive(false);
    }

	
}
