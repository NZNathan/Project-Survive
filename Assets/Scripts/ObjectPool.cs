using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool instance;

    //Objects
    public Bullet[] bullets;

    public RicochetBullet[] rbullets;

	// Use this for initialization
	void Start ()
    {
        instance = this;

        //Get all objects in pool
        bullets = GetComponentsInChildren<Bullet>(true);
        rbullets = GetComponentsInChildren<RicochetBullet>(true);
    }
	
	public Bullet getBullet()
    {
        foreach(Bullet bullet in bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }

        Debug.LogError("No available bullet objects");
        return null;
    }

    public RicochetBullet getRicochetBullet()
    {
        foreach(RicochetBullet rbullet in rbullets)
        {
            if (!rbullet.gameObject.activeInHierarchy)
                return rbullet;
        }

        Debug.LogError("No available rbullet objects");
        return null;
    }
}

