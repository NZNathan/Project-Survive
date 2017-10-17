using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeTarget {

    //Details
    public Faction faction;
    public string firstName;
    public string lastName;

    //Animator
    public RuntimeAnimatorController spriteController;

    //Class
    public CClass characterClass;

    //Traits
    public Trait[] traits;

    //Stats
    public int level;
    public int strength;
    public int agility;
    public int endurance;
    public int attackDamage;

    //Equipment
    public GameObject weapon;

    /// <summary>
    /// Saves the stats and information about this enemy, so it can be recreated at a later point
    /// </summary>
    /// <param name="target"></param>
	public RevengeTarget(Enemy target)
    {
        //Save Details
        firstName = target.firstName;
        lastName = target.lastName;
        faction = target.faction;

        //Save Stats
        int[] stats = target.getStats();
        level = target.getLevel();
        strength = stats[0];
        agility = stats[1];
        endurance = stats[2];

        characterClass = target.getClass();

        traits = target.getTraits();

        spriteController = target.animator.runtimeAnimatorController;
    }
}
