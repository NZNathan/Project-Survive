using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceBullet : MonoBehaviour {

    //Components
    private Rigidbody2D rb2D;

    //Information Variables
    private CMoveCombatable caster;

    //Lifetime Variables
    private float lifespan = 0.3f;
    private float timeShot = 0f;

    //Speed Variables
    private float velocity = .8f;
    private Vector3 dir;

	// Use this for initialization
	public void Setup(CMoveCombatable caster, Vector3 dir)
    {
        rb2D = GetComponent<Rigidbody2D>();

        this.caster = caster;
        this.dir = dir;

        timeShot = Time.time;

        Vector3 spawnPos = new Vector3(caster.transform.position.x, caster.transform.position.y + caster.objectHeight / 2, caster.transform.position.z);
        transform.position = spawnPos + (dir * 0.4f) ;
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
