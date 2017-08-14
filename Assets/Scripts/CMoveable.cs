using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMoveable : C {

    //Components
    [HideInInspector]
    public CircleCollider2D col2D;
    [HideInInspector]
    public AudioSource audioSource;

    //Movement Variables
    [Header("Movement Variables")]
    public float walkSpeed = 1f;
    public float sprintSpeed = 2f;

    [HideInInspector]
    public bool canMove = true;
    protected bool moving = false;
    //Movement
    protected abstract Vector2 movement();

    protected new void Start()
    {
        base.Start();

        //Get Components
        col2D = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void faceLeft()
    {
        transform.localScale = new Vector3(-1 * facingFront, 1, 1);
    }

    public void faceRight()
    {
        transform.localScale = new Vector3(1 * facingFront, 1, 1);
    }

    public void recoverHealth(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        //Stop showHealth so it doesn't remove the health bar off an earilier call
        StopCoroutine("showHealth");
        StartCoroutine("showHealth");

        healthBar.healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    void LateUpdate()
    {

        //if (rb2D.velocity.magnitude < 1.5f && !moving)
            //rb2D.velocity = Vector2.zero;
    }
}
