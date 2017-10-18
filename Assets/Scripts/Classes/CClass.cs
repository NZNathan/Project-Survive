using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class CClass {

	public string name;

	//Poosible abilities that this class can get
	public Ability[] offensiveAbilityPool;
	public Ability[] defensiveAbilityPool;

	//Abilities the character actually has
	public Ability[] abilities;
	private int abilityAmount = 2;

    //Ability Numbers
    private int abilityOne;
    private int abilityTwo;

	//Class basic Attacks
	public Ability basicAttack;
    public Ability heavyAttack;

	//Class stat modifiers
	protected int strengthBonus;
	protected int agilityBonus;
	protected int enduranceBonus;

	public abstract void setupClass();

	protected void selectRandomAbilities()
	{
		abilities = new Ability[abilityAmount];

        //Get Offensive Ability
        abilityOne = Random.Range(0, offensiveAbilityPool.Length);
        abilities[0] =  offensiveAbilityPool[abilityOne];

        //Get Defensive Ability
        abilityTwo = Random.Range(0, defensiveAbilityPool.Length);
        abilities[1] =  defensiveAbilityPool[abilityTwo];
	}

    public void selectAbilities()
    {
        abilities = new Ability[abilityAmount];

        //Get Offensive Ability
        abilities[0] = offensiveAbilityPool[abilityOne];

        //Get Defensive Ability
        abilities[1] = defensiveAbilityPool[abilityTwo];
    }
}
