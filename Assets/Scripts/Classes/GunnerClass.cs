using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerClass : CClass {

	// Use this for initialization
	public GunnerClass () 
	{
		name = "Gunner";
    	setupClass();
		selectRandomAbilities();
	}
	
	public override void setupClass()
	{
		//Set up offensive Abilities
		offensiveAbilityPool = new Ability[2];

		offensiveAbilityPool[0] = new LanceShot();
		offensiveAbilityPool[1] = new RicochetShot();

		//Set up defensive Abilities
		defensiveAbilityPool = new Ability[1];

		defensiveAbilityPool[0] = new DodgeRoll();

		//Set up basic and heavy attacks
		basicAttack = new BasicShoot();
		heavyAttack = new HeavyAttack();
	}
}
