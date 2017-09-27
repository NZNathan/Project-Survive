using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class CClass {

	public string name;

	//Poosible abilities that this class can get
	public Ability[] offensiveAbilityPool;
	public Ability[] defensiveAbilityPool;
	public Ability[] specialAbilityPool;

	//Abilities the character actually has
	public Ability[] abilities;
	private int abilityAmount = 2;

	//Class basic Attacks
	public Ability basicAttack;
    public Ability heavyAttack;

	//Class stat modifiers
	protected int strengthBonus;
	protected int agilityBonus;
	protected int enduranceBonus;

	protected abstract void setupClass();

	protected void selectRandomAbilities()
	{
		abilities = new Ability[abilityAmount];

		//Get Offensive Ability
		int i = Random.Range(0, offensiveAbilityPool.Length);
        abilities[0] =  offensiveAbilityPool[i];

		//Get Defensive Ability
		i = Random.Range(0, defensiveAbilityPool.Length);
        abilities[1] =  defensiveAbilityPool[i];

		//Get Special Ability
		//i = Random.Range(0, specialAbilityPool.Length);
        //classAbilities[2] =  specialAbilityPool[i];
	}
}
