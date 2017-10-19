using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour {

    Enemy enemy;

	// Use this for initialization
	void Start ()
    {
        enemy = GetComponentInParent<Enemy>();

    }

    private void OnBecameVisible()
    {
        //If not a boss and not dead
        if (!enemy.isBoss && !enemy.isDead())
            MusicManager.instance.addEnemy();
    }

    private void OnBecameInvisible()
    {
        if (!enemy.isBoss && !enemy.isDead())
            MusicManager.instance.removeEnemy();
    }
}
