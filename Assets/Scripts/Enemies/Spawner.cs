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
    public Enemy spawnBoss(RevengeTarget revengeTarget)
    {

        Enemy boss = (Enemy) Instantiate(characterSpawnType, transform.position, Quaternion.identity);
        boss.transform.SetParent(transform.parent);

        if (revengeTarget == null)
            SpriteGen.generateEnemy(boss);
        else
            boss.setupRevengeTarget(revengeTarget);

        boss.hasBeenSeen = false;
        boss.isBoss = true;
        boss.gameObject.name = "BOSS:_" + boss.firstName;

        return boss;
    }
	
}
