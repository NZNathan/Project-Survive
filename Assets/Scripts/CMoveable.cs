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
    public abstract Vector2 movement();

    protected new void Start()
    {
        base.Start();

        //Get Components
        col2D = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = MusicManager.instance.soundEffectsVolume;
    }

    public void facePoint(Vector3 point)
    {
        if (point.x < transform.position.x)
            faceLeft();
        else
            faceRight();
    }

    public override void applyStun(float stunTime)
    {
        return;
    }

}
