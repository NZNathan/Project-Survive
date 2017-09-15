using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns in an enemy on Start
/// </summary>
public class Spawner : MonoBehaviour {

    public C characterSpawnType;
    public bool bossSpawn = false;

	// Use this for initialization
	void Start ()
    {
        this.gameObject.SetActive(false);
	}

    public C spawnCharacter()
    {
        C character = Instantiate(characterSpawnType, transform.position, Quaternion.identity);
        character.transform.SetParent(transform.parent);
        return character;
    }

    //REFACTOR
    public Enemy spawnBoss(Enemy boss)
    {
        if (boss == null)
            boss = (Enemy)characterSpawnType;
        else
            boss.setRevengeTarget();

        boss = (Enemy) Instantiate(boss, transform.position, Quaternion.identity);
        boss.transform.SetParent(transform.parent);

        boss.setBoss();
        boss.gameObject.name = "BOSS";

        return boss;
    }
	
}
