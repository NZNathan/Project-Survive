using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeTarget {

    //Details
    public Faction faction;
    public string firstName;
    public string lastName;

    //Animator
    public RuntimeAnimatorController animatorController;

    //Traits
    public Trait[] traits;

    //Stats
    public int level;
    public int strength;
    public int agility;
    public int endurance;
    public int attackDamage;

    //Sprite
    public Sprite[] sprites;

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
        level = stats[0];
        strength = stats[1];
        agility = stats[2];
        endurance = stats[3];

        traits = target.getTraits();

        //Save Sprites
        sprites = target.getSprites();

        animatorController = target.animator.runtimeAnimatorController;
        Debug.Log(animatorController.name);
    }
}
