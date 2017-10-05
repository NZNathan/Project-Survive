using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool instance;

    //Objects
    public Bullet[] bullets;
    public RicochetBullet[] rbullets;
    public LanceBullet[] lbullets;

	// Use this for initialization
	void Start ()
    {
        instance = this;

        //Get all objects in pool
        bullets = GetComponentsInChildren<Bullet>(true);
        rbullets = GetComponentsInChildren<RicochetBullet>(true);
        lbullets = GetComponentsInChildren<LanceBullet>(true);
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

    public LanceBullet getLanceBullet()
    {
        foreach(LanceBullet lbullet in lbullets)
        {
            if (!lbullet.gameObject.activeInHierarchy)
                return lbullet;
        }

        Debug.LogError("No available lbullet objects");
        return null;
    }
}

