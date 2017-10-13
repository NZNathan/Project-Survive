using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns in an enemy on Start
/// </summary>
public class Spawner : MonoBehaviour {

    public C characterSpawnType;
    public bool bossSpawn = false;
    public BossArea bossArea;

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
            SpriteGen.instance.generateEnemy(boss);
        else
            boss.setupRevengeTarget(revengeTarget);

        bossArea.boss = boss;
        boss.isBoss = true;
        boss.gameObject.name = "BOSS:_" + boss.firstName;

        return boss;
    }
	
}
