using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns in an enemy on Start
/// </summary>
public class Spawner : MonoBehaviour {

    public C characterSpawnType;

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
	
}
