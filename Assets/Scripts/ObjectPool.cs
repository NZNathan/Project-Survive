using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool instance;

    //Objects
    public Bullet[] bullets;

	// Use this for initialization
	void Start ()
    {
        instance = this;

        //Get all objects in pool
        bullets = GetComponentsInChildren<Bullet>(true);

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
}

