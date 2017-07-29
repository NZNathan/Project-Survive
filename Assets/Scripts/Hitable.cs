using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour {

    //Components
    protected Rigidbody2D rb2D;

    protected float objectHeight;

    //Health Variables
    public int maxHealth = 100;
    protected int currentHealth;

    protected abstract IEnumerator flash();
    protected abstract void death();

    public void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void knockback(Vector2 target, float force)
    {
        rb2D.AddForce(getDirection(target) * force * -1);
    }

    protected Vector2 getDirection(Vector2 target)
    {
        //Get position of this
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

        //Get direction by subtracting player location
        target = (new Vector3(target.x, target.y, 0) - (Vector3)pos); 

        //Normalize the direction so mouse distance doesn't affect it
        float distance = target.magnitude;
        Vector2 direction = target / distance; // This is now the normalized direction.

        return direction;
    }

    public void loseHealth(int damage)
    {
        StartCoroutine("flash");
        currentHealth -= damage;

        if(currentHealth < 0)
        {
            currentHealth = 0;
            death();
        }
    }

	
}
